using System.Collections.Generic;

namespace UrlDownloader.Interfaces
{
    public interface IDownloadManager
    {
        void StartDownloading(Queue<string> urlQueue);
        void StopDownloading();
        string GetPrefix();
    }
}