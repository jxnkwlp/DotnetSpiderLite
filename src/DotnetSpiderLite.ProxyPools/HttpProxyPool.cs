using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetSpiderLite.ProxyPools
{
    public class HttpProxyPool : IHttpProxyPool
    {
        private Dictionary<string, HttpProxyInfo> _proxies = new Dictionary<string, HttpProxyInfo>();  // 临时字典 
        private List<HttpProxyInfo> _proxyQueue = new List<HttpProxyInfo>(); // 可使用的代理队列

        private IHttpProxyFinder _finder;
        private IProxyValidator _proxyValidator;

        private bool _isDispose;
        private int _reuseInterval = 500;

        private static object _proxyQueueLockKey = new object();

        private int _refreshCacheCount = 30;
        private int _refreshInterval = 2 * 1000;

        public HttpProxyPool(IHttpProxyFinder finder, IProxyValidator proxyValidator, int reuseInterval, int refreshInterval = 2 * 1000)
        {
            _finder = finder;
            _reuseInterval = reuseInterval;
            _refreshInterval = refreshInterval;

            if (refreshInterval < 100)
            {
                throw new ArgumentException("'refreshInterval' must greater than 100 ");
            }

            _proxyValidator = proxyValidator;

            Task.Run(() => RefreshProxies());
        }

        /// <summary>
        ///  刷新代理情况
        /// </summary> 
        private void RefreshProxies()
        {
            ThreadCommonPool threadCommonPool = new ThreadCommonPool(4);

            while (!_isDispose)
            {
                if (_proxies.Count < _refreshCacheCount)
                {
                    // 获取所有代理信息
                    var proxies = _finder.GetProxies();

                    if (proxies == null)
                    {
                        return;
                    }

                    foreach (var proxy in proxies)
                    {
                        threadCommonPool.QueueUserWork(proxy,
                            (webProxy) =>
                            {
                                string key = $"{webProxy.Address.Host}:{webProxy.Address.Port}";

                                if (!_proxies.ContainsKey(key) && _proxyValidator.IsAvailable(webProxy))
                                {
                                    var httpProxy = new HttpProxyInfo(webProxy);

                                    httpProxy.ResetFaildCount();
                                    httpProxy.UpdateNextReuseTime(_reuseInterval);

                                    // 加入可用队列
                                    lock (_proxyQueueLockKey)
                                    {
                                        _proxyQueue.Add(httpProxy);

                                        _proxies.Add(key, httpProxy);
                                    }
                                }
                            });
                    }

                }

                Thread.Sleep(_refreshInterval); // 间隔 {refreshInterval} 更新一次
            }
        }


        public HttpProxyInfo GetProxy()
        {
            lock (_proxyQueueLockKey)
            {
                var now = DateTime.Now;

                // 取出 第一个 可用时间 超过当前时间 的代码
                var proxy = _proxyQueue.OrderBy(t => t.NextReuseTime).FirstOrDefault(t => t.NextReuseTime <= now);

                if (proxy != null)
                {
                    proxy.LastUsedTime = DateTime.Now;
                    _proxyQueue.Remove(proxy);
                    _proxies.Remove(proxy.Id);
                }

                return proxy;
            }
        }

        //private IWebProxy ParseProxy(ProxyInfo info)
        //{
        //    if (info.Address.StartsWith("http"))
        //    {
        //        return new WebProxy(info.Address);
        //    }
        //    else if (info.Address.Contains(":"))
        //    {
        //        string host = info.Address.Split(':')[0];
        //        int port = 0;
        //        int.TryParse(info.Address.Split(':')[1], out port);
        //        return new WebProxy(host, port);
        //    }

        //    return null;
        //}



        public void Dispose()
        {
            _isDispose = true;


        }

        public void ReturnProxy(WebProxy proxy, HttpStatusCode statusCode)
        {
            string key = $"{proxy.Address.Host}:{proxy.Address.Port}";
            if (!_proxies.ContainsKey(key))
                return;

            var proxyInfo = _proxies[key];

            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    proxyInfo.ResetFaildCount();
                    proxyInfo.UpdateNextReuseTime(_reuseInterval);
                    break;

                default:
                    proxyInfo.IncreaseFaildCount();
                    proxyInfo.UpdateNextReuseTime(_reuseInterval * proxyInfo.FaildCount);
                    break;
            }

            // 超过 10次 不可用，则丢弃
            if (proxyInfo.FaildCount > 10)
            {
                _proxies.Remove(key);
                return;
            }

            lock (_proxyQueueLockKey)
            {
                _proxyQueue.Add(proxyInfo);
            }
        }
    }
}
