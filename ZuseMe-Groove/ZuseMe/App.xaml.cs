using ArnoldVinkCode;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Windows;

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

                //Load supported players
                string jsonFile = File.ReadAllText(@"Players.json");
                AppVariables.MediaPlayers = JsonConvert.DeserializeObject<string[]>(jsonFile).ToArray();

                //Register media session events
                await MediaInformation.RegisterMediaSessionManager();

                //Start monitor scrobble task
                AVActions.TaskStartLoop(MediaInformation.MediaScrobbleLoop, AppTasks.vTask_MonitorScrobble);

                //Check api login
                if (Settings.Setting_Load(null, "LastFMSessionToken").ToString() == string.Empty)
                {
                    AppVariables.WindowMain.Show();
                }
            }
            catch { }
        }
    }
}