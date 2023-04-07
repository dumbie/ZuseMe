using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using static ArnoldVinkCode.ApiGitHub;
using static ArnoldVinkCode.AVFiles;
using static ArnoldVinkCode.AVProcess;

namespace ZuseMe
{
    class AppUpdate
    {
        public static async Task UpdateCheck()
        {
            try
            {
                Debug.WriteLine("Checking application update.");

                //Close running application updater
                if (Close_ProcessesByName("Updater.exe", true))
                {
                    await Task.Delay(1000);
                }

                //Check if the updater has been updated
                File_Move("Resources/UpdaterReplace.exe", "Updater.exe", true);

                //Check for available application update
                await CheckAppUpdate();
            }
            catch { }
        }

        //Check for available application update
        private static async Task CheckAppUpdate()
        {
            try
            {
                Debug.WriteLine("Checking for application update.");
                string onlineVersion = (await ApiGitHub_GetLatestVersion("dumbie", "ZuseMe")).ToLower();
                string currentVersion = "v" + Assembly.GetEntryAssembly().FullName.Split('=')[1].Split(',')[0];
                if (!string.IsNullOrWhiteSpace(onlineVersion) && onlineVersion != currentVersion)
                {
                    List<string> MsgBoxAnswers = new List<string>();
                    MsgBoxAnswers.Add("Update");
                    MsgBoxAnswers.Add("Cancel");

                    string MsgBoxResult = await new AVMessageBox().Popup(null, "A newer version has been found: " + onlineVersion, "Would you like to update the application to the newest version available?", MsgBoxAnswers);
                    if (MsgBoxResult == "Update")
                    {
                        Launch_ShellExecute("Updater.exe", "", "-ProcessLaunch", true);
                        Environment.Exit(0);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed checking for application update: " + ex.Message);
            }
        }
    }
}