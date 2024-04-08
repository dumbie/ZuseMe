using static ArnoldVinkCode.AVSettings;
using static ZuseMe.AppVariables;

namespace ZuseMe
{
    public partial class Settings
    {
        //Check - Application Settings
        public static void Settings_Check()
        {
            try
            {
                if (!SettingCheck(vConfiguration, "LastFMUsername")) { SettingSave(vConfiguration, "LastFMUsername", string.Empty); }
                if (!SettingCheck(vConfiguration, "LastFMAuthToken")) { SettingSave(vConfiguration, "LastFMAuthToken", string.Empty); }
                if (!SettingCheck(vConfiguration, "LastFMSessionToken")) { SettingSave(vConfiguration, "LastFMSessionToken", string.Empty); }
                if (!SettingCheck(vConfiguration, "TrackLengthCustom")) { SettingSave(vConfiguration, "TrackLengthCustom", "60"); }
                if (!SettingCheck(vConfiguration, "TrackPercentageScrobble")) { SettingSave(vConfiguration, "TrackPercentageScrobble", "50"); }
                if (!SettingCheck(vConfiguration, "TrackShowOverlay")) { SettingSave(vConfiguration, "TrackShowOverlay", "True"); }
                if (!SettingCheck(vConfiguration, "VolumeShowOverlay")) { SettingSave(vConfiguration, "VolumeShowOverlay", "True"); }
                if (!SettingCheck(vConfiguration, "LastFMUpdateNowPlaying")) { SettingSave(vConfiguration, "LastFMUpdateNowPlaying", "True"); }
            }
            catch { }
        }
    }
}