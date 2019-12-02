using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ophelia
{
    public class NameEqualityComparer : IEqualityComparer<string>
    {
        public static IEqualityComparer<string> Default { get { return new NameEqualityComparer(); } }

        public bool Equals(string x, string y)
        {
            return x.Equals(y, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}
