using System.Net;

namespace DotnetSpiderLite.Downloader
{
    /// <summary>
    ///  定义下载器代理相关
    /// </summary>
    public interface IDownloaderProxy
    {
        /// <summary>
        ///  获取一个代码
        /// </summary> 
        WebProxy GetProxy();

        /// <summary>
        ///  释放代理
        /// </summary> 
        void RelaseProxy(WebProxy proxy, Response response);

    }
}
