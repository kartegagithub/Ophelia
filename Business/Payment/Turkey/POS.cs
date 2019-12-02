using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _PosnetDotNetModule;
using System.Web;
using Ophelia.Web.Extensions;

/* http://www.eyurtsever.com/index.php/tag/posnetdotnetmodule-dll */
/* http://www.eyurtsever.com/index.php/asp-net-hazir-sanal-pos-kodlari-7-banka */
/* http://www.cenksari.com/Yazi/AspNet-Sanal-pos-entegrasyonu-Epayment.aspx */
/* http://metinkacmaz.com/aspnet-sanal-pos-entegrasyonu-epayment-makalesi/12.aspx */

namespace Ophelia.Business.Payment.Turkey
{
    public class POS : Ophelia.Business.Payment.POS
    {
        public override PaymentResponse Confirm(PaymentRequest Request)
        {
            throw new NotImplementedException();
        }
        public override PaymentResponse Charge3D(HttpRequestBase Request)
        {
            throw new NotImplementedException();
        }
        public override string Authenticate3D(PaymentRequest Request)
        {
            throw new NotImplementedException();
        }
        public override PaymentResponse Refund(PaymentCancellationRequest Request)
        {
            throw new NotImplementedException("You must override this funstion");
        }

        public override PaymentResponse Charge(PaymentRequest Request)
        {
            var Response = new PaymentResponse();
            try
            {
                ePayment.cc5payment mycc5pay = new ePayment.cc5payment();
                mycc5pay.host = this.BankPOSURL;
                mycc5pay.name = this.AccountName;
                mycc5pay.password = this.AccountPassword;
                mycc5pay.clientid = this.ClientID;
                mycc5pay.orderresult = 0;
                mycc5pay.oid = Ophelia.Utility.Randomize();
                mycc5pay.currency = this.Currency;
                mycc5pay.chargetype = this.ChargeType;

                mycc5pay.cardnumber = Request.Order.CreditCard.CardNumber;
                mycc5pay.expmonth = string.Format("{0:00}", Request.Order.CreditCard.Month);
                mycc5pay.expyear = Request.Order.CreditCard.Year.ToString().Substring(2, 2);
                mycc5pay.cv2 = string.Format("{0:000}", Request.Order.CreditCard.CVC);
                mycc5pay.subtotal = Request.Order.Amount.ToString();

                if (Request.Order.InstallmentCount <= 0)
                {
                    mycc5pay.taksit = "1";
                }
                else
                {
                    mycc5pay.taksit = Request.Order.InstallmentCount.ToString();
                }

                //yedek bilgiler
                mycc5pay.bname = Request.Order.CreditCard.CardHolderName;
                mycc5pay.phone = Request.Order.CustomerPhone;

                Response.RawRequestData = mycc5pay.ToJson();

                string processResult = mycc5pay.processorder();
                if (processResult == "1" & mycc5pay.appr == "Approved")
                {
                    //bankadan geri dönen
                    Response.Result = true;
                    Response.GroupID = mycc5pay.groupid;
                    Response.ReferenceNumber = mycc5pay.refno;
                    Response.TransactionID = mycc5pay.transid;
                    Response.Code = mycc5pay.code;
                }
                else
                {
                    Response.Result = false;
                    Response.ErrorCode = mycc5pay.err;
                    Response.ErrorMessage = mycc5pay.errmsg;
                }
                Response.RawResponseData = mycc5pay.ToJson();
            }
            catch (Exception ex)
            {
                Response.ErrorCode = "SYS001";
                Response.ErrorMessage = ex.Message + " => " + ex.StackTrace;
            }
            return Response;
        }
    }
}
