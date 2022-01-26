using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ZuseMe.Api;

namespace ZuseMe
{
    public partial class AppTray
    {
        public NotifyIcon sysTrayIcon;
        public ContextMenu sysTrayMenu;

        public AppTray()
        {
            try
            {
                //Create context menu
                sysTrayMenu = new ContextMenu();
                sysTrayMenu.MenuItems.Add("Show", OnShow);
                sysTrayMenu.MenuItems.Add("Website", OnWebsite);
                sysTrayMenu.MenuItems.Add("Exit", OnExit);

                //Create tray icon
                sysTrayIcon = new NotifyIcon();
                sysTrayIcon.Text = "ZuseMe (Last.fm client)";
                sysTrayIcon.Icon = new Icon(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ZuseMe.Assets.ZuseMe.ico"));

                //Add menu to tray icon
                sysTrayIcon.ContextMenu = sysTrayMenu;

                //Show tray icon
                sysTrayIcon.Visible = true;

                //Register events
                sysTrayIcon.DoubleClick += OnShow;
            }
            catch { }
        }

        private void OnShow(object sender, EventArgs e)
        {
            try
            {
                AppVariables.WindowMain.Show();
            }
            catch { }
        }

        private void OnWebsite(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://projects.arnoldvink.com");
            }
            catch { }
        }

        private async void OnExit(object sender, EventArgs e)
        {
            try
            {
                //Remove current scrobble
                await ApiScrobble.RemoveNowPlaying();

                //Hide tray icon
                sysTrayIcon.Visible = false;

                //Exit application
                Environment.Exit(1);
            }
            catch { }
        }
    }
}