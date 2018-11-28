using DotnetSpiderLite.Html;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotnetSpiderLite.HtmlAgilityPack
{
    public class HtmlQuery : IHtmlQuery
    {
        public IHtmlElement Selector(string html, string path, HtmlSelectorPathType pathType = HtmlSelectorPathType.XPath)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            if (pathType == HtmlSelectorPathType.XPath)
            {
                var node = htmlDocument.DocumentNode.SelectSingleNode(path);

                return Convent(node);
            }
            else if (pathType == HtmlSelectorPathType.Css)
            {
                var node = htmlDocument.DocumentNode.QuerySelector(path);

                return Convent(node);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public IList<IHtmlElement> SelectorAll(string html, string path, HtmlSelectorPathType pathType = HtmlSelectorPathType.XPath)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            if (pathType == HtmlSelectorPathType.XPath)
            {
                var node = htmlDocument.DocumentNode.SelectNodes(path);

                return Convent(node);
            }
            else if (pathType == HtmlSelectorPathType.Css)
            {
                var node = htmlDocument.DocumentNode.QuerySelectorAll(path);

                return Convent(node);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private IHtmlElement Convent(HtmlNode node)
        {
            if (node == null)
                return null;

            var ele = new HtmlElement()
            {
                Attributes = node.Attributes.ToDictionary(t => t.Name, t => t.Value),
                ClassName = string.Join(",", node.GetClasses()),
                ID = node.Id,
                InnerHtml = node.InnerHtml,
                OuterHtml = node.OuterHtml,
                InnerText = node.InnerText,
                TagName = node.Name,
            };
            return ele;
        }

        private IList<IHtmlElement> Convent(HtmlNodeCollection nodes)
        {
            if (nodes == null)
                return null;

            var result = new List<IHtmlElement>();

            foreach (var item in nodes)
            {
                result.Add(Convent(item));
            }

            return result;
        }

        private IList<IHtmlElement> Convent(IEnumerable<HtmlNode> nodes)
        {
            if (nodes == null)
                return null;

            var result = new List<IHtmlElement>();

            foreach (var item in nodes)
            {
                result.Add(Convent(item));
            }

            return result;
        }
    }
}