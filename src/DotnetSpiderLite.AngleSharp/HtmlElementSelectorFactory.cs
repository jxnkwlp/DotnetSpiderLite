using DotnetSpiderLite.Html;

namespace DotnetSpiderLite.AngleSharps
{
    public class HtmlElementSelectorFactory : IHtmlElementSelectorFactory
    {
        public IHtmlElementSelector GetSelector(string html)
        {
            return new HtmlElementSelector(html);
        }
    }
}
