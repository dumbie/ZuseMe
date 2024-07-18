using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows.Threading;
using Windows.Media;
using Windows.Media.Control;
using Windows.Storage.Streams;
using ZuseMe.Classes;
using ZuseMe.Windows;
using static ArnoldVinkCode.AVJsonFunctions;
using static ArnoldVinkCode.AVSettings;

namespace ZuseMe
{
    public static class AppVariables
    {
        //Application Variables
        public static Configuration vConfiguration = SettingLoadConfig("ZuseMe.exe.Config");

        //Application Windows
        public static WindowMain WindowMain = new WindowMain();
        public static WindowZune WindowZune = new WindowZune();
        public static WindowOverlay WindowOverlay = new WindowOverlay();
        public static AppTray AppTray = new AppTray();

        //Player Variables
        public static List<PlayersJson> MediaPlayersEnabled = JsonLoadFile<List<PlayersJson>>(@"Profiles\EnabledPlayers.json");
        public static ObservableCollection<PlayersJson> MediaPlayersSupported = JsonLoadFile<ObservableCollection<PlayersJson>>(@"Profiles\SupportedPlayers.json");

        //Zune Variables
        public static string ZuneArtist = string.Empty;
        public static string ZuneAlbum = string.Empty;
        public static string ZuneTitle = string.Empty;

        //Scrobble Variables
        public static bool ScrobblePause = false;
        public static bool ScrobbleReset = false;
        public static bool ScrobbleSubmitted = false;
        public static bool ScrobbleStatusAccepted = false;
        public static string ScrobbleStatusMessage = string.Empty;
        public static int ScrobbleSecondsCurrent = 0;

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
        public static string MediaGenre = string.Empty;
        public static string MediaPrevious = string.Empty;
        public static MediaPlaybackType? MediaPlayType = null;
        public static IRandomAccessStreamReference MediaThumbnail = null;
        public static GlobalSystemMediaTransportControlsSessionPlaybackStatus? MediaPlayStatusCurrent = null;
        public static GlobalSystemMediaTransportControlsSessionPlaybackStatus? MediaPlayStatusPrevious = null;

        //Dispatcher Timers
        public static DispatcherTimer DispatcherTimerOverlay = new DispatcherTimer();

        //Player Variables
        public static GlobalSystemMediaTransportControlsSessionManager SmtcSessionManager = null;
        public static GlobalSystemMediaTransportControlsSession SmtcSessionMedia = null;
        public static string SmtcSessionName = string.Empty;
    }
}