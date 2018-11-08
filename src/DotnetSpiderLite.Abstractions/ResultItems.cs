using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite
{
    /// <summary>
    ///  页面解析的数据结果
    /// </summary>
    public class ResultItems : Dictionary<string, object>, IDictionary<string, object>
    {
        public Page Page { get; private set; }

        public ResultItems(Page page)
        {
            this.Page = page;
        }
    }
}
