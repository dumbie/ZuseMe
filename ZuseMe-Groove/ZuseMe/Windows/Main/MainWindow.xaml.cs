using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using ZuseMe.Api;
using static ArnoldVinkCode.AVFunctions;
using static ArnoldVinkCode.AVSettings;
using static ZuseMe.AppVariables;

namespace ZuseMe
{
    public partial class WindowMain : Window
    {
        public WindowMain()
        {
            InitializeComponent();
        }

        protected override async void OnSourceInitialized(EventArgs e)
        {
            try
            {
                //Main menu functions
                lb_Menu.PreviewKeyUp += lb_Menu_KeyPressUp;
                lb_Menu.PreviewMouseUp += lb_Menu_MousePressUp;

                //Update Last.fm username
                UpdateLastFMUsername();

                //Load Save - Application settings
                await Settings_Load();
                Settings_Save();

                //Set application version
                string currentVersion = "v" + Assembly.GetEntryAssembly().FullName.Split('=')[1].Split(',')[0];
                textblock_Version.Text = currentVersion + " by Arnold Vink";
            }
            catch { }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                this.Hide();
            }
            catch { }
        }

        public void UpdateLastFMUsername()
        {
            try
            {
                string lastFMUsername = SettingLoad(vConfiguration, "LastFMUsername", typeof(string));
                if (string.IsNullOrWhiteSpace(lastFMUsername))
                {
                    menuButtonProfile.ToolTip = new ToolTip() { Content = "Link Last.fm profile" };
                    textblock_LoginUserStatus.Text = "You are currently not linked to Last.fm.";
                    textblock_LoginUserName.Text = string.Empty;
                }
                else
                {
                    menuButtonProfile.ToolTip = new ToolTip() { Content = "Open profile " + lastFMUsername + " in browser" };
                    textblock_LoginUserStatus.Text = "You are currently linked to profile: ";
                    textblock_LoginUserName.Text = lastFMUsername;
                }
            }
            catch { }
        }

        private void OpenLastFMProfile()
        {
            try
            {
                string lastFMUsername = SettingLoad(vConfiguration, "LastFMUsername", typeof(string));
                if (string.IsNullOrWhiteSpace(lastFMUsername))
                {
                    ShowGridPage(stackpanel_Settings);
                }
                else
                {
                    OpenWebsiteBrowser(ApiVariables.UrlProfile + lastFMUsername);
                }
            }
            catch { }
        }

        private async void button_LinkLastFM_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (button_LinkLastFM.Content.ToString() == "Cancel Last.fm link")
                {
                    await ApiAuth.AuthCancelLogin();
                }
                else
                {
                    await ApiAuth.AuthLinkLogin();
                }
            }
            catch { }
        }

        private void button_UnlinkLastFM_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ApiAuth.AuthUnlinkLogin();
            }
            catch { }
        }

        private async void button_ScrobbleStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Media.MediaScrobblePauseToggle();
            }
            catch { }
        }

        private async void button_FocusPlayer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Media.FocusMediaPlayer();
            }
            catch { }
        }
    }
}