using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ophelia.Web.UI.Controls;

namespace Ophelia.Web.UI.Binders.EntityBinder
{
    public class TabControl : List
    {
        public EntityBinder Binder { get; private set; }
        public List<Tab> Tabs { get; protected set; }

        public TabControl(EntityBinder binder)
        {
            this.Binder = binder;
            this.CreateTabs();
        }

        public Tab AddTab(string Title, CollectionBinder.CollectionBinder binder)
        {
            var tab = this.AddTab(Title);
            tab.AddBinder(binder);
            return tab;
        }

        public Tab AddTab(string Title)
        {
            var tab = new Tab(Title, this);
            this.Tabs.Add(tab);
            return tab;
        }

        protected virtual void CreateTabs()
        {
            this.Tabs = new List<Tab>();
        }

        public override void Dispose()
        {
            base.Dispose();

            this.Tabs.Clear();
            this.Tabs = null;
        }
    }
}
