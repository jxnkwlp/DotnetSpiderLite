using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Entity.Attributes
{
    /// <summary>
    ///  匹配目标URL 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TargetUrlAttribute : Attribute
    {
        /// <summary>
        ///  匹配规则
        /// </summary>
        public string UrlMatch { get; set; }

        public TargetUrlAttribute(string urlMatch)
        {
            this.UrlMatch = urlMatch;
        }
    }
}
