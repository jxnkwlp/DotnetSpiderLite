using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Downloader
{
    public interface IDownloadBeforeHandle
    {
        void Handle(Request request);
    }
}
