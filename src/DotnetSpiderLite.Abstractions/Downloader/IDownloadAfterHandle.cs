using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Downloader
{
    public interface IDownloadAfterHandle
    {
        void Handle(Response response);

    }
}
