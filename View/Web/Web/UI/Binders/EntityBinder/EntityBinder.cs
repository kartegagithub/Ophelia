using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ophelia.Web.UI.Controls;

namespace Ophelia.Web.UI.Binders.EntityBinder
{
    public class EntityBinder<T>: WebControl, IDisposable
    {
        public T Entity { get; private set; }
        public Client Client { get; private set; }
        public TabControl TabControl { get; protected set; }

        public EntityBinder(Client client, T entity)
        {
            this.Entity = entity;
            this.Client = client;
            this.CreateTabControl();
        }

        protected virtual void CreateTabControl()
        {
            this.TabControl = new TabControl(this);
        }

        public override void Dispose()
        {
            this.TabControl.Dispose();
            this.TabControl = null;
        }
    }
}
