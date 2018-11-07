using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Abstractions.Html
{
    public interface IHtmlElement //: IHtmlElementQuery
    {
        // IEnumerable<IHtmlElement> Children { get; set; }

        string InnerHtml { get; set; }

        string OuterHtml { get; set; }

        string TagName { get; set; }


        string ClassName { get; set; }

        string ID { get; set; }


        Dictionary<string, string> Attributes { get; set; }

    }
}
