using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using static ArnoldVinkCode.AVInteropDll;
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

        public void ShowWindowDuration(int showSeconds)
        {
            try
            {
                //Check overlay settings
                if (!AVSettings.Load(null, "TrackShowOverlay", typeof(bool)) && !AVSettings.Load(null, "VolumeShowOverlay", typeof(bool)))
                {
                    Debug.WriteLine("Overlay settings are disabled, skipping overlay.");

                    //Hide the overlay
                    this.Hide();
                    return;
                }

                //Check media information
                if (textblock_TrackArtist.Text == "Artist" && textblock_TrackTitle.Text == "Title" && textblock_TrackAlbum.Text == "Album")
                {
                    Debug.WriteLine("Unknown song hiding the overlay.");

                    //Hide the overlay
                    this.Hide();
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
                        this.Hide();
                        return;
                    }
                }

                //Show or hide controls
                bool showControls = AVSettings.Load(null, "ControlOverlay", typeof(bool));
                if (showControls)
                {
                    stackpanel_OverlayControl.Visibility = Visibility.Visible;
                }
                else
                {
                    stackpanel_OverlayControl.Visibility = Visibility.Collapsed;
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
                        if (!border_Overlay.IsMouseOver)
                        {
                            //Hide the overlay
                            this.Hide();

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

        private void border_Overlay_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                //Hide the overlay
                this.Hide();
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
    }
}