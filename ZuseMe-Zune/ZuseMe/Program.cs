using System;
using System.Threading;
using System.Windows.Forms;
using ZuseMe.Forms;

namespace ZuseMe
{
    public class Program
    {
        public static void Main()
        {
            StartupCheck StartupCheck = new StartupCheck();
            TrayMenu TrayMenu = new TrayMenu();

            Console.WriteLine("Mediakeys detection is now enabled.");
            Thread MediaKeysThread = new Thread(MediaKeys.MediaKeysStart);
            MediaKeysThread.Start();

            Console.WriteLine("Waiting for a song to begin or change...");
            ReceiveSong ReceiveSong = new ReceiveSong();
            ReceiveSong.Show();

            Application.Run();
        }
    }
}