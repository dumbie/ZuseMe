using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Windows.Media;
using Windows.Media.Control;
using Windows.Storage.Streams;

namespace ZuseMe
{
    public class MediaInformation
    {
        //MSEdge
        //foobar2000.exe (foo_mediacontrol)
        //SpotifyAB.SpotifyMusic_zpdnekdrzrea0 / Spotify.exe
        public static string[] MediaPlayers = { "Microsoft.ZuneMusic_8wekyb3d8bbwe", "foobar2000.exe" };
        public static int MediaPlayingSeconds = 0;
        public static string MediaPrevious = string.Empty;

        public static async void MediaInformationLoop()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        //Get media manager
                        GlobalSystemMediaTransportControlsSessionManager smtcSessionManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
                        if (smtcSessionManager == null)
                        {
                            Console.WriteLine("No manager session found.");
                            LastFMSend.Stop();
                            continue;
                        }

                        //Get media session
                        IReadOnlyList<GlobalSystemMediaTransportControlsSession> smtcSessions = smtcSessionManager.GetSessions();
                        GlobalSystemMediaTransportControlsSession smtcSessionMedia = smtcSessions.OrderBy(x => MediaPlayers.Any(x.SourceAppUserModelId.Contains)).Where(x => MediaPlayers.Any(x.SourceAppUserModelId.Contains)).FirstOrDefault();
                        if (smtcSessionMedia == null)
                        {
                            Console.WriteLine("No media session found.");
                            LastFMSend.Stop();
                            continue;
                        }

                        //Get media information
                        GlobalSystemMediaTransportControlsSessionTimelineProperties mediaTimeline = smtcSessionMedia.GetTimelineProperties();
                        GlobalSystemMediaTransportControlsSessionMediaProperties mediaProperties = await smtcSessionMedia.TryGetMediaPropertiesAsync();
                        GlobalSystemMediaTransportControlsSessionPlaybackInfo mediaPlayInfo = smtcSessionMedia.GetPlaybackInfo();
                        string sourceApp = smtcSessionMedia.SourceAppUserModelId;

                        //Check media type
                        if (mediaPlayInfo.PlaybackType != MediaPlaybackType.Music)
                        {
                            Console.WriteLine("Other media type playing: " + mediaPlayInfo.PlaybackType);
                            LastFMSend.Stop();
                            continue;
                        }

                        //Check media status
                        string mediaSecondsCurrentString = string.Empty;
                        if (mediaPlayInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
                        {
                            MediaPlayingSeconds++;
                            Console.WriteLine("Media is currently playing for " + MediaPlayingSeconds + " seconds.");
                            mediaSecondsCurrentString = MediaPlayingSeconds.ToString();
                            LastFMSend.Resume();
                        }
                        else if (mediaPlayInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Stopped)
                        {
                            Console.WriteLine("Media is currently stopped.");
                            LastFMSend.Stop();
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("Media is currently: " + mediaPlayInfo.PlaybackStatus);
                            LastFMSend.Pause();
                            continue;
                        }

                        //Load and check media artist
                        string mediaArtist = mediaProperties.Artist;
                        if (string.IsNullOrWhiteSpace(mediaArtist))
                        {
                            mediaArtist = mediaProperties.Subtitle;
                        }
                        if (string.IsNullOrWhiteSpace(mediaArtist))
                        {
                            Console.WriteLine("Unknown media artist.");
                            LastFMSend.Stop();
                            continue;
                        }

                        //Load media title
                        string mediaTitle = mediaProperties.Title;
                        if (string.IsNullOrWhiteSpace(mediaTitle))
                        {
                            Console.WriteLine("Unknown media title.");
                            LastFMSend.Stop();
                            continue;
                        }

                        //Load media album
                        string mediaAlbum = mediaProperties.AlbumTitle;
                        if (string.IsNullOrWhiteSpace(mediaAlbum))
                        {
                            mediaAlbum = "Unknown";
                        }

                        //Load media tracknumber
                        string mediaTracknumber = mediaProperties.TrackNumber.ToString();

                        //Load media duration
                        int mediaSecondsTotalInt = Convert.ToInt32(mediaTimeline.EndTime.TotalSeconds);
                        if (mediaSecondsTotalInt <= 0)
                        {
                            mediaSecondsTotalInt = Convert.ToInt32(ConfigurationManager.AppSettings["TrackLengthDefault"]);
                            Console.WriteLine("Unknown duration using default: " + mediaSecondsTotalInt);
                        }
                        string mediaSecondsTotalString = mediaSecondsTotalInt.ToString();

                        //Check if media changed
                        string mediaCombined = mediaArtist + mediaTitle + mediaAlbum + mediaTracknumber + mediaSecondsTotalString + sourceApp;
                        if (mediaCombined == MediaPrevious)
                        {
                            Console.WriteLine("Media not changed: " + mediaCombined);
                            continue;
                        }

                        //Send media to Last.fm client
                        Console.WriteLine("Media has changed: " + mediaCombined);
                        LastFMSend.Start(mediaArtist, mediaTitle, mediaAlbum, string.Empty, mediaSecondsTotalString, string.Empty);
                        MediaPrevious = mediaCombined;

                        //Load media image bitmap
                        BitmapFrame mediaImageBitmap = await GetMediaThumbnail(mediaProperties.Thumbnail);

                        //Update scrobble window
                        Application.Current.Dispatcher.Invoke(delegate
                        {
                            try
                            {
                                AppVariables.WindowMain.textblock_PlayerDebug.Text = sourceApp;
                                AppVariables.WindowMain.image_TrackCover.Source = mediaImageBitmap;

                                AppVariables.WindowMain.textblock_TrackArtist.Text = mediaArtist;
                                AppVariables.WindowMain.textblock_TrackTitle.Text = mediaTitle;
                                AppVariables.WindowMain.textblock_TrackNumber.Text = "(" + mediaTracknumber + ") ";
                                AppVariables.WindowMain.textblock_TrackAlbum.Text = mediaAlbum;

                                AppVariables.WindowMain.textblock_TrackProgressCurrent.Text = Environment.TickCount.ToString();
                                AppVariables.WindowMain.textblock_TrackProgressTotal.Text = mediaSecondsTotalString;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Failed to update media window: " + ex.Message);
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to read media information: " + ex.Message);
                    }
                    finally
                    {
                        Thread.Sleep(1000);
                    }
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