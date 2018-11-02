using DotnetSpiderLite.Abstractions.Extraction;
using DotnetSpiderLite.Abstractions.Logs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.Abstractions.PageProcessor
{
    /// <summary>
    ///  页面处理器
    /// </summary>
    public interface IPageProcessor : IDisposable
    {
        ILogger Logger { get; set; }

        //IPageExtraction PageExtraction { get; set; }

        Task Process(Page page);

    }
}
