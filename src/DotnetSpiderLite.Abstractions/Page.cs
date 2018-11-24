using DotnetSpiderLite.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotnetSpiderLite
{
    /// <summary>
    ///  表示一个页面体
    /// </summary>
    public class Page
    {
        private string _htmlString;

        /// <summary>
        ///  解析器
        /// </summary>
        public IHtmlElementSelector Selector { get; private set; }

        /// <summary>
        ///  HTTP 响应
        /// </summary>
        public Response Response { get; private set; }

        /// <summary>
        ///  页面字符串内容
        /// </summary> 
        public string Content { get; private set; }

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
        public int MaxRetryTimes { get; set; }

        /// <summary>
        ///  结果数据
        /// </summary>
        public ResultItems ResutItems { get; private set; }


        /// <summary>
        ///  扩展信息
        /// </summary>
        public Dictionary<string, string> Extra { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        ///  新的目标请求
        /// </summary>
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
            GetResponseString();
        }

        /// <summary>
        ///  获取Response的字符串内容
        /// </summary>
        /// <returns></returns>
        public string GetResponseString()
        {
            if (string.IsNullOrEmpty(_htmlString))
            {
                _htmlString = this.Response.Request.Encoding.GetString(Response.Body);
                this.Content = _htmlString;
            }

            return _htmlString;
        }

        /// <summary>
        ///  添加新的请求
        /// </summary> 
        public void AddTargetRequest(string url, string referer = null)
        {
            this.TargetRequests.Add(new Request(new Uri(url)) { Referer = referer });
        }

        /// <summary>
        ///  添加新的请求
        /// </summary> 
        public void AddTargetRequest(Request request)
        {
            this.TargetRequests.Add(request);
        }

        /// <summary>
        ///  添加数据结果
        /// </summary> 
        public void AddResultItem(string key, object value)
        {
            this.ResutItems[key] = value;
        }

        /// <summary>
        ///  设置页面解析器
        /// </summary> 
        public void SetSelector(IHtmlElementSelector htmlElementSelector)
        {
            this.Selector = htmlElementSelector;
        }
    }
}
