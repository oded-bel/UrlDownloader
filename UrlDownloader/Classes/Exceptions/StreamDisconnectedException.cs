using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlDownloader.Classes.Exceptions
{
    public class StreamDisconnectedException : Exception
    {
        public StreamDisconnectedException(string message) : base(message)
        {
                
        }
    }
}
