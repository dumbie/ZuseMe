using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Windows.Media.Control;
using Windows.Storage.Streams;
using ZuseMe.Api;

namespace ZuseMe
{
    public static class MediaInformation
    {
        public static async Task RegisterMediaSessionManager()
        {
            try
            {
                AppVariables.SmtcSessionManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
                AppVariables.SmtcSessionManager.SessionsChanged += SmtcSessionManager_SessionsChanged;
                Debug.WriteLine("Changed smtc session manager.");
                SmtcSessionManager_SessionsChanged(null, null);
            }
            catch { }
        }

        private static async void SmtcSessionManager_SessionsChanged(GlobalSystemMediaTransportControlsSessionManager sender, SessionsChangedEventArgs args)
        {
            try
            {
                IReadOnlyList<GlobalSystemMediaTransportControlsSession> smtcSessions = AppVariables.SmtcSessionManager.GetSessions();
                AppVariables.SmtcSessionMedia = smtcSessions.OrderBy(x => AppVariables.MediaPlayers.Any(x.SourceAppUserModelId.Contains)).Where(x => AppVariables.MediaPlayers.Any(x.SourceAppUserModelId.Contains)).FirstOrDefault();
                if (AppVariables.SmtcSessionMedia == null)
                {
                    Debug.WriteLine("No media session found.");
                    await ResetMediaVariables(true, true, true);
                }
                else
                {
                    SmtcSessionMedia_MediaPropertiesChanged(null, null);
                    SmtcSessionMedia_PlaybackInfoChanged(null, null);
                    AppVariables.SmtcSessionMedia.MediaPropertiesChanged += SmtcSessionMedia_MediaPropertiesChanged;
                    AppVariables.SmtcSessionMedia.PlaybackInfoChanged += SmtcSessionMedia_PlaybackInfoChanged;
                    Debug.WriteLine("Changed smtc session media: " + AppVariables.SmtcSessionMedia.SourceAppUserModelId);

                    //Update scrobble window
                    AVActions.ActionDispatcherInvoke(delegate
                    {
                        try
                        {
                            AppVariables.WindowMain.textblock_PlayerDebug.Text = AppVariables.SmtcSessionMedia.SourceAppUserModelId;
                        }
                        catch { }
                    });
                }
            }
            catch { }
        }

        private static async void SmtcSessionMedia_PlaybackInfoChanged(GlobalSystemMediaTransportControlsSession sender, PlaybackInfoChangedEventArgs args)
        {
            try
            {
                //Get media properties
                GlobalSystemMediaTransportControlsSessionPlaybackInfo mediaPlayInfo = AppVariables.SmtcSessionMedia.GetPlaybackInfo();
                AppVariables.MediaPlaybackStatus = mediaPlayInfo.PlaybackStatus;
                AppVariables.MediaPlaybackType = mediaPlayInfo.PlaybackType;

                //Check media status
                if (mediaPlayInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
                {
                    //Update scrobble window
                    AVActions.ActionDispatcherInvoke(delegate
                    {
                        try
                        {
                            AppVariables.WindowMain.image_PlayStatus.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/Play.png"));
                        }
                        catch { }
                    });

                    await Task.Delay(1000); //Wait for media info
                    Debug.WriteLine("Media is currently playing.");
                    await ApiScrobble.UpdateNowPlaying(AppVariables.MediaArtist, AppVariables.MediaTitle, AppVariables.MediaAlbum, AppVariables.MediaSecondsTotal.ToString());
                }
                else if (mediaPlayInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Stopped)
                {
                    //Update scrobble window
                    AVActions.ActionDispatcherInvoke(delegate
                    {
                        try
                        {
                            AppVariables.WindowMain.image_PlayStatus.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/Stop.png"));
                        }
                        catch { }
                    });

                    Debug.WriteLine("Media is currently stopped.");
                    await ApiScrobble.RemoveNowPlaying();
                }
                else if (mediaPlayInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused)
                {
                    //Update scrobble window
                    AVActions.ActionDispatcherInvoke(delegate
                    {
                        try
                        {
                            AppVariables.WindowMain.image_PlayStatus.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/Pause.png"));
                        }
                        catch { }
                    });

                    Debug.WriteLine("Media is currently paused.");
                    await ApiScrobble.RemoveNowPlaying();
                }
                else
                {
                    //Update scrobble window
                    AVActions.ActionDispatcherInvoke(delegate
                    {
                        try
                        {
                            AppVariables.WindowMain.image_PlayStatus.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/Unknown.png"));
                        }
                        catch { }
                    });

                    Debug.WriteLine("Media is currently: " + mediaPlayInfo.PlaybackStatus);
                }
            }
            catch { }
        }

        private static async void SmtcSessionMedia_MediaPropertiesChanged(GlobalSystemMediaTransportControlsSession sender, MediaPropertiesChangedEventArgs args)
        {
            try
            {
                //Reset media progress
                await ResetMediaVariables(false, false, false);

                //Get media properties
                GlobalSystemMediaTransportControlsSessionMediaProperties mediaProperties = await AppVariables.SmtcSessionMedia.TryGetMediaPropertiesAsync();
                GlobalSystemMediaTransportControlsSessionTimelineProperties mediaTimeline = AppVariables.SmtcSessionMedia.GetTimelineProperties();

                //Load media duration
                int mediaSecondsTotalInt = Convert.ToInt32(mediaTimeline.EndTime.TotalSeconds);
                if (mediaSecondsTotalInt <= 0)
                {
                    mediaSecondsTotalInt = Convert.ToInt32(ConfigurationManager.AppSettings["TrackLengthDefault"]);
                    Debug.WriteLine("Unknown duration using default: " + mediaSecondsTotalInt);
                }
                AppVariables.MediaSecondsTotal = mediaSecondsTotalInt;

                //Load and check media artist
                AppVariables.MediaArtist = mediaProperties.Artist;
                if (string.IsNullOrWhiteSpace(AppVariables.MediaArtist))
                {
                    AppVariables.MediaArtist = mediaProperties.Subtitle;
                }
                if (string.IsNullOrWhiteSpace(AppVariables.MediaArtist))
                {
                    Debug.WriteLine("Unknown media artist.");
                    AppVariables.MediaArtist = "Unknown";
                }

                //Load media title
                AppVariables.MediaTitle = mediaProperties.Title;
                if (string.IsNullOrWhiteSpace(AppVariables.MediaTitle))
                {
                    Debug.WriteLine("Unknown media title.");
                    AppVariables.MediaTitle = "Unknown";
                }

                //Load media album
                AppVariables.MediaAlbum = mediaProperties.AlbumTitle;
                if (string.IsNullOrWhiteSpace(AppVariables.MediaAlbum))
                {
                    Debug.WriteLine("Unknown media album.");
                    AppVariables.MediaAlbum = "Unknown";
                }

                //Load media tracknumber
                AppVariables.MediaTracknumber = mediaProperties.TrackNumber;

                //Load media image bitmap
                BitmapFrame MediaImage = await GetMediaThumbnail(mediaProperties.Thumbnail);

                //Update scrobble window
                AVActions.ActionDispatcherInvoke(delegate
                {
                    try
                    {
                        AppVariables.WindowMain.textblock_TrackArtist.Text = AppVariables.MediaArtist;
                        AppVariables.WindowMain.textblock_TrackTitle.Text = AppVariables.MediaTitle;
                        AppVariables.WindowMain.textblock_TrackAlbum.Text = AppVariables.MediaAlbum;

                        if (AppVariables.MediaTracknumber > 0)
                        {
                            AppVariables.WindowMain.textblock_TrackNumber.Text = "(" + AppVariables.MediaTracknumber + ") ";
                        }
                        else
                        {
                            AppVariables.WindowMain.textblock_TrackNumber.Text = string.Empty;
                        }

                        if (MediaImage == null)
                        {
                            AppVariables.WindowMain.image_TrackCover.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/Thumbnail.png"));
                        }
                        else
                        {
                            AppVariables.WindowMain.image_TrackCover.Source = MediaImage;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Failed to update media window: " + ex.Message);
                    }
                });

                //Check media status
                if (AppVariables.MediaPlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
                {
                    Debug.WriteLine("Media is currently playing submitting to last.fm.");
                    await ApiScrobble.UpdateNowPlaying(AppVariables.MediaArtist, AppVariables.MediaTitle, AppVariables.MediaAlbum, AppVariables.MediaSecondsTotal.ToString());
                }
            }
            catch { }
        }

        //Reset media variables
        private static async Task ResetMediaVariables(bool removeNowPlaying, bool resetInterface, bool resetPlayStatus)
        {
            try
            {
                //Reset variables
                if (resetPlayStatus)
                {
                    AppVariables.MediaPlaybackType = null;
                    AppVariables.MediaPlaybackStatus = null;
                }

                AppVariables.MediaScrobbled = false;
                AppVariables.MediaSecondsCurrent = 0;
                AppVariables.MediaSecondsTotal = 60;

                AppVariables.MediaTracknumber = 0;
                AppVariables.MediaArtist = string.Empty;
                AppVariables.MediaAlbum = string.Empty;
                AppVariables.MediaTitle = string.Empty;
                AppVariables.MediaPrevious = string.Empty;

                //Remove now playing
                if (removeNowPlaying)
                {
                    await ApiScrobble.RemoveNowPlaying();
                }

                //Update scrobble window
                if (resetInterface)
                {
                    AVActions.ActionDispatcherInvoke(delegate
                    {
                        try
                        {
                            AppVariables.WindowMain.textblock_PlayerDebug.Text = String.Empty;
                        }
                        catch { }
                    });
                }
            }
            catch { }
        }

        //Update media thumbnail
        public static async Task<BitmapFrame> GetMediaThumbnail(IRandomAccessStreamReference mediaThumbnail)
        {
            try
            {
                if (mediaThumbnail == null) { return null; }
                using (IRandomAccessStreamWithContentType streamReference = await mediaThumbnail.OpenReadAsync())
                {
                    using (Stream stream = streamReference.AsStream())
                    {
                        return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    }
                }
            }
            catch
            {
                //Debug.WriteLine("Failed loading media thumbnail.");
                return null;
            }
        }
    }
}