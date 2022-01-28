using Windows.Media.Control;

namespace ZuseMe
{
    public static class AppVariables
    {
        //Application Windows
        public static WindowMain WindowMain = new WindowMain();
        public static AppTray AppTray = new AppTray();

        //Player Variables
        public static string[] MediaPlayers = null;

        //Scrobble Variables
        public static bool ScrobbleReset = false;
        public static bool ScrobbleSubmitted = false;
        public static int ScrobbleSecondsCurrent = 0;

        //Playstatus Variables
        public static GlobalSystemMediaTransportControlsSessionPlaybackStatus? MediaPlaybackStatusPrevious = null;

        //Media Variables
        public static int MediaSecondsCurrent = 0;
        public static int MediaSecondsTotalOriginal = 0;
        public static int MediaSecondsTotalCustom = 60;
        public static int MediaTracknumber = 0;
        public static string MediaArtist = string.Empty;
        public static string MediaAlbum = string.Empty;
        public static string MediaTitle = string.Empty;
        public static string MediaPrevious = string.Empty;

        //Smtc Variables
        public static GlobalSystemMediaTransportControlsSessionManager SmtcSessionManager = null;
        public static GlobalSystemMediaTransportControlsSession SmtcSessionMedia = null;
    }
}