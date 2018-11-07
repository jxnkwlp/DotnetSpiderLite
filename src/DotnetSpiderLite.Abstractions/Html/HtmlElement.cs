using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Abstractions.Html
{
    public class HtmlElement // : IHtmlElement
    {
        public string InnerHtml { get; set; }

        public string OuterHtml { get; set; }

        public string TagName { get; set; }


        public string ID { get; set; }
        public string ClassName { get; set; }

        public Dictionary<string, string> Attributes { get; set; }


        //public IEnumerable<IHtmlElement> SelectorAll(string path)
        //{
        //    if (Query == null)
        //        throw new NotSupportedException();

        //    return this.Query.SelectorAll(path);
        //}

        //public IHtmlElement Selector(string path)
        //{
        //    if (Query == null)
        //        throw new NotSupportedException();

        //    return this.Query.Selector(path);
        //}
    }
}
