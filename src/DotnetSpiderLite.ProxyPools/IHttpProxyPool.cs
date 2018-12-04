using System;
using System.Net;

namespace DotnetSpiderLite.ProxyPools
{
    /// <summary>
    ///  定义代理池
    /// </summary>
    public interface IHttpProxyPool : IDisposable
    {
        /// <summary>
        ///  获取一个代理
        /// </summary> 
        HttpProxyInfo GetProxy();

        /// <summary>
        ///  更新代理使用情况
        /// </summary> 
        void ReturnProxy(WebProxy proxy, HttpStatusCode statusCode);

    }
}
