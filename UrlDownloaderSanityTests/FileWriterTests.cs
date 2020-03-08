using System;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UrlDownloader.Classes;
using UrlDownloader.Interfaces;
using UrlDownloader.Models;

namespace UrlDownloaderSanityTests
{
    [TestClass]
    public class FileWriterTests
    {
        private Mock<ILog> _mockLogger = new Mock<ILog>();
        private IFileWriter _fileWriter;

        public FileWriterTests()
        {
            _fileWriter = new FileWriter(_mockLogger.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWriteFile_NullFileStreamInformation_ErrorWriten()
        {
            _fileWriter.WriteFileStream(null);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWriteFile_EmptyFileStreamInformation_ErrorWriten()
        {
            _fileWriter.WriteFileStream(new FileStreamInformation());
        }
    }
}
