using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Business.Payment.Models
{
    public class OrderItem
    {
        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal VAT { get; set; }

        public string ProductCategory { get; set; }

        public string ProductSubCategory { get; set; }

        public MarketPlaceMerchant Merchant { get; set; }

        //public List<OrderItemDiscount> Discounts { get; set; }

        //public List<OrderItemExpense> Expenses { get; set; }

        public OrderItem() {
            //this.Discounts = new List<OrderItemDiscount>();
            //this.Expenses = new List<OrderItemExpense>();
            this.Merchant = new MarketPlaceMerchant();
        }
    }
}
