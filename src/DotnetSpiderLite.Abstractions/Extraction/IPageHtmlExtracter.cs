using DotnetSpiderLite.Abstractions.Downloader;
using DotnetSpiderLite.Abstractions.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Abstractions.Extraction
{
    /// <summary>
    ///  页面解析
    /// </summary>
    public interface IPageHtmlExtracter
    {
        /// <summary>
        ///   提取所有 
        /// </summary> 
        IEnumerable<HtmlElement> ExtractAllByXPath(Page page, string path);


        /// <summary>
        ///  提取所有
        /// </summary> 
        IEnumerable<HtmlElement> ExtractAllByCss(Page page, string path);

        /// <summary>
        ///  提取
        /// </summary> 
        HtmlElement ExtractByXPath(Page page, string path);

        /// <summary>
        ///  提取
        /// </summary> 
        HtmlElement ExtractByCss(Page page, string path);

    }
}
