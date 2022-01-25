using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using static ArnoldVinkCode.AVActions;

namespace ZuseMe
{
    public partial class MediaInformation
    {
        public static async Task MediaInformationLoop()
        {
            try
            {
                while (TaskCheckLoop(AppTasks.vTask_MonitorMedia))
                {
                    try
                    {
                        //Get media manager
                        GlobalSystemMediaTransportControlsSessionManager smtcSessionManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
                        if (smtcSessionManager == null)
                        {
                            Debug.WriteLine("No manager session found.");
                            await MediaResetVariables(true, true, true, false, true);
                            continue;
                        }

                        //Get media session
                        IReadOnlyList<GlobalSystemMediaTransportControlsSession> smtcSessions = smtcSessionManager.GetSessions();
                        GlobalSystemMediaTransportControlsSession smtcSessionMedia = smtcSessions.OrderBy(x => AppVariables.MediaPlayers.Any(x.SourceAppUserModelId.Contains)).Where(x => AppVariables.MediaPlayers.Any(x.SourceAppUserModelId.Contains)).FirstOrDefault();
                        if (smtcSessionMedia == null)
                        {
                            Debug.WriteLine("No media session found.");
                            await MediaResetVariables(true, true, true, false, true);
                            continue;
                        }

                        //Get media information
                        GlobalSystemMediaTransportControlsSessionTimelineProperties mediaTimeline = smtcSessionMedia.GetTimelineProperties();
                        GlobalSystemMediaTransportControlsSessionMediaProperties mediaProperties = await smtcSessionMedia.TryGetMediaPropertiesAsync();
                        GlobalSystemMediaTransportControlsSessionPlaybackInfo mediaPlayInfo = smtcSessionMedia.GetPlaybackInfo();
                        string sourceApp = smtcSessionMedia.SourceAppUserModelId;

                        //Check media type
                        AppVariables.MediaPlaybackType = mediaPlayInfo.PlaybackType;
                        if (AppVariables.MediaPlaybackType != MediaPlaybackType.Music)
                        {
                            Debug.WriteLine("Other media type playing: " + AppVariables.MediaPlaybackType);
                            await MediaResetVariables(true, true, true, false, true);
                            continue;
                        }

                        //Check media status
                        AppVariables.MediaPlaybackStatus = mediaPlayInfo.PlaybackStatus;
                        if (AppVariables.MediaPlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
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
                        }
                        else if (AppVariables.MediaPlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Stopped)
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
                        }
                        else if (AppVariables.MediaPlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused)
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
                        }

                        //Load media artist
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
                        string mediaTracknumberString = mediaProperties.TrackNumber.ToString();

                        //Load media duration
                        AppVariables.MediaSecondsTotalOriginal = Convert.ToInt32(mediaTimeline.EndTime.TotalSeconds);
                        if (AppVariables.MediaSecondsTotalOriginal <= 0)
                        {
                            AppVariables.MediaSecondsTotalCustom = Convert.ToInt32(ConfigurationManager.AppSettings["TrackLengthCustom"]);
                            Debug.WriteLine("Unknown duration using custom: " + AppVariables.MediaSecondsTotalCustom + " seconds.");
                        }
                        else
                        {
                            AppVariables.MediaSecondsTotalCustom = AppVariables.MediaSecondsTotalOriginal;
                        }
                        string mediaSecondsTotalCustomString = AppVariables.MediaSecondsTotalCustom.ToString();
                        string mediaSecondsTotalOriginalString = AppVariables.MediaSecondsTotalOriginal.ToString();

                        //Check if media changed
                        string mediaCombined = AppVariables.MediaArtist + AppVariables.MediaTitle + AppVariables.MediaAlbum + mediaTracknumberString + mediaSecondsTotalCustomString + sourceApp;
                        if (mediaCombined == AppVariables.MediaPrevious)
                        {
                            Debug.WriteLine("Media not changed: " + mediaCombined);
                            await MediaScrobbleCheck();
                            continue;
                        }

                        //Update current media
                        Debug.WriteLine("Media has changed: " + mediaCombined);
                        AppVariables.MediaPrevious = mediaCombined;
                        await MediaResetVariables(false, false, false, true, false);
                        await ApiScrobble.UpdateNowPlaying(AppVariables.MediaArtist, AppVariables.MediaTitle, AppVariables.MediaAlbum, mediaSecondsTotalOriginalString, mediaTracknumberString);

                        //Load media image bitmap
                        BitmapFrame mediaImageBitmap = await GetMediaThumbnail(mediaProperties.Thumbnail);

                        //Update scrobble window
                        Application.Current.Dispatcher.Invoke(delegate
                        {
                            try
                            {
                                AppVariables.WindowMain.textblock_PlayerDebug.Text = sourceApp;
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

                                if (mediaImageBitmap == null)
                                {
                                    AppVariables.WindowMain.image_TrackCover.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/Thumbnail.png"));
                                }
                                else
                                {
                                    AppVariables.WindowMain.image_TrackCover.Source = mediaImageBitmap;
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Failed to update media window: " + ex.Message);
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Failed to update media: " + ex.Message);
                    }
                    finally
                    {
                        await TaskDelayLoop(1000, AppTasks.vTask_MonitorMedia);
                    }
                }
            }
            catch { }
        }

        //Reset media variables
        private static async Task MediaResetVariables(bool removeNowPlaying, bool resetInterface, bool resetPlayStatus, bool resetScrobble, bool resetMedia)
        {
            try
            {
                //Reset scrobble
                if (resetScrobble)
                {
                    AppVariables.ScrobbleRemoved = false;
                    AppVariables.ScrobbleSubmitted = false;
                    AppVariables.ScrobbleSecondsCurrent = 0;
                }

                //Reset media
                if (resetMedia)
                {
                    AppVariables.MediaSecondsTotalCustom = 60;
                    AppVariables.MediaTracknumber = 0;
                    AppVariables.MediaArtist = string.Empty;
                    AppVariables.MediaAlbum = string.Empty;
                    AppVariables.MediaTitle = string.Empty;
                    AppVariables.MediaPrevious = string.Empty;
                }

                //Reset play status
                if (resetPlayStatus)
                {
                    AppVariables.MediaPlaybackType = null;
                    AppVariables.MediaPlaybackStatus = null;
                }

                //Remove now playing
                if (removeNowPlaying && !AppVariables.ScrobbleRemoved)
                {
                    await ApiScrobble.RemoveNowPlaying();
                    AppVariables.ScrobbleRemoved = true;
                }

                //Update scrobble window
                if (resetInterface)
                {
                    AVActions.ActionDispatcherInvoke(delegate
                    {
                        try
                        {
                            AppVariables.WindowMain.textblock_ProgressCurrent.Text = "0:00";
                            AppVariables.WindowMain.textblock_ProgressTotal.Text = "0:00";
                            AppVariables.WindowMain.progress_StatusSong.Value = 0;
                            AppVariables.WindowMain.progress_StatusScrobble.Value = 0;
                            AppVariables.WindowMain.progress_StatusScrobble.Foreground = (SolidColorBrush)Application.Current.Resources["ApplicationAccentLightBrush"];

                            AppVariables.WindowMain.textblock_TrackArtist.Text = "Artist";
                            AppVariables.WindowMain.textblock_TrackAlbum.Text = "Album";
                            AppVariables.WindowMain.textblock_TrackTitle.Text = "Title";
                            AppVariables.WindowMain.textblock_TrackNumber.Text = "(0) ";
                            AppVariables.WindowMain.textblock_PlayerDebug.Text = string.Empty;
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