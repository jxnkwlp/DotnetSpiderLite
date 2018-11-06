using DotnetSpiderLite.Abstractions.Downloader;
using DotnetSpiderLite.Abstractions.Extraction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotnetSpiderLite.Abstractions
{
    public class Page
    {
        public IPageHtmlExtracter PageExtracter { get; set; }

        public Response Response { get; private set; }

        public string Html { get; private set; }

        /// <summary>
        ///  是否跳过
        /// </summary>
        public bool Skip { get; set; }

        /// <summary>
        ///  是否重试
        /// </summary>
        public bool Retry { get; set; }


        public ResultItems ResutItems { get; private set; }


        public HashSet<Request> TargetRequests { get; } = new HashSet<Request>();

        public Page(Response response)
        {
            Response = response;

            ResutItems = new ResultItems(this);

            Init();
        }

        private void Init()
        {
            var bytes = new byte[Response.Body.Length];
            Response.Body.Read(bytes, 0, (int)Response.Body.Length);

            this.Html = Encoding.UTF8.GetString(bytes);
        }


        public void AddTargetRequest(string url)
        {
            this.TargetRequests.Add(new Request(new Uri(url)));
        }

        public void AddTargetRequest(Request request)
        {
            this.TargetRequests.Add(request);
        }


        public void AddResultItem(string key, object value)
        {
            this.ResutItems[key] = value;
        }




    }
}
