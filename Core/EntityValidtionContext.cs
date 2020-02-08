using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LoliSQLLib.Core
{
    public class EntityValidtionContext<IEntity>
    {
        public Type TypeOfEntity { get; private set; }
        public List<PropertyInfo> Properties { get; private set; }

        public EntityValidtionContext(IEntity entity)
        {
            TypeOfEntity = entity.GetType();
            Properties = TypeOfEntity.GetProperties().ToList();
        }
    }
}