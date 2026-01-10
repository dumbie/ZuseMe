using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static ArnoldVinkCode.AVSettings;
using static ZuseMe.AppVariables;

namespace ZuseMe
{
    partial class WindowMain
    {
        async Task Settings_Load()
        {
            try
            {
                textbox_TrackLengthCustom.Text = vSettings.Load("TrackLengthCustom", typeof(string));
                checkbox_TrackShowOverlay.IsChecked = vSettings.Load("TrackShowOverlay", typeof(bool));
                checkbox_VolumeShowOverlay.IsChecked = vSettings.Load("VolumeShowOverlay", typeof(bool));
                checkbox_LastFMUpdateNowPlaying.IsChecked = vSettings.Load("LastFMUpdateNowPlaying", typeof(bool));

                string trackPercentageScrobble = vSettings.Load("TrackPercentageScrobble", typeof(string));
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
                checkbox_WindowsStartup.IsChecked = AVSettings.StartupShortcutCheck(StartupShortcutType.Startup);

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