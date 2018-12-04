using System.Net;

namespace DotnetSpiderLite.ProxyPools
{
    /// <summary>
    ///  代理可用检测
    /// </summary>
    public interface IProxyValidator
    {
        bool IsAvailable(WebProxy proxy);
    }
}
