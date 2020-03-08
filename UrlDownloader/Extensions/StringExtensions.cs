using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlDownloader.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveIllegalFileChars(this string filename)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(c, '_');
            }
            return filename;
        }

        public static string GetPrefix(this string url)
        {
            try
            {
                return url.Split(':')[0];
            }
            catch(Exception ex)
            {
                throw new ArgumentException($"Received an invalid url {url}. ", ex);
            }
        }
    }
}
