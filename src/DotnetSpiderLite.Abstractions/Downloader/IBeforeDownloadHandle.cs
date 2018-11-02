using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Abstractions.Downloader
{
    public interface IBeforeDownloadHandle
    {
        void Handle(DownloadContext context);
    }
}
