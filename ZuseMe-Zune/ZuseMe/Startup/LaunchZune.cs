using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ZuseMe
{
    public class LaunchZune
    {
        public LaunchZune()
        {
            try
            {
                if (int.Parse(ConfigurationManager.AppSettings["StartZuneStartup"]) >= 1)
                {
                    RegistryKey localKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
                    localKey = localKey.OpenSubKey(@"SOFTWARE\Microsoft\Zune");
                    string ZunePath = localKey.GetValue("Installation Directory").ToString();

                    if (!string.IsNullOrEmpty(ZunePath))
                    {
                        Process.Start(ZunePath + "Zune.exe");
                        Console.WriteLine("Zune software launched.");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Zune software could not be found,\nPlease download and (re)install the Zune software.", "ZuseMe");
            }
        }
    }
}