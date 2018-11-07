using AngleSharp;
using AngleSharp.Parser.Html;
using DotnetSpiderLite.Abstractions;
using DotnetSpiderLite.Abstractions.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotnetSpiderLite
{
    public static class PageExtensions
    {
        public static HtmlElement SelectSingleByCss(this Page page, string cssPath)
        {
            HtmlParser htmlParser = new HtmlParser();
            var html = htmlParser.Parse(page.Html);

            var node = html.QuerySelector(cssPath);

            return Convent(node);
        }

        public static IList<HtmlElement> SelectAllByCss(this Page page, string cssPath)
        {
            HtmlParser htmlParser = new HtmlParser();
            var html = htmlParser.Parse(page.Html);

            var nodeList = html.QuerySelectorAll(cssPath);

            var result = new List<HtmlElement>();

            foreach (var item in nodeList)
            {
                result.Add(Convent(item));
            }

            return result;
        }

        static HtmlElement Convent(AngleSharp.Dom.IElement element)
        {
            if (element == null)
                return null;

            var ele = new HtmlElement()
            {
                Attributes = element.Attributes.ToDictionary(t => t.Name, t => t.Value),
                ClassName = element.ClassName,
                InnerHtml = element.InnerHtml,
                TagName = element.TagName,
            };

            //if (element.Children != null)
            //{
            //    var c = new List<HtmlElement>();
            //    foreach (var item in element.Children)
            //    {
            //        c.Add(Convent(item));
            //    }

            //    ele.chi
            //}

            return ele;
        }
    }
}
