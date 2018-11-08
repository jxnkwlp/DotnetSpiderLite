using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Html
{
    public interface IHtmlElementSelectorFactory
    {
        IHtmlElementSelector GetSelector(string html);
    }
}
