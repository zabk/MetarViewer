using System;
using System.Net;

namespace DownloadMetar
{
    public class Downloader
    {
        string downloadURL;
        string _localFilePath;
        public string DownloadedFilePath { get { return _localFilePath; } }
        public Downloader()
        {
            downloadURL = "https://aviationweather.gov/adds/dataserver_current/current/metars.cache.xml";
        }
        public void DownloadMetars()
        {
            //ConfigurationRoot configurationRoot = new ConfigurationRoot();
            string tempFilePath = System.IO.Path.GetTempPath() + "metars_"+DateTime.Now.ToString("ddHHmmss") + ".xml";
            var wc = new WebClient();
            try
            {
                wc.DownloadFile(downloadURL, tempFilePath);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("An issue happened during the download of the metar file", e);
            }

            _localFilePath = tempFilePath;
        }
    }
}
