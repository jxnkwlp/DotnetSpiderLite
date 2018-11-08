using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Downloader
{
    public interface IAfterDownloadHandle
    {
        void Handle(Response response);

    }
}
