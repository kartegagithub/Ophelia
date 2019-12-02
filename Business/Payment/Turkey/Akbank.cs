using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _PosnetDotNetModule;

namespace Ophelia.Business.Payment.Turkey
{
    public class Akbank : POS
    {
        public Akbank()
        {
            this.BankPOSURL = "https://www.sanalakpos.com/servlet/cc5ApiServer";
            this.ChargeType = "Auth";
            this.Currency = "949";
        }
    }
}
