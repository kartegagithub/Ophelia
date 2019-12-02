using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using Ophelia;
using Ophelia.Web.Extensions;
namespace Ophelia.Business.Payment.Global
{
    /*
     * https://github.com/iyzico/iyzipay-dotnet
     */
    public class Iyzico : POS
    {
        public Options Options;

        public CreatePaymentRequest CreatePaymentRequest;

        public override PaymentResponse Confirm(PaymentRequest Request)
        {
            return null;
        }

        /// <summary>
        /// Iyzico satış onaylama adımı
        /// </summary>
        /// <param name="PaymentTransactionId"></param>
        /// <returns></returns>
        public PaymentResponse Confirm(string PaymentTransactionId)
        {
            var Result = new PaymentResponse();
            if (!string.IsNullOrEmpty(PaymentTransactionId))
            {
                this.GetOptionsData();
                CreateApprovalRequest ApprovalRequest = new CreateApprovalRequest();
                ApprovalRequest.Locale = Locale.TR.ToString();
                ApprovalRequest.ConversationId = Ophelia.Utility.GenerateRandomPassword(10);
                ApprovalRequest.PaymentTransactionId = PaymentTransactionId;

                Approval ApprovalResult = Approval.Create(ApprovalRequest, this.Options);
                if (ApprovalResult != null)
                {
                    if (ApprovalResult.Status != "success")
                    {
                        Result.TransactionID = ApprovalResult.PaymentTransactionId;
                        Result.Code = ApprovalResult.ErrorCode;
                        Result.ErrorCode = ApprovalResult.ErrorCode;
                        Result.ErrorMessage = ApprovalResult.ErrorMessage;
                        Result.Result = false;
                    }
                    else
                    {
                        Result.Code = "00";
                        Result.Result = true;
                    }
                }
            }
            return Result;
        }

        public override PaymentResponse Charge(PaymentRequest Request)
        {
            var Result = new PaymentResponse();
            this.GetOptionsData();
            this.GetPaymentRequestData(Request);
            if (this.Options != null && this.CreatePaymentRequest != null)
            {
                Result.RawRequestData = this.CreatePaymentRequest.ToJson().Replace(Request.Order.CreditCard.CardNumber, Request.Order.CreditCard.CardNumber.ToString().Left(4) + "****");

                if (!this.Enable3D)
                {
                    Iyzipay.Model.Payment ChargePayment = Iyzipay.Model.Payment.Create(this.CreatePaymentRequest, this.Options);
                    if (ChargePayment != null)
                    {
                        Result.RawResponseData = ChargePayment.ToJson();
                        if (HttpContext.Current.Session["ConversationId"] != null && HttpContext.Current.Session["ConversationId"].ToString() != ChargePayment.ConversationId)
                        {
                            Result.Code = "01";
                            Result.ErrorCode = "01";
                            Result.ErrorMessage = "Bu istek başka bir uygulama tarafından yapılmıştır.";
                        }
                        else if (ChargePayment.Status != "success")
                        {
                            Result.Code = ChargePayment.ErrorCode;
                            Result.ErrorCode = ChargePayment.ErrorCode;
                            Result.ErrorMessage = ChargePayment.ErrorMessage;
                        }
                        else
                        {
                            Result.Code = "00";
                            Result.Result = true;
                        }

                        HttpContext.Current.Session["ConversationId"] = null;
                    }
                }
                else
                {
                    ThreedsInitialize Charge3DPayment = ThreedsInitialize.Create(this.CreatePaymentRequest, this.Options);
                    if (Charge3DPayment != null)
                    {
                        Result.RawResponseData = Charge3DPayment.ToJson();
                        if (HttpContext.Current.Session["ConversationId"] != null && HttpContext.Current.Session["ConversationId"].ToString() != Charge3DPayment.ConversationId)
                        {
                            Result.Code = "01";
                            Result.ErrorCode = "01";
                            Result.ErrorMessage = "Bu istek başka bir uygulama tarafından yapılmıştır.";
                        }
                        else if (Charge3DPayment.Status != "success")
                        {
                            Result.Code = Charge3DPayment.ErrorCode;
                            Result.ErrorCode = Charge3DPayment.ErrorCode;
                            Result.ErrorMessage = Charge3DPayment.ErrorMessage;
                        }
                        else
                        {
                            Result.PostToPaymentPage = true;
                            Result.UseGatewayPaymentPage = true;
                            Result.PostData = Charge3DPayment.HtmlContent;
                        }

                        HttpContext.Current.Session["ConversationId"] = null;
                    }
                }
            }
            else
            {
                Result.Code = "E1";
                Result.ErrorMessage = "InvalidParameter";
            }

            return Result;
        }

        public override PaymentResponse Refund(PaymentCancellationRequest Request)
        {
            var Result = new PaymentResponse();

            CreateRefundRequest RefundRequest = new CreateRefundRequest();
            RefundRequest.Locale = Locale.TR.ToString();
            RefundRequest.ConversationId = Ophelia.Utility.GenerateRandomPassword(10);
            RefundRequest.PaymentTransactionId = Request.OrderID;
            RefundRequest.Price = Request.OrderAmount.ToString();
            RefundRequest.Price = RefundRequest.Price.Replace(",", ".");
            RefundRequest.Ip = Request.CustomerIPAddress;
            RefundRequest.Currency = this.Currency;

            this.GetOptionsData();
            if (this.Options != null)
            {
                var response = Iyzipay.Model.Refund.Create(RefundRequest, this.Options);
                if (response != null)
                {
                    if (response.Status != "success")
                    {
                        Result.Code = response.ErrorCode;
                        Result.ErrorCode = response.ErrorCode;
                        Result.ErrorMessage = response.ErrorMessage;
                    }
                    else
                    {
                        Result.Code = "00";
                        Result.Result = true;
                    }
                }
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

            string status = Request.Form.Get("status");
            string paymentId = Request.Form.Get("paymentId");
            string conversationData = Request.Form.Get("conversationData");
            string conversationId = Request.Form.Get("conversationId");
            string mdStatus = Request.Form.Get("mdStatus");

            if (status != "success")
            {
                Result.Result = false;
                Result.ErrorCode = "002";
                Result.Code = "002";
                Result.ErrorMessage = "İşlem Başarısız";
            }
            else
            {
                this.GetOptionsData();
                CreateThreedsPaymentRequest request = new CreateThreedsPaymentRequest();
                request.Locale = Locale.TR.ToString();
                request.ConversationId = Ophelia.Utility.GenerateRandomPassword(10);
                request.PaymentId = paymentId;
                request.ConversationData = conversationData;

                ThreedsPayment threedsPayment = ThreedsPayment.Create(request, this.Options);
                Result.RawResponseData = threedsPayment.ToJson();

                if (threedsPayment.Status != "success")
                {
                    Result.Code = threedsPayment.ErrorCode;
                    Result.ErrorCode = threedsPayment.ErrorCode;
                    Result.ErrorMessage = threedsPayment.ErrorMessage;

                }
                else
                {
                    Result.Code = "00";
                    Result.ReferenceNumber = threedsPayment.PaymentId;
                    if (threedsPayment.PaymentItems != null)
                    {
                        Result.ItemResponses = (from response in threedsPayment.PaymentItems
                                                select new OrderItemPaymentResponse()
                                                {
                                                    ProductCode = response.ItemId,
                                                    PayoutAmount = response.SubMerchantPayoutAmount,
                                                    ReferenceNumber = response.PaymentTransactionId
                                                }).ToList();
                    }
                    Result.Result = true;
                }
            }

            return Result;
        }

        public string CreateSubMerchant(string CustomerID, string FirstName, string LastName, string Email, string PhoneNumer, string BankAccountName, string IBAN, string IdentityNumber, string Address)
        {
            CreateSubMerchantRequest request = new CreateSubMerchantRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = Ophelia.Utility.GenerateRandomPassword(10);
            request.SubMerchantExternalId = CustomerID;
            request.SubMerchantType = SubMerchantType.PERSONAL.ToString();
            request.ContactName = FirstName;
            request.ContactSurname = LastName;
            request.Email = Email;
            request.GsmNumber = PhoneNumer;
            request.Name = BankAccountName;
            request.Iban = IBAN;
            request.IdentityNumber = IdentityNumber;
            request.Address = Address;
            request.Currency = this.Currency;

            this.GetOptionsData();
            if (this.Options != null)
            {
                var response = Iyzipay.Model.SubMerchant.Create(request, this.Options);
                if (response != null)
                {
                    if (response.Status != "success")
                    {
                        return "";
                    }
                    else
                    {
                        return response.SubMerchantKey;
                    }
                }
            }
            return "";
        }

        public string UpdateSubMerchant(string SubMerchantKey, string FirstName, string LastName, string Email, string PhoneNumer, string BankAccountName, string IBAN, string IdentityNumber, string Address)
        {
            UpdateSubMerchantRequest request = new UpdateSubMerchantRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = Ophelia.Utility.GenerateRandomPassword(10);
            request.SubMerchantKey = SubMerchantKey;
            request.ContactName = FirstName;
            request.ContactSurname = LastName;
            request.Email = Email;
            request.GsmNumber = PhoneNumer;
            request.Name = BankAccountName;
            request.Iban = IBAN;
            request.IdentityNumber = IdentityNumber;
            request.Address = Address;
            request.Currency = this.Currency;

            this.GetOptionsData();
            if (this.Options != null)
            {
                var response = Iyzipay.Model.SubMerchant.Update(request, this.Options);
                if (response != null)
                {
                    if (response.Status != "success")
                    {
                        return "";
                    }
                    else
                    {
                        return response.Status;
                    }
                }
            }
            return "";
        }

        protected bool GetOptionsData()
        {
            Options options = new Options();
            options.ApiKey = this.AccountName;
            options.SecretKey = this.AccountPassword;
            if (this.Enable3D)
                options.BaseUrl = this.BankPOSURL3D;
            else
                options.BaseUrl = this.BankPOSURL;
            this.Options = options;
            return true;
        }

        public Options GetOptionsData(Iyzico _iyzico)
        {
            Options options = new Options();
            options.ApiKey = _iyzico.AccountName;
            options.SecretKey = _iyzico.AccountPassword;
            if (_iyzico.Enable3D)
                options.BaseUrl = _iyzico.BankPOSURL3D;
            else
                options.BaseUrl = _iyzico.BankPOSURL;

            return options;
        }

        protected bool GetPaymentRequestData(PaymentRequest Request)
        {
            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = Ophelia.Utility.GenerateRandomPassword(10);
            HttpContext.Current.Session["ConversationId"] = request.ConversationId;
            request.Price = Request.Order.Amount.ToString();
            request.Price = request.Price.Replace(",", ".");
            request.PaidPrice = Request.Order.Amount.ToString();
            request.PaidPrice = request.PaidPrice.Replace(",", ".");
            request.Currency = this.Currency;
            request.Installment = Request.Order.InstallmentCount > 0 ? Request.Order.InstallmentCount : 1;
            request.BasketId = Request.Order.ID;
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
            if (this.Enable3D)
                request.CallbackUrl = this.SuccessURL;

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = Request.Order.CreditCard.CardHolderName;
            paymentCard.CardNumber = Request.Order.CreditCard.CardNumber;
            paymentCard.ExpireMonth = Request.Order.CreditCard.Month.ToString();
            paymentCard.ExpireYear = Request.Order.CreditCard.Year.ToString();
            paymentCard.Cvc = Request.Order.CreditCard.CVC;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = Request.Order.CustomerID;
            buyer.Name = Request.Order.CustomerFirstName;
            buyer.Surname = Request.Order.CustomerLastName;
            buyer.GsmNumber = Request.Order.CustomerPhone;
            buyer.Email = Request.Order.CustomerEmailAddress;
            buyer.IdentityNumber = Request.Order.CustomerIdentityNumber;
            buyer.LastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            buyer.RegistrationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            buyer.RegistrationAddress = Request.Order.BillingAddress.Address1 + " " + Request.Order.BillingAddress.Address2;
            buyer.Ip = Request.Order.CustomerIPAddress;
            buyer.City = Request.Order.BillingAddress.City;
            buyer.Country = Request.Order.BillingAddress.Country;
            buyer.ZipCode = Request.Order.BillingAddress.ZipCode;
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = Request.Order.ShippingAddress.FirstName + " " + Request.Order.ShippingAddress.LastName;
            shippingAddress.City = Request.Order.ShippingAddress.City;
            shippingAddress.Country = Request.Order.ShippingAddress.Country;
            shippingAddress.Description = Request.Order.ShippingAddress.Address1 + " " + Request.Order.ShippingAddress.Address2;
            shippingAddress.ZipCode = Request.Order.ShippingAddress.ZipCode;
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = Request.Order.BillingAddress.FirstName + " " + Request.Order.BillingAddress.LastName;
            billingAddress.City = Request.Order.BillingAddress.City;
            billingAddress.Country = Request.Order.BillingAddress.Country;
            billingAddress.Description = Request.Order.BillingAddress.Address1 + " " + Request.Order.BillingAddress.Address2;
            billingAddress.ZipCode = Request.Order.BillingAddress.ZipCode;
            request.BillingAddress = billingAddress;

            request.PaymentSource = Request.Order.PaymentSource;

            List<BasketItem> basketItems = new List<BasketItem>();

            foreach (var item in Request.Order.Items)
            {
                BasketItem BasketItem = new BasketItem();
                BasketItem.Id = item.ProductCode;
                BasketItem.Name = item.ProductName;
                BasketItem.Category1 = item.ProductCategory;
                BasketItem.Category2 = item.ProductSubCategory;
                BasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                BasketItem.Price = item.Price.ToString();
                BasketItem.Price = BasketItem.Price.Replace(",", ".");
                if (this.IsMarketPlace && item.Merchant != null)
                {
                    BasketItem.SubMerchantKey = item.Merchant.Code;
                    BasketItem.SubMerchantPrice = item.Merchant.Amount.ToString();
                    BasketItem.SubMerchantPrice = BasketItem.SubMerchantPrice.Replace(",", ".");
                }
                basketItems.Add(BasketItem);
            }

            request.BasketItems = basketItems;

            this.CreatePaymentRequest = request;

            return true;
        }

        public CreateCheckoutFormInitializeRequest GetCreateCheckoutFormInitializeRequest(PaymentRequest Request,string SuccessURL="")
        {
            CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = Ophelia.Utility.GenerateRandomPassword(10);
            HttpContext.Current.Session["ConversationId"] = request.ConversationId;
            request.Price = Request.Order.Amount.ToString();
            request.Price = request.Price.Replace(",", ".");
            request.PaidPrice = Request.Order.Amount.ToString();
            request.PaidPrice = request.PaidPrice.Replace(",", ".");
            request.Currency = this.Currency;
            request.BasketId = Request.Order.ID;
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
            request.CallbackUrl = SuccessURL;
            Buyer buyer = new Buyer();
            buyer.Id = Request.Order.CustomerID;
            buyer.Name = Request.Order.CustomerFirstName;
            buyer.Surname = Request.Order.CustomerLastName;
            buyer.GsmNumber = Request.Order.CustomerPhone;
            buyer.Email = Request.Order.CustomerEmailAddress;
            buyer.IdentityNumber = Request.Order.CustomerIdentityNumber;
            buyer.LastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            buyer.RegistrationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            buyer.RegistrationAddress = Request.Order.BillingAddress.Address1 + " " + Request.Order.BillingAddress.Address2;
            buyer.Ip = Request.Order.CustomerIPAddress;
            buyer.City = Request.Order.BillingAddress.City;
            buyer.Country = Request.Order.BillingAddress.Country;
            buyer.ZipCode = Request.Order.BillingAddress.ZipCode;
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = Request.Order.ShippingAddress.FirstName + " " + Request.Order.ShippingAddress.LastName;
            shippingAddress.City = Request.Order.ShippingAddress.City;
            shippingAddress.Country = Request.Order.ShippingAddress.Country;
            shippingAddress.Description = Request.Order.ShippingAddress.Address1 + " " + Request.Order.ShippingAddress.Address2;
            shippingAddress.ZipCode = Request.Order.ShippingAddress.ZipCode;
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = Request.Order.BillingAddress.FirstName + " " + Request.Order.BillingAddress.LastName;
            billingAddress.City = Request.Order.BillingAddress.City;
            billingAddress.Country = Request.Order.BillingAddress.Country;
            billingAddress.Description = Request.Order.BillingAddress.Address1 + " " + Request.Order.BillingAddress.Address2;
            billingAddress.ZipCode = Request.Order.BillingAddress.ZipCode;
            request.BillingAddress = billingAddress;

            request.PaymentSource = Request.Order.PaymentSource;

            List<BasketItem> basketItems = new List<BasketItem>();

            foreach (var item in Request.Order.Items)
            {
                BasketItem BasketItem = new BasketItem();
                BasketItem.Id = item.ProductCode;
                BasketItem.Name = item.ProductName;
                BasketItem.Category1 = item.ProductCategory;
                BasketItem.Category2 = item.ProductSubCategory;
                BasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                BasketItem.Price = item.Price.ToString();
                BasketItem.Price = BasketItem.Price.Replace(",", ".");
                if (this.IsMarketPlace && item.Merchant != null)
                {
                    BasketItem.SubMerchantKey = item.Merchant.Code;
                    BasketItem.SubMerchantPrice = item.Merchant.Amount.ToString();
                    BasketItem.SubMerchantPrice = BasketItem.SubMerchantPrice.Replace(",", ".");
                }
                basketItems.Add(BasketItem);
            }

            request.BasketItems = basketItems;
            return request;
        }

        public Iyzico()
        {

        }
    }
}
