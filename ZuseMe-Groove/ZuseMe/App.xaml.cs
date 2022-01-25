using ArnoldVinkCode;
using System;
using System.Configuration;
using System.Windows;

namespace ZuseMe
{
    public partial class App : Application
    {
        //Application Startup
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                //Application startup checks
                StartupCheck StartupCheck = new StartupCheck();

                //Create tray menu
                AppTray TrayMenu = new AppTray();

                //Start monitor media task
                AVActions.TaskStartLoop(MediaInformation.MediaInformationLoop, AppTasks.vTask_MonitorMedia);

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