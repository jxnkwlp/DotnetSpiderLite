using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Html
{
    /// <summary>
    ///  获取 html 查询器
    /// </summary> 
    public interface IHtmlElementSelectorFactory
    {
        IHtmlElementSelector GetSelector(string html);
    }
}
