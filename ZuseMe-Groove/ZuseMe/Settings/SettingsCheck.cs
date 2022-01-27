namespace ZuseMe
{
    public partial class Settings
    {
        //Check - Application Settings
        public static void Settings_Check()
        {
            try
            {
                if (Setting_Load(null, "LastFMUsername") == null) { Setting_Save(null, "LastFMUsername", string.Empty); }
                if (Setting_Load(null, "LastFMAuthToken") == null) { Setting_Save(null, "LastFMAuthToken", string.Empty); }
                if (Setting_Load(null, "LastFMSessionToken") == null) { Setting_Save(null, "LastFMSessionToken", string.Empty); }
                if (Setting_Load(null, "TrackLengthCustom") == null) { Setting_Save(null, "TrackLengthCustom", "60"); }
                if (Setting_Load(null, "TrackPercentageScrobble") == null) { Setting_Save(null, "TrackPercentageScrobble", "50"); }
            }
            catch { }
        }
    }
}