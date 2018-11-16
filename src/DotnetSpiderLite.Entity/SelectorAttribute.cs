using DotnetSpiderLite.Html;
using System;

namespace DotnetSpiderLite.Entity
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class SelectorAttribute : Attribute
    {
        public string Path { get; set; }

        public HtmlSelectorPathType PathType { get; set; }

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
