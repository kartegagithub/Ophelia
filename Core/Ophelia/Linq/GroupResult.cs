using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Linq
{
    public class GroupResult<T> where T : class
    {
        public Expression<Func<T, object>> Key { get; set; }

        public int Count { get; set; }

        public IQueryable<IGrouping<object, T>> Items { get; set; }
        public IQueryable<IGrouping<object, IGrouping<object, T>>> SubGroupItems { get; set; }
        public GroupResult<T> SubGroup { get; set; }

        public override string ToString()

        { return string.Format("{0} ({1})", Key, Count); }
    }
}
