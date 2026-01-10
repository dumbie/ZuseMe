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
                if (!vSettings.Check("LastFMUsername")) { vSettings.Set("LastFMUsername", string.Empty); }
                if (!vSettings.Check("LastFMAuthToken")) { vSettings.Set("LastFMAuthToken", string.Empty); }
                if (!vSettings.Check("LastFMSessionToken")) { vSettings.Set("LastFMSessionToken", string.Empty); }
                if (!vSettings.Check("TrackLengthCustom")) { vSettings.Set("TrackLengthCustom", "60"); }
                if (!vSettings.Check("TrackPercentageScrobble")) { vSettings.Set("TrackPercentageScrobble", "50"); }
                if (!vSettings.Check("TrackShowOverlay")) { vSettings.Set("TrackShowOverlay", "True"); }
                if (!vSettings.Check("VolumeShowOverlay")) { vSettings.Set("VolumeShowOverlay", "True"); }
                if (!vSettings.Check("LastFMUpdateNowPlaying")) { vSettings.Set("LastFMUpdateNowPlaying", "True"); }
            }
            catch { }
        }
    }
}