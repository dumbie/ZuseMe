using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using ZuseMe.Api;
using static ArnoldVinkCode.AVFirewall;

namespace ZuseMe
{
    class AppStartup
    {
        public static async Task Startup()
        {
            try
            {
                Debug.WriteLine("Welcome to ZuseMe.");

                //Application startup checks
                AppCheck.StartupCheck("ZuseMe", ProcessPriorityClass.Normal);

                //Application update checks
                await AppUpdate.UpdateCheck();

                //Allow application in firewall
                string appFilePath = Assembly.GetEntryAssembly().Location;
                Firewall_ExecutableAllow("ZuseMe", appFilePath, true);

                //Check - Application Settings
                Settings.Settings_Check();

                //Load supported players
                SupportedPlayers.LoadSupportedPlayers();

                //Close the Last.fm scrobbler
                Launcher.CloseLastFM();

                //Start monitor player task
                AVActions.TaskStartLoop(AppTasks.vTaskLoop_MonitorPlayer, AppTasks.vTask_MonitorPlayer);

                //Start monitor media task
                AVActions.TaskStartLoop(AppTasks.vTaskLoop_MonitorMedia, AppTasks.vTask_MonitorMedia);

                //Start monitor volume task
                AVActions.TaskStartLoop(AppTasks.vTaskLoop_MonitorVolume, AppTasks.vTask_MonitorVolume);

                //Check api login and show window
                if (string.IsNullOrWhiteSpace(AVSettings.Load(null, "LastFMSessionToken", typeof(string))))
                {
                    AppVariables.WindowMain.Show();
                }
            }
            catch { }
        }

        public static async Task Exit()
        {
            try
            {
                Debug.WriteLine("Exiting ZuseMe.");

                //Remove now playing
                await ApiScrobble.RemoveNowPlaying();

                //Hide tray icon
                AppVariables.AppTray.sysTrayIcon.Visible = false;

                //Exit application
                Environment.Exit(1);
            }
            catch { }
        }
    }
}