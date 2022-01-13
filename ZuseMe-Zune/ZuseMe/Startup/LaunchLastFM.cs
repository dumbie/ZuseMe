using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ZuseMe
{
    public class LaunchLastFM
    {
        public LaunchLastFM()
        {
            try
            {
                if (int.Parse(ConfigurationManager.AppSettings["StartLastfmStartup"]) >= 1)
                {
                    RegistryKey localKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, RegistryView.Registry64);
                    localKey = localKey.OpenSubKey(@"SOFTWARE\Last.fm\Client");
                    string LastFMPath = localKey.GetValue("Path").ToString();

                    if (!string.IsNullOrEmpty(LastFMPath))
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.FileName = LastFMPath;
                        startInfo.Arguments = "--tray";
                        Process.Start(startInfo);
                        Console.WriteLine("Last.fm client launched.");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Last.fm scrobbler client could not be found,\nPlease download and (re)install the Last.fm client.", "ZuseMe");
                return;
            }
        }
    }
}