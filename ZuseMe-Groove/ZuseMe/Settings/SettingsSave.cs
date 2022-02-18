using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ZuseMe
{
    partial class WindowMain
    {
        public void Settings_Save()
        {
            try
            {
                textbox_TrackLengthCustom.TextChanged += (sender, e) =>
                {
                    try
                    {
                        TextBox senderElement = sender as TextBox;
                        int trackLength = Convert.ToInt32(senderElement.Text);
                        if (trackLength >= 20)
                        {
                            Settings.Setting_Save(null, "TrackLengthCustom", senderElement.Text);
                            textbox_TrackLengthCustom.Foreground = (SolidColorBrush)Application.Current.Resources["ValidBrush"];
                        }
                        else
                        {
                            textbox_TrackLengthCustom.Foreground = (SolidColorBrush)Application.Current.Resources["InvalidBrush"];
                        }
                    }
                    catch { }
                };

                combobox_TrackPercentageScrobble.SelectionChanged += (sender, e) =>
                {
                    try
                    {
                        ComboBox senderElement = sender as ComboBox;
                        if (senderElement.SelectedIndex == 0)
                        {
                            Settings.Setting_Save(null, "TrackPercentageScrobble", "25");
                        }
                        else if (senderElement.SelectedIndex == 1)
                        {
                            Settings.Setting_Save(null, "TrackPercentageScrobble", "50");
                        }
                        else if (senderElement.SelectedIndex == 2)
                        {
                            Settings.Setting_Save(null, "TrackPercentageScrobble", "75");
                        }
                        else if (senderElement.SelectedIndex == 3)
                        {
                            Settings.Setting_Save(null, "TrackPercentageScrobble", "90");
                        }
                    }
                    catch { }
                };

                checkbox_WindowsStartup.Click += (sender, e) =>
                {
                    try
                    {
                        Settings.ManageStartupShortcut();
                    }
                    catch { }
                };
            }
            catch { }
        }
    }
}