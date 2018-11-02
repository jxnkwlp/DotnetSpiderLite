using DotnetSpiderLite.Abstractions.Downloader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotnetSpiderLite.Abstractions
{
    public class Page
    {
        public Response Response { get; private set; }

        public string Html
        {
            get
            {
                var bytes = new byte[Response.Body.Length];
                Response.Body.Read(bytes, 0, (int)Response.Body.Length);

                return Encoding.UTF8.GetString(bytes);
            }
        }

        public bool Skip { get; set; }

        public ResultItems ResutItems { get; private set; }


        public HashSet<Request> TargetRequests { get; } = new HashSet<Request>();

        public Page(Response response)
        {
            this.Response = response;
            ResutItems = new ResultItems(this);

        }


        public void AddTargetRequest(string url)
        {
            this.TargetRequests.Add(new Request(Response.DownloadContext, new Uri(url)));
        }


        public void AddResultItem(string key, object value)
        {
            this.ResutItems[key] = value;
        }




    }
}
