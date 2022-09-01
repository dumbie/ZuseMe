using ArnoldVinkCode;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using ZuseMe.Classes;

namespace ZuseMe
{
    public class SupportedPlayers
    {
        public static void LoadSupportedPlayers()
        {
            try
            {
                string jsonFile = File.ReadAllText(@"SupportedPlayers.json");
                AppVariables.MediaPlayers = JsonConvert.DeserializeObject<PlayersJson[]>(jsonFile);
                AppVariables.WindowMain.listbox_SupportedPlayers.ItemsSource = AppVariables.MediaPlayers;

                //Check supported players
                foreach (PlayersJson player in AppVariables.MediaPlayers)
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
                foreach (PlayersJson player in AppVariables.MediaPlayers)
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