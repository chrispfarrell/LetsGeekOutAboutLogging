namespace SynergisticLogging.Web.External.RapidApi
{
    public class RapidApiDadJokeResponse
    {
        public List<Body> body { get; set; }
        public bool success { get; set; }
    }

    public class Body
    {
        public string _id { get; set; }
        public string punchline { get; set; }
        public string setup { get; set; }
        public string type { get; set; }
    }
}
