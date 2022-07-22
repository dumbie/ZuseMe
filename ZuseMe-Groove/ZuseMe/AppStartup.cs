using ArnoldVinkCode;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

                //Close the Last.fm scrobbler
                Launcher.CloseLastFM();

                //Load supported players
                string jsonFile = File.ReadAllText(@"Players.json");
                AppVariables.MediaPlayers = JsonConvert.DeserializeObject<string[]>(jsonFile).ToArray();

                //Register media session events
                await Media.RegisterMediaSessionEvents();

                //Start monitor information task
                AVActions.TaskStartLoop(Media.MediaInformationLoop, AppTasks.vTask_MonitorInformation);

                //Check api login and show window
                if (string.IsNullOrWhiteSpace(AVSettings.Load(null, "LastFMSessionToken", typeof(string))))
                {
                    AppVariables.WindowMain.Show();
                }
            }
            catch { }
        }
    }
}