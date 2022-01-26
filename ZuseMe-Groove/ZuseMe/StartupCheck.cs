using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

namespace ZuseMe
{
    class StartupCheck
    {
        public StartupCheck()
        {
            try
            {
                //Set the working directory to executable directory
                Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

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

                //Check - Application Settings
                Settings.Settings_Check();

                //Close the Last.fm scrobbler
                Launcher.CloseLastFM();
            }
            catch { }
        }
    }
}