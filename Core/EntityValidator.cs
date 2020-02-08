using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LoliSQLLib.DataAttributes.ValidationAttribute;

namespace LoliSQLLib.Core 
{
    public static class EntityValidator<TEntity>
    {
        public static bool IsValid(TEntity entity, EntityValidtionContext<TEntity> context, List<String> errorMessages)
        {
            foreach (var property in context.Properties)
            {
                if(!IsValidProperty(entity, property.Name, context, errorMessages))
                    return false;
            }
            return true;
        }

        public static bool IsValidProperty(TEntity entity, string propertyName , EntityValidtionContext<TEntity> context, List<String> errorMessages)
        {
            PropertyInfo property = context.Properties.First(prop => prop.Name.Equals(propertyName));
            var attrs =  property.GetCustomAttributes(false).Where(a => a is IValidationAttribute);
            if (attrs.Count() > 0)
            {
                foreach (IValidationAttribute attr in attrs)
                {
                    if(attr.IsValid(property.GetValue(entity)))
                        return true;
                }
                return false;

            }
            else return true;
        }
    }
}