using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ZuseMe
{
    class StartupCheck
    {
        public StartupCheck()
        {
            if (Process.GetProcessesByName("ZuseMe").Length > 1)
            {
                MessageBox.Show("ZuseMe is already running.", "ZuseMe");
                Environment.Exit(1);
            }

            if (!File.Exists("ZuseMe.exe.config"))
            {
                MessageBox.Show("File: ZuseMe.exe.config could not be found.", "ZuseMe");
                Environment.Exit(1);
            }

            if (Process.GetProcessesByName("LastFM").Length == 0 || Process.GetProcessesByName("Last.fm").Length == 0 || Process.GetProcessesByName("Last.fm Scrobbler").Length == 0 || Process.GetProcessesByName("Last.fm Desktop Scrobbler").Length == 0)
            {
                Launcher.LaunchLastFM();
            }
        }
    }
}