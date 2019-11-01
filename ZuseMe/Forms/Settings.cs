using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ZuseMe.Forms
{
    public partial class Settings : Form
    {
        System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            string ZuseLengthDefault = ConfigurationManager.AppSettings["ZuseLengthDefault"];
            string ZuseUseMusicBrainz = ConfigurationManager.AppSettings["ZuseUseMusicBrainz"];
            string ZuseUseMediaKeys = ConfigurationManager.AppSettings["ZuseUseMediaKeys"];
            string StartZuneStartup = ConfigurationManager.AppSettings["StartZuneStartup"];
            string exitZuneOnExit = ConfigurationManager.AppSettings["exitZuneOnExit"];
            string StartLastfmStartup = ConfigurationManager.AppSettings["StartLastfmStartup"];
            string exitLastfmOnExit = ConfigurationManager.AppSettings["exitLastfmOnExit"];
            string ZuneFilename = ConfigurationManager.AppSettings["ZuneFilename"];
            string version = Application.ProductVersion;

            txt_ZuseLengthDefault.Text = ZuseLengthDefault;
            cb_ZuseUseMusicBrainz.Checked = ZuseUseMusicBrainz == "1";
            cb_ZuseUseMediaKeys.Checked = ZuseUseMediaKeys == "1";
            cb_StartZuneStartup.Checked = StartZuneStartup == "1";
            cb_exitZuneOnExit.Checked = exitZuneOnExit == "1";
            cb_StartLastfmStartup.Checked = StartLastfmStartup == "1";
            cb_exitLastfmOnExit.Checked = exitLastfmOnExit == "1";
            txt_ZuneFilename.Text = ZuneFilename;
            lbl_version.Text = "v" + version;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            int ZuseLengthDefault = Convert.ToInt32(txt_ZuseLengthDefault.Text);
            int ZuseUseMusicBrainz = cb_ZuseUseMusicBrainz.Checked ? 1 : 0;
            int ZuseUseMediaKeys = cb_ZuseUseMediaKeys.Checked ? 1 : 0;
            int StartZuneStartup = cb_StartZuneStartup.Checked ? 1 : 0;
            int exitZuneOnExit = cb_exitZuneOnExit.Checked ? 1 : 0;
            int StartLastfmStartup = cb_StartLastfmStartup.Checked ? 1 : 0;
            int exitLastfmOnExit = cb_exitLastfmOnExit.Checked ? 1 : 0;
            string ZuneFilename = txt_ZuneFilename.Text;

            if (Regex.IsMatch(ZuseLengthDefault.ToString(), @"^[0-9]+$") && ZuseLengthDefault > 34 && Regex.IsMatch(ZuneFilename, @"^[a-zA-Z]+$"))
            {
                config.AppSettings.Settings["ZuseLengthDefault"].Value = ZuseLengthDefault.ToString();
                config.AppSettings.Settings["ZuseUseMusicBrainz"].Value = ZuseUseMusicBrainz.ToString();
                config.AppSettings.Settings["ZuseUseMediaKeys"].Value = ZuseUseMediaKeys.ToString();
                config.AppSettings.Settings["StartZuneStartup"].Value = StartZuneStartup.ToString();
                config.AppSettings.Settings["exitZuneOnExit"].Value = exitZuneOnExit.ToString();
                config.AppSettings.Settings["StartLastfmStartup"].Value = StartLastfmStartup.ToString();
                config.AppSettings.Settings["exitLastfmOnExit"].Value = exitLastfmOnExit.ToString();
                config.AppSettings.Settings["ZuneFilename"].Value = ZuneFilename.ToString();
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
            int ZuseLengthDefault = 70;
            int ZuseUseMusicBrainz = 1;
            int ZuseUseMediaKeys = 0;
            int StartZuneStartup = 0;
            int exitZuneOnExit = 1;
            int StartLastfmStartup = 1;
            int exitLastfmOnExit = 1;
            string ZuneFilename = "c";

            config.AppSettings.Settings["ZuseLengthDefault"].Value = ZuseLengthDefault.ToString();
            config.AppSettings.Settings["ZuseUseMusicBrainz"].Value = ZuseUseMusicBrainz.ToString();
            config.AppSettings.Settings["ZuseUseMediaKeys"].Value = ZuseUseMediaKeys.ToString();
            config.AppSettings.Settings["StartZuneStartup"].Value = StartZuneStartup.ToString();
            config.AppSettings.Settings["exitZuneOnExit"].Value = exitZuneOnExit.ToString();
            config.AppSettings.Settings["StartLastfmStartup"].Value = StartLastfmStartup.ToString();
            config.AppSettings.Settings["exitLastfmOnExit"].Value = exitLastfmOnExit.ToString();
            config.AppSettings.Settings["ZuneFilename"].Value = ZuneFilename.ToString();
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            this.Close();
            MessageBox.Show("The settings are reset to its defaults.", "ZuseMe");
        }
    }
}
