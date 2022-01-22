using System.Diagnostics;

namespace ZuseMe
{
    public class Launcher
    {
        public static void CloseLastFM()
        {
            try
            {
                foreach (Process LastFMProc in Process.GetProcessesByName("LastFM"))
                {
                    LastFMProc.Kill();
                }
                foreach (Process LastFMProc2 in Process.GetProcessesByName("Last.fm"))
                {
                    LastFMProc2.Kill();
                }
                foreach (Process LastFMProc3 in Process.GetProcessesByName("Last.fm Scrobbler"))
                {
                    LastFMProc3.Kill();
                }
                foreach (Process LastFMProc4 in Process.GetProcessesByName("Last.fm Desktop Scrobbler"))
                {
                    LastFMProc4.Kill();
                }
            }
            catch
            {
                Debug.WriteLine("Failed to close the Last.fm scrobbler client.");
            }
        }
    }
}