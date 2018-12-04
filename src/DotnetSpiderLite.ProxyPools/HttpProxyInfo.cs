using System;
using System.Net;

namespace DotnetSpiderLite.ProxyPools
{
    public class HttpProxyInfo
    {
        public string Id { get; }

        public WebProxy Proxy { get; }

        public DateTime? LastUsedTime { get; set; }

        public DateTime? NextReuseTime { get; set; }

        public int FaildCount { get; private set; }

        public HttpProxyInfo(WebProxy proxy)
        {
            this.Id = proxy.Address.Host + ":" + proxy.Address.Port;
            this.Proxy = proxy;
        }

        public void ResetFaildCount()
        {
            this.FaildCount = 0;
        }

        public void IncreaseFaildCount()
        {
            this.FaildCount++;
        }

        public void UpdateNextReuseTime(int interval)
        {
            if (LastUsedTime.HasValue)
            {
                NextReuseTime = LastUsedTime.Value.AddMilliseconds(interval);
            }
            else
            {
                NextReuseTime = DateTime.Now.AddMilliseconds(interval);
            }

        }
    }
}
