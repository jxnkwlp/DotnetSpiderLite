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
        IList<IHtmlElement> SelectorAll(string path, HtmlSelectorPathType pathType = HtmlSelectorPathType.XPath);

        IHtmlElement Selector(string path, HtmlSelectorPathType pathType = HtmlSelectorPathType.XPath);

    }

}
