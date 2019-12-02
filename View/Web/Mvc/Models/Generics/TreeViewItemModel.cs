using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Web.View.Mvc.Models
{
    public class TreeViewItemModel<TItem> : BaseModel where TItem : class
    {
        public TreeViewItemModel()
            : base()
        {
            this.SubItems = new List<TItem>();
        }

        public TItem Parent { get; set; }
        public IEnumerable<TItem> SubItems { get; set; }
    }
}
