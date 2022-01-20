using System;
using System.Configuration;
using System.Windows;
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
            }
            catch { }
        }

        private void button_StopScrobble_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LastFMSend.Stop();
            }
            catch { }
        }

        private async void button_LinkLastFM_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await ApiAuth.AuthLinkLogin();
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