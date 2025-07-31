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
                textbox_TrackLengthCustom.Text = SettingLoad(vConfiguration, "TrackLengthCustom", typeof(string));
                checkbox_TrackShowOverlay.IsChecked = SettingLoad(vConfiguration, "TrackShowOverlay", typeof(bool));
                checkbox_VolumeShowOverlay.IsChecked = SettingLoad(vConfiguration, "VolumeShowOverlay", typeof(bool));
                checkbox_LastFMUpdateNowPlaying.IsChecked = SettingLoad(vConfiguration, "LastFMUpdateNowPlaying", typeof(bool));

                string trackPercentageScrobble = SettingLoad(vConfiguration, "TrackPercentageScrobble", typeof(string));
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
                checkbox_WindowsStartup.IsChecked = AVSettings.StartupShortcutCheck();

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