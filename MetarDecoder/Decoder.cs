using DownloadMetar;
using GetAirportList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace MetarDecoder
{
    public class Decoder
    {
        private List<string> _metars;
        private List<Airport> _airports;
        //private string _metarFilePath;
        private List<Metar> _decodedMetars;
        public List<MetarResult> MetarResults;

        public Decoder()
        {
            _decodedMetars = new List<Metar>();
            _airports = new List<Airport>();
            MetarResults = new List<MetarResult>();
        }
        
        public void ProcessMetars()
        {
            GetAirportList();
            DownloadAllMetar();
            FilterMetars();
            DecodeMetars();
            MergeMetarAndAirport();
            SetMetarStatus();
            SortMetar();
        }
        private void GetAirportList()
        {
            AirportList airports = new AirportList();
            _airports = airports.GetListOfAirports();
        }
        private void DecodeMetars()
        {
            foreach (var rawMetar in _metars)
            {
                _decodedMetars.Add(DecodeRawMetar(rawMetar));
            }
        }

        private void MergeMetarAndAirport()
        {
            MetarResults = _decodedMetars.Join(
                                 _airports,
                                 metar => metar.AirportICAOCode,
                                 airport => airport.AirportICAOCode,
                                 (metar, airport) => new MetarResult
                                 {
                                     AirportIATACode = airport.AirportIATACode,
                                     IssueTime = metar.ObservationTime,
                                     Minima = airport.Minima,
                                     Visibility = metar.Visibility,
                                     RVR = metar.RVR,
                                     RawMETAR = metar.RawMETAR,
                                     Snowing = metar.Snowing
                                 }
                ).ToList();
        }

        private void SetMetarStatus()
        {
            foreach (var metar in MetarResults)
            {
                if (metar.Snowing)
                {
                    metar.MetarStatus = MetarIssue.Snowy;
                }
                
                if (metar.Visibility < metar.Minima)
                {
                    metar.MetarStatus = MetarIssue.BelowMinima;
                }
                else if (metar.Visibility < metar.Minima*1.25 )
                {
                    metar.MetarStatus = MetarIssue.MarginalWeather;
                }
                
                if (metar.Visibility == 0)
                {
                    metar.MetarStatus = MetarIssue.Error;
                }
            }

            var missingAirports= _airports.Where(a => ! MetarResults.Select(m => m.AirportIATACode).Contains(a.AirportIATACode)).ToList();

            foreach (var item in missingAirports)
            {
                MetarResults.Add(new MetarResult { AirportIATACode = item.AirportIATACode, Minima = item.Minima, Visibility = 0, RawMETAR = "No METAR available", MetarStatus = MetarIssue.NoMetar });
            }
            

        }

        private void SortMetar()
        {
            MetarResults=MetarResults.OrderBy(m => m.MetarStatus).ThenBy(m => m.Visibility).ThenBy(m => m.AirportIATACode).ToList();
        }

        private void DownloadAllMetar()
        {
            Downloader downloader = new Downloader();
            downloader.DownloadMetars();

            XElement MetarData = XElement.Load(downloader.DownloadedFilePath);
            _metars = MetarData.Descendants("METAR").Select(m => (string)m.Element("raw_text").Value).ToList();
        }

        private void FilterMetars()
        {
            var selectedAirports = _airports.Select(a=>a.AirportICAOCode);
            _metars = _metars.Where(m => selectedAirports.Contains(m.Substring(0, 4))).ToList();
        }

        private Metar DecodeRawMetar(string rawMetar)
        {
            string airportPattern = @"^\w{4}";
            string observationTimePattern = @"\d{6}Z";
            //string cloudBasePattern = @"FEW\d{3}(TCU){0,1}|SCT\d{3}(TCU){0,1}|BKN\d{3}(TCU){0,1}|OVC\d{3}(TCU){0,1}|CAVOK";
            //string windPattern = @"\d{5}KT|\d{5}G\d{2}KT|VRB\d{2}KT|d{}3Vd{3}|\d{5}MPS";
            string visibilityPattern = @"\b\d{4}\b|\b\d{4}?[SWEN]?[SWEN]\b|CAVOK|\b\d?/?\d?SM\b|\b\d \d?/?\d?SM\b|\b\d{4}?NDV\b|\bM1/4SM\b|\b////\b";
            string rvrPattern = @"R\d{2}?[LRC]/\d{4}";
            //string temperaturePattern = @"\b\d{2}/\d{2}\b";
            string snowPattern = @" [+|-]?\w{0,4}SN";

            var result = new Metar();

            result.RawMETAR = rawMetar;
            result.AirportICAOCode = Regex.Match(rawMetar, airportPattern).ToString();
            result.Visibility = VisibilityConverter(Regex.Match(rawMetar, visibilityPattern).ToString());
            result.ObservationTime = Regex.Match(rawMetar,observationTimePattern).ToString();
            result.Snowing = Regex.IsMatch(rawMetar, snowPattern);
            var RVRs= Regex.Matches(rawMetar, rvrPattern);
            foreach (var RVR in RVRs)
            {
                result.RVR += RVR.ToString();
            }
            return result;
        }

        private int VisibilityConverter(string visibilityString)
        {
            int visibility = 9999;

            try
            {
                switch (visibilityString)
                {
                    case "CAVOK": visibility = 9999; break;
                    case "": visibility = 0; break;
                    case "SM": visibility = 0; break;
                    case "M1/4SM": visibility = 400; break;

                    default:

                        if (visibilityString.IndexOf("SM") > 0)
                        {
                            visibilityString = visibilityString.Substring(0, visibilityString.Length - 2);
                            if (visibilityString.IndexOf('/') > 0)
                            {
                                var denominator = Convert.ToDouble(visibilityString.Substring(visibilityString.Length - 1, 1));
                                var divider = Convert.ToDouble(visibilityString.Substring(visibilityString.Length - 3, 1));
                                var integer = 0d;
                                if (visibilityString.Length > 3)
                                {
                                    integer = Convert.ToDouble(visibilityString.Substring(0, visibilityString.Length - 4));
                                }

                                visibility = Convert.ToInt32((integer + denominator / divider) * 1609);
                            }
                            else visibility = Convert.ToInt32(Convert.ToInt32(visibilityString) * 1609);
                        }
                        else visibility = Convert.ToInt32(Convert.ToInt32(visibilityString.Substring(0, 4)));

                        break;
                }


                //if (visibilityString == "CAVOK")
                //{
                //    visibility = 9999;
                //}
                //else if (visibilityString == "" || visibilityString == "SM")
                //{
                //    visibility = 0;
                //}
                //else if (visibilityString == "M1/4SM")
                //{
                //    visibility = 400;
                //}
                //else if (visibilityString.IndexOf("SM") > 0)
                //{
                //    visibilityString = visibilityString.Substring(0, visibilityString.Length - 2);
                //    if (visibilityString.IndexOf('/') > 0)
                //    {
                //        var denominator = Convert.ToDouble(visibilityString.Substring(visibilityString.Length-1,1));
                //        var divider = Convert.ToDouble(visibilityString.Substring(visibilityString.Length-3, 1));
                //        var integer = 0d;
                //        if (visibilityString.Length>3)
                //        {
                //            integer= Convert.ToDouble(visibilityString.Substring(0, visibilityString.Length - 4));
                //        }
                        
                //        visibility = Convert.ToInt32((integer+denominator/divider) * 1609);
                //    }
                //    else visibility = Convert.ToInt32(Convert.ToInt32(visibilityString) * 1609);
                //}
                //else visibility = Convert.ToInt32(Convert.ToInt32(visibilityString.Substring(0, 4)));
            }
            catch (Exception)
            {

                visibility = -1;
            }

            return visibility;
        }

    }

}
