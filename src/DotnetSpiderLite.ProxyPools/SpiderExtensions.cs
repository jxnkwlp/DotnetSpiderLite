using DotnetSpiderLite.ProxyPools;
using System;
using System.Collections.Generic;
using System.Net;

namespace DotnetSpiderLite
{
    /// <summary>
    ///  扩展
    /// </summary>
    public static class SpiderExtensions
    {
        /// <summary>
        ///  使用代理池
        /// </summary>
        /// <param name="spider"></param>
        /// <param name="proxyFinder">代理获取</param>
        /// <param name="proxyValidator">代理有效验证</param>
        /// <param name="reuseInterval">复用间隔时间(ms)</param>
        /// <returns></returns>
        public static Spider UseHttpProxyPools(this Spider spider, IHttpProxyFinder proxyFinder, IProxyValidator proxyValidator, int reuseInterval, int refreshInterval = 2 * 1000)
        {
            DownloaderProxyPools downloaderProxy = new DownloaderProxyPools(proxyFinder, proxyValidator, reuseInterval, refreshInterval);

            spider.SetDownloaderProxy(downloaderProxy);

            return spider;
        }

        /// <summary>
        ///  使用代理池
        /// </summary> 
        public static Spider UseHttpProxyPools(this Spider spider, IHttpProxyFinder proxyFinder, int reuseInterval, int refreshInterval = 2 * 1000)
        {
            return UseHttpProxyPools(spider, proxyFinder, new DefaultProxyValidator(), reuseInterval, refreshInterval);
        }

        /// <summary>
        ///  使用代理池
        /// </summary> 
        public static Spider UseHttpProxyPools(this Spider spider, IHttpProxyFinder proxyFinder)
        {
            return UseHttpProxyPools(spider, proxyFinder, new DefaultProxyValidator(), 5000);
        }

        /// <summary>
        ///  使用代理池
        /// </summary> 
        public static Spider UseHttpProxyPools(this Spider spider, params WebProxy[] proxies)
        {
            if (proxies == null || proxies.Length == 0)
            {
                throw new ArgumentNullException(nameof(proxies));
            }

            return UseHttpProxyPools(spider, new HttpProxyFinder() { Proxies = proxies }, new DefaultProxyValidator(), 5000);
        }

        /// <summary>
        ///  使用代理池
        /// </summary> 
        public static Spider UseHttpProxyPools(this Spider spider, int reuseInterval, int refreshInterval, params WebProxy[] proxies)
        {
            if (proxies == null || proxies.Length == 0)
            {
                throw new ArgumentNullException(nameof(proxies));
            }

            return UseHttpProxyPools(spider, new HttpProxyFinder() { Proxies = proxies }, new DefaultProxyValidator(), reuseInterval, refreshInterval);
        }

        /// <summary>
        ///  使用代理池
        /// </summary> 
        public static Spider UseHttpProxyPools(this Spider spider, int reuseInterval, params WebProxy[] proxies)
        {
            if (proxies == null || proxies.Length == 0)
            {
                throw new ArgumentNullException(nameof(proxies));
            }

            return UseHttpProxyPools(spider, new HttpProxyFinder() { Proxies = proxies }, new DefaultProxyValidator(), reuseInterval);
        }

    }

    internal class HttpProxyFinder : IHttpProxyFinder
    {
        public IList<WebProxy> Proxies { get; set; }

        public IList<WebProxy> GetProxies()
        {
            return Proxies;
        }
    }
}
