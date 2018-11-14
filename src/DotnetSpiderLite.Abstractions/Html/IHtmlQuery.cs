using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Html
{
    /// <summary>
    ///  Html 查询
    /// </summary>
    public interface IHtmlQuery
    {
        /// <summary>
        ///  查询所有
        /// </summary> 
        IList<IHtmlElement> SelectorAll(string html, string path, HtmlSelectorPathType pathType = HtmlSelectorPathType.XPath);

        /// <summary>
        ///  查询一个
        /// </summary> 
        IHtmlElement Selector(string html, string path, HtmlSelectorPathType pathType = HtmlSelectorPathType.XPath);

    }
}
