using System;
using System.IO;

namespace MetarDecoder
{
    public class Metar
    {
        public string AirportICAOCode { get; set; }
        public string RawMETAR { get; set; }
        public int Visibility { get; set; }
        public string RVR { get; set; }

    }

}
