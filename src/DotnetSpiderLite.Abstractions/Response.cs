using DotnetSpiderLite.Abstractions.Downloader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotnetSpiderLite.Abstractions
{
    public class Response
    {
        public Request Request { get; private set; }

        public Stream Body { get; set; }

        public string ContentType { get; set; }

        public int StatusCode { get; set; }


        public Dictionary<string, string> Extra { get; private set; } = new Dictionary<string, string>();


        public Response(Request request)
        {
            this.Request = request;
        }
    }
}
