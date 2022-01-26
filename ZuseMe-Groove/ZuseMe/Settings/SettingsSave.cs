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

                checkbox_WindowsStartup.Click += (sender, e) => { Settings.ManageStartupShortcut(); };
            }
            catch { }
        }
    }
}