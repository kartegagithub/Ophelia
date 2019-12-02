using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _PosnetDotNetModule;
using Ophelia.Web.Extensions;
using System.IO;
using System.Web;
using System.Net;
using System.Security.Cryptography;
using System.Web.Script.Serialization;

namespace Ophelia.Business.Payment
{
    public abstract class POS
    {
        public string Mode { get; set; }

        public string Version { get; set; }

        public string BankPOSURL { get; set; }

        public string BankPOSURL3D { get; set; }

        public string CancellationBankPOSURL { get; set; }

        public string ConfirmationBankPOSURL { get; set; }

        public string TerminalID { get; set; }

        public string AccountName { get; set; }

        public string AccountPassword { get; set; }

        public string ClientID { get; set; }

        public string UserID { get; set; }

        public string MerchantID { get; set; }

        public string SubMerchantID { get; set; }

        public string Currency { get; set; }

        public string ChargeType { get; set; }

        public string ProvisionUserID { get; set; }

        public string ProvisionPassword { get; set; }

        public string RefundProvisionUserID { get; set; }

        public string RefundProvisionPassword { get; set; }

        public bool Enable3D { get; set; }

        public bool UseGatewayPaymentPage { get; set; }

        public bool IsMarketPlace { get; set; }

        public string SuccessURL { get; set; }

        public string FailURL { get; set; }

        public string StoreKey3D { get; set; }

        public long VirtualPOSID { get; set; }

        public abstract PaymentResponse Charge(PaymentRequest Request);

        public abstract string Authenticate3D(PaymentRequest Request);

        public abstract PaymentResponse Charge3D(HttpRequestBase Request);

        public abstract PaymentResponse Refund(PaymentCancellationRequest Request);

        public abstract PaymentResponse Confirm(PaymentRequest Request);

        public PaymentResponse Charge3D(HttpRequest Request)
        {
            return this.Charge3D(new HttpRequestWrapper(Request));
        }

        public virtual POS CreateInstance(string CountryCode, string BankCode) {
            if (CountryCode == "TR") {
                if (BankCode == "67") {
                    return new Turkey.YapiKredi();
                }
                else if (BankCode == "15")
                {
                    return new Turkey.VakifBank();
                }
                else if (BankCode == "64")
                {
                    return new Turkey.IsBankasi();
                }
                else if (BankCode == "62")
                {
                    return new Turkey.Garanti();
                }
                else if (BankCode == "46")
                {
                    return new Turkey.Akbank();
                }
                else {
                    return new Turkey.POS();
                }
            }
            return null;
        }

        public string GetSHA1(string SHA1Data)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] hashbytes = Encoding.GetEncoding("ISO-8859-9").GetBytes(SHA1Data);
            byte[] inputbytes = sha.ComputeHash(hashbytes);
            return inputbytes.ByteArrayToString();
        }
    }
}
