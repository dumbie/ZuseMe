using Microsoft.Win32;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms;

namespace ZuseMe
{
    public class Launcher
    {
        public static void LaunchLastFM()
        {
            try
            {
                if (int.Parse(ConfigurationManager.AppSettings["StartLastfmOnStartup"]) >= 1)
                {
                    RegistryKey localKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64).OpenSubKey(@"SOFTWARE\Last.fm\Client");
                    string lastFMPath = localKey.GetValue("Path").ToString();

                    if (!string.IsNullOrEmpty(lastFMPath))
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.FileName = lastFMPath;
                        startInfo.Arguments = "--tray";
                        Process.Start(startInfo);
                        Console.WriteLine("Last.fm client has been launched.");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Last.fm scrobbler client could not be found,\nplease download and (re)install the Last.fm client v2.1.37.", "ZuseMe");
                return;
            }
        }
    }
}