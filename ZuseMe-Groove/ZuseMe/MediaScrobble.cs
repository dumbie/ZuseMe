using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Windows.Media;
using Windows.Media.Control;
using ZuseMe.Api;

namespace ZuseMe
{
    public partial class Media
    {
        public static async Task MediaScrobblePauseToggle()
        {
            try
            {
                if (AppVariables.ScrobblePause)
                {
                    AppVariables.ScrobblePause = false;
                    AppVariables.WindowMain.image_TrackCover.Opacity = 1.00;
                    AppVariables.WindowMain.image_ScrobblePause.Visibility = Visibility.Collapsed;
                    AppVariables.WindowMain.menuButtonStatus.ToolTip = new ToolTip() { Content = "Pause scrobbling" };
                    AppVariables.WindowMain.menuButtonStatusImage.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/PauseLight.png"));
                    AppVariables.WindowMain.menuButtonStatusText.Text = "Pause";
                    AppVariables.AppTray.sysTrayIcon.Icon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("ZuseMe.Assets.ZuseMe.ico"));
                    AppVariables.MediaForceStatusCheck = true;
                }
                else
                {
                    AppVariables.ScrobblePause = true;
                    AppVariables.WindowMain.image_TrackCover.Opacity = 0.50;
                    AppVariables.WindowMain.image_ScrobblePause.Visibility = Visibility.Visible;
                    AppVariables.WindowMain.menuButtonStatus.ToolTip = new ToolTip() { Content = "Resume scrobbling" };
                    AppVariables.WindowMain.menuButtonStatusImage.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/PlayLight.png"));
                    AppVariables.WindowMain.menuButtonStatusText.Text = "Resume";
                    AppVariables.AppTray.sysTrayIcon.Icon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("ZuseMe.Assets.ZuseMeDark.ico"));
                    await ApiScrobble.RemoveNowPlaying();
                }
            }
            catch { }
        }

        public static async Task MediaScrobbleCheck(GlobalSystemMediaTransportControlsSessionPlaybackInfo mediaPlayInfo)
        {
            try
            {
                Debug.WriteLine("Media " + mediaPlayInfo.PlaybackStatus + " (S" + AppVariables.ScrobbleSecondsCurrent + "/M" + AppVariables.MediaSecondsCurrent + "/T" + AppVariables.MediaSecondsTotal + " seconds)" + " (Scrobbled " + AppVariables.ScrobbleSubmitted + ")");

                //Get media progress
                int scrobbleTargetSeconds = AVSettings.Load(null, "TrackPercentageScrobble", typeof(int)) * AppVariables.MediaSecondsTotal / 100;
                int scrobblePercentage = 100 * AppVariables.ScrobbleSecondsCurrent / scrobbleTargetSeconds;
                int mediaPercentage = 100 * AppVariables.MediaSecondsCurrent / AppVariables.MediaSecondsTotal;
                bool mediaTypeValid = mediaPlayInfo.PlaybackType == MediaPlaybackType.Music;

                //Scrobble media to Last.fm
                if (!AppVariables.ScrobblePause && !AppVariables.ScrobbleSubmitted && mediaTypeValid && AppVariables.ScrobbleSecondsCurrent >= scrobbleTargetSeconds)
                {
                    if (AppVariables.MediaSecondsTotalUnknown)
                    {
                        AppVariables.ScrobbleSubmitted = await ApiScrobble.ScrobbleTrack(AppVariables.MediaArtist, AppVariables.MediaTitle, AppVariables.MediaAlbum, string.Empty, AppVariables.MediaTracknumber.ToString());
                    }
                    else
                    {
                        AppVariables.ScrobbleSubmitted = await ApiScrobble.ScrobbleTrack(AppVariables.MediaArtist, AppVariables.MediaTitle, AppVariables.MediaAlbum, AppVariables.MediaSecondsTotal.ToString(), AppVariables.MediaTracknumber.ToString());
                    }
                }

                //Update scrobble window
                AVActions.ActionDispatcherInvoke(delegate
                {
                    try
                    {
                        AppVariables.WindowMain.textblock_ProgressScrobbleCurrent.Text = AVFunctions.SecondsToHms(AppVariables.ScrobbleSecondsCurrent, false);
                        AppVariables.WindowMain.textblock_ProgressScrobbleTotal.Text = AVFunctions.SecondsToHms(scrobbleTargetSeconds, false);

                        string progressTotalString = AVFunctions.SecondsToHms(AppVariables.MediaSecondsTotal, false);
                        if (AppVariables.MediaSecondsTotalUnknown)
                        {
                            progressTotalString += "?";
                        }

                        string progressCurrentString = AVFunctions.SecondsToHms(AppVariables.MediaSecondsCurrent, false);
                        if (AppVariables.MediaSecondsCurrentUnknown)
                        {
                            progressCurrentString += "?";
                        }

                        AppVariables.WindowMain.textblock_ProgressMediaCurrent.Text = progressCurrentString;
                        AppVariables.WindowMain.textblock_ProgressMediaTotal.Text = progressTotalString;

                        AppVariables.WindowMain.progress_StatusSong.Value = mediaPercentage;
                        if (AppVariables.ScrobbleSubmitted)
                        {
                            AppVariables.WindowMain.progress_StatusScrobble.Value = 100;
                            AppVariables.WindowMain.progress_StatusScrobble.Foreground = (SolidColorBrush)Application.Current.Resources["ValidBrush"];
                        }
                        else
                        {
                            AppVariables.WindowMain.progress_StatusScrobble.Value = scrobblePercentage;
                            if (mediaPlayInfo.PlaybackType == MediaPlaybackType.Music)
                            {
                                AppVariables.WindowMain.progress_StatusScrobble.Foreground = (SolidColorBrush)Application.Current.Resources["ApplicationAccentLightBrush"];
                            }
                            else
                            {
                                AppVariables.WindowMain.progress_StatusScrobble.Foreground = (SolidColorBrush)Application.Current.Resources["IgnoredBrush"];
                            }
                        }
                    }
                    catch { }
                });

                //Update current seconds
                if (mediaPlayInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
                {
                    if (!AppVariables.ScrobblePause)
                    {
                        AppVariables.ScrobbleSecondsCurrent++;
                    }
                    if (AppVariables.MediaSecondsCurrentUnknown)
                    {
                        AppVariables.MediaSecondsCurrent++;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to check scrobble: " + ex.Message);
            }
        }
    }
}