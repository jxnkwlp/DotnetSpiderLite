using DotnetSpiderLite.Html;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;

namespace DotnetSpiderLite.HtmlAgilityPack
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
