using ArnoldVinkCode;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Windows.Media;
using Windows.Media.Control;
using ZuseMe.Api;
using static ArnoldVinkCode.AVActions;

namespace ZuseMe
{
    public class MediaScrobble
    {
        public static async Task MediaScrobbleLoop()
        {
            try
            {
                while (TaskCheckLoop(AppTasks.vTask_MonitorScrobble))
                {
                    try
                    {
                        //Scrobble song
                        int scrobbleTarget = AppVariables.MediaSecondsTotal / 2;
                        int scrobblePercentage = 100 * AppVariables.MediaSecondsCurrent / scrobbleTarget;
                        if (!AppVariables.MediaScrobbled && AppVariables.MediaPlaybackType == MediaPlaybackType.Music && AppVariables.MediaSecondsCurrent >= scrobbleTarget)
                        {
                            AppVariables.MediaScrobbled = true;
                            await ApiScrobble.ScrobbleTrack(AppVariables.MediaArtist, AppVariables.MediaTitle, AppVariables.MediaAlbum, AppVariables.MediaSecondsTotal.ToString());
                        }

                        //Update scrobble window
                        AVActions.ActionDispatcherInvoke(delegate
                        {
                            try
                            {
                                if (AppVariables.MediaScrobbled)
                                {
                                    AppVariables.WindowMain.progress_PlayStatus.Value = 100;
                                    AppVariables.WindowMain.progress_PlayStatus.Foreground = new SolidColorBrush((Color)Application.Current.Resources["ValidColor"]);
                                }
                                else
                                {
                                    AppVariables.WindowMain.progress_PlayStatus.Value = scrobblePercentage;
                                    AppVariables.WindowMain.progress_PlayStatus.Foreground = new SolidColorBrush((Color)Application.Current.Resources["ApplicationAccentLightColor"]);
                                }
                            }
                            catch { }
                        });

                        //Update current seconds
                        if (AppVariables.MediaPlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
                        {
                            AppVariables.MediaSecondsCurrent++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to check scrobble media: " + ex.Message);
                    }
                    finally
                    {
                        await AVActions.TaskDelayLoop(1000, AppTasks.vTask_MonitorScrobble);
                    }
                }
            }
            catch { }
        }
    }
}