using AngleSharp;
using AngleSharp.Parser.Html;
using DotnetSpiderLite.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite
{
    public static class PageExtensions
    {
        public static string SelectSingleByCss(this Page page, string cssPath)
        {
            HtmlParser htmlParser = new HtmlParser();
            var html = htmlParser.Parse(page.Html);
             
            return html.QuerySelector(cssPath)?.InnerHtml;
        }


    }
}
