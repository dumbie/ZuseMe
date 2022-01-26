using System;
using System.IO;
using System.Reflection;

namespace ZuseMe
{
    partial class WindowMain
    {
        void Settings_Load()
        {
            try
            {
                textbox_TrackLengthCustom.Text = Settings.Setting_Load(null, "TrackLengthCustom").ToString();

                //Set the application name to string to check shortcuts
                string targetName = Assembly.GetEntryAssembly().GetName().Name;

                //Check if application is set to launch on Windows startup
                string targetFileStartup = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), targetName + ".url");
                if (File.Exists(targetFileStartup))
                {
                    checkbox_WindowsStartup.IsChecked = true;
                }
            }
            catch { }
        }
    }
}