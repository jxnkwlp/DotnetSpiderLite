using DotnetSpiderLite.Html;
using System;

namespace DotnetSpiderLite.Entity.Attributes
{
    /// <summary>
    ///  元素选择器
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class SelectorAttribute : Attribute
    {
        public string Path { get; set; }

        public HtmlSelectorPathType PathType { get; set; }

        public SelectorMatch SelectorMatch { get; set; }

        public string FromAttribute { get; set; }


        public SelectorAttribute(string path)
        {
            this.Path = path;
        }

        public SelectorAttribute(string path, HtmlSelectorPathType pathType) : this(path)
        {
            this.PathType = pathType;
        }
    }

}
