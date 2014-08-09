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

namespace GoogleScholarParser
{
    public partial class MainForm : Form
    {
        private const string url = "https://scholar.google.com";
        private int index;
        private int count;

        public MainForm()
        {
            InitializeComponent();
            listViewResult.Visible = false;
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            toolStripStatusLabelResponse.Text = "";

            Thread threadParse = new Thread(Parse);
            threadParse.Start();

            File.Delete("log.html");
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

        private void Parse()
        {
            index = 0;
            count = 0;
            do
            {
                try
                {
                    HttpWebRequest request = HttpWebRequest.CreateHttp(GetRequest(index, textBoxRequest.Text));
                    request.Credentials = CredentialCache.DefaultCredentials;
                    //request.UserAgent = i.ToString();
                    request.ContentType = @"text/html; charset=windows-1251";
                    request.Headers.Add(HttpRequestHeader.AcceptLanguage, @"ru-RU,ru;q=0.9,en;q=0.8");
                    WebResponse response = request.GetResponse();
                    toolStripStatusLabelResponse.Text = "Success";

                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream, Encoding.Default);
                    string responseFromServer = reader.ReadToEnd();
                    File.WriteAllText("log.html", responseFromServer, Encoding.Default);
                    
                    ParseHtmlDocument(responseFromServer);
                }
                catch (WebException ex)
                {
                    toolStripStatusLabelResponse.Text = "Error";

                    Stream dataStream = ex.Response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream, Encoding.Default);
                    string responseFromServer = reader.ReadToEnd();
                    GetCaptchaImage(responseFromServer);
                    File.WriteAllText("log.html", responseFromServer, Encoding.Default);
                }
                index++;
                count--;
            }
            while((count > 0 && checkBoxCount.Checked) || (count > 0 && index < numericUpDownCount.Value && !checkBoxCount.Checked));
        }

        private void ParseHtmlDocument(string input)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
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
                    count = int.Parse(b) / 10;
                }
                //запись
                DataResults dataResults = new DataResults();

                HtmlNodeCollection bodyNodeNames = doc.DocumentNode.SelectNodes("//h3[@class='gs_rt']");
                HtmlNodeCollection bodyNodeDatas = doc.DocumentNode.SelectNodes("//div[@class='gs_a']");
                string[] names = new string[bodyNodeNames.Count];
                Data[] data = new Data[bodyNodeDatas.Count];
                for (int i = 0; i < bodyNodeNames.Count; i++)
                {
                    names[i] = bodyNodeNames[i].InnerText;
                    data[i] = dataResults.GetData(bodyNodeDatas[i].InnerText);
                }
                SaveData(names, data);
            }
            catch
            {
                File.WriteAllText("log.html", input, Encoding.Default);
            }
        }

        private void GetCaptchaImage(string input)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(input);
            string captcha = "";
            try
            {
                HtmlNode bodyNode = doc.DocumentNode.SelectSingleNode("//img");
                CaptchaForm captchaForm = new CaptchaForm(url + bodyNode.Attributes["src"].Value);
                captchaForm.ShowDialog();
                captcha = GetRequest(index, textBoxRequest.Text) + "&captcha=" + captchaForm.Captcha + "&submit=Отправить";
            }
            catch
            {
                File.WriteAllText("log.html", input, Encoding.Default);
            }
            ParseCaptchaDocument(captcha);
        }

        private void ParseCaptchaDocument(string input)
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.CreateHttp(input);
                request.Credentials = CredentialCache.DefaultCredentials;
                //request.UserAgent = i.ToString();
                request.ContentType = @"text/html; charset=windows-1251";
                request.Headers.Add(HttpRequestHeader.AcceptLanguage, @"ru-RU,ru;q=0.9,en;q=0.8");
                WebResponse response = request.GetResponse();
                toolStripStatusLabelResponse.Text = "Success Captcha";

                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream, Encoding.Default);
                string responseFromServer = reader.ReadToEnd();
                File.WriteAllText("log.html", responseFromServer, Encoding.Default);
                ParseHtmlDocument(responseFromServer);
            }
            catch (WebException ex)
            {
                toolStripStatusLabelResponse.Text = "Error Captcha";
                Stream dataStream = ex.Response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream, Encoding.Default);
                string responseFromServer = reader.ReadToEnd();
                File.WriteAllText("log.html", responseFromServer, Encoding.Default);
            }
        }

        private string GetRequest(int numb, string input)
        {
            string[] arr = input.Split(' ');
            string req = "";
            for (int i = 0; i < arr.Length; i++)
            {
                req += arr[i];
                req += "+";
            }
            req = req.Remove(req.Length - 1);
            return url + "/scholar?start=" + (numb * 10).ToString() + "&q=" + req + "&btnG";
        }

        private void SaveData(string[] names, Data[] data)
        {
            string conStr = "server=127.0.0.1;user=root;database=google_scholar;password=;";
            using (MySqlConnection con = new MySqlConnection(conStr))
            {
                try
                {
                    con.Open();
                    File.Delete("log.txt");

                    for (int i = 0; i < names.Length; i++)
                    {
                        File.AppendAllText("log.txt", names[i] + "\n" + data[i].autors + "\n" + data[i].year + "\n" + data[i].publishing + "\n\n");
                
                        string sqlResults = "INSERT INTO results_table (id, name, autors, year, publishing) VALUES (null, @Name, @Autors, @Year, @Publishing);";
                        MySqlCommand cmdResults = new MySqlCommand(sqlResults, con);
                        //создаем параметры и добавляем их в коллекцию
                        cmdResults.Parameters.AddWithValue("@Name", names[i]);
                        cmdResults.Parameters.AddWithValue("@Autors", data[i].autors);
                        cmdResults.Parameters.AddWithValue("@Year", data[i].year);
                        cmdResults.Parameters.AddWithValue("@Publishing", data[i].publishing);
                        cmdResults.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
