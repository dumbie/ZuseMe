using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ZuseMe
{
    public class SendLastFM
    {
        private string ZuseMePluginId = "wmp";
        private int kDefaultPort = 33367;
        private Socket fd;
        private NetworkStream ns;
        private StreamWriter sw;
        private StreamReader sr;

        private void SendMessageToClient(string LastFMCommand)
        {
            try
            {
                Connect();
                sw.WriteLine(LastFMCommand);
                sw.Flush();
                Disconnect();
                Console.WriteLine("Command has been sent to the Last.fm Scrobbler.");
            }
            catch
            {
                Console.WriteLine("Command could not been sent to the Last.fm Scrobbler.");
            }
        }

        private void Connect()
        {
            try
            {
                fd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                fd.Connect(new IPEndPoint(IPAddress.Loopback, kDefaultPort));
                ns = new NetworkStream(fd);
                sw = new StreamWriter(ns);
                sr = new StreamReader(ns);
            }
            catch
            {
                Console.WriteLine("Could not connect to the Last.fm Scrobbler.");
            }
        }

        private void Disconnect()
        {
            try
            {
                ns.Close();
                sw.Close();
                sr.Close();
                fd.Shutdown(SocketShutdown.Both);
            }
            catch
            {
                Console.WriteLine("Could not disconnect from the Last.fm Scrobbler.");
            }
        }

        MediaKeys MediaKeys = new MediaKeys();
        public void Start(string ZuneArtist, string ZuneTrack, string ZuneAlbum, string ZuneId, int ZuneLength, string ZuneFilename)
        {
            string LastFMCommand = "START c=" + ZuseMePluginId + "&" +
                           "a=" + Escape(ZuneArtist) + "&" +
                           "t=" + Escape(ZuneTrack) + "&" +
                           "b=" + Escape(ZuneAlbum) + "&" +
                           "m=" + Escape(ZuneId) + "&" +
                           "l=" + Escape(ZuneLength.ToString()) + "&" +
                           "p=" + Escape(ZuneFilename);
            SendMessageToClient(LastFMCommand);
            MediaKeys.ZunePausedMe = 0;
        }

        public void Stop()
        {
            string LastFMCommand = "STOP c=" + ZuseMePluginId;
            SendMessageToClient(LastFMCommand);
            MediaKeys.ZunePausedMe = 1;
        }

        public void Pause()
        {
            string LastFMCommand = "PAUSE c=" + ZuseMePluginId;
            SendMessageToClient(LastFMCommand);
            MediaKeys.ZunePausedMe = 1;
        }

        public void Resume()
        {
            string LastFMCommand = "RESUME c=" + ZuseMePluginId;
            SendMessageToClient(LastFMCommand);
            MediaKeys.ZunePausedMe = 0;
        }

        private string Escape(string str)
        {
            return str.Replace("&", "&&");
        }
    }
}