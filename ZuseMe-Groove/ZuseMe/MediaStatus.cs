using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Windows.Media;
using Windows.Media.Control;
using ZuseMe.Api;
using static ArnoldVinkCode.AVActions;

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

                if (mediaPlayInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
                {
                    //Update scrobble window
                    ActionDispatcherInvoke(delegate
                    {
                        try
                        {
                            AppVariables.WindowMain.image_PlayStatus.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/Play.png"));
                        }
                        catch { }
                    });

                    if (!AppVariables.ScrobblePause && mediaTypeValid && (forceUpdate || mediaStatusChanged))
                    {
                        if (AppVariables.MediaSecondsTotalUnknown)
                        {
                            await ApiScrobble.UpdateNowPlaying(AppVariables.MediaArtist, AppVariables.MediaTitle, AppVariables.MediaAlbum, string.Empty, AppVariables.MediaTracknumber.ToString());
                        }
                        else
                        {
                            await ApiScrobble.UpdateNowPlaying(AppVariables.MediaArtist, AppVariables.MediaTitle, AppVariables.MediaAlbum, AppVariables.MediaSecondsTotal.ToString(), AppVariables.MediaTracknumber.ToString());
                        }
                        Debug.WriteLine("Media is currently playing.");
                    }
                    else if (!mediaTypeValid && forceUpdate)
                    {
                        await ApiScrobble.RemoveNowPlaying();
                        Debug.WriteLine("Invalid media type playing.");
                    }
                }
                else if (mediaPlayInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Stopped)
                {
                    //Update scrobble window
                    ActionDispatcherInvoke(delegate
                    {
                        try
                        {
                            AppVariables.WindowMain.image_PlayStatus.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/Stop.png"));
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
                    ActionDispatcherInvoke(delegate
                    {
                        try
                        {
                            AppVariables.WindowMain.image_PlayStatus.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/Pause.png"));
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
                    ActionDispatcherInvoke(delegate
                    {
                        try
                        {
                            AppVariables.WindowMain.image_PlayStatus.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/Unknown.png"));
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
                Debug.WriteLine("Failed to check status: " + ex.Message);
            }
            finally
            {
                AppVariables.MediaForceStatusCheck = false;
                AppVariables.MediaPlaybackStatusPrevious = mediaPlayInfo.PlaybackStatus;
            }
        }
    }
}