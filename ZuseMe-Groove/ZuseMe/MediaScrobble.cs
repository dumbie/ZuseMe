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
    public partial class MediaInformation
    {
        public static async Task MediaScrobbleCheck()
        {
            try
            {
                Debug.WriteLine("Media " + AppVariables.MediaPlaybackStatus + " (" + AppVariables.ScrobbleSecondsCurrent + "/" + AppVariables.MediaSecondsTotalCustom + " seconds)" + " (Scrobbled " + AppVariables.ScrobbleSubmitted + ")");

                //Scrobble song
                int scrobbleTarget = AppVariables.MediaSecondsTotalCustom / 2;
                int scrobblePercentage = 100 * AppVariables.ScrobbleSecondsCurrent / scrobbleTarget;
                int songPercentage = 100 * AppVariables.ScrobbleSecondsCurrent / AppVariables.MediaSecondsTotalCustom;
                if (!AppVariables.ScrobbleSubmitted && AppVariables.MediaPlaybackType == MediaPlaybackType.Music && AppVariables.ScrobbleSecondsCurrent >= scrobbleTarget)
                {
                    AppVariables.ScrobbleSubmitted = true;
                    await ApiScrobble.ScrobbleTrack(AppVariables.MediaArtist, AppVariables.MediaTitle, AppVariables.MediaAlbum, AppVariables.MediaSecondsTotalOriginal.ToString(), AppVariables.MediaTracknumber.ToString());
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
                            AppVariables.WindowMain.progress_StatusScrobble.Foreground = (SolidColorBrush)Application.Current.Resources["ApplicationAccentLightBrush"];
                        }
                    }
                    catch { }
                });

                //Update current seconds
                if (AppVariables.MediaPlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
                {
                    AppVariables.ScrobbleSecondsCurrent++;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to update scrobble: " + ex.Message);
            }
        }
    }
}