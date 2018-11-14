using DotnetSpiderLite.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotnetSpiderLite
{
    /// <summary>
    ///  define an response page  
    /// </summary>
    public class Page
    {
        public IHtmlElementSelector Selector { get; private set; }

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

        /// <summary>
        ///  重试最大次数，0始终重试
        /// </summary>
        public int MaxRetryCount { get; set; }


        public ResultItems ResutItems { get; private set; }


        /// <summary>
        ///  extra info 
        /// </summary>
        public Dictionary<string, string> Extra { get; private set; } = new Dictionary<string, string>();


        public HashSet<Request> TargetRequests { get; } = new HashSet<Request>();

        public Page(Response response)
        {
            Response = response;

            ResutItems = new ResultItems(this);

            Init();
        }

        public Page(Response response, IHtmlElementSelector elementSelector) : this(response)
        {
            this.Selector = elementSelector;
        }

        private void Init()
        {
            //var bytes = new byte[Response.Body.Length];
            //Response.Body.Read(bytes, 0, (int)Response.Body.Length);

            //this.Html = this.Response.Request.Encoding.GetString(bytes);

            using (StreamReader sr = new StreamReader(Response.Body, this.Response.Request.Encoding))
            {
                this.Html = sr.ReadToEnd();
            }

        }


        public void AddTargetRequest(string url, string referer = null)
        {
            this.TargetRequests.Add(new Request(new Uri(url)) { Referer = referer });
        }

        public void AddTargetRequest(Request request)
        {
            this.TargetRequests.Add(request);
        }


        public void AddResultItem(string key, object value)
        {
            this.ResutItems[key] = value;
        }

        public void SetSelector(IHtmlElementSelector htmlElementSelector)
        {
            this.Selector = htmlElementSelector;
        }
    }
}
