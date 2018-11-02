using DotnetSpiderLite.Abstractions.Downloader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotnetSpiderLite.Abstractions
{
    public class Response
    {
        public DownloadContext DownloadContext { get; private set; }


        public Stream Body { get; set; }

        public string ContentType { get; set; }

        public int StatusCode { get; set; }



        public Response(DownloadContext downloadContext)
        {
            this.DownloadContext = downloadContext;
        }
    }
}
