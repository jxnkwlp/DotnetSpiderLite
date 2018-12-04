using DotnetSpiderLite.Downloader;
using System.Net;

namespace DotnetSpiderLite.ProxyPools
{
    public class DownloaderProxyPools : IDownloaderProxy
    {
        private readonly IHttpProxyPool proxyPool;

        public DownloaderProxyPools(IHttpProxyFinder proxyFinder, IProxyValidator proxyValidator, int reuseInterval, int refreshInterval)
        {
            proxyPool = new HttpProxyPool(proxyFinder, proxyValidator, reuseInterval, refreshInterval);
        }

        public WebProxy GetProxy()
        {
            var proxy = proxyPool.GetProxy();

            return proxy == null ? null : proxy.Proxy;
        }

        public void RelaseProxy(WebProxy proxy, Response response)
        {
            proxyPool.ReturnProxy(proxy, (HttpStatusCode)response.StatusCode);
        }
    }
}
