using DotnetSpiderLite.Entity.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotnetSpiderLite.Entity
{
    public class EntityInterpreter : IEntityInterpreter
    {
        public EntityDefine Handle<TEntity>(TEntity entity) where TEntity : IEntity
        {
            EntityDefine define = new EntityDefine()
            {
                Properties = new List<EntityDefine.Property>(),
                Entity = entity,
            };

            var targetUrls = new List<string>();
            var helpUrls = new List<string>();

            var entityAttributes = System.Attribute.GetCustomAttributes(entity.GetType());

            foreach (var attribute in entityAttributes)
            {
                if (attribute is TargetUrlAttribute targetUrlAttribute)
                {
                    targetUrls.Add(targetUrlAttribute.UrlMatch);
                }
                else if (attribute is HelperUrlAttribute helperUrlAttribute)
                {
                    helpUrls.Add(helperUrlAttribute.UrlMatch);
                }

            }

            define.TargetUrls = targetUrls.ToArray();
            define.HelperUrls = helpUrls.ToArray();

            var properties = entity.GetType().GetProperties().Where(t => t.CanRead && t.CanWrite).ToList();

            foreach (var property in properties)
            {
                EntityDefine.Property propertyDefine = new EntityDefine.Property()
                {
                    Name = property.Name,
                    Type = property.PropertyType,
                };

                if (propertyDefine.Selectors == null)
                    propertyDefine.Selectors = new List<SelectorAttribute>();

                var attributes = property.GetCustomAttributes(true);

                foreach (var attribute in attributes)
                {
                    if (attribute is SelectorAttribute selectorAttribute)
                    {
                        propertyDefine.Selectors.Add(selectorAttribute);
                    }
                }

                define.Properties.Add(propertyDefine);
            }

            return define;
        }
    }
}
