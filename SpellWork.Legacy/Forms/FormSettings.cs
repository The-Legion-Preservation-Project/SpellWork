using System;
using System.Configuration;
using System.Windows.Forms;
using SpellWork.Database;
using SpellWork.Properties;

namespace SpellWork.Forms
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        private void CbUseDbConnectCheckedChanged(object sender, EventArgs e)
        {
            _gbDbSetting.Enabled = ((CheckBox)sender).Checked;
            this._bTestConnect.Enabled = _gbDbSetting.Enabled;
        }

        private void BSaveSettingsClick(object sender, EventArgs e)
        {
            ConfigurationManager.AppSettings["Host"] = _tbHost.Text;
            ConfigurationManager.AppSettings["PortOrPipe"] = _tbPort.Text;
            ConfigurationManager.AppSettings["User"] = _tbUser.Text;
            ConfigurationManager.AppSettings["Pass"] = _tbPass.Text;
            ConfigurationManager.AppSettings["WorldDbName"] = _tbBase.Text;
            ConfigurationManager.AppSettings["UseDbConnect"] = _cbUseDBConnect.Checked ? "true" : "false";
            ConfigurationManager.AppSettings["DbcPath"] = _tbPath.Text;
            ConfigurationManager.AppSettings["GtPath"] = _tbGtPath.Text;
            ConfigurationManager.AppSettings["Locale"] = _tbLocale.Text;

            MySqlConnection.TestConnect();

            if (((Button)sender).Text != @"Save")
                if (MySqlConnection.Connected)
                    MessageBox.Show(@"Connection successful!", @"MySQL Connections!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (((Button)sender).Text != @"Save")
                return;

            Settings.Default.Save();
            Close();

            Application.Restart();
        }

        private void SettingsFormLoad(object sender, EventArgs e)
        {
            _tbHost.Text = ConfigurationManager.AppSettings["Host"];
            _tbPort.Text = ConfigurationManager.AppSettings["PortOrPipe"];
            _tbUser.Text = ConfigurationManager.AppSettings["User"];
            _tbPass.Text = ConfigurationManager.AppSettings["Pass"];
            _tbBase.Text = ConfigurationManager.AppSettings["WorldDbName"];
            _gbDbSetting.Enabled = _cbUseDBConnect.Checked = ConfigurationManager.AppSettings["UseDbConnect"].Equals("true");
            _tbPath.Text = ConfigurationManager.AppSettings["DbcPath"];
            _tbGtPath.Text = ConfigurationManager.AppSettings["GtPath"];
            _tbLocale.Text = ConfigurationManager.AppSettings["Locale"];
        }

        private void FormSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void _tbPathClick(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ConfigurationManager.AppSettings["DbcPath"] = folderBrowserDialog1.SelectedPath;
                Settings.Default.Save();
            }
        }
    }
}
