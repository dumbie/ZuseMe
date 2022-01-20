using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace ZuseMe
{
    class StartupCheck
    {
        public StartupCheck()
        {
            try
            {
                //Check if ZuseMe is running
                if (Process.GetProcessesByName("ZuseMe").Length > 1)
                {
                    MessageBox.Show("ZuseMe is already running.", "ZuseMe");
                    Environment.Exit(1);
                }

                //Check the config file
                if (!File.Exists("ZuseMe.exe.config"))
                {
                    MessageBox.Show("File: ZuseMe.exe.config could not be found.", "ZuseMe");
                    Environment.Exit(1);
                }

                //Close the Last.fm scrobbler
                Launcher.CloseLastFM();
            }
            catch { }
        }
    }
}