using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Business.Payment
{
    public class PaymentCancellationRequest
    {
        public double OrderAmount { get; set; }
        public double Amount { get; set; }
        public double LoyaltyPointsAmount { get; set; }
        public string OrderID { get; set; }
        public string BankTransactionCode { get; set; }
        public string CustomerEmailAddress { get; set; }
        public string CustomerIPAddress { get; set; }
        public string CustomerName { get; set; }

        public List<Models.MarketPlaceMerchant> Merchants { get; set; }
        public PaymentCancellationRequest()
        {
            this.Merchants = new List<Models.MarketPlaceMerchant>();
        }
    }
}
