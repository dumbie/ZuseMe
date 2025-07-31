using ArnoldVinkCode;
using ArnoldVinkStyles;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Windows.Media;
using Windows.Media.Control;
using ZuseMe.Api;
using static ArnoldVinkCode.AVSettings;
using static ZuseMe.AppVariables;

namespace ZuseMe
{
    public partial class Media
    {
        private static async Task MediaStatusCheck(bool forceUpdate)
        {
            try
            {
                bool mediaTypeValid = AppVariables.MediaPlayType == MediaPlaybackType.Music;
                bool mediaStatusChanged = AppVariables.MediaPlayStatusPrevious != AppVariables.MediaPlayStatusCurrent;

                //Show media overlay
                if (mediaStatusChanged)
                {
                    if (AppVariables.MediaPlayStatusCurrent == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing || AppVariables.MediaPlayStatusCurrent == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused)
                    {
                        AVDispatcherInvoke.DispatcherInvoke(delegate
                        {
                            try
                            {
                                AppVariables.WindowOverlay.ShowWindowDuration(3000);
                            }
                            catch { }
                        });
                    }
                }

                if (AppVariables.MediaPlayStatusCurrent == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
                {
                    if (!AppVariables.ScrobblePause && mediaTypeValid && (forceUpdate || mediaStatusChanged))
                    {
                        //Update Last.fm now playing
                        bool updateNowPlayingSetting = SettingLoad(vConfiguration, "LastFMUpdateNowPlaying", typeof(bool));
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
                                AVDispatcherInvoke.DispatcherInvoke(delegate
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
                            AVDispatcherInvoke.DispatcherInvoke(delegate
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
                        AVDispatcherInvoke.DispatcherInvoke(delegate
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
                else if (AppVariables.MediaPlayStatusCurrent == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Stopped)
                {
                    //Update scrobble window
                    AVDispatcherInvoke.DispatcherInvoke(delegate
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
                else if (AppVariables.MediaPlayStatusCurrent == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused)
                {
                    //Update scrobble window
                    AVDispatcherInvoke.DispatcherInvoke(delegate
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
                    AVDispatcherInvoke.DispatcherInvoke(delegate
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
                        Debug.WriteLine("Media is currently: " + AppVariables.MediaPlayStatusCurrent);
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
                AppVariables.MediaPlayStatusPrevious = AppVariables.MediaPlayStatusCurrent;
            }
        }
    }
}