using ArnoldVinkCode;
using System;
using System.Windows;
using System.Windows.Interop;

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
            }
            catch { }
        }

        public void ShowWindowDuration(int showSeconds)
        {
            try
            {
                //Show the overlay
                this.Show();

                //Start overlay timer
                AppVariables.DispatcherTimerOverlay.Interval = TimeSpan.FromMilliseconds(showSeconds);
                AppVariables.DispatcherTimerOverlay.Tick += delegate
                {
                    try
                    {
                        //Hide the overlay
                        this.Hide();

                        //Renew the timer
                        AVFunctions.TimerRenew(ref AppVariables.DispatcherTimerOverlay);
                    }
                    catch { }
                };
                AVFunctions.TimerReset(AppVariables.DispatcherTimerOverlay);
            }
            catch { }
        }

        private void Border_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                //Hide the overlay
                this.Hide();
            }
            catch { }
        }
    }
}