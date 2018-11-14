using DotnetSpiderLite.Downloader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DotnetSpiderLite
{
    /// <summary>
    ///  表示 HTTP 响应
    /// </summary>
    public class Response
    {
        public Request Request { get; private set; }

        public Uri ResponseUri { get; set; }

        public Stream Body { get; set; }

        public string ContentType { get; set; }

        public int StatusCode { get; set; }

        public long ContentLength { get; set; }

        public Dictionary<string, string> Extra { get; private set; } = new Dictionary<string, string>();


       // public string ResponseCookie { get; set; }

        public CookieCollection ResponseCookies { get; set; }

        public Response(Request request)
        {
            this.Request = request;
        }

        public override string ToString()
        {
            return $"Length:{ContentLength}, ContentType:{ContentType}, ResponseUrl:{ResponseUri}";
        }
    }
}
