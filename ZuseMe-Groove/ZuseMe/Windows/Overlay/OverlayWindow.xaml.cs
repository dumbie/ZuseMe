using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using static ArnoldVinkCode.AVInteropDll;
using static ArnoldVinkCode.AVSettings;
using static ArnoldVinkCode.AVWindowFunctions;
using static ArnoldVinkCode.ProcessClasses;
using static ArnoldVinkCode.ProcessFunctions;

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

                //Update the window style
                WindowUpdateStyleVisible(vInteropWindowHandle, true, true, false);
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

        public void ShowWindowDuration(int showSeconds)
        {
            try
            {
                //Check overlay settings
                if (!SettingLoad(null, "TrackShowOverlay", typeof(bool)) && !SettingLoad(null, "VolumeShowOverlay", typeof(bool)))
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
                ProcessMulti foregroundProcess = GetProcessMultiFromWindowHandle(GetForegroundWindow());
                if (foregroundProcess != null)
                {
                    bool skipOverlayPath = AppVariables.MediaPlayersSupported.Any(x => foregroundProcess.Path.ToLower().StartsWith(x.ProcessName.ToLower()));
                    bool skipOverlayExecutable = AppVariables.MediaPlayersSupported.Any(x => foregroundProcess.ExecutableName.ToLower().StartsWith(x.ProcessName.ToLower()));
                    if (skipOverlayPath || skipOverlayExecutable)
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
                AppVariables.DispatcherTimerOverlay.Interval = TimeSpan.FromMilliseconds(showSeconds);
                AppVariables.DispatcherTimerOverlay.Tick += delegate
                {
                    try
                    {
                        //Check if mouse is over
                        if (!grid_Overlay.IsMouseOver)
                        {
                            //Hide the overlay
                            HideWindow();

                            //Renew the timer
                            AVFunctions.TimerRenew(ref AppVariables.DispatcherTimerOverlay);
                        }
                    }
                    catch { }
                };
                AVFunctions.TimerReset(AppVariables.DispatcherTimerOverlay);
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

        private async void button_FocusPlayer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Media.FocusMediaPlayer();
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
                //Extend hide timer
                AVFunctions.TimerReset(AppVariables.DispatcherTimerOverlay);
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