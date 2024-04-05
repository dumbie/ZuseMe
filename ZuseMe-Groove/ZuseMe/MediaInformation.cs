﻿using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Windows.Media.Control;
using Windows.Storage.Streams;
using static ArnoldVinkCode.AVActions;
using static ArnoldVinkCode.AVSettings;

namespace ZuseMe
{
    public partial class Media
    {
        public static async Task UpdateMediaInformation()
        {
            try
            {
                //Get media properties
                if (AppVariables.SmtcSessionMedia == null) { return; }
                GlobalSystemMediaTransportControlsSessionMediaProperties mediaProperties;
                GlobalSystemMediaTransportControlsSessionTimelineProperties mediaTimeline;
                GlobalSystemMediaTransportControlsSessionPlaybackInfo mediaPlayInfo;
                try
                {
                    mediaProperties = await AppVariables.SmtcSessionMedia.TryGetMediaPropertiesAsync();
                    mediaTimeline = AppVariables.SmtcSessionMedia.GetTimelineProperties();
                    mediaPlayInfo = AppVariables.SmtcSessionMedia.GetPlaybackInfo();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failed to get media properties: " + ex.Message);
                    await ResetMediaPlayerAccess();
                    return;
                }

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

                //Load media genre
                if (mediaProperties.Genres.Any())
                {
                    AppVariables.MediaGenre = string.Join(", ", mediaProperties.Genres);
                }
                else
                {
                    AppVariables.MediaGenre = string.Empty;
                }

                //Load media tracknumber
                AppVariables.MediaTracknumber = mediaProperties.TrackNumber;

                //Load media position
                int mediaPosition = Convert.ToInt32(mediaTimeline.Position.TotalSeconds);
                AppVariables.MediaSecondsCurrent = mediaPosition;
                if (mediaPosition <= 0)
                {
                    AppVariables.MediaSecondsCurrentUnknown = true;
                    //Debug.WriteLine("Unknown position using custom: " + AppVariables.MediaSecondsCurrent + " seconds.");
                }
                else
                {
                    AppVariables.MediaSecondsCurrentUnknown = false;
                }

                //Load media duration
                int mediaDuration = Convert.ToInt32(mediaTimeline.EndTime.TotalSeconds);
                if (mediaDuration <= 0)
                {
                    AppVariables.MediaSecondsTotal = SettingLoad(null, "TrackLengthCustom", typeof(int));
                    AppVariables.MediaSecondsTotalUnknown = true;
                    //Debug.WriteLine("Unknown duration using custom: " + AppVariables.MediaSecondsTotal + " seconds.");
                }
                else
                {
                    AppVariables.MediaSecondsTotal = mediaDuration;
                    AppVariables.MediaSecondsTotalUnknown = false;
                }

                //Check if media changed
                string mediaCombined = AppVariables.MediaArtist + AppVariables.MediaTitle + AppVariables.MediaAlbum + AppVariables.MediaTracknumber.ToString() + AppVariables.MediaSecondsTotal.ToString() + AppVariables.SmtcSessionMedia.SourceAppUserModelId;
                if (mediaCombined == AppVariables.MediaPrevious)
                {
                    Debug.WriteLine("Media not changed: " + mediaCombined);
                    await MediaResetVariables(false, false, false, false, AppVariables.ScrobbleReset);
                    await MediaStatusCheck(mediaPlayInfo, AppVariables.MediaForceStatusCheck);
                    await MediaScrobbleCheck(mediaPlayInfo);
                    return;
                }
                else
                {
                    Debug.WriteLine("Media has changed: " + mediaCombined);
                    await MediaResetVariables(false, false, false, false, true);
                    await MediaStatusCheck(mediaPlayInfo, true);
                    AppVariables.MediaPrevious = mediaCombined;
                }

                //Load media image bitmap
                BitmapFrame mediaImageBitmap = await GetMediaThumbnail(mediaProperties.Thumbnail);

                //Update scrobble and notification window
                DispatcherInvoke(delegate
                {
                    try
                    {
                        string mediaArtist = "Unknown artist";
                        if (!string.IsNullOrWhiteSpace(AppVariables.MediaArtist))
                        {
                            mediaArtist = AppVariables.MediaArtist;
                        }
                        AppVariables.WindowMain.textblock_TrackArtist.Text = mediaArtist;
                        AppVariables.WindowOverlay.textblock_TrackArtist.Text = mediaArtist;

                        string mediaTitle = "Unknown title";
                        if (!string.IsNullOrWhiteSpace(AppVariables.MediaTitle))
                        {
                            mediaTitle = AppVariables.MediaTitle;
                        }
                        AppVariables.WindowMain.textblock_TrackTitle.Text = mediaTitle;
                        AppVariables.WindowOverlay.textblock_TrackTitle.Text = mediaTitle;

                        string mediaAlbum = "Unknown album";
                        if (!string.IsNullOrWhiteSpace(AppVariables.MediaAlbum))
                        {
                            mediaAlbum = AppVariables.MediaAlbum;
                        }
                        AppVariables.WindowMain.textblock_TrackAlbum.Text = mediaAlbum;
                        AppVariables.WindowOverlay.textblock_TrackAlbum.Text = mediaAlbum;

                        string mediaGenre = string.Empty;
                        if (!string.IsNullOrWhiteSpace(AppVariables.MediaGenre))
                        {
                            mediaGenre = AppVariables.MediaGenre;
                        }
                        AppVariables.WindowMain.textblock_TrackGenre.Text = mediaGenre;

                        if (AppVariables.MediaTracknumber > 0)
                        {
                            AppVariables.WindowMain.textblock_TrackNumber.Text = "(" + AppVariables.MediaTracknumber + ") ";
                            AppVariables.WindowOverlay.textblock_TrackNumber.Text = "(" + AppVariables.MediaTracknumber + ") ";
                        }
                        else
                        {
                            AppVariables.WindowMain.textblock_TrackNumber.Text = string.Empty;
                            AppVariables.WindowOverlay.textblock_TrackNumber.Text = string.Empty;
                        }

                        if (mediaImageBitmap == null)
                        {
                            AppVariables.WindowMain.image_TrackCover.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/ThumbnailDark.png"));
                            AppVariables.WindowOverlay.image_TrackCover.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/ThumbnailLight.png"));
                        }
                        else
                        {
                            AppVariables.WindowMain.image_TrackCover.Source = mediaImageBitmap;
                            AppVariables.WindowOverlay.image_TrackCover.Source = mediaImageBitmap;
                        }

                        //Set application tray text
                        string trayText = "ZuseMe (" + mediaArtist + " - " + mediaTitle + ")";
                        AppVariables.AppTray.NotifyIcon.Text = AVFunctions.StringCut(trayText, 59, "...)");

                        //Check overlay setting
                        if (SettingLoad(null, "TrackShowOverlay", typeof(bool)))
                        {
                            //Show media overlay
                            AppVariables.WindowOverlay.ShowWindowDuration(3000);
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
                Debug.WriteLine("Failed to update media information: " + ex.Message);
            }
        }

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