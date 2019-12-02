using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _PosnetDotNetModule;

namespace Ophelia.Business.Payment.Turkey
{
    public class IsBankasi : POS
    {
        public IsBankasi()
        {
            this.BankPOSURL = "https://spos.isbank.com.tr/servlet/cc5ApiServer";
            this.ChargeType = "Auth";
            this.Currency = "949";
        }
    }
}
