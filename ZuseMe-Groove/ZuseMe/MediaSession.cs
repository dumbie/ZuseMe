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
        public static async Task RegisterMediaSessionEvents()
        {
            try
            {
                AppVariables.SmtcSessionManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
                AppVariables.SmtcSessionManager.SessionsChanged += SmtcSessionManager_SessionsChanged;
                SmtcSessionManager_SessionsChanged(null, null);
                Debug.WriteLine("Changed smtc session manager.");
            }
            catch { }
        }

        public static async void SmtcSessionManager_SessionsChanged(GlobalSystemMediaTransportControlsSessionManager sender, SessionsChangedEventArgs args)
        {
            try
            {
                await Task.Delay(500);

                //Get active media session
                IReadOnlyList<GlobalSystemMediaTransportControlsSession> smtcSessions = AppVariables.SmtcSessionManager.GetSessions();
                foreach (GlobalSystemMediaTransportControlsSession mediaSession in smtcSessions)
                {
                    Debug.WriteLine("Media session found: " + mediaSession.SourceAppUserModelId);
                }

                //Load enabled players
                IEnumerable<string> enabledPlayers = AppVariables.MediaPlayers.Where(x => x.Enabled).Select(x => x.ProcessName);

                //Check enabled players
                AppVariables.SmtcSessionMedia = smtcSessions.OrderBy(x => enabledPlayers.Any(x.SourceAppUserModelId.Contains)).Where(x => enabledPlayers.Any(x.SourceAppUserModelId.Contains)).FirstOrDefault();
                if (AppVariables.SmtcSessionMedia == null)
                {
                    await MediaResetVariables(true, true, true, true, true);
                    Debug.WriteLine("No media session matching player profile found.");
                }
                else
                {
                    AppVariables.SmtcSessionMedia.MediaPropertiesChanged += delegate
                    {
                        Debug.WriteLine("Media properties changed.");
                        AppVariables.ScrobbleReset = true;
                    };
                    ActionDispatcherInvoke(delegate
                    {
                        try
                        {
                            AppVariables.WindowMain.textblock_PlayerDebug.Text = AppVariables.SmtcSessionMedia.SourceAppUserModelId;
                        }
                        catch { }
                    });
                    Debug.WriteLine("Changed smtc session media: " + AppVariables.SmtcSessionMedia.SourceAppUserModelId);
                }
            }
            catch { }
        }
    }
}