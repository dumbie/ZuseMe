using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media.Control;
using ZuseMe.Classes;
using static ArnoldVinkCode.AVActions;
using static ArnoldVinkCode.AVProcess;

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
                async Task<GlobalSystemMediaTransportControlsSessionManager> TaskAction()
                {
                    try
                    {
                        return await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
                    }
                    catch { }
                    return null;
                }
                AppVariables.SmtcSessionManager = await TaskStartReturn(TaskAction).WaitAsync(TimeSpan.FromMilliseconds(2000));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to request SMTC access: " + ex.Message);
            }
            finally
            {
                if (AppVariables.SmtcSessionManager == null)
                {
                    await UpdateMediaPlayer(null);
                }
            }
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
                List<string> enabledPlayers = new List<string>();
                foreach (PlayersJson player in AppVariables.MediaPlayersSupported)
                {
                    if (player.Enabled)
                    {
                        if (player.SmtcSessionName != null)
                        {
                            enabledPlayers.Add(player.SmtcSessionName);
                        }
                        else
                        {
                            enabledPlayers.Add(player.ProcessName);
                        }
                    }
                }

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
                if (currentPlayerString != AppVariables.SmtcSessionName)
                {
                    if (string.IsNullOrWhiteSpace(currentPlayerString))
                    {
                        //Update smtc player variables
                        AppVariables.SmtcSessionMedia = null;
                        AppVariables.SmtcSessionName = string.Empty;

                        await MediaResetVariables(true, true, true, true, true);
                        Debug.WriteLine("No matching media player session profile found.");
                    }
                    else
                    {
                        //Update smtc player variables
                        AppVariables.SmtcSessionMedia = currentPlayer;
                        AppVariables.SmtcSessionName = currentPlayerString;

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
                DispatcherInvoke(delegate
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
                Debug.WriteLine("Focusing on player window: " + AppVariables.SmtcSessionName);

                //Get supported player
                PlayersJson playerJson = AppVariables.MediaPlayersSupported.Where(x => x.SmtcSessionName == AppVariables.SmtcSessionName || x.ProcessName == AppVariables.SmtcSessionName).FirstOrDefault();

                //Check application type
                ProcessMulti processMultiPlayer = null;
                if (playerJson.ProcessName.EndsWith(".exe"))
                {
                    processMultiPlayer = Get_ProcessesMultiByName(playerJson.ProcessName, true).FirstOrDefault();
                }
                else
                {
                    processMultiPlayer = Get_ProcessesMultiByAppUserModelId(playerJson.ProcessName).FirstOrDefault();
                }

                if (processMultiPlayer != null)
                {
                    await Show_ProcessByWindowHandle(processMultiPlayer.WindowHandleMain);
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