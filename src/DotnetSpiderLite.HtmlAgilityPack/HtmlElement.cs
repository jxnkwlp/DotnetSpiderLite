using DotnetSpiderLite.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.HtmlAgilityPack
{
    public class HtmlElement : IHtmlElement
    {
        HtmlQuery _htmlQuery = new HtmlQuery();

        public HtmlElement()
        {
        }

        public IEnumerable<IHtmlElement> Children { get; set; }

        public string InnerHtml { get; set; }
        public string OuterHtml { get; set; }
        public string InnerText { get; set; }
        public string TagName { get; set; }
        public string ClassName { get; set; }
        public string ID { get; set; }
        public Dictionary<string, string> Attributes { get; set; }

        public IHtmlElement Selector(string path, HtmlSelectorPathType pathType = HtmlSelectorPathType.XPath)
        {
            return _htmlQuery.Selector(InnerHtml, path, pathType);
        }

        public IList<IHtmlElement> SelectorAll(string path, HtmlSelectorPathType pathType = HtmlSelectorPathType.XPath)
        {
            return _htmlQuery.SelectorAll(InnerHtml, path, pathType);
        }
    }
}
