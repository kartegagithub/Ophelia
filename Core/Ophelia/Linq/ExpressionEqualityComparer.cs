using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia
{
    public class ExpressionEqualityComparer<T> : IEqualityComparer<T>
    {
        readonly Func<T, T, bool> equalsExpression;
        readonly Func<T, int> hashCodeExpression;

        public ExpressionEqualityComparer(Func<T, T, bool> equalsExpression, Func<T, int> hashCodeExpression)
        {
            this.equalsExpression = equalsExpression;
            this.hashCodeExpression = hashCodeExpression;
        }

        public bool Equals(T x, T y)
        {
            return this.equalsExpression(x, y);
        }

        public int GetHashCode(T obj)
        {
            return this.hashCodeExpression(obj);
        }
    }
}
