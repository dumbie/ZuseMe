using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Windows.Media;
using Windows.Media.Control;
using ZuseMe.Api;

namespace ZuseMe
{
    public partial class Media
    {
        public static async Task MediaScrobbleCheck(GlobalSystemMediaTransportControlsSessionPlaybackInfo mediaPlayInfo)
        {
            try
            {
                Debug.WriteLine("Media " + mediaPlayInfo.PlaybackStatus + " (" + AppVariables.ScrobbleSecondsCurrent + "/" + AppVariables.MediaSecondsTotalCustom + " seconds)" + " (Scrobbled " + AppVariables.ScrobbleSubmitted + ")");

                //Reset scrobble progress
                await MediaResetVariables(false, false, false, false, AppVariables.ScrobbleReset);

                //Get media progress
                int scrobbleTarget = Convert.ToInt32(Settings.Setting_Load(null, "TrackPercentageScrobble")) * AppVariables.MediaSecondsTotalCustom / 100;
                int scrobblePercentage = 100 * AppVariables.ScrobbleSecondsCurrent / scrobbleTarget;
                int songPercentage = 0;
                if (AppVariables.MediaSecondsCurrent != 0)
                {
                    songPercentage = 100 * AppVariables.MediaSecondsCurrent / AppVariables.MediaSecondsTotalCustom;
                }
                else
                {
                    songPercentage = 100 * AppVariables.ScrobbleSecondsCurrent / AppVariables.MediaSecondsTotalCustom;
                }

                //Scrobble media to Last.fm
                if (!AppVariables.ScrobbleSubmitted && mediaPlayInfo.PlaybackType == MediaPlaybackType.Music && AppVariables.ScrobbleSecondsCurrent >= scrobbleTarget)
                {
                    AppVariables.ScrobbleSubmitted = await ApiScrobble.ScrobbleTrack(AppVariables.MediaArtist, AppVariables.MediaTitle, AppVariables.MediaAlbum, AppVariables.MediaSecondsTotalOriginal.ToString(), AppVariables.MediaTracknumber.ToString());
                }

                //Update scrobble window
                AVActions.ActionDispatcherInvoke(delegate
                {
                    try
                    {
                        AppVariables.WindowMain.textblock_ProgressCurrent.Text = AVFunctions.SecondsToHms(AppVariables.ScrobbleSecondsCurrent);
                        string progressTotalString = AVFunctions.SecondsToHms(AppVariables.MediaSecondsTotalCustom);
                        if (AppVariables.MediaSecondsTotalOriginal != AppVariables.MediaSecondsTotalCustom)
                        {
                            progressTotalString += "?";
                        }
                        AppVariables.WindowMain.textblock_ProgressTotal.Text = progressTotalString;

                        AppVariables.WindowMain.progress_StatusSong.Value = songPercentage;
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
                    AppVariables.ScrobbleSecondsCurrent++;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to check scrobble: " + ex.Message);
            }
        }
    }
}