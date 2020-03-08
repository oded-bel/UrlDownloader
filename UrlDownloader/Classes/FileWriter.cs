using log4net;
using System;
using System.IO;
using UrlDownloader.Interfaces;
using UrlDownloader.Models;

namespace UrlDownloader.Classes
{
    public class FileWriter : IFileWriter
    {
        #region Dependencies and Ctor

        private ILog _logger;
        private const int BufferBytes = 8 * 1024;

        public FileWriter(ILog logger)
        {
            _logger = logger;
        }

        #endregion
        #region Public methods

        public void WriteFileStream(FileStreamInformation fileStreamInformation)
        {
            if(fileStreamInformation == null || string.IsNullOrEmpty(fileStreamInformation.FullPath) || fileStreamInformation.FileStream == null)
            {
                var message = "Received invalid file stream information to write!";
                _logger.Error(message);
                throw new ArgumentException(message);
            }
            fileStreamInformation.FullPath = CreateLocalFileName(fileStreamInformation.FullPath);
            WriteFile(fileStreamInformation);
        }
        
        public void ResumeFileWrite(FileStreamInformation fileStreamInformation)
        {
            _logger.Debug($"Resuming file stream download for file:{fileStreamInformation.FullPath}");
            WriteFile(fileStreamInformation);
        }

        #endregion
        #region Private methods
        private String CreateLocalFileName(string fullPath)
        {
            FileInfo fi = new FileInfo(fullPath);
            if (!Directory.Exists(fi.DirectoryName))
                Directory.CreateDirectory(fi.DirectoryName);

            string fext = Path.GetExtension(fullPath);
            if (File.Exists(fullPath))
            {
                int c = 0;
                string fname_woe = Path.GetFileNameWithoutExtension(fullPath);
                string ffname = "";
                do
                {
                    ffname = fi.Directory.FullName + Path.DirectorySeparatorChar + fname_woe + string.Format("({0})", c++) + fext;
                } while (File.Exists(ffname));

                fullPath = ffname;
            }

            return fullPath;
        }


        private void WriteFile(FileStreamInformation fileStreamInformation)
        {
            //create file stream
            _logger.Debug("Creating local file stream.");
            byte[] buffer = new byte[BufferBytes];
            int len;
            using (var fileStream = File.Create(fileStreamInformation.FullPath))
            { 
                while ((len = fileStreamInformation.FileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fileStream.Write(buffer, 0, len);
                    fileStreamInformation.BytesRead += len;
                }
            }
            if(fileStreamInformation.Size != fileStreamInformation.BytesRead)
        }

        #endregion
    }
}
