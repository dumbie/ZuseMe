using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ZuseMe.Api;

namespace ZuseMe
{
    public class AppExit
    {
        public static async Task Exit_Prompt()
        {
            try
            {
                List<string> messageAnswers = new List<string>();
                messageAnswers.Add("Exit application");
                messageAnswers.Add("Cancel");

                string messageResult = AVMessageBox.Popup(AppVariables.WindowMain, "Do you really want to exit ZuseMe?", "This will stop scrobbling songs to Last.fm.", messageAnswers);
                if (messageResult == "Exit application")
                {
                    await Exit();
                }
            }
            catch { }
        }

        public static async Task Exit()
        {
            try
            {
                Debug.WriteLine("Exiting application.");

                //Remove now playing
                await ApiScrobble.RemoveNowPlaying();

                //Hide tray icon
                AppVariables.AppTray.NotifyIcon.Visible = false;

                //Exit application
                Environment.Exit(0);
            }
            catch { }
        }
    }
}