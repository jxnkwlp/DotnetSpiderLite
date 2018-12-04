using System;
using System.Collections.Generic;
using System.Net;

namespace DotnetSpiderLite.Downloader
{
    /// <summary>
    ///  简单的代理池，随机使用其中一个代理
    /// </summary>
    public class SimpleDownloaderProxyPools : IDownloaderProxy
    {
        private List<WebProxy> _proxies = new List<WebProxy>();
        private Random _random = new Random(Guid.NewGuid().GetHashCode());
        private object _lockkey = new object();

        /// <summary>
        ///  简单的代理池，随机使用其中一个代理
        /// </summary>
        /// <param name="proxies">指定一些代理</param>
        public SimpleDownloaderProxyPools(params WebProxy[] proxies)
        {
            if (proxies != null && proxies.Length > 0)
            {
                _proxies.AddRange(proxies);
            }
        }

        public virtual WebProxy GetProxy()
        {
            lock (_lockkey)
            {
                var index = _random.Next(_proxies.Count);

                return _proxies[index];
            }
        }

        public virtual void RelaseProxy(WebProxy proxy, Response response)
        {
        }

    }
}
