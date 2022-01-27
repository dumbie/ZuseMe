using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Windows.Media;
using Windows.Media.Control;
using ZuseMe.Api;
using static ArnoldVinkCode.AVActions;

namespace ZuseMe
{
    public partial class MediaInformation
    {
        public static async Task MediaScrobbleLoop()
        {
            try
            {
                while (TaskCheckLoop(AppTasks.vTask_MonitorScrobble))
                {
                    try
                    {
                        if (AppVariables.MediaPlaybackStatus == null) { continue; }
                        Debug.WriteLine("Media " + AppVariables.MediaPlaybackStatus + " (" + AppVariables.ScrobbleSecondsCurrent + "/" + AppVariables.MediaSecondsTotalCustom + " seconds)" + " (Scrobbled " + AppVariables.ScrobbleSubmitted + ")");

                        //Scrobble song
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
                                    if (AppVariables.MediaPlaybackType == MediaPlaybackType.Music)
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
                        if (AppVariables.MediaPlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
                        {
                            AppVariables.ScrobbleSecondsCurrent++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Failed to update scrobble: " + ex.Message);
                    }
                    finally
                    {
                        await TaskDelayLoop(1000, AppTasks.vTask_MonitorScrobble);
                    }
                }
            }
            catch { }
        }
    }
}