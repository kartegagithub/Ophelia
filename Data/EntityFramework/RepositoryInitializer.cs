using Ophelia.Data.EntityFramework.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Data.EntityFramework
{
    public class RepositoryInitializer : IRepositoryInitializer
    {
        private static bool RepositoryIFilter(Type typeObj, Object criteriaObj)
        {
            if (typeObj.Name.StartsWith(criteriaObj as string))
                return true;
            else
                return false;
        }

        #region IRepositoryInitializer Members

        public IEnumerable<KeyValuePair<string, Type>> Initialize()
        {
            Type recentType = this.GetType();
            IEnumerable<Type> allTypes = recentType.Assembly.GetTypes();
            foreach (var rType in allTypes)
            {
                object[] attributes = rType.GetCustomAttributes(typeof(EntityTypeAttribute), false);
                if (attributes.Length == 0)
                    continue;

                yield return new KeyValuePair<string, Type>((attributes[0] as EntityTypeAttribute).EntityName, rType);
            }
        }

        #endregion
    }

}