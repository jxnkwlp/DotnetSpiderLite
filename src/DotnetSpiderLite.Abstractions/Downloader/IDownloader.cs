using DotnetSpiderLite.Logs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.Downloader
{
    /// <summary> 
    ///  Downloader 
    /// </summary>
    public interface IDownloader : IDisposable //, ICloneable
    {
        ILogger Logger { get; set; }


        Task<Response> DownloadAsync(Request request);

        void AddBeforeDownloadHandle(IBeforeDownloadHandle handle);

        void AddAfterDownloadHandle(IAfterDownloadHandle handle);


        IDownloader Clone();

    }
}
