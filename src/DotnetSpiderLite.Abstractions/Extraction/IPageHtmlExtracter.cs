using DotnetSpiderLite.Abstractions.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Abstractions.Extraction
{
    public interface IPageHtmlExtracter
    {/// <summary>
     ///   提取所有 
     /// </summary>  
        IEnumerable<HtmlElement> ExtractAllByXPath(string path);


        /// <summary>
        ///  提取所有
        /// </summary>  
        IEnumerable<HtmlElement> ExtractAllByCss(string path);

        /// <summary>
        ///  提取
        /// </summary>  
        HtmlElement ExtractByXPath(string path);

        /// <summary>
        ///  提取
        /// </summary> 
        HtmlElement ExtractByCss(string path);
    }
}
