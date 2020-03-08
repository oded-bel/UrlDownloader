using System;
using System.IO;
using System.Net;
using log4net;
using UrlDownloader.Interfaces;
using UrlDownloader.Models;

namespace UrlDownloader.Classes.Downloaders
{
    public class HttpDownloader : DownloadManager
    {
        public const bool SupportsResume = true;
        public const string Prefix = "http";

        public HttpDownloader(IFileWriter fileWriter, ILog logger) : base(fileWriter, logger)
        {
        }

        public override void  SendDownloadRequest(FileStreamInformation fileStreamInformation)
        {
            if (string.IsNullOrEmpty(fileStreamInformation.Url))
            {
                throw new ArgumentException($"Received an invalid url for ftp download. Url : {fileStreamInformation.Url}");
            }
            using (var webClient = new WebClient())
            {
                fileStreamInformation.FileStream = webClient.OpenRead(fileStreamInformation.Url);
                fileStreamInformation.BytesRead = 0;
                fileStreamInformation.ResumeEnabled = false;               
            }
        }

        public override string GetPrefix()
        {
            return Prefix;
        }

    }
}
