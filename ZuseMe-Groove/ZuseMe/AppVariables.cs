using System.Windows.Threading;
using Windows.Media.Control;
using ZuseMe.Classes;
using ZuseMe.Windows;

namespace ZuseMe
{
    public static class AppVariables
    {
        //Application Windows
        public static WindowMain WindowMain = new WindowMain();
        public static WindowOverlay WindowOverlay = new WindowOverlay();
        public static AppTray AppTray = new AppTray();

        //Player Variables
        public static PlayersJson[] MediaPlayers = null;

        //Scrobble Variables
        public static bool ScrobblePause = false;
        public static bool ScrobbleReset = false;
        public static bool ScrobbleSubmitted = false;
        public static bool ScrobbleStatusAccepted = false;
        public static string ScrobbleStatusMessage = string.Empty;
        public static int ScrobbleSecondsCurrent = 0;

        //Playstatus Variables
        public static GlobalSystemMediaTransportControlsSessionPlaybackStatus? MediaPlaybackStatusPrevious = null;

        //Volume variables
        public static int VolumeLevelPrevious = -1;
        public static bool VolumeMutePrevious = false;

        //Media Variables
        public static bool MediaForceStatusCheck = false;
        public static int MediaSecondsCurrent = 0;
        public static bool MediaSecondsCurrentUnknown = false;
        public static int MediaSecondsTotal = 60;
        public static bool MediaSecondsTotalUnknown = false;
        public static int MediaTracknumber = 0;
        public static string MediaArtist = string.Empty;
        public static string MediaAlbum = string.Empty;
        public static string MediaTitle = string.Empty;
        public static string MediaPrevious = string.Empty;

        //Dispatcher Timers
        public static DispatcherTimer DispatcherTimerOverlay = new DispatcherTimer();

        //Player Variables
        public static GlobalSystemMediaTransportControlsSessionManager SmtcSessionManager = null;
        public static GlobalSystemMediaTransportControlsSession SmtcSessionMedia = null;
        public static string SmtcSessionMediaProcess = string.Empty;
    }
}