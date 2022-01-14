using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ZuseMe.Forms
{
    public partial class Settings : Form
    {
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            string TrackLengthDefault = ConfigurationManager.AppSettings["TrackLengthDefault"];
            string StartLastfmOnStartup = ConfigurationManager.AppSettings["StartLastfmOnStartup"];
            string CloseLastfmOnExit = ConfigurationManager.AppSettings["CloseLastfmOnExit"];
            string version = Application.ProductVersion;

            txt_TrackLengthDefault.Text = TrackLengthDefault;
            cb_StartLastfmOnStartup.Checked = StartLastfmOnStartup == "1";
            cb_CloseLastfmOnExit.Checked = CloseLastfmOnExit == "1";
            lbl_version.Text = "v" + version;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            int TrackLengthDefault = Convert.ToInt32(txt_TrackLengthDefault.Text);
            int StartLastfmOnStartup = cb_StartLastfmOnStartup.Checked ? 1 : 0;
            int CloseLastfmOnExit = cb_CloseLastfmOnExit.Checked ? 1 : 0;

            if (Regex.IsMatch(TrackLengthDefault.ToString(), @"^[0-9]+$") && TrackLengthDefault > 34)
            {
                config.AppSettings.Settings["TrackLengthDefault"].Value = TrackLengthDefault.ToString();
                config.AppSettings.Settings["StartLastfmOnStartup"].Value = StartLastfmOnStartup.ToString();
                config.AppSettings.Settings["CloseLastfmOnExit"].Value = CloseLastfmOnExit.ToString();
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                this.Close();
                //MessageBox.Show("Your settings have been saved.", "ZuseMe");
            }
            else
            {
                MessageBox.Show("Please enter a valid length or drive location,\nMinimum unknown track length is 35 seconds.", "ZuseMe");
            }
        }

        private void btn_Reset_Click(object sender, EventArgs e)
        {
            int TrackLengthDefault = 60;
            int StartLastfmOnStartup = 1;
            int CloseLastfmOnExit = 1;

            config.AppSettings.Settings["TrackLengthDefault"].Value = TrackLengthDefault.ToString();
            config.AppSettings.Settings["StartLastfmOnStartup"].Value = StartLastfmOnStartup.ToString();
            config.AppSettings.Settings["CloseLastfmOnExit"].Value = CloseLastfmOnExit.ToString();
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            this.Close();
            MessageBox.Show("Settings are reset to it's defaults.", "ZuseMe");
        }
    }
}