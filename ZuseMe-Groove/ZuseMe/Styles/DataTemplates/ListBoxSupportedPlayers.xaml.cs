using System;
using System.Windows;
using System.Windows.Controls;
using ZuseMe;
using ZuseMe.Classes;

namespace ArnoldVinkCode.Styles
{
    public partial class ListBoxSupportedPlayers : ResourceDictionary
    {
        void Checkbox_SupportedPlayer_Click(object sender, EventArgs e)
        {
            try
            {
                CheckBox senderButton = sender as CheckBox;
                PlayersJson selectedPlayer = senderButton.DataContext as PlayersJson;

                //Update player setting
                AVSettings.Save(null, "Player" + selectedPlayer.ProcessName, senderButton.IsChecked);

                //Update media sessions
                Media.SmtcSessionManager_SessionsChanged(null, null);
            }
            catch { }
        }
    }
}