using ArnoldVinkCode;
using System;
using System.Configuration;
using System.Windows;
using ZuseMe.Api;

namespace ZuseMe
{
    public partial class App : Application
    {
        //Application Startup
        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                //Application startup checks
                StartupCheck StartupCheck = new StartupCheck();

                //Create tray menu
                AppTray TrayMenu = new AppTray();

                //Remove current scrobble
                await ApiScrobble.RemoveNowPlaying();

                //Register media session events
                await MediaInformation.RegisterMediaSessionManager();

                //Start scrobble monitor task
                AVActions.TaskStartLoop(MediaScrobble.MediaScrobbleLoop, AppTasks.vTask_MonitorScrobble);

                //Check api login
                if (Convert.ToString(ConfigurationManager.AppSettings["LastFMSessionToken"]) == string.Empty)
                {
                    AppVariables.WindowMain.Show();
                }
            }
            catch { }
        }
    }
}