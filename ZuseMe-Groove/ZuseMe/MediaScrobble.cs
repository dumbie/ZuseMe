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
                    AppVariables.WindowMain.image_ScrobblePause.Visibility = Visibility.Collapsed;
                    AppVariables.WindowMain.button_PauseResume.ToolTip = new ToolTip() { Content = "Pause scrobbling" };
                    AppVariables.WindowMain.image_PauseResume.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/PauseDark.png"));
                    AppVariables.AppTray.sysTrayIcon.Icon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("ZuseMe.Assets.ZuseMe.ico"));
                    AppVariables.MediaForceStatusCheck = true;
                }
                else
                {
                    AppVariables.ScrobblePause = true;
                    AppVariables.WindowMain.image_ScrobblePause.Visibility = Visibility.Visible;
                    AppVariables.WindowMain.button_PauseResume.ToolTip = new ToolTip() { Content = "Resume scrobbling" };
                    AppVariables.WindowMain.image_PauseResume.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/PlayDark.png"));
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
                int scrobbleTargetSeconds = Convert.ToInt32(Settings.Setting_Load(null, "TrackPercentageScrobble")) * AppVariables.MediaSecondsTotal / 100;
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
                        AppVariables.WindowMain.textblock_ProgressCurrent.Text = AVFunctions.SecondsToHms(AppVariables.ScrobbleSecondsCurrent);
                        string progressTotalString = AVFunctions.SecondsToHms(scrobbleTargetSeconds) + "/" + AVFunctions.SecondsToHms(AppVariables.MediaSecondsTotal);
                        if (AppVariables.MediaSecondsTotalUnknown)
                        {
                            progressTotalString += "?";
                        }
                        AppVariables.WindowMain.textblock_ProgressTotal.Text = progressTotalString;

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