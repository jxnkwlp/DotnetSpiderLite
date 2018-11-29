using DotnetSpiderLite.Entity.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Entity
{
    public class EntityDefine
    {
        public IEntity Entity { get; set; }

        public string[] HelperUrls { get; set; }

        public string[] TargetUrls { get; set; }



        public IList<Property> Properties { get; set; }

        public class Property
        {
            public Type Type { get; set; }

            public string Name { get; set; }

            public IList<SelectorAttribute> Selectors { get; set; }
        }
    }

    public class SelectorAttributeData
    {

    }
}
