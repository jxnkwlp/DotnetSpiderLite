using DotnetSpiderLite.Logs;
using System;
using System.Threading.Tasks;

namespace DotnetSpiderLite.PageProcessor
{
    /// <summary>
    ///  页面处理器 <see cref="IPageProcessor"/>
    /// </summary>
    public interface IPageProcessor : IDisposable
    {
        ILogger Logger { get; set; }

        /// <summary>
        ///  处理
        /// </summary> 
        void Process(Page page);

    }
}
