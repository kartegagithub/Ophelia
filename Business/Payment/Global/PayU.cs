using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Business.Payment.Global
{
    /*
     * https://github.com/PayU/plugin_magento/blob/master/app/code/community/PayU/Account/controllers/PaymentController.php
     * https://github.com/yasinkuyu/PayU-TR/blob/master/3.x/payments/payu_tr.php
     * http://www.volkanelektronik.com.tr/store/banks3d/3dformpayu.aspx
     */
    public class PayU : POS
    {
        public Dictionary<string, object> Data;
        public Dictionary<string, string> oResponseCodes;
        public Dictionary<string, string> oReturnCodes;

        public int OrderTimeout { get; set; }
        public Dictionary<string, string> ResponseCodes
        {
            get
            {
                if (this.oResponseCodes == null)
                {
                    this.oResponseCodes.Add("AUTHORIZED", "If the payment was authorized");
                    this.oResponseCodes.Add("3DS_ENROLLED", "The payment authorization needs to be confirmed by the Shopper with his BANK using 3DS");
                    this.oResponseCodes.Add("ALREADY_AUTHORIZED", "If the Shopper tries to place a new order with the same ORDER_REF and HASH as a previous one");
                    this.oResponseCodes.Add("AUTHORIZATION_FAILED", "The payment was NOT authorized");
                    this.oResponseCodes.Add("INVALID_CUSTOMER_INFO", "Required data from the Shopper is missing or if malformed");
                    this.oResponseCodes.Add("INVALID_PAYMENT_INFO", "Card data is NOT correct");
                    this.oResponseCodes.Add("INVALID_ACCOUNT", "The Merchant name is NOT correct");
                    this.oResponseCodes.Add("INVALID_PAYMENT_METHOD_CODE", "Payment method code is NOT recognized");
                    this.oResponseCodes.Add("INVALID_CURRENCY", "Payment currency is NOT recognized");
                    this.oResponseCodes.Add("REQUEST_EXPIRED", "If between ORDER_DATE is and payment date has passed more than 10 minutes or more than ORDER_TIMEOUT set by the merchant");
                    this.oResponseCodes.Add("HASH_MISMATCH", "If HASH sent by the Merchant does NOT match the HASH calculated by PayU");
                    this.oResponseCodes.Add("WRONG_VERSION", "If ALU version sent by the Merchant does NOT exist");
                    this.oResponseCodes.Add("INVALID_CC_TOKEN", "If CC_TOKEN sent by the Merchant is NOT valid");
                }
                return this.oResponseCodes;
            }
        }

        public Dictionary<string, string> ReturnCodes
        {
            get
            {
                if (this.oReturnCodes == null)
                {
                    this.oReturnCodes.Add("GW_ERROR_GENERIC", "An error occurred during processing. Please retry the operation");
                    this.oReturnCodes.Add("GW_ERROR_GENERIC_3D", "An error occurred during 3DS processing");
                    this.oReturnCodes.Add("GWERROR_-19", "Authentication failed");
                    this.oReturnCodes.Add("GWERROR_-9", "Error in card expiration date field");
                    this.oReturnCodes.Add("GWERROR_-8", "Invalid card number");
                    this.oReturnCodes.Add("GWERROR_-3", "Call acquirer support call number");
                    this.oReturnCodes.Add("GWERROR_-2", "An error occurred during processing. Please retry the operation");
                    this.oReturnCodes.Add("GWERROR_03", "Invalid merchant");
                    this.oReturnCodes.Add("GWERROR_04", "Restricted card");
                    this.oReturnCodes.Add("GWERROR_05", "Authorization declined");
                    this.oReturnCodes.Add("GWERROR_06", "Error - retry");
                    this.oReturnCodes.Add("GWERROR_08", "Invalid amount");
                    this.oReturnCodes.Add("GWERROR_13", "Invalid amount");
                    this.oReturnCodes.Add("GWERROR_14", "No such card");
                    this.oReturnCodes.Add("GWERROR_15", "No such card/issuer");
                    this.oReturnCodes.Add("GWERROR_19", "Re-enter transaction");
                    this.oReturnCodes.Add("GWERROR_20", "Invalid response");
                    this.oReturnCodes.Add("GWERROR_30", "Format error");
                    this.oReturnCodes.Add("GWERROR_34", "Credit card number failed the fraud");
                    this.oReturnCodes.Add("GWERROR_36", "Credit restricted");
                    this.oReturnCodes.Add("GWERROR_41", "Lost card");
                    this.oReturnCodes.Add("GWERROR_43", "Stolen card, pick up");
                    this.oReturnCodes.Add("GWERROR_51", "Insufficient funds");
                    this.oReturnCodes.Add("GWERROR_53", "No savings account");
                    this.oReturnCodes.Add("GWERROR_54", "Expired card");
                    this.oReturnCodes.Add("GWERROR_55", "Incorrect PIN");
                    this.oReturnCodes.Add("GWERROR_57", "Transaction not permitted on card");
                    this.oReturnCodes.Add("GWERROR_58", "Not permitted to merchant");
                    this.oReturnCodes.Add("GWERROR_61", "Exceeds amount limit");
                    this.oReturnCodes.Add("GWERROR_62", "Restricted card");
                    this.oReturnCodes.Add("GWERROR_63", "Security violation");
                    this.oReturnCodes.Add("GWERROR_65", "Exceeds frequency limit");
                    this.oReturnCodes.Add("GWERROR_75", "PIN tries exceeded");
                    this.oReturnCodes.Add("GWERROR_82", "Time-out at issuer");
                    this.oReturnCodes.Add("GWERROR_84", "Invalid cvv");
                    this.oReturnCodes.Add("GWERROR_89", "Authentication failure");
                    this.oReturnCodes.Add("GWERROR_91", "A technical problem occurred. Issuer cannot process");
                    this.oReturnCodes.Add("GWERROR_92", "Router unavailable");
                    this.oReturnCodes.Add("GWERROR_93", "Violation of law");
                    this.oReturnCodes.Add("GWERROR_94", "Duplicate transmission");
                    this.oReturnCodes.Add("GWERROR_96", "System malfunction");
                    this.oReturnCodes.Add("GWERROR_98", "Error during canceling transaction");
                    this.oReturnCodes.Add("GWERROR_99", "Incorrect card brand");
                    this.oReturnCodes.Add("GWERROR_102", "Acquirer timeout");
                    this.oReturnCodes.Add("GWERROR_105", "3DS authentication error");
                    this.oReturnCodes.Add("GWERROR_2204", "No permission to process the card installment");
                    this.oReturnCodes.Add("GWERROR_2304", "There is an ongoing process your order");
                    this.oReturnCodes.Add("GWERROR_5007", "Debit cards only supports 3D operations");
                    this.oReturnCodes.Add("ALREADY_AUTHORIZED", "Re-enter transaction");
                    this.oReturnCodes.Add("NEW_ERROR", "Message flow error");
                    this.oReturnCodes.Add("WRONG_ERROR", "Re-enter transaction");
                    this.oReturnCodes.Add("-9999", "Banned operation");
                    this.oReturnCodes.Add("1", "Call acquirer support call number");
                }
                return this.oReturnCodes;
            }
        }

        public override PaymentResponse Charge(PaymentRequest Request)
        {
            var Result = new PaymentResponse();
            this.GetData(Request);
            if (!this.UseGatewayPaymentPage)
            {
                var response = this.BankPOSURL.RequestURL(this.Data);
                Result = this.GetPOSResponse(response);
                Result.RawRequestData = this.Data.ToQueryString().Replace(Request.Order.CreditCard.CardNumber, Request.Order.CreditCard.CardNumber.ToString().Left(4) + "****");
                Result.RawResponseData = response;
            }
            else {
                string POSTData = "<form action='" + this.BankPOSURL + "' method='post' id='payForm' name='payForm'>";
                foreach (var item in this.Data)
                {
                    POSTData += "<input type='hidden' name='" + item.Key + "' value='" + item.Value + "'>";
                }
                POSTData += "</form><script>$(document).ready(function(){document.forms['payForm'].submit()});</script>";
                Result.PostData = POSTData;
                Result.UseGatewayPaymentPage = true;
                Result.PostToPaymentPage = true;
            }
            return Result;
        }

        public bool VerifyControlSignature(System.Web.HttpRequestBase request)
        {
            var ctrl = request.QueryString["ctrl"];
            if (string.IsNullOrEmpty(ctrl))
                return false;
            var url = request.Url.ToString().Replace("&ctrl=" + ctrl, "").Replace("?ctrl=" + ctrl, "");
            var hashString = url.Length.ToString() + url;
            var binaryHash = new System.Security.Cryptography.HMACMD5(Encoding.UTF8.GetBytes(this.AccountPassword)).ComputeHash(Encoding.UTF8.GetBytes(hashString));
            var hash = BitConverter.ToString(binaryHash).Replace("-", string.Empty).ToLowerInvariant();
            return hash == ctrl.ToLowerInvariant();
        }

        public override PaymentResponse Confirm(PaymentRequest Request)
        {
            var Result = new PaymentResponse();
            var nameValues = new Dictionary<string, object>();
            nameValues.Add("MERCHANT", this.MerchantID);
            nameValues.Add("ORDER_REF", Request.Order.ID);
            nameValues.Add("ORDER_AMOUNT", Request.Order.Amount.ToString().Replace(",", "."));
            if (!string.IsNullOrEmpty(this.Currency))
                nameValues["ORDER_CURRENCY"] = this.Currency;
            nameValues.Add("IDN_DATE", DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss"));
            this.GenerateHash(Request, nameValues);

            string response = this.ConfirmationBankPOSURL.RequestURL(nameValues);
            Result.RawRequestData = nameValues.ToQueryString();
            Result.RawResponseData = response;

            response = response.Replace("<EPAYMENT>", "").Replace("</EPAYMENT>", "");
            string[] resultProperties = response.Split('|');
            Result.OrderID = resultProperties[0];
            Result.Code = resultProperties[1];
            Result.Description = resultProperties[2];
            Result.Date = resultProperties[3];
            Result.HashData = resultProperties[4];
            if (Result.Code == "1")
            {
                Result.Code = "00";
                Result.Result = true;
            }
            else {
                Result.ErrorCode = Result.Code;
                Result.ErrorMessage = Result.Description;
            }
            return Result;
        }

        public override PaymentResponse Refund(PaymentCancellationRequest Request)
        {
            var Result = new PaymentResponse();
            var nameValues = new Dictionary<string, object>();
            nameValues.Add("MERCHANT", this.MerchantID);
            nameValues.Add("ORDER_REF", Request.OrderID);
            nameValues.Add("ORDER_AMOUNT", Request.OrderAmount.ToString().Replace(",", "."));
            if (!string.IsNullOrEmpty(this.Currency))
                nameValues["ORDER_CURRENCY"] = this.Currency;
            if (Request.Amount > 0)
                nameValues.Add("AMOUNT", Request.Amount.ToString().Replace(",", "."));
            else
                nameValues.Add("AMOUNT", Request.OrderAmount.ToString().Replace(",", "."));

            nameValues.Add("IRN_DATE", DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss"));
            if (Request.LoyaltyPointsAmount > 0)
                nameValues.Add("LOYALTY_POINTS_AMOUNT", Request.LoyaltyPointsAmount.ToString().Replace(",", "."));
            if (Request.Merchants.Count > 0)
            {
                var index = 0;
                foreach (var merchant in Request.Merchants)
                {
                    nameValues.Add("ORDER_MPLACE_MERCHANT[" + index + "]", merchant.ID);
                    nameValues.Add("ORDER_MPLACE_AMOUNT[" + index + "]", merchant.Amount.ToString().Replace(",", "."));
                    index++;
                }
            }
            this.GenerateHash(null, nameValues);

            string response = this.CancellationBankPOSURL.RequestURL(nameValues);
            Result.RawRequestData = nameValues.ToQueryString();
            Result.RawResponseData = response;

            response = response.Replace("<EPAYMENT>", "").Replace("</EPAYMENT>", "");
            string[] resultProperties = response.Split('|');
            Result.OrderID = resultProperties[0];
            Result.Code = resultProperties[1];
            Result.Description = resultProperties[2];
            Result.Date = resultProperties[3];
            Result.HashData = resultProperties[4];
            if (Result.Code == "1")
            {
                Result.Code = "00";
                Result.Result = true;
            }
            else {
                Result.ErrorCode = Result.Code;
                Result.ErrorMessage = Result.Description;
            }
            return Result;
        }

        public override string Authenticate3D(PaymentRequest Request)
        {
            return null;
        }

        public override PaymentResponse Charge3D(System.Web.HttpRequestBase Request)
        {
            var Result = new PaymentResponse();
            if (!string.IsNullOrEmpty(Request["REFNO"]))
                Result.ReferenceNumber = Request["REFNO"];
            if (!string.IsNullOrEmpty(Request["ALIAS"]))
                Result.Alias = Request["ALIAS"];
            if (!string.IsNullOrEmpty(Request["STATUS"]))
                Result.Status = Request["STATUS"];
            if (!string.IsNullOrEmpty(Request["RETURN_CODE"]))
                Result.Code = Request["RETURN_CODE"];
            if (!string.IsNullOrEmpty(Request["RETURN_MESSAGE"]))
                Result.Description = Request["RETURN_MESSAGE"];
            if (!string.IsNullOrEmpty(Request["DATE"]))
                Result.Date = Request["DATE"];
            if (!string.IsNullOrEmpty(Request["ORDER_REF"]))
                Result.OrderID = Request["ORDER_REF"];
            if (!string.IsNullOrEmpty(Request["AUTH_CODE"]))
                Result.AuthCode = Request["AUTH_CODE"];
            if (!string.IsNullOrEmpty(Request["HASH"]))
                Result.HashData = Request["HASH"];
            if (!string.IsNullOrEmpty(Request["RRN"]))
                Result.RRN = Request["RRN"];

            if (Result.Status != "SUCCESS")
            {
                Result.ErrorCode = Result.Code;
                Result.ErrorMessage = Result.Description;
            }
            else
            {
                Result.Code = "00";
                Result.Result = true;
            }
            return Result;
        }

        protected Dictionary<string, object> GetData(PaymentRequest Request)
        {
            if (this.Data == null)
            {
                Request.Order.PayMethod = "CCVISAMC";
                if (string.IsNullOrEmpty(Request.Order.CustomerIPAddress) || Request.Order.CustomerIPAddress == "::1")
                    Request.Order.CustomerIPAddress = "127.0.0.1";

                this.Data = new Dictionary<string, object>();
                this.Data["MERCHANT"] = this.AccountName;
                this.Data["ORDER_REF"] = Request.Order.ID.ToString();
                this.Data["ORDER_DATE"] = Request.Order.DateCreated.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");

                this.Data["ORDER_PNAME[]"] = Request.Order.Title;
                this.Data["ORDER_PCODE[]"] = "ORDER";
                this.Data["ORDER_PRICE[]"] = Request.Order.Amount.ToString().Replace(",", ".");
                this.Data["ORDER_QTY[]"] = 1;
                this.Data["ORDER_VAT[]"] =  0;

                if (Request.Order.ShippingAmount > 0)
                    this.Data["ORDER_SHIPPING"] = Request.Order.ShippingAmount.ToString().Replace(",", ".");
                if (!string.IsNullOrEmpty(this.Currency))
                    this.Data["PRICES_CURRENCY"] = this.Currency;
                if (Request.Order.DiscountAmount > 0)
                    this.Data["DISCOUNT"] = Request.Order.DiscountAmount.ToString().Replace(",", ".");
                if (!string.IsNullOrEmpty(Request.Order.PayMethod))
                    this.Data["PAY_METHOD"] = Request.Order.PayMethod;
                if (!string.IsNullOrEmpty(Request.Order.InstallmentOptions))
                    this.Data["INSTALLMENT_OPTIONS"] = Request.Order.InstallmentOptions;

                if (!string.IsNullOrEmpty(Request.Order.BillingAddress.LastName))
                    this.Data["BILL_LNAME"] = Request.Order.BillingAddress.LastName;
                if (!string.IsNullOrEmpty(Request.Order.BillingAddress.FirstName))
                    this.Data["BILL_FNAME"] = Request.Order.BillingAddress.FirstName;
                if (!string.IsNullOrEmpty(Request.Order.BillingAddress.Email))
                    this.Data["BILL_EMAIL"] = Request.Order.BillingAddress.Email;
                if (!string.IsNullOrEmpty(Request.Order.BillingAddress.Phone))
                    this.Data["BILL_PHONE"] = Request.Order.BillingAddress.Phone;
                if (!string.IsNullOrEmpty(Request.Order.BillingAddress.Country))
                    this.Data["BILL_COUNTRYCODE"] = Request.Order.BillingAddress.Country;
                if (!string.IsNullOrEmpty(Request.Order.BillingAddress.ZipCode))
                    this.Data["BILL_ZIPCODE"] = Request.Order.BillingAddress.ZipCode;
                if (!string.IsNullOrEmpty(Request.Order.BillingAddress.Address1))
                    this.Data["BILL_ADDRESS"] = Request.Order.BillingAddress.Address1;
                if (!string.IsNullOrEmpty(Request.Order.BillingAddress.Address2))
                    this.Data["BILL_ADDRESS2"] = Request.Order.BillingAddress.Address2;
                if (!string.IsNullOrEmpty(Request.Order.BillingAddress.City))
                    this.Data["BILL_CITY"] = Request.Order.BillingAddress.City;
                if (!string.IsNullOrEmpty(Request.Order.BillingAddress.State))
                    this.Data["BILL_STATE"] = Request.Order.BillingAddress.State;
                if (!string.IsNullOrEmpty(Request.Order.BillingAddress.Fax))
                    this.Data["BILL_FAX"] = Request.Order.BillingAddress.Fax;

                if (!string.IsNullOrEmpty(Request.Order.ShippingAddress.FirstName))
                    this.Data["DELIVERY_FNAME"] = Request.Order.ShippingAddress.FirstName;
                if (!string.IsNullOrEmpty(Request.Order.ShippingAddress.LastName))
                    this.Data["DELIVERY_LNAME"] = Request.Order.ShippingAddress.LastName;
                if (!string.IsNullOrEmpty(Request.Order.ShippingAddress.Email))
                    this.Data["DELIVERY_EMAIL"] = Request.Order.ShippingAddress.Email;
                if (!string.IsNullOrEmpty(Request.Order.ShippingAddress.CompanyName))
                    this.Data["DELIVERY_COMPANY"] = Request.Order.ShippingAddress.CompanyName;
                if (!string.IsNullOrEmpty(Request.Order.ShippingAddress.Phone))
                    this.Data["DELIVERY_PHONE"] = Request.Order.ShippingAddress.Phone;
                if (!string.IsNullOrEmpty(Request.Order.ShippingAddress.Address1))
                    this.Data["DELIVERY_ADDRESS"] = Request.Order.ShippingAddress.Address1;
                if (!string.IsNullOrEmpty(Request.Order.ShippingAddress.Address2))
                    this.Data["DELIVERY_ADDRESS2"] = Request.Order.ShippingAddress.Address2;
                if (!string.IsNullOrEmpty(Request.Order.ShippingAddress.ZipCode))
                    this.Data["DELIVERY_ZIPCODE"] = Request.Order.ShippingAddress.ZipCode;
                if (!string.IsNullOrEmpty(Request.Order.ShippingAddress.City))
                    this.Data["DELIVERY_CITY"] = Request.Order.ShippingAddress.City;
                if (!string.IsNullOrEmpty(Request.Order.ShippingAddress.State))
                    this.Data["DELIVERY_STATE"] = Request.Order.ShippingAddress.State;
                if (!string.IsNullOrEmpty(Request.Order.ShippingAddress.Country))
                    this.Data["DELIVERY_COUNTRYCODE"] = Request.Order.ShippingAddress.Country;

                if (!this.UseGatewayPaymentPage)
                {
                    if (Request.UseLoyaltyPoints)
                        this.Data["USE_LOYALTY_POINTS"] = "YES";

                    this.Data["CC_NUMBER"] = Request.Order.CreditCard.CardNumber;
                    this.Data["EXP_MONTH"] = Request.Order.CreditCard.Month.ToString();
                    this.Data["EXP_YEAR"] = Request.Order.CreditCard.Year.ToString();
                    this.Data["CC_CVV"] = Request.Order.CreditCard.CVC;
                    this.Data["CC_OWNER"] = Request.Order.CreditCard.CardHolderName;

                    this.Data["CLIENT_IP"] = Request.Order.CustomerIPAddress;
                    this.Data["CLIENT_TIME"] = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");

                    if (Request.Order.InstallmentCount > 0)
                        this.Data["SELECTED_INSTALLMENTS_NUMBER"] = Request.Order.InstallmentCount.ToString();
                }
                if (this.OrderTimeout > 0)
                    this.Data["ORDER_TIMEOUT"] = this.OrderTimeout.ToString();

                if (!string.IsNullOrEmpty(this.Mode))
                {
                    if (this.Mode.StartsWith("1"))
                        this.Data["TESTORDER"] = "1";
                    if (this.Mode.Contains("2"))
                        this.Data["DEBUG"] = "1";
                }
                if (this.UseGatewayPaymentPage)
                {
                    this.Data["AUTOMODE"] = "1";
                }
                this.Data["BACK_REF"] = this.SuccessURL;
                if (!string.IsNullOrEmpty(Request.Language))
                    this.Data["LANGUAGE"] = Request.Language;

                if (!this.UseGatewayPaymentPage)
                {
                    this.Data = this.Data.OrderBy(op => op.Key).ToDictionary<KeyValuePair<string, object>, string, object>(pair => pair.Key, pair => pair.Value);
                    var ExcludedKeys = new string[] { "ORDER_HASH" };
                    this.GenerateHash(Request, this.Data, ExcludedKeys);
                }
                else {
                    var ExcludedKeys = new string[] { "ORDER_HASH", "TESTORDER", "DEBUG", "LANGUAGE", "AUTOMODE", "ORDER_TIMEOUT", "TIMEOUT_URL", "BACK_REF", "LU_TOKEN_TYPE", "LU_ENABLE_TOKEN", "DELIVERY_*", "BILL_*" };
                    this.GenerateHash(Request, this.Data, ExcludedKeys);
                }
            }
            return this.Data;
        }

        protected string GenerateHash(PaymentRequest Request = null, Dictionary<string, object> Dictionary = null, string[] ExcludedKeys = null)
        {
            if (Dictionary == null)
            {
                this.GetData(Request);
                Dictionary = this.Data;
            }

            var data = "";
            foreach (var entry in Dictionary)
            {
                bool Add = false;
                if (ExcludedKeys == null)
                {
                    Add = true;
                }
                else {
                    bool Found = false;
                    foreach (var key in ExcludedKeys)
                    {
                        if (key.IndexOf("*") > -1)
                        {
                            var tmp = key.Replace("*", "");
                            if (entry.Key.StartsWith(tmp))
                            {
                                Found = true;
                                break;
                            }
                            tmp = "";
                        }
                        else if (key == entry.Key)
                        {
                            Found = true;
                            break;
                        }
                    }
                    Add = !Found;
                }
                if (Add)
                {
                    data += Encoding.UTF8.GetByteCount(Convert.ToString(entry.Value)) + Convert.ToString(entry.Value);
                }
            }
            var keyByte = Encoding.UTF8.GetBytes(this.AccountPassword);
            var hmacmd5 = new System.Security.Cryptography.HMACMD5(keyByte);
            var binaryHash = hmacmd5.ComputeHash(Encoding.UTF8.GetBytes(data));
            Dictionary["ORDER_HASH"] = BitConverter.ToString(binaryHash).Replace("-", string.Empty).ToLowerInvariant();
            return Convert.ToString(Dictionary["ORDER_HASH"]);
        }

        public PaymentResponse GetPOSResponse(string xml)
        {
            var Result = new PaymentResponse();
            var doc = new System.Xml.XmlDocument();
            doc.LoadXml(xml);

            if (doc.GetElementsByTagName("REFNO").Count > 0)
                Result.ReferenceNumber = doc.GetElementsByTagName("REFNO").Item(0).InnerText;
            if (doc.GetElementsByTagName("ALIAS").Count > 0)
                Result.Alias = doc.GetElementsByTagName("ALIAS").Item(0).InnerText;
            if (doc.GetElementsByTagName("STATUS").Count > 0)
                Result.Status = doc.GetElementsByTagName("STATUS").Item(0).InnerText;
            if (doc.GetElementsByTagName("RETURN_CODE").Count > 0)
                Result.Code = doc.GetElementsByTagName("RETURN_CODE").Item(0).InnerText;
            if (doc.GetElementsByTagName("RETURN_MESSAGE").Count > 0)
                Result.Description = doc.GetElementsByTagName("RETURN_MESSAGE").Item(0).InnerText;
            if (doc.GetElementsByTagName("DATE").Count > 0)
                Result.Date = doc.GetElementsByTagName("DATE").Item(0).InnerText;
            if (doc.GetElementsByTagName("URL_3DS").Count > 0)
                Result.URL = doc.GetElementsByTagName("URL_3DS").Item(0).InnerText;
            if (doc.GetElementsByTagName("ORDER_REF").Count > 0)
                Result.OrderID = doc.GetElementsByTagName("ORDER_REF").Item(0).InnerText;
            if (doc.GetElementsByTagName("AUTH_CODE").Count > 0)
                Result.AuthCode = doc.GetElementsByTagName("AUTH_CODE").Item(0).InnerText;
            if (doc.GetElementsByTagName("HASH").Count > 0)
                Result.HashData = doc.GetElementsByTagName("HASH").Item(0).InnerText;
            if (doc.GetElementsByTagName("RRN").Count > 0)
                Result.RRN = doc.GetElementsByTagName("RRN").Item(0).InnerText;
            if (Result.Status == "SUCCESS")
            {
                if (Result.Code == "3DS_ENROLLED")
                {
                    Result.Status = "REDIRECT";
                    Result.PostToPaymentPage = true;
                }
                else
                {
                    Result.Code = "00";
                    Result.Result = true;
                }
            }
            else
            {
                Result.ErrorCode = Result.Code;
                Result.ErrorMessage = Result.Description;
            }
            return Result;
        }

        public PayU()
        {
            this.OrderTimeout = 6000;
            this.CancellationBankPOSURL = "https://secure.payu.com.tr/order/irn.php";
            this.ConfirmationBankPOSURL = "https://secure.payu.com.tr/order/idn.php";
        }
    }
}
