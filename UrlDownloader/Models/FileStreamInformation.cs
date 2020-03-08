using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlDownloader.Models
{
    public class FileStreamInformation
    {
        public Stream FileStream { get; set; }  
        public long Size { get; set; }
        public bool ResumeEnabled { get; set; }
        public string FullPath { get; set; }
        public int BytesRead { get; internal set; }
        public string Url { get; set; }
        public int RetryCount { get; internal set; }
    }
}
