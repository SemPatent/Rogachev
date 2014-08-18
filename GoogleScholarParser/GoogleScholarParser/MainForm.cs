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

namespace GoogleScholarParser
{
    public partial class MainForm : Form
    {
        private const string url = "https://scholar.google.com";
        private int index;
        private int count;
        private int num;
        private bool isError;

        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            toolStripStatusLabelResponse.Text = "";
            isError = false;

            Thread threadParse = new Thread(Parse);
            threadParse.Start();
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

        private void toolStripMenuItemDatabaseSettings_Click(object sender, EventArgs e)
        {
            DatabaseSettingForm databaseSettingForm = new DatabaseSettingForm();
            databaseSettingForm.ShowDialog();
        }

        private void Parse()
        {
            File.Delete("log.txt");
            File.Delete("log.html");
            index = 0;
            count = 0;
            num = 0;
            do
            {
                GetRequest(GetStringGoogleRequest(index, textBoxRequest.Text));
                index++;
                toolStripStatusLabelResponse.Text = index.ToString() + "/" + num.ToString();
                count--;
            }
            while ((count > 0 && checkBoxCount.Checked && !isError) || (count > 0 && index < numericUpDownCount.Value && !checkBoxCount.Checked && !isError));
            if (isError)
            {
                toolStripStatusLabelResponse.Text = "Error - " + index.ToString() + "/" + num.ToString();
            }
            else
            {
                toolStripStatusLabelResponse.Text = "Success - " + index.ToString() + "/" + num.ToString();
            }
        }

        private void ParseGoogleDocument(string input)
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
                captcha = GetStringGoogleRequest(index, textBoxRequest.Text) + "&captcha=" + captchaForm.Captcha + "&submit=Отправить";
                GetRequest(captcha);
            }
            catch
            {
                File.WriteAllText("log.html", input, Encoding.Default);
            }
        }

        private void GetRequest(string input)
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.CreateHttp(input);
                request.Credentials = CredentialCache.DefaultCredentials;
                request.ContentType = @"text/html; charset=windows-1251";
                //request.Proxy = new WebProxy("190.78.79.27:8080");

                request.Headers.Add(HttpRequestHeader.AcceptLanguage, @"ru-RU,ru;q=0.9,en;q=0.8");
                WebResponse response = request.GetResponse();

                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream, Encoding.Default);
                string responseFromServer = reader.ReadToEnd();
                File.WriteAllText("log.html", responseFromServer, Encoding.Default);

                ParseGoogleDocument(responseFromServer);
            }
            catch (WebException ex)
            {
                toolStripStatusLabelResponse.Text = "Error";
                isError = true;
                Stream dataStream = ex.Response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream, Encoding.Default);
                string responseFromServer = reader.ReadToEnd();
                File.WriteAllText("log.html", responseFromServer, Encoding.Default);

                GetCaptchaImage(responseFromServer);
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

        private void SaveData(string[] names, Data[] data)
        {
            string conStr = "server=" + ConfigurationManager.AppSettings["server"] + ";user=" +
                                        ConfigurationManager.AppSettings["user"] + ";database=" +
                                        ConfigurationManager.AppSettings["database"] + ";password=" +
                                        ConfigurationManager.AppSettings["password"] + ";";
            using (MySqlConnection con = new MySqlConnection(conStr))
            {
                try
                {
                    con.Open();

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
