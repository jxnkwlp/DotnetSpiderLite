using DotnetSpiderLite.Logs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.Downloader
{
    /// <summary> 
    ///  定义一个下载器 <see cref="IDownloader"/>
    /// </summary>
    public interface IDownloader : IDisposable //, ICloneable
    {
        ILogger Logger { get; set; }

        Task<Response> DownloadAsync(Request request);

        void AddDownloadBeforeHandle(IDownloadBeforeHandle handle);

        void AddDownloadAfterHandle(IDownloadAfterHandle handle);
         
        IDownloader Clone();

    }
}
