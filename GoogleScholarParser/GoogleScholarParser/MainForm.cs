using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using System.Net;
using System.IO;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using Npgsql;

namespace GoogleScholarParser
{
    public struct Data
    {
        public DateTime date;
        public string language;
        public string text;
        public string files;
        public string owners;
        public int id_type_table;
        public int id_elibrary;
        public string university;
        public string journal;
        public string volume_journal;
        public string num_journal;
        public int year;
        public string pages;
    }

    public partial class MainForm : Form
    {
        private Thread threadParse;
        private const string url = "https://scholar.google.com";
        private int index;
        private int count;
        private int num;
        private bool isEnd;
        //private bool isError;

        private string[] proxy;
        private int indexProxy;
        private string[] quest;
        private int indexQuest;

        public MainForm()
        {
            InitializeComponent();
            if (ConfigurationManager.AppSettings["proxy"] == "true")
            {
                proxy = GetProxy(DateTime.Today);
            }

            //string responseFromServer = File.ReadAllText("example.htm");
            //ParseGoogleDocument(responseFromServer);

            toolStripStatusLabelResponse.Text = "Ready";
        }
        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (quest == null)
            {
                quest = new string[1] {textBoxRequest.Text};
            }
            if (threadParse != null)
            {
                threadParse.Abort();
                buttonStart.Text = "Start";
            }
            else
            {
                threadParse = new Thread(Parse);
                threadParse.Start();
                buttonStart.Text = "Stop";
            }
        }
        private void checkBoxCount_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCount.Checked)
            {
                labelCount.Enabled = false;
                numericUpDownCount.Enabled = false;
            }
            else
            {
                labelCount.Enabled = true;
                numericUpDownCount.Enabled = true;
            }
        }
        private void toolStripMenuItemSettings_Click(object sender, EventArgs e)
        {
            DatabaseSettingForm databaseSettingForm = new DatabaseSettingForm();
            databaseSettingForm.ShowDialog();
        }
        private void loadCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialogCSV = new OpenFileDialog();
            openFileDialogCSV.Filter = "CSV Files|*.csv";
            openFileDialogCSV.Title = "Select a CSV File";

            if (openFileDialogCSV.ShowDialog() == DialogResult.OK)
            {
                foreach (string filename in openFileDialogCSV.FileNames)
                {
                    StreamReader reader = new StreamReader(File.OpenRead(filename));

                    while (!reader.EndOfStream)
                    {
                        Data data = new Data();
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');
                        string[] date = values[0].Split('-');
                        data.date = new DateTime(int.Parse(date[2]), int.Parse(date[1]), int.Parse(date[0]));
                        data.language = values[1];
                        data.text = values[2];
                        data.files = values[3];
                        data.owners = values[4];
                        data.id_type_table = int.Parse(values[5]);
                        data.id_elibrary = int.Parse(values[6]);
                        data.university = values[7];
                        data.journal = values[8];
                        data.volume_journal = values[9];
                        data.num_journal = values[10];
                        data.year = int.Parse(values[11]);
                        data.pages = values[12];

                        Data[] dat = new Data[1];
                        dat[0] = data;
                        SaveData(dat);
                    }
                }
                
            }
        }

        private void toolStripMenuItemOpenRequests_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialogTXT = new OpenFileDialog();
            openFileDialogTXT.Filter = "TXT Files|*.txt";
            openFileDialogTXT.Title = "Select a TXT File";

            if (openFileDialogTXT.ShowDialog() == DialogResult.OK)
            {
                quest = File.ReadAllLines(openFileDialogTXT.FileName);
                toolStripStatusLabelResponse.Text = "Load";
            }
        }

        private void Parse()
        {
            File.Delete("log.txt");
            File.Delete("log.html");

            toolStripStatusLabelResponse.Text = "";
            
            isEnd = false;
            //isError = false;
            index = 0;
            count = 0;
            num = 0;
            indexProxy = 0;
            indexQuest = 2;
            for (int i = 0; i < quest.Length; i++)
            {
                do
                {
                    GetRequest(GetStringGoogleRequest(index, quest[indexQuest]), proxy[indexProxy]);
                    index++;
                    toolStripStatusLabelResponse.Text = index.ToString() + "/" + num.ToString();
                    count--;
                }
                while ((count > 0 && checkBoxCount.Checked && !isEnd) || (count > 0 && index < numericUpDownCount.Value && !checkBoxCount.Checked && !isEnd));
                indexQuest++;
                count = 0;
                num = 0;
                toolStripStatusLabelResponse.Text = "Success - " + index.ToString() + "/" + num.ToString();
            }
            quest = null;
        }
        private void ParseGoogleDocument(string input)
        {
            HtmlDocument doc = new HtmlDocument();

            doc.LoadHtml(input);
            try
            {
                if (count == 0)
                {
                    HtmlNode bodyNodeCount = doc.DocumentNode.SelectSingleNode("//div[@id='gs_ab_md']");
                    string c = bodyNodeCount.InnerText;
                    string b = "";
                    int l = 0;
                    while (c[l] != '(')
                    {
                        if (Char.IsDigit(c[l]))
                        {
                            b += c[l];
                        }
                        l++;
                    }
                    num = count = int.Parse(b) / 10;
                }
                //запись
                DataResults dataResults = new DataResults();

                HtmlNodeCollection bodyNodeNames = doc.DocumentNode.SelectNodes("//h3[@class='gs_rt']");
                HtmlNodeCollection bodyNodeDatas = doc.DocumentNode.SelectNodes("//div[@class='gs_a']");
                if (bodyNodeNames.Count < 10)
                {
                    isEnd = true;
                }
                if (bodyNodeNames.Count > 0)
                {
                    Data[] data = new Data[bodyNodeNames.Count];
                    DataString[] dat = new DataString[bodyNodeDatas.Count];
                    for (int i = 0; i < bodyNodeNames.Count; i++)
                    {
                        dat[i] = dataResults.GetData(bodyNodeDatas[i].InnerText);

                        data[i].text = dataResults.DeleteShit(bodyNodeNames[i].InnerText);
                        data[i].date = DateTime.Today;
                        data[i].language = Language(bodyNodeNames[i].InnerText);
                        try
                        {
                            data[i].files = dataResults.DeleteShit(bodyNodeNames[i].LastChild.Attributes["href"].Value);
                        }
                        catch
                        {
                            data[i].files = "-";
                        }
                        data[i].id_elibrary = 0;
                        data[i].id_type_table = 0;

                        data[i].owners = dat[i].owners;
                        data[i].university = dat[i].university;
                        data[i].journal = dat[i].journal;
                        data[i].volume_journal = dat[i].volume_journal;
                        data[i].num_journal = dat[i].num_journal;
                        data[i].year = dat[i].year;
                        data[i].pages = dat[i].pages;
                    }
                    //SaveData(data);
                    SaveToCSV(data, Directory.GetCurrentDirectory() + "\\CSV\\" + quest[indexQuest] +".csv");
                }
            }
            catch
            {
                File.WriteAllText("log.html", input, Encoding.Default);
            }
        }
        private void GetCaptchaImage(string input)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(input);
            string captcha = "";
            try
            {
                HtmlNode bodyNode = doc.DocumentNode.SelectSingleNode("//img");
                CaptchaForm captchaForm = new CaptchaForm(url + bodyNode.Attributes["src"].Value);
                captchaForm.ShowDialog();
                captcha = GetStringGoogleRequest(index, textBoxRequest.Text) + "&captcha=" + captchaForm.Captcha + "&submit=Отправить";
                //GetRequest(captcha);
            }
            catch
            {
                File.WriteAllText("log.html", input, Encoding.Default);
            }
        }
        private void GetRequest(string input, string inputProxy)
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.CreateHttp(input);
                request.Credentials = CredentialCache.DefaultCredentials;
                request.ContentType = @"text/html; charset=windows-1251";
                if (ConfigurationManager.AppSettings["proxy"] == "true")
                {
                    request.Proxy = new WebProxy(inputProxy);
                }
                request.Timeout = 5000;

                request.Headers.Add(HttpRequestHeader.AcceptLanguage, @"ru-RU,ru;q=0.9,en;q=0.8");
                WebResponse response = request.GetResponse();

                Stream dataStream = response.GetResponseStream();
                //StreamReader reader = new StreamReader(dataStream, Encoding.Default);
                StreamReader reader = new StreamReader(dataStream, Encoding.GetEncoding("windows-1251"));
                string responseFromServer = reader.ReadToEnd();
                File.WriteAllText("log.html", responseFromServer, Encoding.Default);

                ParseGoogleDocument(responseFromServer);
            }
            catch //(WebException ex)
            {
                toolStripStatusLabelResponse.Text = "Error";
                indexProxy++;
                toolStripStatusLabelProxy.Text = indexProxy.ToString();
                if (indexProxy == proxy.Length)
                {
                    indexProxy = 0;
                }
                if (!isEnd)
                {
                    GetRequest(input, proxy[indexProxy]);
                }
                //isError = true;
                //Stream dataStream = ex.Response.GetResponseStream();
                //StreamReader reader = new StreamReader(dataStream, Encoding.Default);
                //string responseFromServer = reader.ReadToEnd();
                //File.WriteAllText("log.html", responseFromServer, Encoding.Default);

                //GetCaptchaImage(responseFromServer);
            }
        }
        private string GetStringGoogleRequest(int numb, string input)
        {
            string[] arr = input.Split(' ');
            string req = "";
            for (int i = 0; i < arr.Length; i++)
            {
                req += arr[i];
                req += "+";
            }
            req = req.Remove(req.Length - 1);
            return url + "/scholar?start=" + (numb * 10).ToString() + "&q=" + req + "&btnG=";
        }
        private void SaveData(Data[] data)
        {
            /*string conStr = "server=" + ConfigurationManager.AppSettings["server"] + ";user=" +
                                        ConfigurationManager.AppSettings["user"] + ";database=" +
                                        ConfigurationManager.AppSettings["database"] + ";password=" +
                                        ConfigurationManager.AppSettings["password"] + ";";*/
            string conStr = String.Format("Server={0};Port={1};" +
                    "User Id={2};Password={3};Database={4};",
                    ConfigurationManager.AppSettings["server"], "5432", ConfigurationManager.AppSettings["user"],
                    ConfigurationManager.AppSettings["password"], ConfigurationManager.AppSettings["database"]);
            using (MySqlConnection con = new MySqlConnection(conStr))
            {
                try
                {
                    con.Open();

                    for (int i = 0; i < data.Length; i++)
                    {
                        File.AppendAllText("log.txt", data[i].date.ToShortDateString() + "\n" +
                                                      data[i].language + "\n" +
                                                      data[i].text + "\n" +
                                                      data[i].files + "\n" +
                                                      data[i].owners + "\n" +
                                                      data[i].id_type_table.ToString() + "\n" +
                                                      data[i].id_elibrary.ToString() + "\n" +
                                                      data[i].university + "\n" +
                                                      data[i].journal + "\n" +
                                                      data[i].volume_journal + "\n" +
                                                      data[i].num_journal + "\n" +
                                                      data[i].year.ToString() + "\n" + 
                                                      data[i].pages + "\n\n");

                        string sqlResults = "INSERT INTO " + ConfigurationManager.AppSettings["table"] + " (date, language, text, files, owners, id_type_table, id_elibrary, university, journal, volume_journal, num_journal, year, pages, id)" +
                            " VALUES (@Date, @Language, @Text, @Files, @Owners, @Id_type_table, @Id_elibrary, @University, @Journal, @Volume_journal, @Num_journal, @Year, @Pages, null);";
                        MySqlCommand cmdResults = new MySqlCommand(sqlResults, con);
                        //создаем параметры и добавляем их в коллекцию
                        cmdResults.Parameters.AddWithValue("@Date", data[i].date.ToUniversalTime());
                        cmdResults.Parameters.AddWithValue("@Language", data[i].language);
                        cmdResults.Parameters.AddWithValue("@Text", data[i].text);
                        cmdResults.Parameters.AddWithValue("@Files", data[i].files);
                        cmdResults.Parameters.AddWithValue("@Owners", data[i].owners);
                        cmdResults.Parameters.AddWithValue("@Id_type_table", data[i].id_type_table);
                        cmdResults.Parameters.AddWithValue("@Id_elibrary", data[i].id_elibrary);
                        cmdResults.Parameters.AddWithValue("@University", data[i].university);
                        cmdResults.Parameters.AddWithValue("@Journal", data[i].journal);
                        cmdResults.Parameters.AddWithValue("@Volume_journal", data[i].volume_journal);
                        cmdResults.Parameters.AddWithValue("@Num_journal", data[i].num_journal);
                        cmdResults.Parameters.AddWithValue("@Year", data[i].year);
                        cmdResults.Parameters.AddWithValue("@Pages", data[i].pages);
                        cmdResults.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private string[] GetProxy(DateTime date)
        {
            string[] filenames = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Proxy", "*.txt");
            string[][] proxies = new string[filenames.Length][];
            for (int i = 0; i < filenames.Length; i++)
            {
                proxies[i] = File.ReadAllLines(filenames[i]);
            }
            return Array(proxies);
        }
        private string[] Array(string[][] array)
        {
            int length = 0;
            for(int i=0; i<array.Length; i++) {
                length += array[i].Length;
            }
            int count = 0;
            string[] result = new string[length];
            for(int i=0; i<array.Length; i++) {
                for(int j=0; j<array[i].Length; j++) {
                    result[count] = array[i][j];
                    count++;
                }
            }
            return result;
        }
        private string Language(string input)
        {
            Regex regRu = new Regex(@"[А-Я][а-я]");
            Regex regEn = new Regex(@"[A-Z][a-z]");
            MatchCollection matchesRu = regRu.Matches(input);
            MatchCollection matchesEn = regEn.Matches(input);
            if (matchesEn.Count > matchesRu.Count) 
            {
                return "english";
            }
            else
            {
                return "русский";
            }
        }

        private void SaveToCSV(Data[] data, string filename)
        {
            StreamWriter sw = new StreamWriter(filename, true, Encoding.UTF8);
            foreach (Data row in data)
            {
                sw.Write(row.date.ToShortDateString() + ";");
                sw.Write(row.language + ";");
                sw.Write(row.text + ";");
                sw.Write(row.files + ";");
                sw.Write(row.owners + ";");
                sw.Write(row.id_type_table.ToString() + ";");
                sw.Write(row.id_elibrary.ToString() + ";");
                sw.Write(row.university + ";");
                sw.Write(row.journal + ";");
                sw.Write(row.volume_journal + ";");
                sw.Write(row.num_journal + ";");
                sw.Write(row.year.ToString() + ";");
                sw.Write(row.pages);

                sw.Write(sw.NewLine);
            }
            sw.Close();
        }

    }
}
