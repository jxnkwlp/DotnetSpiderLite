using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DotnetSpiderLite.Downloader
{
    /// <summary>
    ///  单一代理
    /// </summary>
    public class DownloaderProxy : IDownloaderProxy
    {
        private WebProxy _proxy;

        /// <summary>
        ///  单一代理
        /// </summary>
        /// <param name="proxy">指定一个代理</param>
        public DownloaderProxy(WebProxy proxy)
        {
            _proxy = proxy;
        }

        public DownloaderProxy()
        {
        }

        public virtual WebProxy GetProxy()
        {
            return _proxy;
        }

        public virtual void RelaseProxy(WebProxy proxy, Response response)
        {
        }
    }
}
