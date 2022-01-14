using System.Threading;
using System.Windows.Forms;

namespace ZuseMe
{
    public class Program
    {
        public static void Main()
        {
            //Application startup checks
            StartupCheck StartupCheck = new StartupCheck();

            //Create tray menu
            TrayMenu TrayMenu = new TrayMenu();

            //Stop current scrobble
            LastFMSend.Stop();

            //Start media information thread
            Thread MediaInformationThread = new Thread(MediaInformation.MediaInformationLoop);
            MediaInformationThread.Start();

            //Run application
            Application.Run();
        }
    }
}