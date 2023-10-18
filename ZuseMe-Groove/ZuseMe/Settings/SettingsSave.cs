using ArnoldVinkCode;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static ArnoldVinkCode.AVSettings;

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
                            SettingSave(null, "TrackLengthCustom", senderElement.Text);
                            textbox_TrackLengthCustom.Foreground = (SolidColorBrush)Application.Current.Resources["ApplicationValidBrush"];
                        }
                        else
                        {
                            textbox_TrackLengthCustom.Foreground = (SolidColorBrush)Application.Current.Resources["ApplicationInvalidBrush"];
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
                            SettingSave(null, "TrackPercentageScrobble", "25");
                        }
                        else if (senderElement.SelectedIndex == 1)
                        {
                            SettingSave(null, "TrackPercentageScrobble", "50");
                        }
                        else if (senderElement.SelectedIndex == 2)
                        {
                            SettingSave(null, "TrackPercentageScrobble", "75");
                        }
                        else if (senderElement.SelectedIndex == 3)
                        {
                            SettingSave(null, "TrackPercentageScrobble", "90");
                        }
                    }
                    catch { }
                };

                checkbox_WindowsStartup.Click += (sender, e) =>
                {
                    try
                    {
                        AVSettings.StartupShortcutManage("Launcher.exe", false);
                    }
                    catch { }
                };

                checkbox_TrackShowOverlay.Click += (sender, e) =>
                {
                    try
                    {
                        CheckBox senderElement = sender as CheckBox;
                        SettingSave(null, "TrackShowOverlay", senderElement.IsChecked);
                    }
                    catch { }
                };

                checkbox_VolumeShowOverlay.Click += (sender, e) =>
                {
                    try
                    {
                        CheckBox senderElement = sender as CheckBox;
                        SettingSave(null, "VolumeShowOverlay", senderElement.IsChecked);
                    }
                    catch { }
                };

                checkbox_LastFMUpdateNowPlaying.Click += (sender, e) =>
                {
                    try
                    {
                        CheckBox senderElement = sender as CheckBox;
                        SettingSave(null, "LastFMUpdateNowPlaying", senderElement.IsChecked);
                    }
                    catch { }
                };
            }
            catch { }
        }
    }
}