using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Windows.Media;
using Windows.Media.Control;
using Windows.Storage.Streams;
using ZuseMe.Api;

namespace ZuseMe
{
    public partial class MediaInformation
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
                //Wait for media update
                await Task.Delay(500);

                IReadOnlyList<GlobalSystemMediaTransportControlsSession> smtcSessions = AppVariables.SmtcSessionManager.GetSessions();
                AppVariables.SmtcSessionMedia = smtcSessions.OrderBy(x => AppVariables.MediaPlayers.Any(x.SourceAppUserModelId.Contains)).Where(x => AppVariables.MediaPlayers.Any(x.SourceAppUserModelId.Contains)).FirstOrDefault();
                if (AppVariables.SmtcSessionMedia == null)
                {
                    Debug.WriteLine("No media session found.");
                    await MediaResetVariables(true, true, true, true, true);
                }
                else
                {
                    SmtcSessionMedia_MediaPropertiesChanged(null, null);
                    SmtcSessionMedia_PlaybackInfoChanged(null, null);
                    SmtcSessionMedia_TimelinePropertiesChanged(null, null);
                    AppVariables.SmtcSessionMedia.MediaPropertiesChanged += SmtcSessionMedia_MediaPropertiesChanged;
                    AppVariables.SmtcSessionMedia.PlaybackInfoChanged += SmtcSessionMedia_PlaybackInfoChanged;
                    AppVariables.SmtcSessionMedia.TimelinePropertiesChanged += SmtcSessionMedia_TimelinePropertiesChanged;
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

        private static async void SmtcSessionMedia_TimelinePropertiesChanged(GlobalSystemMediaTransportControlsSession sender, TimelinePropertiesChangedEventArgs args)
        {
            try
            {
                //Wait for media update
                await Task.Delay(500);

                //Get media properties
                GlobalSystemMediaTransportControlsSessionTimelineProperties mediaTimeline = AppVariables.SmtcSessionMedia.GetTimelineProperties();

                //Load media duration
                AppVariables.MediaSecondsCurrent = Convert.ToInt32(mediaTimeline.Position.TotalSeconds);
            }
            catch { }
        }

        private static async void SmtcSessionMedia_PlaybackInfoChanged(GlobalSystemMediaTransportControlsSession sender, PlaybackInfoChangedEventArgs args)
        {
            try
            {
                //Wait for media update
                await Task.Delay(500);

                //Get media properties
                GlobalSystemMediaTransportControlsSessionPlaybackInfo mediaPlayInfo = AppVariables.SmtcSessionMedia.GetPlaybackInfo();

                //Load media status
                AppVariables.MediaPlaybackStatus = mediaPlayInfo.PlaybackStatus;
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

                    Debug.WriteLine("Media is currently playing.");
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
                //Prevent multiple changes
                long ticksCurrent = Stopwatch.GetTimestamp();
                if (ticksCurrent - AppVariables.TicksPrevious < 50000) { return; }
                AppVariables.TicksPrevious = ticksCurrent;

                //Wait for media update
                await Task.Delay(500);

                //Reset media progress
                await MediaResetVariables(false, false, false, false, true);

                //Get media properties
                GlobalSystemMediaTransportControlsSessionMediaProperties mediaProperties = await AppVariables.SmtcSessionMedia.TryGetMediaPropertiesAsync();
                GlobalSystemMediaTransportControlsSessionTimelineProperties mediaTimeline = AppVariables.SmtcSessionMedia.GetTimelineProperties();
                GlobalSystemMediaTransportControlsSessionPlaybackInfo mediaPlayInfo = AppVariables.SmtcSessionMedia.GetPlaybackInfo();

                //Load media artist
                AppVariables.MediaArtist = mediaProperties.Artist;
                if (string.IsNullOrWhiteSpace(AppVariables.MediaArtist))
                {
                    AppVariables.MediaArtist = mediaProperties.AlbumArtist;
                }

                //Load media title
                AppVariables.MediaTitle = mediaProperties.Title;
                if (string.IsNullOrWhiteSpace(AppVariables.MediaTitle))
                {
                    AppVariables.MediaTitle = mediaProperties.Subtitle;
                }

                //Load media album
                AppVariables.MediaAlbum = mediaProperties.AlbumTitle;

                //Load media tracknumber
                AppVariables.MediaTracknumber = mediaProperties.TrackNumber;
                string mediaTracknumberString = mediaProperties.TrackNumber.ToString();

                //Load media duration
                AppVariables.MediaSecondsCurrent = Convert.ToInt32(mediaTimeline.Position.TotalSeconds);
                AppVariables.MediaSecondsTotalOriginal = Convert.ToInt32(mediaTimeline.EndTime.TotalSeconds);
                if (AppVariables.MediaSecondsTotalOriginal <= 0)
                {
                    AppVariables.MediaSecondsTotalCustom = Convert.ToInt32(Settings.Setting_Load(null, "TrackLengthCustom"));
                    Debug.WriteLine("Unknown duration using custom: " + AppVariables.MediaSecondsTotalCustom + " seconds.");
                }
                else
                {
                    AppVariables.MediaSecondsTotalCustom = AppVariables.MediaSecondsTotalOriginal;
                }
                string mediaDurationString = AppVariables.MediaSecondsTotalOriginal.ToString();

                //Load media type
                AppVariables.MediaPlaybackType = mediaPlayInfo.PlaybackType;

                //Check if media changed
                string mediaCombined = AppVariables.MediaArtist + AppVariables.MediaTitle + AppVariables.MediaAlbum + mediaTracknumberString + mediaDurationString + AppVariables.SmtcSessionMedia.SourceAppUserModelId;
                if (mediaCombined == AppVariables.MediaPrevious)
                {
                    Debug.WriteLine("Media not changed: " + mediaCombined);
                    return;
                }

                //Update now playing
                Debug.WriteLine("Media has changed: " + mediaCombined);
                AppVariables.MediaPrevious = mediaCombined;
                if (AppVariables.MediaPlaybackType == MediaPlaybackType.Music)
                {
                    await ApiScrobble.UpdateNowPlaying(AppVariables.MediaArtist, AppVariables.MediaTitle, AppVariables.MediaAlbum, mediaDurationString, mediaTracknumberString);
                }
                else
                {
                    await ApiScrobble.RemoveNowPlaying();
                }

                //Load media image bitmap
                BitmapFrame mediaImageBitmap = await GetMediaThumbnail(mediaProperties.Thumbnail);

                //Update scrobble window
                AVActions.ActionDispatcherInvoke(delegate
                {
                    try
                    {
                        string mediaArtist = "Unknown";
                        if (!string.IsNullOrWhiteSpace(AppVariables.MediaArtist))
                        {
                            mediaArtist = AppVariables.MediaArtist;
                        }
                        AppVariables.WindowMain.textblock_TrackArtist.Text = mediaArtist;

                        string mediaTitle = "Unknown";
                        if (!string.IsNullOrWhiteSpace(AppVariables.MediaTitle))
                        {
                            mediaTitle = AppVariables.MediaTitle;
                        }
                        AppVariables.WindowMain.textblock_TrackTitle.Text = mediaTitle;

                        string mediaAlbum = "Unknown";
                        if (!string.IsNullOrWhiteSpace(AppVariables.MediaAlbum))
                        {
                            mediaAlbum = AppVariables.MediaAlbum;
                        }
                        AppVariables.WindowMain.textblock_TrackAlbum.Text = mediaAlbum;

                        if (AppVariables.MediaTracknumber > 0)
                        {
                            AppVariables.WindowMain.textblock_TrackNumber.Text = "(" + AppVariables.MediaTracknumber + ") ";
                        }
                        else
                        {
                            AppVariables.WindowMain.textblock_TrackNumber.Text = string.Empty;
                        }

                        if (mediaImageBitmap == null)
                        {
                            AppVariables.WindowMain.image_TrackCover.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/Thumbnail.png"));
                        }
                        else
                        {
                            AppVariables.WindowMain.image_TrackCover.Source = mediaImageBitmap;
                        }

                        //Set tray text
                        string trayText = "ZuseMe (" + mediaArtist + " - " + mediaTitle + ")";
                        if (trayText.Length >= 64)
                        {
                            trayText = AVFunctions.StringCut(trayText, 59, "...)");
                        }
                        AppVariables.AppTray.sysTrayIcon.Text = trayText;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Failed to update media window: " + ex.Message);
                    }
                });
            }
            catch { }
        }

        //Reset media variables
        private static async Task MediaResetVariables(bool removeNowPlaying, bool resetInterface, bool resetPlayStatus, bool resetMedia, bool resetScrobble)
        {
            try
            {
                //Reset scrobble
                if (resetScrobble)
                {
                    AppVariables.ScrobbleSubmitted = false;
                    AppVariables.ScrobbleSecondsCurrent = 0;
                }

                //Reset media
                if (resetMedia)
                {
                    AppVariables.MediaSecondsCurrent = 0;
                    AppVariables.MediaSecondsTotalOriginal = 0;
                    AppVariables.MediaSecondsTotalCustom = 60;
                    AppVariables.MediaTracknumber = 0;
                    AppVariables.MediaArtist = string.Empty;
                    AppVariables.MediaAlbum = string.Empty;
                    AppVariables.MediaTitle = string.Empty;
                    AppVariables.MediaPrevious = string.Empty;
                }

                //Reset playstatus
                if (resetPlayStatus)
                {
                    AppVariables.MediaPlaybackType = null;
                    AppVariables.MediaPlaybackStatus = null;
                }

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
                            AppVariables.AppTray.sysTrayIcon.Text = "ZuseMe (Last.fm client)";

                            AppVariables.WindowMain.textblock_ProgressCurrent.Text = "0:00";
                            AppVariables.WindowMain.textblock_ProgressTotal.Text = "0:00";
                            AppVariables.WindowMain.progress_StatusSong.Value = 0;
                            AppVariables.WindowMain.progress_StatusScrobble.Value = 0;
                            AppVariables.WindowMain.progress_StatusScrobble.Foreground = (SolidColorBrush)Application.Current.Resources["ApplicationAccentLightBrush"];

                            AppVariables.WindowMain.textblock_TrackArtist.Text = "Artist";
                            AppVariables.WindowMain.textblock_TrackAlbum.Text = "Album";
                            AppVariables.WindowMain.textblock_TrackTitle.Text = "Title";
                            AppVariables.WindowMain.textblock_TrackNumber.Text = "(0) ";
                            AppVariables.WindowMain.textblock_PlayerDebug.Text = "No media player opened.";
                            AppVariables.WindowMain.image_PlayStatus.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/Unknown.png"));
                            AppVariables.WindowMain.image_TrackCover.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/Thumbnail.png"));
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