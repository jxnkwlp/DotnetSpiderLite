using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Html
{
    /// <summary>
    ///  表示一个 html 元素节点
    /// </summary>
    public interface IHtmlElement : IHtmlElementSelector
    {
        /// <summary>
        ///  子元素
        /// </summary>
        IEnumerable<IHtmlElement> Children { get; set; }

        /// <summary>
        ///  内HTML
        /// </summary>
        string InnerHtml { get; set; }

        /// <summary>
        ///  HTML 包含自己
        /// </summary>
        string OuterHtml { get; set; }

        /// <summary>
        ///  text 文本
        /// </summary>
        string InnerText { get; set; }

        /// <summary>
        ///  TagName
        /// </summary>
        string TagName { get; set; }

        /// <summary>
        ///  class 
        /// </summary>
        string ClassName { get; set; }

        /// <summary>
        ///  id
        /// </summary>
        string ID { get; set; }

        /// <summary>
        ///  属性
        /// </summary>
        Dictionary<string, string> Attributes { get; set; }

        /// <summary>
        ///  获取属性值
        /// </summary> 
        string GetAttribute(string name);
    }
}
