using DotnetSpiderLite.Logs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Downloader
{
    /// <summary>
    ///  下载器前置处理 
    /// </summary>
    public interface IDownloadBeforeHandle
    {
        ILogger Logger { get; set; }

        void Handle(Request request);
    }
}
