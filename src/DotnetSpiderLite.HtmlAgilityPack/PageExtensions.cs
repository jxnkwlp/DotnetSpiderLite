//using DotnetSpiderLite;
//using HtmlAgilityPack;
//using System;

//namespace DotnetSpiderLite.HtmlAgilityPack
//{
//    public static class PageExtensions
//    {
//        //public static HtmlDocument ToHtmlDocument(this string html)
//        //{
//        //    var doc = new HtmlDocument();

//        //    doc.LoadHtml(html);

//        //    return doc;
//        //}


//        public static string SelectSingle(this Page page, string xpath)
//        {
//            HtmlDocument htmlDocument = new HtmlDocument();

//            htmlDocument.LoadHtml(page.Html);

//            return htmlDocument.DocumentNode.SelectSingleNode(xpath)?.InnerHtml;
//        }
//    }
//}
