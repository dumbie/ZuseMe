using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Windows.Media;
using Windows.Media.Control;
using ZuseMe.Api;
using static ArnoldVinkCode.AVActions;
using static ArnoldVinkCode.AVSettings;

namespace ZuseMe
{
    public partial class Media
    {
        private static async Task MediaStatusCheck(GlobalSystemMediaTransportControlsSessionPlaybackInfo mediaPlayInfo, bool forceUpdate)
        {
            try
            {
                bool mediaTypeValid = mediaPlayInfo.PlaybackType == MediaPlaybackType.Music;
                bool mediaStatusChanged = AppVariables.MediaPlaybackStatusPrevious != mediaPlayInfo.PlaybackStatus;

                //Show media overlay
                if (mediaStatusChanged)
                {
                    if (mediaPlayInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing || mediaPlayInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused)
                    {
                        DispatcherInvoke(delegate
                        {
                            try
                            {
                                AppVariables.WindowOverlay.ShowWindowDuration(3000);
                            }
                            catch { }
                        });
                    }
                }

                if (mediaPlayInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
                {
                    if (!AppVariables.ScrobblePause && mediaTypeValid && (forceUpdate || mediaStatusChanged))
                    {
                        //Update Last.fm now playing
                        bool updateNowPlayingSetting = SettingLoad(null, "LastFMUpdateNowPlaying", typeof(bool));
                        if (updateNowPlayingSetting)
                        {
                            async Task TaskAction()
                            {
                                bool updatedNowPlaying = false;
                                if (AppVariables.MediaSecondsTotalUnknown)
                                {
                                    updatedNowPlaying = await ApiScrobble.UpdateNowPlaying(AppVariables.MediaArtist, AppVariables.MediaTitle, AppVariables.MediaAlbum, string.Empty, AppVariables.MediaTracknumber.ToString());
                                }
                                else
                                {
                                    updatedNowPlaying = await ApiScrobble.UpdateNowPlaying(AppVariables.MediaArtist, AppVariables.MediaTitle, AppVariables.MediaAlbum, AppVariables.MediaSecondsTotal.ToString(), AppVariables.MediaTracknumber.ToString());
                                }

                                //Update scrobble window
                                DispatcherInvoke(delegate
                                {
                                    try
                                    {
                                        if (updatedNowPlaying)
                                        {
                                            AppVariables.WindowMain.image_PlayStatus.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/PlayGreen.png"));
                                        }
                                        else
                                        {
                                            AppVariables.WindowMain.image_PlayStatus.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/PlayOrange.png"));
                                        }
                                    }
                                    catch { }
                                });
                            }
                            AVActions.TaskStartBackground(TaskAction);
                        }
                        else
                        {
                            //Update scrobble window
                            DispatcherInvoke(delegate
                            {
                                try
                                {
                                    AppVariables.WindowMain.image_PlayStatus.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/PlayAccent.png"));
                                }
                                catch { }
                            });
                        }

                        Debug.WriteLine("Media is currently playing.");
                    }
                    else if (!mediaTypeValid && forceUpdate)
                    {
                        //Update Last.fm now playing
                        await ApiScrobble.RemoveNowPlaying();

                        //Update scrobble window
                        DispatcherInvoke(delegate
                        {
                            try
                            {
                                AppVariables.WindowMain.image_PlayStatus.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/PlayOrange.png"));
                            }
                            catch { }
                        });

                        Debug.WriteLine("Invalid media type playing.");
                    }
                }
                else if (mediaPlayInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Stopped)
                {
                    //Update scrobble window
                    DispatcherInvoke(delegate
                    {
                        try
                        {
                            AppVariables.WindowMain.image_PlayStatus.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/StopAccent.png"));
                        }
                        catch { }
                    });

                    if (mediaTypeValid && mediaStatusChanged)
                    {
                        await ApiScrobble.RemoveNowPlaying();
                        Debug.WriteLine("Media is currently stopped.");
                    }
                }
                else if (mediaPlayInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused)
                {
                    //Update scrobble window
                    DispatcherInvoke(delegate
                    {
                        try
                        {
                            AppVariables.WindowMain.image_PlayStatus.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/PauseAccent.png"));
                        }
                        catch { }
                    });

                    if (mediaTypeValid && mediaStatusChanged)
                    {
                        await ApiScrobble.RemoveNowPlaying();
                        Debug.WriteLine("Media is currently paused.");
                    }
                }
                else
                {
                    //Update scrobble window
                    DispatcherInvoke(delegate
                    {
                        try
                        {
                            AppVariables.WindowMain.image_PlayStatus.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/UnknownAccent.png"));
                        }
                        catch { }
                    });

                    if (mediaTypeValid && mediaStatusChanged)
                    {
                        await ApiScrobble.RemoveNowPlaying();
                        Debug.WriteLine("Media is currently: " + mediaPlayInfo.PlaybackStatus);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to check media status: " + ex.Message);
            }
            finally
            {
                AppVariables.MediaForceStatusCheck = false;
                AppVariables.MediaPlaybackStatusPrevious = mediaPlayInfo.PlaybackStatus;
            }
        }
    }
}