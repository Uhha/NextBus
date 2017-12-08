namespace NextBus
{
    public class StopsForLocation
    {
        public int code { get; set; }
        public long currentTime { get; set; }
        public Data data { get; set; }
        public string text { get; set; }
        public int version { get; set; }
    }

    public class Data
    {
        public bool limitExceeded { get; set; }
        public Stop[] stops { get; set; }
    }

    public class Stop
    {
        public string code { get; set; }
        public string direction { get; set; }
        public string id { get; set; }
        public float lat { get; set; }
        public int locationType { get; set; }
        public float lon { get; set; }
        public string name { get; set; }
        public Route[] routes { get; set; }
        public string wheelchairBoarding { get; set; }
    }

    public class Route
    {
        public Agency agency { get; set; }
        public string color { get; set; }
        public string description { get; set; }
        public string id { get; set; }
        public string longName { get; set; }
        public string shortName { get; set; }
        public string textColor { get; set; }
        public int type { get; set; }
        public string url { get; set; }
    }

    public class Agency
    {
        public string disclaimer { get; set; }
        public string id { get; set; }
        public string lang { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public bool privateService { get; set; }
        public string timezone { get; set; }
        public string url { get; set; }
    }
}
