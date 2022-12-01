using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media.Control;
using static ArnoldVinkCode.AVActions;

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
                if (currentPlayer != AppVariables.SmtcSessionMediaPrevious)
                {
                    if (AppVariables.SmtcSessionMedia == null)
                    {
                        await MediaResetVariables(true, true, true, true, true);
                        Debug.WriteLine("No matching media player session profile found.");

                        //Update previous player
                        AppVariables.SmtcSessionMediaPrevious = string.Empty;
                    }
                    else
                    {
                        UpdateMediaPlayerInterface();
                        //AppVariables.SmtcSessionMedia.MediaPropertiesChanged += SmtcSessionMedia_MediaPropertiesChanged;
                        Debug.WriteLine("Changed media player session to: " + AppVariables.SmtcSessionMedia.SourceAppUserModelId);

                        //Update previous player
                        AppVariables.SmtcSessionMediaPrevious = AppVariables.SmtcSessionMedia.SourceAppUserModelId;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to update media player session: " + ex.Message);
            }
        }

        //Reset on media properties change
        private static void SmtcSessionMedia_MediaPropertiesChanged(GlobalSystemMediaTransportControlsSession sender, MediaPropertiesChangedEventArgs args)
        {
            try
            {
                Debug.WriteLine("Media properties changed.");
                AppVariables.ScrobbleReset = true;
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
    }
}