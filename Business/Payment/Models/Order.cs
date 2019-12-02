using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Business.Payment.Models
{
    public class Order
    {
        public string ID { get; set; }

        public string Title { get; set; }

        public double Amount { get; set; }

        public double DiscountAmount { get; set; }

        public double ExpenseAmount { get; set; }

        public double ShippingAmount { get; set; }

        public string CustomerEmailAddress { get; set; }

        public string CustomerIPAddress { get; set; }

        public string CustomerID { get; set; }

        public string CustomerName { get; set; }

        public string CustomerFirstName { get; set; }

        public string CustomerLastName { get; set; }

        public string CustomerPhone { get; set; }

        public string CustomerCountryCode { get; set; }

        public string CustomerIdentityNumber { get; set; }

        public DateTime DateCreated { get; set; }

        public int InstallmentCount { get; set; }

        public string PayMethod { get; set; }

        public string InstallmentOptions { get; set; }

        public string PaymentSource { get; set; }

        public Address BillingAddress { get; set; }

        public Address ShippingAddress { get; set; }

        public CreditCard CreditCard { get; set; }

        public List<OrderItem> Items { get; set; }

        public MarketPlaceMerchant Merchant { get; set; }

        public Order() {
            this.BillingAddress = new Address();
            this.ShippingAddress = new Address();
            this.Items = new List<OrderItem>();
            this.CreditCard = new CreditCard();
            this.Merchant = new MarketPlaceMerchant();
            this.Title = "Order";
        }
    }
}
