using DotnetSpiderLite.Abstractions.Downloader;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Abstractions.Extraction
{
    /// <summary>
    ///  页面解析
    /// </summary>
    public interface IPageExtract
    {
        IEnumerable<Request> Extract(Page page);

    }
}
