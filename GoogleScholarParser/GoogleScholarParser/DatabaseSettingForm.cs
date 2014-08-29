using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Configuration;

namespace GoogleScholarParser
{
    public partial class DatabaseSettingForm : Form
    {
        public DatabaseSettingForm()
        {
            InitializeComponent();
            textBoxServer.Text = ConfigurationManager.AppSettings["server"];
            textBoxUser.Text = ConfigurationManager.AppSettings["user"];
            textBoxDatabase.Text = ConfigurationManager.AppSettings["database"];
            textBoxTable.Text = ConfigurationManager.AppSettings["table"];
            textBoxPassword.Text = ConfigurationManager.AppSettings["password"];
            if (ConfigurationManager.AppSettings["proxy"] == "true")
            {
                checkBoxProxy.CheckState = CheckState.Checked;
            } 
            else
            {
                checkBoxProxy.CheckState = CheckState.Unchecked;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Configuration currentConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            currentConfig.AppSettings.Settings["server"].Value = textBoxServer.Text;
            currentConfig.AppSettings.Settings["user"].Value = textBoxUser.Text;
            currentConfig.AppSettings.Settings["database"].Value = textBoxDatabase.Text;
            currentConfig.AppSettings.Settings["table"].Value = textBoxTable.Text;
            currentConfig.AppSettings.Settings["password"].Value = textBoxPassword.Text;
            if (checkBoxProxy.CheckState == CheckState.Checked)
            {
                currentConfig.AppSettings.Settings["proxy"].Value = "true";
            } 
            else
            {
                currentConfig.AppSettings.Settings["proxy"].Value = "false";
            }
            currentConfig.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
