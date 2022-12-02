using ArnoldVinkCode;

namespace ZuseMe
{
    public partial class Settings
    {
        //Check - Application Settings
        public static void Settings_Check()
        {
            try
            {
                if (!AVSettings.Check(null, "LastFMUsername")) { AVSettings.Save(null, "LastFMUsername", string.Empty); }
                if (!AVSettings.Check(null, "LastFMAuthToken")) { AVSettings.Save(null, "LastFMAuthToken", string.Empty); }
                if (!AVSettings.Check(null, "LastFMSessionToken")) { AVSettings.Save(null, "LastFMSessionToken", string.Empty); }
                if (!AVSettings.Check(null, "TrackLengthCustom")) { AVSettings.Save(null, "TrackLengthCustom", "60"); }
                if (!AVSettings.Check(null, "TrackPercentageScrobble")) { AVSettings.Save(null, "TrackPercentageScrobble", "50"); }
                if (!AVSettings.Check(null, "TrackShowOverlay")) { AVSettings.Save(null, "TrackShowOverlay", "True"); }
                if (!AVSettings.Check(null, "VolumeShowOverlay")) { AVSettings.Save(null, "VolumeShowOverlay", "True"); }
                if (!AVSettings.Check(null, "ControlOverlay")) { AVSettings.Save(null, "ControlOverlay", "True"); }
                if (!AVSettings.Check(null, "LastFMUpdateNowPlaying")) { AVSettings.Save(null, "LastFMUpdateNowPlaying", "True"); }
            }
            catch { }
        }
    }
}