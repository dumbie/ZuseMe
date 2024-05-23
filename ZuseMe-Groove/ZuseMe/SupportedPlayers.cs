using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Linq;
using ZuseMe.Classes;

namespace ZuseMe
{
    public class SupportedPlayers
    {
        public static void LoadSupportedPlayers()
        {
            try
            {
                //Set listbox source
                AppVariables.WindowMain.listbox_SupportedPlayers.ItemsSource = AppVariables.MediaPlayersSupported;

                //Check supported players
                bool jsonUpdated = false;
                foreach (PlayersJson playerSupported in AppVariables.MediaPlayersSupported)
                {
                    try
                    {
                        PlayersJson playerEnabled = AppVariables.MediaPlayersEnabled.Where(x => x.ProcessName == playerSupported.ProcessName).FirstOrDefault();
                        if (playerEnabled == null)
                        {
                            jsonUpdated = true;
                            playerSupported.Enabled = true;
                        }
                        else
                        {
                            playerSupported.Enabled = playerEnabled.Enabled;
                        }
                    }
                    catch { }
                }

                //Save enabled players to json
                if (jsonUpdated)
                {
                    var enabledPlayers = AppVariables.MediaPlayersSupported.Select(x => new { x.Enabled, x.ProcessName });
                    AVJsonFunctions.JsonSaveObject(enabledPlayers, @"Profiles\EnabledPlayers.json");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load supported players: " + ex.Message);
            }
        }
    }
}