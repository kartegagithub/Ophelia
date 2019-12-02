using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ophelia.Web.View.Mvc.Models
{
    public class DynamicListModel
    {
        public IEnumerable<DynamicListItemModel> Items { get; set; }
    }
}