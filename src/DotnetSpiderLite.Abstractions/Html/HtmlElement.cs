using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Abstractions.Html
{
    public class HtmlElement
    {
        public IEnumerable<HtmlElement> Children { get; set; }

        public string InnerHtml { get; set; }

        public string OuterHtml { get; set; }

        public string TagName { get; set; }

        public string ClassName { get; set; }

        public Dictionary<string, string> Attributes { get; set; }

    }
}
