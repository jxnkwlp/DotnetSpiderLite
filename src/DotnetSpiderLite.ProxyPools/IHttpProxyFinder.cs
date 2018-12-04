using System.Collections.Generic;
using System.Net;

namespace DotnetSpiderLite.ProxyPools
{
    /// <summary>
    ///  定义动态获取代理信息
    /// </summary> 
    public interface IHttpProxyFinder
    {
        IList<WebProxy> GetProxies();
    }
}
