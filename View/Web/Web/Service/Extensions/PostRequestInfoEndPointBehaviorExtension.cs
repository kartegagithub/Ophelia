using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Web.Service.Extensions
{
    public class PostRequestInfoEndPointBehaviorExtension : BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            return new PostRequestInfoEndPointBehavior(this.Application, this.Caching);
        }
        public override Type BehaviorType
        {
            get { return typeof(PostRequestInfoEndPointBehavior); }
        }

        [ConfigurationProperty("application")]
        public string Application
        {
            get
            {
                return (string)base["application"];
            }
            set
            {
                base["application"] = value;
            }
        }

        [ConfigurationProperty("caching")]
        public bool Caching
        {
            get
            {
                return (bool)base["caching"];
            }
            set
            {
                base["caching"] = value;
            }
        }
    }
}
