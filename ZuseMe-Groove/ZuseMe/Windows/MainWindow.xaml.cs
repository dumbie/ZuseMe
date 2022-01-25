using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using ZuseMe.Api;

namespace ZuseMe
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            try
            {
                //Check api login
                if (Convert.ToString(ConfigurationManager.AppSettings["LastFMSessionToken"]) == string.Empty)
                {
                    stackpanel_Scrobble.Visibility = Visibility.Collapsed;
                    stackpanel_Settings.Visibility = Visibility.Visible;
                    button_ShowScrobble.Visibility = Visibility.Visible;
                    button_ShowSettings.Visibility = Visibility.Collapsed;
                }

                //Update Last.fm username
                UpdateLastFMUsername();
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
                string lastFMUsername = Convert.ToString(ConfigurationManager.AppSettings["LastFMUsername"]);
                if (lastFMUsername == string.Empty)
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
                string lastFMUsername = Convert.ToString(ConfigurationManager.AppSettings["LastFMUsername"]);
                if (lastFMUsername == string.Empty)
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
    }
}