using System;
using System.Collections.Generic;

namespace GetAirportList
{
    public class Airport
    {
        public string AirportICAOCode { get; set; }
        public string AirportIATACode { get; set; }
        public int Minima { get; set; }
    }

    public class AirportList
    {
        private List<Airport> _airports { get; set; }
        public List<Airport> GetListOfAirports()
        {
            _airports = new List<Airport> { new Airport { AirportICAOCode = "LHBP", AirportIATACode = "BUD", Minima = 75 },
                                            new Airport { AirportICAOCode = "LOWW", AirportIATACode = "VIE", Minima = 75 },
                                            new Airport { AirportICAOCode = "EPKT", AirportIATACode = "KTW", Minima = 75 },
                                            new Airport { AirportICAOCode = "EGGW", AirportIATACode = "LTN", Minima = 8175 },
                                            new Airport { AirportICAOCode = "UKKK", AirportIATACode = "IEV", Minima = 9750 },
                                            new Airport { AirportICAOCode = "LROP", AirportIATACode = "OTP", Minima = 1550 },
                                            new Airport { AirportICAOCode = "EHEH", AirportIATACode = "EIN", Minima = 550 },
                                            new Airport { AirportICAOCode = "LQSA", AirportIATACode = "SJJ", Minima = 550 },
                                            new Airport { AirportICAOCode = "LHDC", AirportIATACode = "DEB", Minima = 550 },
                                            new Airport { AirportICAOCode = "LKPR", AirportIATACode = "PRG", Minima = 10000 },
                                            new Airport { AirportICAOCode = "PPIZ", AirportIATACode = "TST", Minima=550}
            };
            
            
            return _airports;
        }


    }
}
