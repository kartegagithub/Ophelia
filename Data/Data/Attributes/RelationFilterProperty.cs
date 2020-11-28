using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Data.Attributes
{
    public class RelationFilterProperty: Attribute
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
        public Comparison Comparison { get; set; }
        public RelationFilterProperty()
        {
            this.Comparison = Comparison.Equal;
        }
    }
}
