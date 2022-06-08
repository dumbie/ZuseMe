using ArnoldVinkCode;
using System;
using System.Windows;

namespace ZuseMe.Windows
{
    public partial class WindowOverlay : Window
    {
        public WindowOverlay()
        {
            InitializeComponent();
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