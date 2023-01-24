using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace ZuseMe
{
    partial class WindowMain
    {
        async Task Settings_Load()
        {
            try
            {
                textbox_TrackLengthCustom.Text = AVSettings.Load(null, "TrackLengthCustom", typeof(string));
                checkbox_TrackShowOverlay.IsChecked = AVSettings.Load(null, "TrackShowOverlay", typeof(bool));
                checkbox_VolumeShowOverlay.IsChecked = AVSettings.Load(null, "VolumeShowOverlay", typeof(bool));
                checkbox_LastFMUpdateNowPlaying.IsChecked = AVSettings.Load(null, "LastFMUpdateNowPlaying", typeof(bool));

                string trackPercentageScrobble = AVSettings.Load(null, "TrackPercentageScrobble", typeof(string));
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

                //Wait for settings to have loaded
                await Task.Delay(1500);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load application settings: " + ex.Message);
            }
        }
    }
}