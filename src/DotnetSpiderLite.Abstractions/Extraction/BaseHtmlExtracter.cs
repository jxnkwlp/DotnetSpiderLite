using DotnetSpiderLite.Abstractions.Html;
using System.Collections.Generic;

namespace DotnetSpiderLite.Abstractions.Extraction
{
    public abstract class BaseHtmlExtracter : IHtmlExtracter
    {
        public abstract IEnumerable<IHtmlElement> ExtractAllByCss(string html, string path);

        public virtual IEnumerable<IHtmlElement> ExtractAllByCss(Page page, string path)
        {
            if (string.IsNullOrEmpty(page.Html))
            {
                return null;
            }

            return ExtractAllByCss(page.Html, path);
        }

        public abstract IEnumerable<IHtmlElement> ExtractAllByXPath(string html, string path);

        public virtual IEnumerable<IHtmlElement> ExtractAllByXPath(Page page, string path)
        {
            if (string.IsNullOrEmpty(page.Html))
            {
                return null;
            }

            return ExtractAllByXPath(page.Html, path);
        }

        public abstract IHtmlElement ExtractByCss(string html, string path);

        public virtual IHtmlElement ExtractByCss(Page page, string path)
        {
            if (string.IsNullOrEmpty(page.Html))
            {
                return null;
            }

            return ExtractByCss(page.Html, path);
        }

        public abstract IHtmlElement ExtractByXPath(string html, string path);

        public virtual IHtmlElement ExtractByXPath(Page page, string path)
        {
            if (string.IsNullOrEmpty(page.Html))
            {
                return null;
            }

            return ExtractByXPath(page.Html, path);
        }
    }
}