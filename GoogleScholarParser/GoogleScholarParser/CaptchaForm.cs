using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoogleScholarParser
{
    public partial class CaptchaForm : Form
    {
        private string captcha;
        public string Captcha
        {
            get { return captcha; }
        }

        public CaptchaForm(string input)
        {
            InitializeComponent();
            pictureBoxCaptcha.Load(input);
            captcha = "";
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            captcha = textBoxCaptcha.Text;
            this.Close();
        }
    }
}
