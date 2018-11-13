using DotnetSpiderLite.Logs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Downloader
{
    public interface IDownloadBeforeHandle
    {
        ILogger Logger { get; set; }

        void Handle(Request request);
    }
}
