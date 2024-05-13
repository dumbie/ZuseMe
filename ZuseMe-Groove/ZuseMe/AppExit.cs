using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ZuseMe.Api;

namespace ZuseMe
{
    public class AppExit
    {
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