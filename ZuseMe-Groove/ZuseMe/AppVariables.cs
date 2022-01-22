using System.Configuration;
using Windows.Media;
using Windows.Media.Control;

namespace ZuseMe
{
    public static class AppVariables
    {
        //Application Configuration
        public static Configuration ApplicationConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        //Application Windows
        public static MainWindow WindowMain = new MainWindow();

        //Media Variables
        //MSEdge
        //foobar2000.exe (foo_mediacontrol)
        //SpotifyAB.SpotifyMusic_zpdnekdrzrea0 / Spotify.exe
        public static string[] MediaPlayers = { "Microsoft.ZuneMusic_8wekyb3d8bbwe", "foobar2000.exe", "Spotify.exe" };

        public static MediaPlaybackType? MediaPlaybackType = null;
        public static GlobalSystemMediaTransportControlsSessionPlaybackStatus? MediaPlaybackStatus = null;

        public static bool MediaScrobbled = false;
        public static int MediaSecondsCurrent = 0;
        public static int MediaSecondsTotal = 60;

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