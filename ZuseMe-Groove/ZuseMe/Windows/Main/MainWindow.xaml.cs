using ArnoldVinkCode;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using ZuseMe.Api;

namespace ZuseMe
{
    public partial class WindowMain : Window
    {
        public WindowMain()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            try
            {
                //Check api login
                if (string.IsNullOrWhiteSpace(AVSettings.Load(null, "LastFMSessionToken", typeof(string))))
                {
                    stackpanel_Scrobble.Visibility = Visibility.Collapsed;
                    stackpanel_Settings.Visibility = Visibility.Visible;
                    button_ShowScrobble.Visibility = Visibility.Visible;
                    button_ShowSettings.Visibility = Visibility.Collapsed;
                }

                //Update Last.fm username
                UpdateLastFMUsername();

                //Load Save - Application settings
                Settings_Load();
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
                string lastFMUsername = AVSettings.Load(null, "LastFMUsername", typeof(string));
                if (string.IsNullOrWhiteSpace(lastFMUsername))
                {
                    button_OpenProfile.ToolTip = new ToolTip() { Content = "Link profile" };
                    textblock_LoginName.Text = "You are currently not linked to Last.fm.";
                }
                else
                {
                    button_OpenProfile.ToolTip = new ToolTip() { Content = "Open profile " + lastFMUsername };
                    textblock_LoginName.Text = "You are currently linked to: " + lastFMUsername;
                }
            }
            catch { }
        }

        private void button_OpenProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string lastFMUsername = AVSettings.Load(null, "LastFMUsername", typeof(string));
                if (string.IsNullOrWhiteSpace(lastFMUsername))
                {
                    stackpanel_Scrobble.Visibility = Visibility.Collapsed;
                    stackpanel_Settings.Visibility = Visibility.Visible;
                    button_ShowScrobble.Visibility = Visibility.Visible;
                    button_ShowSettings.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Process.Start(ApiVariables.UrlProfile + lastFMUsername);
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

        private void button_ShowScrobble_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                stackpanel_Scrobble.Visibility = Visibility.Visible;
                stackpanel_Settings.Visibility = Visibility.Collapsed;
                button_ShowScrobble.Visibility = Visibility.Collapsed;
                button_ShowSettings.Visibility = Visibility.Visible;
            }
            catch { }
        }

        private void button_ShowSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                stackpanel_Scrobble.Visibility = Visibility.Collapsed;
                stackpanel_Settings.Visibility = Visibility.Visible;
                button_ShowScrobble.Visibility = Visibility.Visible;
                button_ShowSettings.Visibility = Visibility.Collapsed;
            }
            catch { }
        }

        private async void button_PauseResume_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Media.MediaScrobblePauseToggle();
            }
            catch { }
        }
    }
}