using System;
using Autofac;
using UrlDownloader.Classes;
using UrlDownloader.Classes.Downloaders;
using log4net;
using UrlDownloader.Interfaces;

namespace UrlDownloader.Modules
{
    public class UrlDownloaderModule : Module
    {


        protected override void Load(ContainerBuilder builder)
        {
            log4net.Config.XmlConfigurator.Configure();
            builder.Register(c => LogManager.GetLogger(typeof(Object))).As<ILog>();
            builder.RegisterType<DownloadOrchestrator>().AsSelf();
            builder.RegisterType<HttpDownloader>().As<IDownloadManager>();
            builder.RegisterType<FtpDownloader>().As<IDownloadManager>();
            builder.RegisterType<FileWriter>().As<IFileWriter>();
        }


    }
}
