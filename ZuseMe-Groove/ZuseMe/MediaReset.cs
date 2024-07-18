﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZuseMe.Api;
using static ArnoldVinkCode.AVActions;

namespace ZuseMe
{
    public partial class Media
    {
        private static async Task MediaResetVariables(bool removeNowPlaying, bool resetInterface, bool resetPlayStatus, bool resetMedia, bool resetScrobble)
        {
            try
            {
                //Reset scrobble
                if (resetScrobble)
                {
                    AppVariables.ScrobbleReset = false;
                    AppVariables.ScrobbleSubmitted = false;
                    AppVariables.ScrobbleStatusAccepted = false;
                    AppVariables.ScrobbleStatusMessage = string.Empty;
                    AppVariables.ScrobbleSecondsCurrent = 0;
                }

                //Reset media
                if (resetMedia)
                {
                    AppVariables.MediaSecondsCurrent = 0;
                    AppVariables.MediaSecondsCurrentUnknown = false;
                    AppVariables.MediaSecondsTotal = 60;
                    AppVariables.MediaSecondsTotalUnknown = false;
                    AppVariables.MediaTracknumber = 0;
                    AppVariables.MediaArtist = string.Empty;
                    AppVariables.MediaAlbum = string.Empty;
                    AppVariables.MediaTitle = string.Empty;
                    AppVariables.MediaGenre = string.Empty;
                    AppVariables.MediaPrevious = string.Empty;
                    AppVariables.MediaPlayStatusCurrent = null;
                    AppVariables.MediaPlayType = null;
                    AppVariables.MediaThumbnail = null;
                }

                //Reset playstatus
                if (resetPlayStatus)
                {
                    AppVariables.MediaPlayStatusPrevious = null;
                }

                //Remove now playing
                if (removeNowPlaying)
                {
                    await ApiScrobble.RemoveNowPlaying();
                }

                //Update scrobble window
                if (resetInterface)
                {
                    DispatcherInvoke(delegate
                    {
                        try
                        {
                            //Reset tray
                            AppVariables.AppTray.NotifyIcon.Text = "ZuseMe (Last.fm client)";

                            //Reset main
                            AppVariables.WindowMain.textblock_PlayerDebug.Text = "No media player opened.";
                            AppVariables.WindowMain.textblock_ProgressScrobbleCurrent.Text = "0:00";
                            AppVariables.WindowMain.textblock_ProgressScrobbleTotal.Text = "0:00";
                            AppVariables.WindowMain.textblock_ProgressMediaCurrent.Text = "0:00";
                            AppVariables.WindowMain.textblock_ProgressMediaTotal.Text = "0:00";
                            AppVariables.WindowMain.progress_StatusSong.Value = 0;
                            AppVariables.WindowMain.progress_StatusScrobble.Value = 0;
                            AppVariables.WindowMain.progress_StatusScrobble.Foreground = (SolidColorBrush)Application.Current.Resources["ApplicationAccentLightBrush"];

                            AppVariables.WindowMain.textblock_ScrobbleStatus.Text = "Waiting for song to scrobble.";
                            AppVariables.WindowMain.textblock_TrackArtist.Text = "Artist";
                            AppVariables.WindowMain.textblock_TrackAlbum.Text = "Album";
                            AppVariables.WindowMain.textblock_TrackGenre.Text = "Genre";
                            AppVariables.WindowMain.textblock_TrackTitle.Text = "Title";
                            AppVariables.WindowMain.textblock_TrackNumber.Text = "(0) ";
                            AppVariables.WindowMain.image_PlayStatus.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/UnknownAccent.png"));
                            AppVariables.WindowMain.image_TrackCover.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/ThumbnailDark.png"));

                            //Reset overlay
                            AppVariables.WindowOverlay.textblock_TrackArtist.Text = "Artist";
                            AppVariables.WindowOverlay.textblock_TrackAlbum.Text = "Album";
                            AppVariables.WindowOverlay.textblock_TrackTitle.Text = "Title";
                            AppVariables.WindowOverlay.textblock_TrackNumber.Text = "(0) ";
                            AppVariables.WindowOverlay.image_TrackCover.Source = new BitmapImage(new Uri("pack://application:,,,/ZuseMe;component/Assets/ThumbnailLight.png"));
                        }
                        catch { }
                    });
                }
            }
            catch { }
        }
    }
}