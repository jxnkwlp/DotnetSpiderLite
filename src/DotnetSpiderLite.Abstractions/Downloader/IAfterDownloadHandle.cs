using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Abstractions.Downloader
{
    public interface IAfterDownloadHandle
    {
        void Handle(Response response);

    }
}
