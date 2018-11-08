using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Abstractions.Html
{
    /// <summary>
    ///  表示一个 html 元素节点
    /// </summary>
    public interface IHtmlElement : IHtmlElementSelector
    {
        IEnumerable<IHtmlElement> Children { get; set; }

        string InnerHtml { get; set; }

        string OuterHtml { get; set; }

        string InnerText { get; set; }

        string TagName { get; set; }

        string ClassName { get; set; }

        string ID { get; set; }


        Dictionary<string, string> Attributes { get; set; }

    }
}
