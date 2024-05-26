using System;
using System.IO.Compression;
using System.IO;
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
            //downloadURL = "https://aviationweather.gov/adds/dataserver_current/current/metars.cache.xml";
            downloadURL = "https://aviationweather.gov/data/cache/metars.cache.xml.gz";
        }
        public void DownloadMetars()
        {
            //ConfigurationRoot configurationRoot = new ConfigurationRoot();
            string tempFilePath = System.IO.Path.GetTempPath() + "metars_"+DateTime.Now.ToString("ddHHmmss") + ".xml.gz";
            var wc = new WebClient();
            try
            {
                wc.DownloadFile(downloadURL, tempFilePath);
                FileInfo fileInfo = new FileInfo(tempFilePath);
                _localFilePath = _unzip(fileInfo);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("An issue happened during the download of the metar file", e);
            }

            //_localFilePath = tempFilePath;
        }

        private string _unzip(FileInfo fileToDecompress)
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        //Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
                    }
                }

                return newFileName;
            }
        }
    }
}
