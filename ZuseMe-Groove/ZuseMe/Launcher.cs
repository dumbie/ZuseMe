using Microsoft.Win32;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows;

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
                Console.WriteLine("Failed to close the Last.fm scrobbler client.");
            }
        }

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