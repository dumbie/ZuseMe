using ArnoldVinkCode;
using System;
using System.Windows;
using System.Windows.Interop;
using static ArnoldVinkCode.AVInteropDll;

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