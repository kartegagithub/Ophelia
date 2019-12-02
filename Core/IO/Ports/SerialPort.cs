using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ophelia;

namespace Ophelia.IO.Ports
{
    public class SerialPort : Port
    {
        public string Name { get; internal set; }
        public int BaudRate { get; internal set; }
        public int DataBits { get; internal set; }
        public System.IO.Ports.StopBits StopBits { get; internal set; }
        public System.IO.Ports.Parity Parity { get; internal set; }
        public bool Reading { get; internal set; }
        public bool Writing { get; internal set; }
        protected System.IO.Ports.SerialPort InnerPort { get; private set; }
        public List<char> DiscardedChars { get; private set; }

        public SerialPort()
        {
            this.DiscardedChars = new List<char>();
        }
        public virtual string Read()
        {
            if (this.Available)
            {
                var readValue = new byte[] { Convert.ToByte(this.InnerPort.ReadByte()) };
                return System.Text.Encoding.ASCII.GetString(readValue).Remove(this.DiscardedChars.ToArray());
            }
            return null;
        }
        public virtual string ReadUntil(string[] stopChars)
        {
            if (!this.Reading && this.Available)
            {
                this.Reading = true;
                var result = "";
                this.OnBeforeRead();
                while (true)
                {
                    var readValue = this.Read();
                    if (stopChars.Contains(readValue))
                        break;
                    result += readValue;
                }
                this.OnAfterRead(result);
                this.Reading = false;
                return result;
            }
            return null;
        }
        public virtual string ReadExisting()
        {
            if (!this.Reading && this.Available)
            {
                this.Reading = true;
                this.OnBeforeRead();
                var val = this.InnerPort.ReadExisting().Remove(this.DiscardedChars.ToArray());
                this.OnAfterRead(val);
                this.Reading = false;
                return val;
            }
            return null;
        }
        public virtual string ReadLine()
        {
            if (!this.Reading && this.Available)
            {
                this.Reading = true;
                this.OnBeforeRead();
                var val = this.InnerPort.ReadLine().Remove(this.DiscardedChars.ToArray());
                this.OnAfterRead(val);
                this.Reading = false;
                return val;
            }
            return null;
        }

        public virtual void Write(string text)
        {
            if (!this.Writing && this.Available)
            {
                this.Writing = true;
                this.InnerPort.Write(text);
                this.Writing = false;
            }
        }
        public virtual void WriteCharCodes(string asciiCharCodes, char splitter = ',')
        {
            if (!this.Writing && this.Available)
            {
                this.Writing = true;
                this.InnerPort.Write(asciiCharCodes.Split(splitter).ToIntPresentation());
                this.Writing = false;
            }
        }
        public virtual void WriteLine(string text)
        {
            if (!this.Writing && this.Available)
            {
                this.Writing = true;
                this.InnerPort.WriteLine(text);
                this.Writing = false;
            }
        }
        public virtual void Write(byte[] buffer, int offset, int count)
        {
            if (!this.Writing && this.Available)
            {
                this.Writing = true;
                this.InnerPort.Write(buffer, offset, count);
                this.Writing = false;
            }
        }
        public virtual void Write(char[] buffer, int offset, int count)
        {
            if (!this.Writing && this.Available)
            {
                this.Writing = true;
                this.InnerPort.Write(buffer, offset, count);
                this.Writing = false;
            }
        }
        public SerialPort InitPort(string name, int baudRate, System.IO.Ports.Parity parity, int dataBits, System.IO.Ports.StopBits stopBits)
        {
            this.InitPort(name, baudRate, parity, dataBits);
            this.StopBits = stopBits;
            return this;
        }
        public SerialPort InitPort(string name, int baudRate, System.IO.Ports.Parity parity, int dataBits)
        {
            this.InitPort(name, baudRate, parity);
            this.DataBits = dataBits;
            return this;
        }
        public SerialPort InitPort(string name, int baudRate, System.IO.Ports.Parity parity)
        {
            this.InitPort(name, baudRate);
            this.Parity = parity;
            return this;
        }
        public SerialPort InitPort(string name, int baudRate)
        {
            this.InitPort(name);
            this.BaudRate = baudRate;
            return this;
        }
        public SerialPort InitPort(string name)
        {
            this.Name = name;
            return this;
        }
        public SerialPort Open()
        {
            if (this.InnerPort == null)
            {
                this.InnerPort = new System.IO.Ports.SerialPort(this.Name, this.BaudRate, this.Parity, this.DataBits, this.StopBits);
                this.InnerPort.Open();
                this.Available = true;
            }
            return this;
        }
        protected virtual void OnBeforeRead()
        {

        }
        protected virtual void OnAfterRead(string result)
        {

        }
        public void Close()
        {
            if (this.InnerPort != null)
            {
                this.InnerPort.Close();
                this.InnerPort = null;
                this.Available = false;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            this.Close();
            if (this.DiscardedChars != null)
            {
                this.DiscardedChars.Clear();
                this.DiscardedChars = null;
            }
        }
    }
}
