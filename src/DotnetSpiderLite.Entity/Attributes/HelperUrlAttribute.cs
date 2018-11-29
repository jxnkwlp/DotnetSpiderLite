using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Entity.Attributes
{
    /// <summary>
    ///  发现URL
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class HelperUrlAttribute : Attribute
    {
        /// <summary>
        ///  匹配规则
        /// </summary>
        public string UrlMatch { get; set; }

        public HelperUrlAttribute(string urlMatch)
        {
            this.UrlMatch = urlMatch;
        }


    }
}
