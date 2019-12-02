using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Data.EntityFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class EntityTypeAttribute : Attribute
    {
        public Type EntityType { get; private set; }

        public string EntityName { get; private set; }

        public EntityTypeAttribute(Type entityType)
        {
            this.EntityType = entityType;
            this.EntityName = entityType.FullName;
        }
    }
}
