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
            SendLastFM SendLastFM = new SendLastFM();
            SendLastFM.Stop();
        }
        private void OnWebsite(object sender, EventArgs e)
        {
            Process.Start("https://projects.arnoldvink.com");
        }
        private void OnExit(object sender, EventArgs e)
        {
            try
            {
                int exitZuneOnExit = int.Parse(ConfigurationManager.AppSettings["exitZuneOnExit"]);
                int exitLastfmOnExit = int.Parse(ConfigurationManager.AppSettings["exitLastfmOnExit"]);
                SendLastFM SendLastFM = new SendLastFM();
                SendLastFM.Stop();

                if (exitZuneOnExit >= 1)
                {
                    foreach (Process ZuneProc in Process.GetProcessesByName("Zune"))
                    {
                        ZuneProc.Kill();
                    }
                }
                if (exitLastfmOnExit >= 1)
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
                }
                foreach (Process ZuseMePlayingProc in Process.GetProcessesByName("ZuseMePlaying"))
                {
                    ZuseMePlayingProc.Kill();
                }
            }
            catch { }
            sysTrayIcon.Visible = false;
            Environment.Exit(1);
        }
    }
}