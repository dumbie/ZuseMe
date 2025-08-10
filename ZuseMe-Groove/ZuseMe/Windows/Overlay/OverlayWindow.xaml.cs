using ArnoldVinkCode;
using ArnoldVinkStyles;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using static ArnoldVinkCode.AVInteropDll;
using static ArnoldVinkCode.AVProcess;
using static ArnoldVinkCode.AVSettings;
using static ArnoldVinkCode.AVWindowFunctions;
using static ZuseMe.AppVariables;

namespace ZuseMe.Windows
{
    public partial class WindowOverlay : Window
    {
        //Window Initialize
        public WindowOverlay() { InitializeComponent(); }

        //Window Variables
        private IntPtr vInteropWindowHandle = IntPtr.Zero;

        //Window Initialized
        protected override void OnSourceInitialized(EventArgs e)
        {
            try
            {
                //Get interop window handle
                vInteropWindowHandle = new WindowInteropHelper(this).EnsureHandle();

                //Set render mode to software
                HwndSource hwndSource = HwndSource.FromHwnd(vInteropWindowHandle);
                HwndTarget hwndTarget = hwndSource.CompositionTarget;
                hwndTarget.RenderMode = RenderMode.SoftwareOnly;

                //Update window visibility
                WindowUpdateVisibility(vInteropWindowHandle, true);

                //Update window style
                WindowUpdateStyle(vInteropWindowHandle, true, true, true, false);
            }
            catch { }
        }

        public void HideWindow()
        {
            try
            {
                //Show or hide media controls
                border_Control.Visibility = Visibility.Collapsed;

                //Hide the overlay
                this.Hide();
            }
            catch { }
        }

        public void ShowWindowDuration(uint showMilliSeconds)
        {
            try
            {
                //Check overlay settings
                if (!SettingLoad(vConfiguration, "TrackShowOverlay", typeof(bool)) && !SettingLoad(vConfiguration, "VolumeShowOverlay", typeof(bool)))
                {
                    Debug.WriteLine("Overlay settings are disabled, skipping overlay.");

                    //Hide the overlay
                    HideWindow();
                    return;
                }

                //Check media information
                if (textblock_TrackArtist.Text == "Artist" && textblock_TrackTitle.Text == "Title" && textblock_TrackAlbum.Text == "Album")
                {
                    Debug.WriteLine("Unknown song hiding the overlay.");

                    //Hide the overlay
                    HideWindow();
                    return;
                }

                //Check if media player window is active
                ProcessMulti foregroundProcess = Get_ProcessMultiByWindowHandle(GetForegroundWindow());
                if (foregroundProcess != null)
                {
                    bool skipOverlayAppUserModelId = AppVariables.MediaPlayersSupported.Any(x => foregroundProcess.AppUserModelId.ToLower().StartsWith(x.ProcessName.ToLower()));
                    bool skipOverlayExecutable = AppVariables.MediaPlayersSupported.Any(x => foregroundProcess.ExeName.ToLower().StartsWith(x.ProcessName.ToLower()));
                    if (skipOverlayAppUserModelId || skipOverlayExecutable)
                    {
                        Debug.WriteLine("Media player window is active, skipping overlay.");

                        //Hide the overlay
                        HideWindow();
                        return;
                    }
                }

                //Show the overlay
                this.Show();

                //Start overlay hide timer
                TimerOverlay.Interval = showMilliSeconds;
                TimerOverlay.TickSet = delegate
                {
                    try
                    {
                        AVDispatcherInvoke.DispatcherInvoke(delegate
                        {
                            //Check if mouse is over
                            if (!grid_Overlay.IsMouseOver)
                            {
                                //Stop the timer
                                TimerOverlay.Stop();

                                //Hide the overlay
                                HideWindow();
                            }
                        });
                    }
                    catch { }
                };
                TimerOverlay.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed showing overlay: " + ex.Message);
            }
        }

        private void grid_Overlay_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                //Hide the overlay
                HideWindow();
            }
            catch { }
        }

        private async void button_ControlPlayPause_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await AppVariables.SmtcSessionMedia.TryTogglePlayPauseAsync();
            }
            catch { }
        }

        private async void button_ControlNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await AppVariables.SmtcSessionMedia.TrySkipNextAsync();
            }
            catch { }
        }

        private async void button_ControlPrevious_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await AppVariables.SmtcSessionMedia.TrySkipPreviousAsync();
            }
            catch { }
        }

        private async void button_FocusPlayer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    await Media.FocusMediaPlayer();
                }
                else if (e.RightButton == MouseButtonState.Pressed)
                {
                    AppVariables.WindowMain.Show();
                }
            }
            catch { }
        }

        private void slider_ControlVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                int targetVolume = (int)e.NewValue;

                AVAudioDevice.AudioVolumeSet(targetVolume, false);
                textblock_ControlVolume.Text = targetVolume.ToString();

                Debug.WriteLine("Changed volume to: " + targetVolume);
            }
            catch { }
        }

        private void button_ControlMute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool muted = AVAudioDevice.AudioMuteSwitch(false);
                Debug.WriteLine("Muted volume: " + muted);
            }
            catch { }
        }

        private void grid_Overlay_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                //Show or hide media controls
                border_Control.Visibility = Visibility.Visible;
            }
            catch { }
        }

        private void grid_Overlay_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                //Extend overlay timer
                AppVariables.TimerOverlay.Start();
            }
            catch { }
        }

        private async void slider_ProgressNow_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                long seekPosition = (long)(10000000 * e.NewValue);
                await AppVariables.SmtcSessionMedia.TryChangePlaybackPositionAsync(seekPosition);
            }
            catch { }
        }
    }
}