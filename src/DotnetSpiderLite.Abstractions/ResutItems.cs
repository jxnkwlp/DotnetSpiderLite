using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Abstractions
{
    /// <summary>
    ///  页面解析的数据结果
    /// </summary>
    public class ResutItems : Dictionary<string, object>, IDictionary<string, object>
    {
        public ResutItems(Page page)
        {

        }
    }
}
