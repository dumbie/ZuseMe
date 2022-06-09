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