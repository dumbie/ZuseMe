﻿using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using static ArnoldVinkCode.AVInteropDll;
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
        protected override async void OnSourceInitialized(EventArgs e)
        {
            try
            {
                //Get interop window handle
                vInteropWindowHandle = new WindowInteropHelper(this).EnsureHandle();

                //Set render mode to software
                HwndSource hwndSource = HwndSource.FromHwnd(vInteropWindowHandle);
                HwndTarget hwndTarget = hwndSource.CompositionTarget;
                hwndTarget.RenderMode = RenderMode.SoftwareOnly;

                //Set the window style
                IntPtr updatedStyle = new IntPtr((uint)WindowStyles.WS_VISIBLE);
                await SetWindowLongAuto(vInteropWindowHandle, (int)WindowLongFlags.GWL_STYLE, updatedStyle);

                //Set the window style ex
                IntPtr updatedExStyle = new IntPtr((uint)(WindowStylesEx.WS_EX_TOPMOST | WindowStylesEx.WS_EX_NOACTIVATE));
                await SetWindowLongAuto(vInteropWindowHandle, (int)WindowLongFlags.GWL_EXSTYLE, updatedExStyle);

                //Set the window as top most
                SetWindowPos(vInteropWindowHandle, (IntPtr)WindowPosition.TopMost, 0, 0, 0, 0, (int)(WindowSWP.NOMOVE | WindowSWP.NOSIZE));
            }
            catch { }
        }

        public void ShowWindowDuration(int showSeconds)
        {
            try
            {
                //Check if media player window is active
                ProcessMulti foregroundProcess = GetProcessMultiFromWindowHandle(GetForegroundWindow());
                bool skipOverlay = AppVariables.MediaPlayers.Any(x => foregroundProcess.ExecutableName.ToLower().StartsWith(x.ProcessName.ToLower()));
                if (skipOverlay)
                {
                    Debug.WriteLine("Media player window is active, skipping overlay.");
                    return;
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

                //Check media information
                if (!showControls && textblock_TrackArtist.Text == "Unknown" && textblock_TrackTitle.Text == "Unknown" && textblock_TrackAlbum.Text == "Unknown")
                {
                    Debug.WriteLine("Unknown song hiding the overlay.");

                    //Hide the overlay
                    this.Hide();
                    return;
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
            catch { }
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