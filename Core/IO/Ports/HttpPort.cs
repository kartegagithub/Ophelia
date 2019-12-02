using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.IO.Ports
{
    public class HttpPort : Port
    {
        public int Number { get; internal set; }
        protected HttpListener Listener { get; private set; }
        public bool Listening { get; internal set; }
        public string DataToWrite { get; internal set; }
        public delegate void OnReceive(HttpListenerContext ctx);

        public HttpPort()
        {
            
        }
        public void WriteInLoop()
        {
            while (true)
            {
                this.Write();
            }
        }
        public void Write(string text)
        {
            this.DataToWrite = text;
            this.Write();
        }
        public void Listen(OnReceive onReceiveFunc)
        {
            if (!this.Listening)
            {
                this.Listening = true;
                try
                {
                    this.Listener.Start();
                    this.Available = true;
                    var ctx = this.Listener.GetContext();
                    onReceiveFunc?.Invoke(ctx);
                }
                catch (Exception)
                {
                    this.Available = false;
                    throw;
                }

                this.Listening = false;
            }
        }
        public void Write()
        {
            this.Listen(((ctx) =>
            {
                var data = Encoding.ASCII.GetBytes(this.DataToWrite);

                var response = ctx.Response;
                response.ContentLength64 = data.Length;
                response.ContentEncoding = Encoding.UTF8;
                response.ContentType = "text/HTML";
                response.OutputStream.Write(data, 0, data.Length);
            }));
        }
        public HttpListenerContext Read()
        {
            HttpListenerContext context = null;
            this.Listen(((ctx) =>
            {
                context = ctx;
            }));
            return context;
        }
        public void Open(int number)
        {
            if(this.Listener == null)
            {
                this.Number = number;
                this.Listener = new HttpListener();
                this.Listener.Prefixes.Add("http://*:" + this.Number + "/");
            }
        }
        public override void Dispose()
        {
            base.Dispose();
            this.Listener.Stop();
            this.Listener.Close();
            this.Listener = null;
        }
    }
}
