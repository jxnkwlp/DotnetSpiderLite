using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Abstractions.Html
{
    /// <summary>
    ///  Html 查询
    /// </summary>
    public interface IHtmlQuery
    {
        IList<IHtmlElement> SelectorAll(string html, string path, HtmlSelectorPathType pathType = HtmlSelectorPathType.XPath);

        IHtmlElement Selector(string html, string path, HtmlSelectorPathType pathType = HtmlSelectorPathType.XPath);

    }
}
