using System;
using System.IO;
using System.Net;
using log4net;
using UrlDownloader.Interfaces;
using UrlDownloader.Models;

namespace UrlDownloader.Classes.Downloaders
{
    public class FtpDownloader : DownloadManager
    {

        public const string Prefix = "ftp";

        public FtpDownloader(IFileWriter fileWriter, ILog logger) : base(fileWriter, logger)
        {
        }

        public override void SendDownloadRequest(FileStreamInformation fileStreamInformation)
        {
            if(string.IsNullOrEmpty(fileStreamInformation.Url))
            {
                throw new ArgumentException($"Received an invalid url for ftp download. Url : {fileStreamInformation.Url}");
            }
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(fileStreamInformation.Url);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            fileStreamInformation.FileStream = response.GetResponseStream();
            fileStreamInformation.BytesRead = 0;
            fileStreamInformation.Size = response.ContentLength;
            fileStreamInformation.ResumeEnabled = String.Compare(response.Headers["Accept-Ranges"], "bytes", true) == 0; ;
        }


        public override void SendResumeRequest(FileStreamInformation fileStreamInformation)
        {
            if (string.IsNullOrEmpty(fileStreamInformation.Url))
            {
                throw new ArgumentException($"Received an invalid url for ftp download. Url : {fileStreamInformation.Url}");
            }
            if(fileStreamInformation.ResumeEnabled != true)
            {
                throw new ArgumentException($"Cannot resume on this url.");
            }
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(fileStreamInformation.Url);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.ContentOffset = fileStreamInformation.BytesRead;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            fileStreamInformation.FileStream = response.GetResponseStream();
            fileStreamInformation.Size = response.ContentLength;
            fileStreamInformation.ResumeEnabled = String.Compare(response.Headers["Accept-Ranges"], "bytes", true) == 0; ;
        }

        public override string GetPrefix()
        {
            return Prefix;
        }

    }
}
