//using DotnetSpiderLite.Abstractions.Html;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DotnetSpiderLite.AngleSharps
//{
//    public class AngleSharpHtmlQuery : IHtmlElementQuery
//    {
//        private string _html;

//        public AngleSharpHtmlQuery(string html)
//        {

//        }

//        public IHtmlElement Selector(string path)
//        {
//            AngleSharp.Parser.Html.HtmlParser htmlParser = new AngleSharp.Parser.Html.HtmlParser();
//            var document = htmlParser.Parse(_html);


//        }

//        public IEnumerable<IHtmlElement> SelectorAll(string path)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
