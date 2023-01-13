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
        //Request media player access
        public static async Task RequestMediaPlayerAccess()
        {
            try
            {
                Debug.WriteLine("Requesting SMTC access.");

                //Might cause Windows Explorer issue when looping.
                var smtcSessionManagerTask = GlobalSystemMediaTransportControlsSessionManager.RequestAsync().AsTask();
                AppVariables.SmtcSessionManager = await TaskStartReturnTimeout(smtcSessionManagerTask, 3000);
                if (AppVariables.SmtcSessionManager == null)
                {
                    await UpdateMediaPlayer(null);
                    Debug.WriteLine("Failed to get SMTC access.");
                }
                else
                {
                    Debug.WriteLine("Received SMTC access.");
                }
            }
            catch { }
        }

        //Reset media player access
        public static async Task ResetMediaPlayerAccess()
        {
            try
            {
                AppVariables.SmtcSessionManager = null;
                AppVariables.SmtcSessionMedia = null;
                await UpdateMediaPlayer(null);

                Debug.WriteLine("Reset SMTC access.");
            }
            catch { }
        }

        //Update media player session
        public static async Task UpdateMediaPlayerSession()
        {
            try
            {
                //Debug.WriteLine("Getting SMTC sessions.");

                //Get active media player sessions
                IReadOnlyList<GlobalSystemMediaTransportControlsSession> smtcSessions = null;
                try
                {
                    smtcSessions = AppVariables.SmtcSessionManager.GetSessions();
                    if (smtcSessions == null || smtcSessions.Count == 0)
                    {
                        Debug.WriteLine("No SMTC sessions.");
                        await UpdateMediaPlayer(null);
                        return;
                    }
                    else
                    {
                        //Debug.WriteLine("Received SMTC sessions.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failed getting SMTC sessions: " + ex.Message);
                    await RequestMediaPlayerAccess();
                    return;
                }

                //Debug.WriteLine("Media sessions found: " + smtcSessions.Count);
                //foreach (GlobalSystemMediaTransportControlsSession mediaSession in smtcSessions)
                //{
                //    Debug.WriteLine("Media session found: " + mediaSession.SourceAppUserModelId);
                //}

                //Load enabled players
                IEnumerable<string> enabledPlayers = AppVariables.MediaPlayersSupported.Where(x => x.Enabled).Select(x => x.ProcessName);

                //Check enabled players
                GlobalSystemMediaTransportControlsSession currentPlayer = smtcSessions.OrderBy(x => enabledPlayers.Any(x.SourceAppUserModelId.Contains)).Where(x => enabledPlayers.Any(x.SourceAppUserModelId.Contains)).FirstOrDefault();

                //Check if player has changed
                await UpdateMediaPlayer(currentPlayer);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to update media player session: " + ex.Message);
            }
        }

        private static async Task UpdateMediaPlayer(GlobalSystemMediaTransportControlsSession currentPlayer)
        {
            try
            {
                string currentPlayerString = currentPlayer == null ? string.Empty : currentPlayer.SourceAppUserModelId;
                if (currentPlayerString != AppVariables.SmtcSessionMediaProcess)
                {
                    if (string.IsNullOrWhiteSpace(currentPlayerString))
                    {
                        //Update smtc player variables
                        AppVariables.SmtcSessionMedia = null;
                        AppVariables.SmtcSessionMediaProcess = string.Empty;

                        await MediaResetVariables(true, true, true, true, true);
                        Debug.WriteLine("No matching media player session profile found.");
                    }
                    else
                    {
                        //Update smtc player variables
                        AppVariables.SmtcSessionMedia = currentPlayer;
                        AppVariables.SmtcSessionMediaProcess = currentPlayerString;

                        //AppVariables.SmtcSessionMedia.TimelinePropertiesChanged += SmtcSessionMedia_TimelinePropertiesChanged;
                        //AppVariables.SmtcSessionMedia.PlaybackInfoChanged += SmtcSessionMedia_PlaybackInfoChanged;
                        //AppVariables.SmtcSessionMedia.MediaPropertiesChanged += SmtcSessionMedia_MediaPropertiesChanged;

                        UpdateMediaPlayerInterface();
                        Debug.WriteLine("Changed media player session to: " + currentPlayerString);
                    }
                }
            }
            catch { }
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