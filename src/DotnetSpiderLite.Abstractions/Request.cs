using DotnetSpiderLite.Abstractions.Downloader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotnetSpiderLite.Abstractions
{
    public class Request
    {
        public string Referer { get; set; }
        public string Connection { get; set; }
        public string AcceptLanguage { get; set; } = "zh-CN,zh;q=0.9,en;q=0.8";
        public string AcceptEncoding { get; set; }
        public string Accept { get; set; } = "text/html";
        public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36";
        public string ContentType { get; set; }

        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public string Method { get; set; }


        public Uri Uri { get; }
        public Stream Body { get; set; }

        public Dictionary<string, object> Headers { get; private set; } = new Dictionary<string, object>();


        public readonly Dictionary<string, object> Properties = new Dictionary<string, object>();





        public Request(Uri uri)
        {
            this.Uri = uri;
        }
    }
}
