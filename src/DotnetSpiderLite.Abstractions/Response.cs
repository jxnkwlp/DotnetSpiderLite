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
        /// <summary>
        ///  HTTP 请求
        /// </summary>
        public Request Request { get; private set; }

        /// <summary>
        ///  响应URI
        /// </summary>
        public Uri ResponseUri { get; set; }

        /// <summary>
        ///  响应 body 内容
        /// </summary>
        public byte[] Body { get; set; }

        /// <summary>
        ///  ContentType
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///  响应状态码
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        ///  响应内容长度
        /// </summary>
        public long ContentLength { get; set; } = -1;

        /// <summary>
        ///  扩展
        /// </summary>
        public Dictionary<string, string> Extra { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        ///  响应 cookies 集合
        /// </summary>
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
