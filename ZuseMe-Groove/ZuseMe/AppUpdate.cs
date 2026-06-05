using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using static ArnoldVinkCode.AVUpdate;

namespace ZuseMe
{
    public class AppUpdate
    {
        //Check for available application update
        public static async Task UpdateCheckInterface()
        {
            try
            {
                Debug.WriteLine("Checking for application update.");

                UpdateCheckResult updateCheckResult = await UpdateCheck("dumbie", "ZuseMe");
                if (updateCheckResult.UpdateFound)
                {
                    List<string> messageBoxAnswers = new List<string>();
                    messageBoxAnswers.Add("Update");
                    messageBoxAnswers.Add("Cancel");

                    string messageResult = AVMessageBox.Popup(null, "Newer version has been found: " + updateCheckResult.UpdateVersion, "Would you like to update the application to the newest version available?", messageBoxAnswers);
                    if (messageResult == "Update")
                    {
                        AVUpdate.UpdateRestart();
                    }
                }
                else
                {
                    List<string> messageBoxAnswers = new List<string>();
                    messageBoxAnswers.Add("Ok");

                    AVMessageBox.Popup(null, "No new application update has been found", "", messageBoxAnswers);
                }
            }
            catch { }
        }
    }
}