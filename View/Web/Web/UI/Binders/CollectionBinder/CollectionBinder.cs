using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ophelia.Web.UI.Controls;

namespace Ophelia.Web.UI.Binders.CollectionBinder
{
    public class CollectionBinder<T>: WebControl, IDisposable
    {
        public IEnumerable<T> DataSource { get; private set; }

        public Client Client { get; private set; }

        public CollectionBinder(Client client, IEnumerable<T> dataSource, )
        {
            this.Client = client;
            this.DataSource = DataSource;
        }

        public override void Dispose()
        {
            base.Dispose();

        }
    }
}
