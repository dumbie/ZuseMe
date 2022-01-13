using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ZuseMe
{
    public class LaunchZuseMePlaying
    {
        public LaunchZuseMePlaying()
        {
            try
            {
                Process.Start(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources//ZuseMePlaying.exe"));
                Console.WriteLine("ZuseMePlaying launched.");
            }
            catch
            {
                MessageBox.Show("File: Resources\\ZuseMePlaying.exe could not be found.", "ZuseMe");
                Environment.Exit(1);
            }
        }
    }
}
