using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ZuseMe.Forms;

namespace ZuseMe
{
    public partial class TrayMenu
    {
        private NotifyIcon sysTrayIcon;
        private ContextMenu sysTrayMenu;

        public TrayMenu()
        {
            // Create a context menu for systray.
            sysTrayMenu = new ContextMenu();
            sysTrayMenu.MenuItems.Add("Stop Scrobble", OnStopScrobble);
            sysTrayMenu.MenuItems.Add("Settings", OnSettings);
            sysTrayMenu.MenuItems.Add("Website", OnWebsite);
            sysTrayMenu.MenuItems.Add("Exit", OnExit);

            // Initialize the tray notify icon.
            sysTrayIcon = new NotifyIcon();
            sysTrayIcon.Text = "ZuseMe";
            sysTrayIcon.Icon = new Icon(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ZuseMe.Resources.ZuseMe.ico"));

            // Add menu to tray icon and show it.
            sysTrayIcon.ContextMenu = sysTrayMenu;
            sysTrayIcon.Visible = true;
        }
        private void OnSettings(object sender, EventArgs e)
        {
            Settings Settings = new Settings();
            Settings.Show();
        }
        private void OnStopScrobble(object sender, EventArgs e)
        {
            LastFMSend.Stop();
        }
        private void OnWebsite(object sender, EventArgs e)
        {
            Process.Start("https://projects.arnoldvink.com");
        }
        private void OnExit(object sender, EventArgs e)
        {
            try
            {
                //Stop current scrobble
                LastFMSend.Stop();

                //Close last.fm client
                int CloseLastfmOnExit = int.Parse(ConfigurationManager.AppSettings["CloseLastfmOnExit"]);
                if (CloseLastfmOnExit >= 1)
                {
                    foreach (Process LastFMProc in Process.GetProcessesByName("LastFM"))
                    {
                        LastFMProc.Kill();
                    }
                    foreach (Process LastFMProc2 in Process.GetProcessesByName("Last.fm"))
                    {
                        LastFMProc2.Kill();
                    }
                    foreach (Process LastFMProc3 in Process.GetProcessesByName("Last.fm Scrobbler"))
                    {
                        LastFMProc3.Kill();
                    }
                    foreach (Process LastFMProc4 in Process.GetProcessesByName("Last.fm Desktop Scrobbler"))
                    {
                        LastFMProc4.Kill();
                    }
                }

                //Hide tray icon
                sysTrayIcon.Visible = false;

                //Exit application
                Environment.Exit(1);
            }
            catch { }
        }
    }
}