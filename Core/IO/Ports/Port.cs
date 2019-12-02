using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.IO.Ports
{
    public abstract class Port: IDisposable
    {
        public DateTime StartDate { get; internal set; }
        public bool Available { get; internal set; }

        public virtual void Dispose()
        {
            
        }
        //public int NumericAvailable { get; internal set; }
        //private HttpListener Listener { get; set; }
        //public void Write(string text)
        //{
        //    var threadStart = new System.Threading.ThreadStart(this.WriteInThread);
        //    var thread = new System.Threading.Thread(threadStart);
        //    thread.Start(text);
        //}
        //protected void WriteInThread()
        //{
        //    if (!this.Writing && this.Available)
        //    {
        //        this.Writing = true;
        //        if (this.Listener == null)
        //            this.Listener = new HttpListener();

        //        this.Listener.Start();

        //    }
        //}
        //private void CreateListener()
        //{

        //}
    }
}
