using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Business.Payment.Models
{
    public class CreditCard
    {
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string CVC { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public string FormattedMonth()
        {
            if (this.Month < 10)
            {
                return "0" + this.Month.ToString();
            }
            else
            {
                return this.Month.ToString();
            }
        }
        public string FormattedYear(string format = "yyyy")
        {
            if (format == "yyyy")
            {
                return this.Year.ToString();
            }
            else
            {
                return this.Year.ToString().Right(2);
            }
        }
        public string FormattedExpiryDate(string format = "MMyy")
        {
            if (format.Equals("MMyy", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.FormattedMonth() + this.FormattedYear("yy");
            }
            else if (format.Equals("MMyyyy", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.FormattedMonth() + this.FormattedYear();
            }
            return this.Month + "" + this.Year;
        }
    }
}
