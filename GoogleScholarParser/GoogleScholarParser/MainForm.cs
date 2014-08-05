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
            File.Delete("log.txt");
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
                HtmlNodeCollection bodyNodeNames = doc.DocumentNode.SelectNodes("//h3[@class='gs_rt']");
                HtmlNodeCollection bodyNodeAutors = doc.DocumentNode.SelectNodes("//div[@class='gs_a']");
                string[] names = new string[bodyNodeNames.Count];
                string[] autors = new string[bodyNodeAutors.Count];
                for (int i = 0; i < bodyNodeNames.Count; i++)
                {
                    names[i] = bodyNodeNames[i].InnerText;
                    autors[i] = bodyNodeAutors[i].InnerText;
                    File.AppendAllText("log.txt", names[i] + "\n" + autors[i] + "\n\n");
                }
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
            Captcha(captcha);
        }

        private void Captcha(string input)
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
    }
}
