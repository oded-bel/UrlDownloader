using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using UrlDownloader.Classes.Exceptions;
using UrlDownloader.Extensions;
using UrlDownloader.Interfaces;
using UrlDownloader.Models;

namespace UrlDownloader.Classes
{
    public abstract class DownloadManager : IDownloadManager
    {
        #region Ctor, Dependencies and paramters

        private static string LocalDownloadPath = ConfigurationManager.AppSettings["LocalPath"] ?? "C:\\Temp\\";
        private static string ResumeRetryCount = ConfigurationManager.AppSettings["ResumeRetries"] ?? "5";
        private IFileWriter _fileWriter;
        private bool _isStopped = false;
        private ILog _logger;
        private Queue<FileStreamInformation> _resumeQueue;

        public DownloadManager(IFileWriter fileWriter, ILog logger)
        {
            _fileWriter = fileWriter;
            _logger = logger;
            _resumeQueue = new Queue<FileStreamInformation>();
        }

        #endregion

        #region Public methods

        public void StartDownloading(Queue<string> urlQueue)
        {
            StartConsumingDownloadQueue(urlQueue);
            StartConsumingResumeQueue();
        }

        public void StopDownloading()
        {
            _isStopped = true;
        }

        #endregion

        #region PrivateMethods
        private void StartConsumingDownloadQueue(Queue<string> urlQueue)
        {
            _logger.Debug($"Starting to consume queue for {GetPrefix()} urls");
            _isStopped = false;
            while (urlQueue.Count != 0 && !_isStopped)
            {
                FileStreamInformation fileStreamInformation = new FileStreamInformation();
                try
                {
                    fileStreamInformation.Url = urlQueue.Dequeue();                   
                    SendDownloadRequest(fileStreamInformation);
                    fileStreamInformation.FullPath = LocalDownloadPath + fileStreamInformation.Url.RemoveIllegalFileChars();
                    _fileWriter.WriteFileStream(fileStreamInformation);
                }
                catch (StreamDisconnectedException ex)
                {
                    _logger.Warn($"Received an StreamDisconnectedException while downloading file {fileStreamInformation.FullPath}. Trying to resume.", ex);
                    if (fileStreamInformation.ResumeEnabled)
                    {
                        fileStreamInformation.RetryCount = int.Parse(ResumeRetryCount);
                        _resumeQueue.Enqueue(fileStreamInformation);
                    }
                }
                catch (Exception ex)
                {
                    if (File.Exists(fileStreamInformation.FullPath))
                    {
                        File.Delete(fileStreamInformation.FullPath);
                    }
                    _logger.Error($"Failed to download file with url: {fileStreamInformation.FullPath}", ex);
                }
            }
        }

        private void StartConsumingResumeQueue()
        {
            _logger.Debug("Starting to consume resume queue.");
            while (_resumeQueue.Count != 0 && !_isStopped)
            {
                FileStreamInformation fileStreamInformation = _resumeQueue.Dequeue();
                try
                {
                    SendDownloadRequest(fileStreamInformation);
                    _fileWriter.ResumeFileWrite(fileStreamInformation);
                }
                catch (StreamDisconnectedException ex)
                {
                    _logger.Warn($"Received an StreamDisconnectedException while downloading file {fileStreamInformation.FullPath}. Trying to resume.", ex);
                    if(fileStreamInformation.RetryCount-- > 0)
                    {
                        _resumeQueue.Enqueue(fileStreamInformation);
                    }
                }
                catch (Exception ex)
                {
                    if(File.Exists(fileStreamInformation.FullPath))
                    {
                        File.Delete(fileStreamInformation.FullPath);
                    }
                    _logger.Error($"Failed to download file with url: {fileStreamInformation.FullPath}", ex);
                }
            }
        }
        #endregion

        #region Abstract Methods

        public abstract void SendDownloadRequest(FileStreamInformation fileStreamInformation);
        public abstract string GetPrefix();
        public abstract void SendResumeRequest(FileStreamInformation fileStreamInformation);
        #endregion
    }
}
