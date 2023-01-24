using ArnoldVinkCode;
using System;
using System.Diagnostics;
using ZuseMe.Classes;

namespace ZuseMe
{
    public class SupportedPlayers
    {
        public static void LoadSupportedPlayers()
        {
            try
            {
                //Load players from json
                AVJsonFunctions.JsonLoadFile(ref AppVariables.MediaPlayersSupported, "SupportedPlayers.json");
                AVJsonFunctions.JsonLoadFile(ref AppVariables.MediaPlayersEnabled, "EnabledPlayers.json");

                //Set listbox source
                AppVariables.WindowMain.listbox_SupportedPlayers.ItemsSource = AppVariables.MediaPlayersSupported;

                //Check supported players
                bool jsonUpdated = false;
                foreach (PlayersJson player in AppVariables.MediaPlayersSupported)
                {
                    try
                    {
                        if (!AVSettings.Check(null, "Player" + player.ProcessName))
                        {
                            AVSettings.Save(null, "Player" + player.ProcessName, true);
                        }
                    }
                    catch { }
                }

                //Update supported players
                foreach (PlayersJson player in AppVariables.MediaPlayersSupported)
                {
                    try
                    {
                        player.Enabled = AVSettings.Load(null, "Player" + player.ProcessName, typeof(bool));
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load supported players: " + ex.Message);
            }
        }
    }
}