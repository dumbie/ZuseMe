using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ZuseMe.Api;
using static ArnoldVinkCode.AVSettings;
using static ArnoldVinkCode.Styles.MainColors;
using static ZuseMe.AppVariables;

namespace ZuseMe
{
    class AppStartup
    {
        public static async Task Startup()
        {
            try
            {
                Debug.WriteLine("Welcome to ZuseMe.");

                //Application update accent color
                ChangeApplicationAccentColor("#BA0000");

                //Application startup checks
                AppCheck.StartupCheck("ZuseMe", ProcessPriorityClass.Normal);

                //Application update checks
                await AppUpdate.UpdateCheck();

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
                if (string.IsNullOrWhiteSpace(SettingLoad(vConfiguration, "LastFMSessionToken", typeof(string))))
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
                AppVariables.AppTray.NotifyIcon.Visible = false;

                //Exit application
                Environment.Exit(1);
            }
            catch { }
        }
    }
}