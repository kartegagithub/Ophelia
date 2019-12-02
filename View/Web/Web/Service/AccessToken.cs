using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Web.Service
{
    [DataContract]
    public class AccessToken
    {
        [DataMember]
        public Guid Guid { get; set; }
        [DataMember]
        public long UserID { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string DeviceID { get; set; }
        [DataMember]
        public string SessionID { get; set; }
        [DataMember]
        public string UserAgent { get; set; }
        [DataMember]
        public string IpAddress { get; set; }
        [DataMember]
        public string Application { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime DateExpire { get; set; }
    }
}
