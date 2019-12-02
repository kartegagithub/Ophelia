using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ophelia.Web.View.Mvc.Html
{
    public class MvcInputGroup : IDisposable
    {
        private bool disposed;
        private readonly TextWriter writer;

        public MvcInputGroup(TextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("TextWriter");
            }
            this.writer = writer;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
            "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase")]
        public void Dispose()
        {
            Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                this.disposed = true;
                this.writer.Write("</div>");
            }
        }

        public void EndGroup()
        {
            Dispose(true);
        }

    }
}
