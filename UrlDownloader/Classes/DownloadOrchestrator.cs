using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlDownloader.Extensions;
using Autofac;
using UrlDownloader.Modules;
using UrlDownloader.Constants;
using log4net;
using UrlDownloader.Interfaces;

namespace UrlDownloader.Classes
{
    public class DownloadOrchestrator
    {
        #region Ctor dependencies and parameters
        private ILog _logger;

        //Due to time constraints using an in memory queues, 
        //in a live environment would use persistant queues
        private Dictionary<string, Queue<string>> _downloadQueues;


        //In this example we will be using one downloader per prefix, 
        //In a live environment we might have a dedicated machine per downloader and 
        //increased specific downloader counts for the more common/resource heavy urls
        private List<IDownloadManager> _downloadManagers;
        
        public DownloadOrchestrator()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new UrlDownloaderModule());

            var container = builder.Build();
            _logger = container.Resolve<ILog>();
            _downloadQueues = new Dictionary<string, Queue<string>>();
            foreach(var supported in UrlDownloaderConstants.SupportedTypes)
            {
                var queue = new Queue<string>();
                _downloadQueues[supported] = queue;
            }
            _downloadManagers = container.Resolve<IEnumerable<IDownloadManager>>().ToList();
        }
        #endregion

        #region Public methods
        public void DownloadResources(IEnumerable<string> urls)
        {
            PopulateQueues(urls);
            var tasks = StartDownladers();
            Task.WaitAll(tasks.ToArray());
        }

        #endregion
        #region Private methods
        private void StopDownloaders()
        {
            foreach (var downloader in _downloadManagers)
            {
                  downloader.StopDownloading();
            }
        }


        private void PopulateQueues(IEnumerable<string> urls)
        {
            foreach(var url in urls)
            {
                try
                {
                    if (!UrlDownloaderConstants.SupportedTypes.Contains(url.GetPrefix()))
                    {
                        _logger.Warn("Received an unsupported Url format");
                        continue;
                    }
                    _downloadQueues[url.GetPrefix()].Enqueue(url);
                }
                catch(Exception ex)
                {
                    _logger.Error($"Received an error while tying to parse url: {url}",ex);
                }
            }
        }

        private IEnumerable<Task> StartDownladers()
        {
            foreach(var downloader in _downloadManagers)
            {
                 yield return Task.Run(() => downloader.StartDownloading(_downloadQueues[downloader.GetPrefix()]));
            }
        }

        #endregion
    }
}
