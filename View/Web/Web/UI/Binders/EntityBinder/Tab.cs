using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ophelia.Web.UI.Controls;

namespace Ophelia.Web.UI.Binders.EntityBinder
{
    public class Tab : Panel
    {
        public string Title { get; set; }
        public TabControl TabControl { get; private set; }
        
        public CollectionBinder.CollectionBinder AddBinder(CollectionBinder.CollectionBinder binder)
        {
            this.Controls.Add(binder);
            return binder;
        }

        public Tab(string Title, TabControl TabControl)
        {
            this.TabControl = TabControl;
            this.Title = this.TabControl.Binder.Client.TranslateText(Title);
        }
    }
}
