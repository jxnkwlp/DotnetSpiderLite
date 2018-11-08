using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Downloader
{
    public interface IBeforeDownloadHandle
    {
        void Handle(Request request);
    }
}
