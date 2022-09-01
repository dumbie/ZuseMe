using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

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
                sysTrayMenu.MenuItems.Add("Show window", OnShow);
                sysTrayMenu.MenuItems.Add("Pause/Resume", OnPause);
                sysTrayMenu.MenuItems.Add("Website", OnWebsite);
                sysTrayMenu.MenuItems.Add("Exit", OnExit);

                //Create tray icon
                sysTrayIcon = new NotifyIcon();
                sysTrayIcon.Text = "ZuseMe (Last.fm client)";
                sysTrayIcon.Icon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("ZuseMe.Assets.ZuseMe.ico"));

                //Add menu to tray icon
                sysTrayIcon.ContextMenu = sysTrayMenu;

                //Show tray icon
                sysTrayIcon.Visible = true;

                //Register events
                sysTrayIcon.MouseClick += TrayIcon_MiddleClick;
                sysTrayIcon.DoubleClick += OnShow;
            }
            catch { }
        }

        private async void TrayIcon_MiddleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Middle)
                {
                    await Media.MediaScrobblePauseToggle();
                }
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

        private async void OnPause(object sender, EventArgs e)
        {
            try
            {
                await Media.MediaScrobblePauseToggle();
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
                await AppStartup.Exit();
            }
            catch { }
        }
    }
}