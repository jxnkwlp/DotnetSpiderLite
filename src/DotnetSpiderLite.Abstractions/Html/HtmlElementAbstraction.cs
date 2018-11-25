using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Html
{
    public abstract class HtmlElementAbstraction : IHtmlElement
    {
        public virtual IEnumerable<IHtmlElement> Children { get; set; }

        public virtual string InnerHtml { get; set; }
        public virtual string OuterHtml { get; set; }
        public virtual string InnerText { get; set; }
        public virtual string TagName { get; set; }
        public virtual string ClassName { get; set; }
        public virtual string ID { get; set; }
        public virtual Dictionary<string, string> Attributes { get; set; }

        public virtual string GetAttribute(string name)
        {
            if (Attributes == null)
                return null;

            if (Attributes.ContainsKey(name))
                return Attributes[name];

            return null;
        }

        public abstract IHtmlElement Selector(string path, HtmlSelectorPathType pathType = HtmlSelectorPathType.XPath);
        public abstract IList<IHtmlElement> SelectorAll(string path, HtmlSelectorPathType pathType = HtmlSelectorPathType.XPath);
    }
}
