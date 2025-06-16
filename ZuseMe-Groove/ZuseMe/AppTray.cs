using ArnoldVinkCode;
using System;
using System.Drawing;
using System.Windows.Forms;
using static ArnoldVinkCode.AVFunctions;

namespace ZuseMe
{
    public partial class AppTray
    {
        public NotifyIcon NotifyIcon = new NotifyIcon();
        public ContextMenuStrip ContextMenu = new ContextMenuStrip();

        public AppTray()
        {
            try
            {
                //Create context menu
                ContextMenu.Items.Add("Pause or resume", null, OnPauseResume);
                ContextMenu.Items.Add("-");
                ContextMenu.Items.Add("Show window", null, OnShowWindow);
                ContextMenu.Items.Add("Open profile", null, OnOpenProfile);
                ContextMenu.Items.Add("-");
                ContextMenu.Items.Add("Website", null, OnWebsite);
                ContextMenu.Items.Add("Exit", null, OnExit);

                //Create tray icon
                NotifyIcon.Text = "ZuseMe (Last.fm client)";
                NotifyIcon.Icon = new Icon(AVEmbedded.EmbeddedResourceToStream(null, "ZuseMe.Assets.ZuseMe.ico"));

                //Add menu to tray icon
                NotifyIcon.ContextMenuStrip = ContextMenu;

                //Show tray icon
                NotifyIcon.Visible = true;

                //Register events
                NotifyIcon.MouseClick += TrayIcon_MouseClick;
                NotifyIcon.DoubleClick += OnShowWindow;
            }
            catch { }
        }

        private async void TrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    //Show media overlay
                    AppVariables.WindowOverlay.ShowWindowDuration(3000);
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    //Pause or resume scrobbling
                    await Media.MediaScrobblePauseToggle();
                }
            }
            catch { }
        }

        private void OnShowWindow(object sender, EventArgs e)
        {
            try
            {
                AppVariables.WindowMain.Show();
            }
            catch { }
        }

        private async void OnPauseResume(object sender, EventArgs e)
        {
            try
            {
                await Media.MediaScrobblePauseToggle();
            }
            catch { }
        }

        private void OnOpenProfile(object sender, EventArgs e)
        {
            try
            {
                AppVariables.WindowMain.OpenLastFMProfile();
            }
            catch { }
        }

        private void OnWebsite(object sender, EventArgs e)
        {
            try
            {
                OpenWebsiteBrowser("https://projects.arnoldvink.com");
            }
            catch { }
        }

        private async void OnExit(object sender, EventArgs e)
        {
            try
            {
                await AppExit.Exit_Prompt();
            }
            catch { }
        }
    }
}