using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Web.Service
{
    public class WebApiObjectRequest<T>
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public T Data { get; set; }
        public string TypeName { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public List<FileData> Files { get; set; }
        public WebApiObjectRequest()
        {
            this.Parameters = new Dictionary<string, object>();
            this.Files = new List<FileData>();
        }
    }

    public class FileData
    {
        private byte[] oByteData = null;
        public string KeyName { get; set; }
        public string FileName { get; set; }
        public byte[] ByteData
        {
            get
            {
                if (this.oByteData == null && !string.IsNullOrEmpty(this.Base64Data))
                {
                    try
                    {
                        if (this.Base64Data.IndexOf(',') > -1)
                            this.Base64Data = this.Base64Data.Substring(this.Base64Data.IndexOf(',') + 1);
                        this.oByteData = Convert.FromBase64String(this.Base64Data);
                        this.Base64Data = "";
                    }
                    catch
                    {
                        return this.oByteData;
                    }
                }
                return this.oByteData;
            }
            set
            {
                this.oByteData = value;
            }
        }
        public string Base64Data { get; set; }
    }
}
