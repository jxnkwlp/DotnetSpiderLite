using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using DotnetSpiderLite.Html;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotnetSpiderLite.AngleSharps
{
    public class HtmlQuery : IHtmlQuery
    {
        HtmlParser _htmlParser = new HtmlParser();

        public IHtmlElement Selector(string html, string path, HtmlSelectorPathType pathType = HtmlSelectorPathType.XPath)
        {
            var dom = _htmlParser.Parse(html);
            if (pathType == HtmlSelectorPathType.Css)
            {
                var node = dom.QuerySelector(path);

                return Convent(node);
            }
            else
            {
                throw new NotSupportedException();
            }

        }

        public IList<IHtmlElement> SelectorAll(string html, string path, HtmlSelectorPathType pathType = HtmlSelectorPathType.XPath)
        {
            var dom = _htmlParser.Parse(html);

            if (pathType == HtmlSelectorPathType.Css)
            {
                var node = dom.QuerySelectorAll(path);

                return Convent(node);
            }
            else
            {
                throw new NotSupportedException();
            }
        }


        IHtmlElement Convent(AngleSharp.Dom.IElement element)
        {
            if (element == null)
                return null;

            var ele = new HtmlElement()
            {
                Attributes = element.Attributes.ToDictionary(t => t.Name, t => t.Value),
                ClassName = element.ClassName,
                ID = element.Id,
                InnerHtml = element.InnerHtml,
                OuterHtml = element.OuterHtml,
                InnerText = element.TextContent,
                TagName = element.TagName,
            };
            return ele;
        }

        IList<IHtmlElement> Convent(IHtmlCollection<IElement> collection)
        {
            if (collection == null)
                return null;

            var result = new List<IHtmlElement>();

            foreach (var item in collection)
            {
                result.Add(Convent(item));
            }

            return result;
        }


    }
}
