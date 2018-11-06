using DotnetSpiderLite.Abstractions;
using DotnetSpiderLite.Abstractions.Extraction;
using DotnetSpiderLite.Abstractions.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.AngleSharps
{
    public class AngleSharpHtmlExtracter : IPageHtmlExtracter
    {
        public IEnumerable<HtmlElement> ExtractAllByCss(Page page, string path)
        {
            AngleSharp.Parser.Html.HtmlParser htmlParser = new AngleSharp.Parser.Html.HtmlParser();
            var html = htmlParser.Parse(page.Html);

            var all = html.QuerySelectorAll(path);

            var result = new List<HtmlElement>();

            foreach (var item in all)
            {
                result.Add(Convent(item));
            }

            return result;
        }

        public IEnumerable<HtmlElement> ExtractAllByXPath(Page page, string path)
        {
            throw new NotSupportedException();
        }

        public HtmlElement ExtractByCss(Page page, string path)
        {
            AngleSharp.Parser.Html.HtmlParser htmlParser = new AngleSharp.Parser.Html.HtmlParser();
            var html = htmlParser.Parse(page.Html);

            var ele = html.QuerySelector(path);

            return Convent(ele);
        }

        public HtmlElement ExtractByXPath(Page page, string path)
        {
            throw new NotSupportedException();
        }


        HtmlElement Convent(AngleSharp.Dom.IElement element)
        {
            var ele = new HtmlElement()
            {
                Attributes = element.Attributes.ToDictionary(t => t.Name, t => t.Value),
                ClassName = element.ClassName,
                InnerHtml = element.InnerHtml,
                TagName = element.TagName,

            };

            if (element.Children != null)
            {
                var c = new List<HtmlElement>();
                foreach (var item in element.Children)
                {
                    c.Add(Convent(item));
                }
            }

            return ele;
        }

    }
}
