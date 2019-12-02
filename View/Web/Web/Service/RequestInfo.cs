using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Web.Service
{
    [DataContract(IsReference=true)]
    public class RequestInfo
    {
        [DataMember]
        public long ID { get; set; }
        [DataMember]
        public long UserID { get; set; }
        [DataMember]
        public string SessionID { get; set; }
        [DataMember]
        public string IPAddress { get; set; }
        [DataMember]
        public string UserAgent { get; set; }
        [DataMember]
        public string Application { get; set; }
        [DataMember]
        public string MachineName { get; set; }
        [DataMember]
        public string AdditionalInfo { get; set; }
        [DataMember]
        public string RequesterProcess { get; set; }
        [DataMember]
        public string RequesterFunction { get; set; }
        [DataMember]
        public string RequestedFunction { get; set; }
        [DataMember]
        public List<RequestParameter> Parameters { get; set; }
        [DataMember]
        public DateTime RequestStartTime { get; set; }
        [DataMember]
        public DateTime RequestEndTime { get; set; }
        [DataMember]
        public string AccessToken { get; set; }
        [DataMember]
        public bool CachingEnabled { get; set; }
        [DataMember]
        public string CultureCode { get; set; }
        [DataMember]
        public int LanguageID { get; set; }
        [DataMember]
        public int DomainID { get; set; }
        [DataMember]
        public string ApplicationName { get; set; }
        [DataMember]
        public int ApplicationRole { get; set; }
    }
}
