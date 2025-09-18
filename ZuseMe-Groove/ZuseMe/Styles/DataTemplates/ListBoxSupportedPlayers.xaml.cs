using ArnoldVinkCode;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ZuseMe;
using ZuseMe.Classes;
using static ArnoldVinkCode.AVFunctions;

namespace ArnoldVinkStyles
{
    public partial class ListBoxSupportedPlayers : ResourceDictionary
    {
        private void Checkbox_SupportedPlayer_Click(object sender, EventArgs e)
        {
            try
            {
                CheckBox senderButton = sender as CheckBox;

                //Save enabled players to json
                var enabledPlayers = AppVariables.MediaPlayersSupported.Select(x => new { x.Enabled, x.ProcessName });
                AVJsonFunctions.JsonSaveObject(enabledPlayers, @"Profiles\EnabledPlayers.json");
            }
            catch { }
        }

        private void Button_SupportedPlayer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button senderButton = sender as Button;
                PlayersJson selectedPlayer = senderButton.DataContext as PlayersJson;
                OpenWebsiteBrowser(selectedPlayer.Link);
            }
            catch { }
        }
    }
}