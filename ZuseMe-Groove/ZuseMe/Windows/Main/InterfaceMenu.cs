using ArnoldVinkCode;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static ArnoldVinkCode.AVUpdate;

namespace ZuseMe
{
    partial class WindowMain
    {
        //Handle main menu mouse/touch tapped
        async void lb_Menu_MousePressUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //Check if an actual ListBoxItem is clicked
                if (!AVFunctions.ListBoxItemClickCheck((DependencyObject)e.OriginalSource)) { return; }

                //Check which mouse button is pressed
                await lb_Menu_SingleTap();
            }
            catch { }
        }

        //Handle main menu keyboard/controller tapped
        async void lb_Menu_KeyPressUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Space) { await lb_Menu_SingleTap(); }
            }
            catch { }
        }

        //Handle main menu single tap
        async Task lb_Menu_SingleTap()
        {
            try
            {
                if (lb_Menu.SelectedIndex >= 0)
                {
                    StackPanel SelStackPanel = (StackPanel)lb_Menu.SelectedItem;
                    if (SelStackPanel.Name == "menuButtonScrobble") { ShowGridPage(stackpanel_Scrobble); }
                    else if (SelStackPanel.Name == "menuButtonProfile") { OpenLastFMProfile(); }
                    else if (SelStackPanel.Name == "menuButtonPlayers") { ShowGridPage(stackpanel_Players); }
                    else if (SelStackPanel.Name == "menuButtonSettings") { ShowGridPage(stackpanel_Settings); }
                    else if (SelStackPanel.Name == "menuButtonUpdate")
                    {
                        //Check for available application update
                        await UpdateCheck("dumbie", "ZuseMe", false);
                    }
                    else if (SelStackPanel.Name == "menuButtonClose") { this.Close(); }
                    else if (SelStackPanel.Name == "menuButtonExit") { await AppExit.Exit_Prompt(); }
                }
            }
            catch { }
        }

        //Display a certain grid page
        void ShowGridPage(FrameworkElement elementTarget)
        {
            try
            {
                stackpanel_Scrobble.Visibility = Visibility.Collapsed;
                stackpanel_Settings.Visibility = Visibility.Collapsed;
                stackpanel_Players.Visibility = Visibility.Collapsed;
                elementTarget.Visibility = Visibility.Visible;
            }
            catch { }
        }
    }
}