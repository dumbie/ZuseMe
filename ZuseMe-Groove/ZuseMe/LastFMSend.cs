using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ZuseMe
{
    public class LastFMSend
    {
        private const int lastFMServerPort = 33367;
        private const string lastFMPluginId = "wmp";
        private static Socket socket;
        private static NetworkStream networkStream;
        private static StreamWriter streamWriter;
        private static StreamReader streamReader;

        private static void SendMessageToClient(string lastFMCommand)
        {
            try
            {
                Connect();
                streamWriter.WriteLine(lastFMCommand);
                streamWriter.Flush();
                Disconnect();
                Console.WriteLine("Command has been sent to the Last.fm Scrobbler: " + lastFMCommand);
            }
            catch
            {
                Console.WriteLine("Command could not been sent to the Last.fm Scrobbler: " + lastFMCommand);
            }
        }

        private static void Connect()
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(new IPEndPoint(IPAddress.Loopback, lastFMServerPort));
                networkStream = new NetworkStream(socket);
                streamWriter = new StreamWriter(networkStream);
                streamReader = new StreamReader(networkStream);
            }
            catch
            {
                Console.WriteLine("Could not connect to the Last.fm Scrobbler.");
            }
        }

        private static void Disconnect()
        {
            try
            {
                networkStream.Dispose();
                streamWriter.Dispose();
                streamReader.Dispose();
                socket.Dispose();
            }
            catch
            {
                Console.WriteLine("Could not disconnect from the Last.fm Scrobbler.");
            }
        }

        public static void Start(string mediaArtist, string mediaTrack, string mediaAlbum, string mediaId, string mediaDurationSeconds, string mediaFilePath)
        {
            MediaInformation.MediaPlayingSeconds = 0;
            string lastFMCommand = "START c=" + lastFMPluginId + "&a=" + Escape(mediaArtist) + "&t=" + Escape(mediaTrack) + "&b=" + Escape(mediaAlbum) + "&m=" + Escape(mediaId) + "&l=" + Escape(mediaDurationSeconds) + "&p=" + Escape(mediaFilePath);
            SendMessageToClient(lastFMCommand);
        }

        public static void Stop()
        {
            MediaInformation.MediaPlayingSeconds = 0;
            MediaInformation.MediaPrevious = string.Empty;
            string lastFMCommand = "STOP c=" + lastFMPluginId;
            SendMessageToClient(lastFMCommand);
        }

        public static void Pause()
        {
            string lastFMCommand = "PAUSE c=" + lastFMPluginId;
            SendMessageToClient(lastFMCommand);
        }

        public static void Resume()
        {
            string lastFMCommand = "RESUME c=" + lastFMPluginId;
            SendMessageToClient(lastFMCommand);
        }

        private static string Escape(string str)
        {
            return str.Replace("&", "&&");
        }
    }
}