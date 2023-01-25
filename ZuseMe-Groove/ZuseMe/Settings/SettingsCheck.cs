using static ArnoldVinkCode.AVSettings;

namespace ZuseMe
{
    public partial class Settings
    {
        //Check - Application Settings
        public static void Settings_Check()
        {
            try
            {
                if (!SettingCheck(null, "LastFMUsername")) { SettingSave(null, "LastFMUsername", string.Empty); }
                if (!SettingCheck(null, "LastFMAuthToken")) { SettingSave(null, "LastFMAuthToken", string.Empty); }
                if (!SettingCheck(null, "LastFMSessionToken")) { SettingSave(null, "LastFMSessionToken", string.Empty); }
                if (!SettingCheck(null, "TrackLengthCustom")) { SettingSave(null, "TrackLengthCustom", "60"); }
                if (!SettingCheck(null, "TrackPercentageScrobble")) { SettingSave(null, "TrackPercentageScrobble", "50"); }
                if (!SettingCheck(null, "TrackShowOverlay")) { SettingSave(null, "TrackShowOverlay", "True"); }
                if (!SettingCheck(null, "VolumeShowOverlay")) { SettingSave(null, "VolumeShowOverlay", "True"); }
                if (!SettingCheck(null, "LastFMUpdateNowPlaying")) { SettingSave(null, "LastFMUpdateNowPlaying", "True"); }
            }
            catch { }
        }
    }
}