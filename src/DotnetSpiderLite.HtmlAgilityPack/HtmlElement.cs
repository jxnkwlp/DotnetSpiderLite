using DotnetSpiderLite.Html;
using System.Collections.Generic;

namespace DotnetSpiderLite.HtmlAgilityPack
{
    public class HtmlElement : HtmlElementAbstraction
    {
        private HtmlQuery _htmlQuery = new HtmlQuery();

        public override IHtmlElement Selector(string path, HtmlSelectorPathType pathType = HtmlSelectorPathType.XPath)
        {
            return _htmlQuery.Selector(InnerHtml, path, pathType);
        }

        public override IList<IHtmlElement> SelectorAll(string path, HtmlSelectorPathType pathType = HtmlSelectorPathType.XPath)
        {
            return _htmlQuery.SelectorAll(InnerHtml, path, pathType);
        }
    }
}