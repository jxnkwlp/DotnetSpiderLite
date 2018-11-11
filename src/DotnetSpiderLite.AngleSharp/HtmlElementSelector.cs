using DotnetSpiderLite.Html;
using System.Collections.Generic;

namespace DotnetSpiderLite.AngleSharps
{
    public class HtmlElementSelector : IHtmlElementSelector
    {
        HtmlQuery _htmlQuery = new HtmlQuery();

        string _html = string.Empty;


        public HtmlElementSelector(string html)
        {
            _html = html;
        }

        public IHtmlElement Selector(string path, HtmlSelectorPathType pathType = HtmlSelectorPathType.XPath)
        {
            return _htmlQuery.Selector(_html, path, pathType);
        }

        public IList<IHtmlElement> SelectorAll(string path, HtmlSelectorPathType pathType = HtmlSelectorPathType.XPath)
        {
            return _htmlQuery.SelectorAll(_html, path, pathType);
        }

    }
}
