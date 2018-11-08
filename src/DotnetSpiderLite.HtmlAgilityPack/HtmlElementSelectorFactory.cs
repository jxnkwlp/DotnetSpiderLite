using DotnetSpiderLite.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.HtmlAgilityPack
{
    public class HtmlElementSelectorFactory : IHtmlElementSelectorFactory
    {
        public IHtmlElementSelector GetSelector(string html)
        {
            return new HtmlElementSelector(html);
        }
    }
}
