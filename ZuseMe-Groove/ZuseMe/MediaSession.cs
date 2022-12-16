using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media.Control;
using static ArnoldVinkCode.AVActions;
using static ArnoldVinkCode.AVInteropDll;
using static ArnoldVinkCode.ProcessClasses;
using static ArnoldVinkCode.ProcessFunctions;
using static ArnoldVinkCode.ProcessUwpFunctions;

namespace ZuseMe
{
    public partial class Media
    {
        //Update media player session
        public static async Task UpdateMediaPlayerSession()
        {
            try
            {
                //Get active media player session
                GlobalSystemMediaTransportControlsSessionManager smtcSessionManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
                IReadOnlyList<GlobalSystemMediaTransportControlsSession> smtcSessions = smtcSessionManager.GetSessions();
                //Debug.WriteLine("Media sessions found: " + smtcSessions.Count);
                //foreach (GlobalSystemMediaTransportControlsSession mediaSession in smtcSessions)
                //{
                //    Debug.WriteLine("Media session found: " + mediaSession.SourceAppUserModelId);
                //}

                //Load enabled players
                IEnumerable<string> enabledPlayers = AppVariables.MediaPlayers.Where(x => x.Enabled).Select(x => x.ProcessName);

                //Check enabled players
                AppVariables.SmtcSessionMedia = smtcSessions.OrderBy(x => enabledPlayers.Any(x.SourceAppUserModelId.Contains)).Where(x => enabledPlayers.Any(x.SourceAppUserModelId.Contains)).FirstOrDefault();

                //Check if player has changed
                string currentPlayer = AppVariables.SmtcSessionMedia == null ? string.Empty : AppVariables.SmtcSessionMedia.SourceAppUserModelId;
                if (currentPlayer != AppVariables.SmtcSessionMediaProcess)
                {
                    if (AppVariables.SmtcSessionMedia == null)
                    {
                        await MediaResetVariables(true, true, true, true, true);
                        Debug.WriteLine("No matching media player session profile found.");

                        //Update smtc player process
                        AppVariables.SmtcSessionMediaProcess = string.Empty;
                    }
                    else
                    {
                        UpdateMediaPlayerInterface();

                        //AppVariables.SmtcSessionMedia.TimelinePropertiesChanged += SmtcSessionMedia_TimelinePropertiesChanged;
                        //AppVariables.SmtcSessionMedia.PlaybackInfoChanged += SmtcSessionMedia_PlaybackInfoChanged;
                        //AppVariables.SmtcSessionMedia.MediaPropertiesChanged += SmtcSessionMedia_MediaPropertiesChanged;
                        Debug.WriteLine("Changed media player session to: " + AppVariables.SmtcSessionMedia.SourceAppUserModelId);

                        //Update smtc player process
                        AppVariables.SmtcSessionMediaProcess = AppVariables.SmtcSessionMedia.SourceAppUserModelId;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to update media player session: " + ex.Message);
            }
        }

        private static void SmtcSessionMedia_TimelinePropertiesChanged(GlobalSystemMediaTransportControlsSession sender, TimelinePropertiesChangedEventArgs args)
        {
            try
            {
                Debug.WriteLine("Timeline properties changed.");
                //AppVariables.ScrobbleReset = true;
            }
            catch { }
        }

        private static void SmtcSessionMedia_PlaybackInfoChanged(GlobalSystemMediaTransportControlsSession sender, PlaybackInfoChangedEventArgs args)
        {
            try
            {
                Debug.WriteLine("Playback info changed.");
                //AppVariables.ScrobbleReset = true;
            }
            catch { }
        }

        //Reset on media properties change
        private static void SmtcSessionMedia_MediaPropertiesChanged(GlobalSystemMediaTransportControlsSession sender, MediaPropertiesChangedEventArgs args)
        {
            try
            {
                Debug.WriteLine("Media properties changed.");
                //AppVariables.ScrobbleReset = true;
            }
            catch { }
        }

        //Update media player interface
        private static void UpdateMediaPlayerInterface()
        {
            try
            {
                ActionDispatcherInvoke(delegate
                {
                    try
                    {
                        AppVariables.WindowMain.textblock_PlayerDebug.Text = AppVariables.SmtcSessionMedia.SourceAppUserModelId;
                    }
                    catch { }
                });
            }
            catch { }
        }

        //Focus on media player window
        public static async Task FocusMediaPlayer()
        {
            try
            {
                Debug.WriteLine("Focusing on player window: " + AppVariables.SmtcSessionMediaProcess);

                //Check application type
                ProcessMulti processMultiPlayer = null;
                if (AppVariables.SmtcSessionMediaProcess.ToLower().EndsWith(".exe"))
                {
                    string processName = Path.GetFileNameWithoutExtension(AppVariables.SmtcSessionMediaProcess);
                    Process processPlayer = GetProcessByNameOrTitle(processName, false, true);
                    processMultiPlayer = ConvertProcessToProcessMulti(processPlayer, null, null);
                }
                else
                {
                    processMultiPlayer = GetUwpProcessMultiByAppUserModelId(AppVariables.SmtcSessionMediaProcess);
                }

                if (processMultiPlayer != null)
                {
                    await FocusProcessWindow(processMultiPlayer.Name, processMultiPlayer.Identifier, processMultiPlayer.WindowHandle, WindowShowCommand.None, false, false);
                }
                else
                {
                    Debug.WriteLine("No player window found to focus on.");
                }
            }
            catch
            {
                Debug.WriteLine("Failed focusing on player window.");
            }
        }
    }
}