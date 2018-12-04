using DotnetSpiderLite.Logs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.Downloader
{
    /// <summary> 
    ///  定义一个下载器 <see cref="IDownloader"/>
    /// </summary>
    public interface IDownloader : IDisposable //, ICloneable
    {
        /// <summary>
        ///  日志
        /// </summary>
        ILogger Logger { get; set; }

        /// <summary>
        ///  指定代理
        /// </summary>
        WebProxy Proxy { get; set; }

        /// <summary>
        ///  执行下载
        /// </summary> 
        Task<Response> DownloadAsync(Request request);

        /// <summary>
        ///  添加下载前置操作
        /// </summary> 
        void AddDownloadBeforeHandle(IDownloadBeforeHandle handle);

        /// <summary>
        ///  添加下载后置操作
        /// </summary> 
        void AddDownloadAfterHandle(IDownloadAfterHandle handle);

        /// <summary>
        ///  复制
        /// </summary> 
        IDownloader Clone();

    }
}
