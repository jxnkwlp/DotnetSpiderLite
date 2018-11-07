using DotnetSpiderLite.Abstractions.Extraction;
using DotnetSpiderLite.Abstractions.Html;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotnetSpiderLite.AngleSharps
{
    public class AngleSharpHtmlExtracter : BaseHtmlExtracter
    {
        public override IEnumerable<IHtmlElement> ExtractAllByCss(string html, string path)
        {
            AngleSharp.Parser.Html.HtmlParser htmlParser = new AngleSharp.Parser.Html.HtmlParser();
            var document = htmlParser.Parse(html);

            var all = document.QuerySelectorAll(path);

            var result = new List<HtmlElement>();

            foreach (var item in all)
            {
                result.Add(Convent(item));
            }

            return result;
        }

        public override IEnumerable<IHtmlElement> ExtractAllByXPath(string html, string path)
        {
            throw new NotSupportedException();
        }

        public override IHtmlElement ExtractByCss(string html, string path)
        {
            AngleSharp.Parser.Html.HtmlParser htmlParser = new AngleSharp.Parser.Html.HtmlParser();
            var document = htmlParser.Parse(html);

            var ele = document.QuerySelector(path);

            return Convent(ele);
        }

        public override IHtmlElement ExtractByXPath(string html, string path)
        {
            throw new NotSupportedException();
        }

        private IHtmlElement Convent(AngleSharp.Dom.IElement element)
        {
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