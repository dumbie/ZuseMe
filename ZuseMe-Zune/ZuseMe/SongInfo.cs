using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ZuseMe
{
    public class SongInfo
    {
        SendLastFM SendLastFM = new SendLastFM();
        public int ZuneRunning;
        public string ZuneTrack;
        public string ZuneArtist;
        public string ZuneAlbum;

        public string ZuneId = "ZuseMe";
        public string ZuneFilename = ConfigurationManager.AppSettings["ZuneFilename"] + ":/";
        public string previousSong;
        public int ZuseLength;

        public void SendSongInfo()
        {
            if (ZuneRunning == 1 && !string.IsNullOrEmpty(ZuneTrack) && !string.IsNullOrEmpty(ZuneArtist) && !string.IsNullOrEmpty(ZuneAlbum))
            {
                string song = ZuneArtist + " - " + ZuneTrack + " - " + ZuneAlbum + " - " + ZuseLength + " - " + ZuneRunning;

                if (previousSong != song)
                {
                    //If the song has changed
                    previousSong = song;

                    //Check MusicBrainz Settings
                    if (int.Parse(ConfigurationManager.AppSettings["ZuseUseMusicBrainz"]) >= 1)
                    {
                        GetSongInfoLength();
                    }
                    else
                    {
                        ZuseLength = int.Parse(ConfigurationManager.AppSettings["ZuseLengthDefault"]);
                    }

                    Console.WriteLine(ZuneArtist + " - " + ZuneTrack + " - " + ZuneAlbum + " - " + ZuseLength + " - " + ZuneId + " - " + ZuneFilename);

                    //Start scrobbling
                    SendLastFM.Start(ZuneArtist, ZuneTrack, ZuneAlbum, ZuneId, ZuseLength, ZuneFilename);
                }
            }
            else
            {
                string song = ZuneArtist + " - " + ZuneTrack + " - " + ZuneAlbum + " - " + ZuseLength + " - " + ZuneRunning;

                if (previousSong != song)
                {
                    //If the song has changed
                    previousSong = song;
                    Console.WriteLine("Zune has stopped playing or has been closed or opened.");

                    //Stop scrobbling
                    SendLastFM.Stop();
                }
            }
        }

        //Get track duration from MusicBrainz
        public void GetSongInfoLength()
        {
            try
            {
                var WebClient = new WebClient();
                WebClient.Headers.Add("User-Agent", "ZuseMe/" + Application.ProductVersion);
                var xml = WebClient.DownloadString(new Uri("http://musicbrainz.org/ws/1/track/?type=xml&limit=1&title=" + ZuneTrack + "&artist=" + ZuneArtist));

                XNamespace ns = "http://musicbrainz.org/ns/mmd-1.0";
                XDocument doc = XDocument.Parse(xml);

                try
                {
                    var DurationXML = from c in doc.Descendants(ns + "track") select (string)c.Element(ns + "duration");
                    ZuseLength = int.Parse(DurationXML.First()) / 1000;

                    if (ZuseLength < 35)
                    {
                        Console.WriteLine("MusicBrainz: Track length is too short, using default length.");
                        ZuseLength = int.Parse(ConfigurationManager.AppSettings["ZuseLengthDefault"]);
                    }
                }
                catch
                {
                    if (ZuseLength < 35)
                    {
                        Console.WriteLine("MusicBrainz: Track length could not be found.");
                        ZuseLength = int.Parse(ConfigurationManager.AppSettings["ZuseLengthDefault"]);
                    }
                }
            }
            catch
            {
                if (ZuseLength < 35)
                {
                    Console.WriteLine("MusicBrainz: Track length could not be found (Connection error).");
                    ZuseLength = int.Parse(ConfigurationManager.AppSettings["ZuseLengthDefault"]);
                }
                return;
            }
        }
    }
}