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
                    await UpdateMediaPlayerSmtc(null);
                }
            }
        }

        //Update media player session
        public static async Task UpdateMediaPlayerSession()
        {
            try
            {
                //Update media player Zune software
                if (UpdateMediaPlayerZune())
                {
                    return;
                }

                //Debug.WriteLine("Getting SMTC sessions.");

                //Get active media player sessions
                IReadOnlyList<GlobalSystemMediaTransportControlsSession> smtcSessions = null;
                try
                {
                    smtcSessions = AppVariables.SmtcSessionManager.GetSessions();
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

                //Update media player smtc
                if (currentPlayer == null)
                {
                    Debug.WriteLine("No SMTC sessions.");
                    await UpdateMediaPlayerSmtc(null);
                }
                else
                {
                    Debug.WriteLine("Found SMTC sessions.");
                    await UpdateMediaPlayerSmtc(currentPlayer);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to update media player session: " + ex.Message);
            }
        }

        private static bool UpdateMediaPlayerZune()
        {
            try
            {
                //Check if Zune software is running
                if (Check_RunningProcessByName("Zune.exe", true))
                {
                    //Get supported player
                    PlayersJson playerJson = AppVariables.MediaPlayersSupported.Where(x => x.ProcessName == "Zune.exe").FirstOrDefault();
                    if (playerJson.Enabled)
                    {
                        if (AppVariables.SmtcSessionName != "Zune.exe")
                        {
                            //Update smtc player variables
                            AppVariables.SmtcSessionMedia = null;
                            AppVariables.SmtcSessionName = "Zune.exe";

                            UpdateMediaPlayerInterface();
                            Debug.WriteLine("Changed media player session to: Zune.exe");
                            return true;
                        }
                        else if (playerJson.Enabled && AppVariables.SmtcSessionName == "Zune.exe")
                        {
                            Debug.WriteLine("Media player session already set to: Zune.exe");
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("UpdateMediaPlayerZune failed: " + ex.Message);
                return false;
            }
        }

        private static async Task UpdateMediaPlayerSmtc(GlobalSystemMediaTransportControlsSession currentPlayer)
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

                        UpdateMediaPlayerInterface();
                        Debug.WriteLine("Changed media player session to: " + currentPlayerString);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("UpdateMediaPlayerSmtc failed: " + ex.Message);
            }
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
                        AppVariables.WindowMain.textblock_PlayerDebug.Text = AppVariables.SmtcSessionName;
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