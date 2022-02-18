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

                string trackPercentageScrobble = Settings.Setting_Load(null, "TrackPercentageScrobble").ToString();
                if (trackPercentageScrobble == "25")
                {
                    combobox_TrackPercentageScrobble.SelectedIndex = 0;
                }
                else if (trackPercentageScrobble == "50")
                {
                    combobox_TrackPercentageScrobble.SelectedIndex = 1;
                }
                else if (trackPercentageScrobble == "75")
                {
                    combobox_TrackPercentageScrobble.SelectedIndex = 2;
                }
                else if (trackPercentageScrobble == "90")
                {
                    combobox_TrackPercentageScrobble.SelectedIndex = 3;
                }

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