using Windows.Media;
using Windows.Media.Control;

namespace ZuseMe
{
    public static class AppVariables
    {
        //Application Windows
        public static WindowMain WindowMain = new WindowMain();
        public static AppTray AppTray = new AppTray();

        //Player Variables
        //MSEdge
        //foobar2000.exe (foo_mediacontrol)
        //SpotifyAB.SpotifyMusic_zpdnekdrzrea0 / Spotify.exe
        public static string[] MediaPlayers = { "Microsoft.ZuneMusic_8wekyb3d8bbwe" };

        //Scrobble Variables
        public static bool ScrobbleSubmitted = false;
        public static int ScrobbleSecondsCurrent = 0;

        //Playstatus Variables
        public static MediaPlaybackType? MediaPlaybackType = null;
        public static GlobalSystemMediaTransportControlsSessionPlaybackStatus? MediaPlaybackStatus = null;

        //Media Variables
        public static int MediaSecondsCurrent = 0;
        public static int MediaSecondsTotalOriginal = 0;
        public static int MediaSecondsTotalCustom = 60;
        public static int MediaTracknumber = 0;
        public static string MediaArtist = string.Empty;
        public static string MediaAlbum = string.Empty;
        public static string MediaTitle = string.Empty;
        public static string MediaPrevious = string.Empty;
        public static long TicksPrevious = 0;

        //Smtc Variables
        public static GlobalSystemMediaTransportControlsSessionManager SmtcSessionManager = null;
        public static GlobalSystemMediaTransportControlsSession SmtcSessionMedia = null;
    }
}