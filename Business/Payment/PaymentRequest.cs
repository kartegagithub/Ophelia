using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Business.Payment
{
    public class PaymentRequest
    {
        public long VirtualPOSID { get; set; }
        public Models.Order Order { get; set; }
        public string Language { get; set; }
        public bool UseLoyaltyPoints { get; set; }

        public PaymentRequest()
        {
            this.Order = new Models.Order();
        }
    }
}
