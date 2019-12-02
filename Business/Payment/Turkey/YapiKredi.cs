using System;
using _PosnetDotNetModule;
using Ophelia.Web.Extensions;

namespace Ophelia.Business.Payment.Turkey
{
    public class YapiKredi : POS
    {
        public YapiKredi()
        {
            this.BankPOSURL = "https://www.posnet.ykb.com/PosnetWebService/XML";
            this.Currency = "YT";
        }

        public override PaymentResponse Charge(PaymentRequest Request)
        {
            var Response = new PaymentResponse();
            try
            {

                var ccno = Request.Order.CreditCard.CardNumber.ToString();
                var expdate = Request.Order.CreditCard.Year.ToString().Replace("20", string.Empty) + Request.Order.CreditCard.Month;
                var cvc = string.Format("{0:000}", Request.Order.CreditCard.CVC);
                var amount = Request.Order.Amount.ToString();
                var currencycode = this.Currency;
                var instnumber = Request.Order.InstallmentCount.ToString();
                var orderid = Request.Order.ID;
                if (string.IsNullOrEmpty(orderid))
                    orderid = Ophelia.Utility.Randomize();

                C_Posnet posnetObj = new C_Posnet();
                bool result = false;
                posnetObj.SetURL(this.BankPOSURL);
                posnetObj.SetMid(this.AccountName);
                posnetObj.SetTid(this.TerminalID);
                result = posnetObj.DoSaleTran(ccno, expdate, cvc, orderid, amount, currencycode, instnumber, "", "");

                if (Request.Order.InstallmentCount > 0) { posnetObj.SetKOICode(Request.Order.InstallmentCount.ToString()); }

                if (posnetObj.GetApprovedCode() == "1")
                {
                    Response.Result = true;
                    Response.Code = posnetObj.GetAuthcode();
                    Response.ReferenceNumber = posnetObj.GetHostlogkey();
                }
                else
                {
                    Response.Result = false;
                    Response.ErrorCode = "BNK001";
                    Response.ErrorMessage = posnetObj.GetResponseText();
                }
                Response.RawResponseData = posnetObj.ToJson().Replace(ccno, ccno.Left(4) + "****");
            }
            catch (Exception ex)
            {
                Response.Result = false;
                Response.ErrorCode = "SYS001";
                Response.ErrorMessage = ex.Message + " " + ex.StackTrace;
            }
            return Response;
        }
    }
}
