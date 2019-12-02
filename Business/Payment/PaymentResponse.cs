using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Business.Payment
{
    public class PaymentResponse
    {
        public bool Result { get; set; }
        public string GroupID { get; set; }
        public string ReferenceNumber{ get; set; }
        public string RRN { get; set; }
        public string TransactionID { get; set; }
        public string Code{ get; set; }
        public string Description { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string CardNumberMasked { get; set; }
        public string CardType { get; set; }
        public string Alias { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        public string URL { get; set; }
        public string OrderID { get; set; }
        public string AuthCode { get; set; }
        public string HashData { get; set; }
        public bool PostToPaymentPage { get; set; }
        public string PostData { get; set; }
        public bool UseGatewayPaymentPage { get; set; }
        public string RawRequestData { get; set; }
        public string RawResponseData { get; set; }
        public long VirtualPOSID { get; set; }
        public List<OrderItemPaymentResponse> ItemResponses { get; set; }
    }

    public class OrderItemPaymentResponse
    {
        public string ProductCode { get; set; }
        public string ReferenceNumber { get; set; }
        public string PayoutAmount { get; set; }
    }
}
