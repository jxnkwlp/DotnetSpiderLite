using DotnetSpiderLite.Html;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        IHtmlElement Convent(HtmlNode node)
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

        IList<IHtmlElement> Convent(HtmlNodeCollection nodes)
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

        IList<IHtmlElement> Convent(IEnumerable<HtmlNode> nodes)
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
