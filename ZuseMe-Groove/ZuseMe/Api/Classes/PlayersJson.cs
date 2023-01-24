using System;

namespace ZuseMe.Classes
{
    [Serializable]
    public class PlayersJson
    {
        public bool Enabled { get; set; }
        public string Identifier { get; set; }
        public string ProcessName { get; set; }
        public string Note { get; set; }
        public string Link { get; set; }
    }
}