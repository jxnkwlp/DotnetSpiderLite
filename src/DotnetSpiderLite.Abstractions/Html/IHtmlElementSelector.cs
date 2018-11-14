using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Html
{
    /// <summary>
    ///  html 查询器
    /// </summary>
    public interface IHtmlElementSelector
    {
        /// <summary>
        ///  查询所有
        /// </summary> 
        IList<IHtmlElement> SelectorAll(string path, HtmlSelectorPathType pathType = HtmlSelectorPathType.XPath);

        /// <summary>
        ///  查询一个
        /// </summary> 
        IHtmlElement Selector(string path, HtmlSelectorPathType pathType = HtmlSelectorPathType.XPath);

    }

}
