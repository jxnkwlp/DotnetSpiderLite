using DotnetSpiderLite.Downloader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DotnetSpiderLite
{
    /// <summary>
    ///  表示 HTTP 请求
    /// </summary>
    public class Request
    {
        public string Referer { get; set; }
        public bool KeepAlive { get; set; } = true;
        public string AcceptLanguage { get; set; } = "zh-CN,zh;q=0.9,en;q=0.8";
        public string AcceptEncoding { get; set; }
        public string Accept { get; set; } = "text/html";
        public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36";
        public string ContentType { get; set; } = "text/html; charset=utf-8";

        /// <summary>
        ///  编码。默认 UTF8
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>
        ///  默认Get
        /// </summary>
        public string Method { get; set; } = "GET";

        /// <summary>
        ///  请求URI
        /// </summary>
        public Uri Uri { get; }

        public byte[] Body { get; set; }

        public Dictionary<string, string> Headers { get; private set; } = new Dictionary<string, string>();

        public Dictionary<string, string> Extra { get; private set; } = new Dictionary<string, string>();


        public CookieContainer CookieContainer { get; private set; }

        public string CookieHeader
        {
            get
            {
                if (CookieContainer == null)
                    return null;

                return CookieContainer.GetCookieHeader(this.Uri);
            }
        }


        public Request(Uri uri)
        {
            this.Uri = uri;
            this.Method = "GET";
            this.CookieContainer = DownloaderCookieContainer.Instance;
        }


        public void AddCookie(string name, string value)
        {
            CookieContainer.Add(this.Uri, new Cookie(name, value));
        }

        public void AddCookie(Cookie cookie)
        {
            CookieContainer.Add(this.Uri, cookie);
        }


        public override string ToString()
        {
            return $"{this.Method.ToUpper()} {this.Uri}";
        }



        //public virtual string GetIdentity()
        //{

        //}

    }
}
