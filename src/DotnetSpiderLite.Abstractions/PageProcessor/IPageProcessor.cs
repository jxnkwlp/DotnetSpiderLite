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

        /// <summary>
        ///  解析 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        IEnumerable<Request> ExtractRequest(Page page);

        /// <summary>
        ///  处理
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task Process(Page page);

    }
}
