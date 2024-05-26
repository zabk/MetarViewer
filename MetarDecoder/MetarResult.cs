namespace MetarDecoder
{
    public class MetarResult
    {
        public string AirportIATACode { get; set; }
        public string IssueTime { get; set; }
        public int Minima { get; set; }
        public int Visibility { get; set; }
        public string RVR { get; set; }
        public string RawMETAR { get; set; }
        public bool Snowing { get; set; }
        public MetarIssue MetarStatus { get; set; }
        
    }

}
