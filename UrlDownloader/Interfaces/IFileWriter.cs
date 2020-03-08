using UrlDownloader.Models;

namespace UrlDownloader.Interfaces
{
    public interface IFileWriter
    {
        void WriteFileStream(FileStreamInformation fileStreamInformation);
        void ResumeFileWrite(FileStreamInformation fileStreamInformation);
    }
}