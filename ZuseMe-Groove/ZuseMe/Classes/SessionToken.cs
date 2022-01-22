namespace ZuseMe.Classes
{
    public class Session
    {
        public SessionToken session { get; set; }
    }

    public class SessionToken
    {
        public string name { get; set; }
        public string key { get; set; }
        public string subscriber { get; set; }
    }
}