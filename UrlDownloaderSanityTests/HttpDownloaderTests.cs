using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlDownloader.Classes.Downloaders;
using UrlDownloader.Interfaces;
using UrlDownloader.Models;

namespace UrlDownloaderSanityTests
{


    [TestClass]
    public class HttpDownloaderTests
    {
        private Queue<string> _downloadQueue = new Queue<string>();

        private HttpDownloader _httpDownloader;
        private Mock<IFileWriter> _mockFileWriter = new Mock<IFileWriter>();
        private Mock<ILog> _mockLogger = new Mock<ILog>();


        public HttpDownloaderTests()
        {
            _httpDownloader = new HttpDownloader(_mockFileWriter.Object, _mockLogger.Object);
        }

        [TestMethod]
        public void DownloadManager_TestGoogleDownload_Success()
        {
            var fileStreamInformation = new FileStreamInformation();
            fileStreamInformation.Url = $@"http://www.google.com/";

            _httpDownloader.SendDownloadRequest(fileStreamInformation);

            Assert.IsNotNull(fileStreamInformation.FileStream);
        }
    }
}
