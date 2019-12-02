using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _PosnetDotNetModule;
using System.IO;
using System.Net;
using System.Web;
namespace Ophelia.Business.Payment.Turkey
{
    public class Garanti : POS
    {
        /* http://www.erencelik.com/asp-net-garanti-sanal-pos-entegrasyonu/ */
        public override PaymentResponse Charge3D(System.Web.HttpRequestBase Request)
        {
            string strMode = Request.Form.Get("mode");
            string strApiVersion = Request.Form.Get("apiversion");
            string strTerminalProvUserID = Request.Form.Get("terminalprovuserid");
            string strType = Request.Form.Get("txntype");
            string strAmount = Request.Form.Get("txnamount");
            string strCurrencyCode = Request.Form.Get("txncurrencycode");
            string strInstallmentCount = Request.Form.Get("txninstallmentcount");
            string strTerminalUserID = Request.Form.Get("terminaluserid");
            string strOrderID = Request.Form.Get("oid");
            string strCustomeripaddress = Request.Form.Get("customeripaddress");
            string strcustomeremailaddress = Request.Form.Get("customeremailaddress");
            string strTerminalID = Request.Form.Get("clientid");
            string _strTerminalID = "0" + strTerminalID;
            string strTerminalMerchantID = Request.Form.Get("terminalmerchantid");
            string strStoreKey = this.StoreKey3D;
            string strProvisionPassword = this.ProvisionPassword;
            string strSuccessURL = Request.Form.Get("successurl");
            string strErrorURL = Request.Form.Get("errorurl");
            string strCardholderPresentCode = "13";//3D Model işlemde bu değer 13 olmalı
            string strMotoInd = "N";
            string strAuthenticationCode = HttpContext.Current.Server.UrlEncode(Request.Form.Get("cavv"));
            string strSecurityLevel = HttpContext.Current.Server.UrlEncode(Request.Form.Get("eci"));
            string strTxnID = HttpContext.Current.Server.UrlEncode(Request.Form.Get("xid"));
            string strMD = HttpContext.Current.Server.UrlEncode(Request.Form.Get("md"));
            string strMDStatus = Request.Form.Get("mdstatus");
            string strMDStatusText = Request.Form.Get("mderrormessage");
            string strHostAddress = this.BankPOSURL;//Provizyon için xml'in post edileceği adres
            string SecurityData = GetSHA1(strProvisionPassword + _strTerminalID).ToUpper();
            string HashData = GetSHA1(strOrderID + strTerminalID + strAmount + SecurityData).ToUpper();//Daha kısıtlı bilgileri HASH ediyoruz.

            //strMDStatus.Equals(1)//"Tam Doğrulama"
            //strMDStatus.Equals(2)//"Kart Sahibi veya bankası sisteme kayıtlı değil"
            //strMDStatus.Equals(3)//"Kartın bankası sisteme kayıtlı değil"
            //strMDStatus.Equals(4)//"Doğrulama denemesi, kart sahibi sisteme daha sonra kayıt olmayı seçmiş"
            //strMDStatus.Equals(5)//"Doğrulama yapılamıyor"
            //strMDStatus.Equals(6)//"3-D Secure Hatası"
            //strMDStatus.Equals(7)//"Sistem Hatası"
            //strMDStatus.Equals(8)//"Bilinmeyen Kart No"
            //strMDStatus.Equals(0)//"Doğrulama Başarısız, 3-D Secure imzası geçersiz."

            //Hashdata kontrolü için bankadan dönen secure3dhash değeri alınıyor.
            string strHashData = Request.Form.Get("secure3dhash");
            string ValidateHashData = GetSHA1(strTerminalID + strOrderID + strAmount + strSuccessURL + strErrorURL + strType + strInstallmentCount + strStoreKey + SecurityData).ToUpper();

            //İlk gönderilen ve bankadan dönen HASH değeri yeni üretilenle eşleşiyorsa;
            if (strHashData == ValidateHashData)
            {
                //Tam Doğrulama, Kart Sahibi veya bankası sisteme kayıtlı değil, Kartın bankası sisteme kayıtlı değil
                //Doğrulama denemesi, kart sahibi sisteme daha sonra kayıt olmayı seçmiş responselarını alan
                //işlemler için Provizyon almaya çalışıyoruz
                if (strMDStatus == "1" | strMDStatus == "2" | strMDStatus == "3" | strMDStatus == "4")
                {
                    //Provizyona Post edilecek XML Şablonu
                    string strXML = null;
                    strXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "<GVPSRequest>" + "<Mode>" + strMode + "</Mode>" + "<Version>" + strApiVersion + "</Version>" + "<ChannelCode></ChannelCode>" + "<Terminal><ProvUserID>" + strTerminalProvUserID + "</ProvUserID><HashData>" + HashData + "</HashData><UserID>" + strTerminalUserID + "</UserID><ID>" + strTerminalID + "</ID><MerchantID>" + strTerminalMerchantID + "</MerchantID></Terminal>" + "<Customer><IPAddress>" + strCustomeripaddress + "</IPAddress><EmailAddress>" + strcustomeremailaddress + "</EmailAddress></Customer>" + "<Card><Number></Number><ExpireDate></ExpireDate><CVV2></CVV2></Card>" + "<Order><OrderID>" + strOrderID + "</OrderID><GroupID></GroupID><AddressList><Address><Type>B</Type><Name></Name><LastName></LastName><Company></Company><Text></Text><District></District><City></City><PostalCode></PostalCode><Country></Country><PhoneNumber></PhoneNumber></Address></AddressList></Order>" + "<Transaction>" + "<Type>" + strType + "</Type><InstallmentCnt>" + strInstallmentCount + "</InstallmentCnt><Amount>" + strAmount + "</Amount><CurrencyCode>" + strCurrencyCode + "</CurrencyCode><CardholderPresentCode>" + strCardholderPresentCode + "</CardholderPresentCode><MotoInd>" + strMotoInd + "</MotoInd>" + "<Secure3D><AuthenticationCode>" + strAuthenticationCode + "</AuthenticationCode><SecurityLevel>" + strSecurityLevel + "</SecurityLevel><TxnID>" + strTxnID + "</TxnID><Md>" + strMD + "</Md></Secure3D>" + "</Transaction>" + "</GVPSRequest>";

                    try
                    {
                        string data = "data=" + strXML;

                        WebRequest _WebRequest = WebRequest.Create(strHostAddress);
                        _WebRequest.Method = "POST";

                        byte[] byteArray = Encoding.UTF8.GetBytes(data);
                        _WebRequest.ContentType = "application/x-www-form-urlencoded";
                        _WebRequest.ContentLength = byteArray.Length;

                        Stream dataStream = _WebRequest.GetRequestStream();
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        dataStream.Close();

                        WebResponse _WebResponse = _WebRequest.GetResponse();
                        Console.WriteLine(((HttpWebResponse)_WebResponse).StatusDescription);
                        dataStream = _WebResponse.GetResponseStream();

                        StreamReader reader = new StreamReader(dataStream);
                        string responseFromServer = reader.ReadToEnd();

                        var Result = this.GetPOSResponse(responseFromServer);
                        Result.RawRequestData = data;
                        Result.RawResponseData = responseFromServer;
                        return Result;
                    }
                    catch (Exception ex)
                    {
                        var Result = new PaymentResponse();
                        Result.Result = false;
                        Result.ErrorCode = "001";
                        Result.ErrorMessage = ex.Message + " " + ex.StackTrace;
                        return Result;
                    }

                }
                else
                {
                    var Result = new PaymentResponse();
                    Result.Result = false;
                    Result.ErrorCode = "002";
                    Result.ErrorMessage = "İşlem Başarısız";
                    return Result;
                }
            }
            else
            {
                var Result = new PaymentResponse();
                Result.Result = false;
                Result.ErrorCode = "001";
                Result.ErrorMessage = "Güvenlik Uyarısı. Sayısal İmza Geçerli Degil";
                return Result;
            }
        }

        public override string Authenticate3D(PaymentRequest Request)
        {
            string strMode = this.Mode;
            string strVersion = this.Version;
            string strTerminalID = this.TerminalID;
            string _strTerminalID = "0" + strTerminalID; //Başına 0 eklenerek 9 digite tamamlanmalıdır. 
            string strProvUserID = this.ProvisionUserID;
            string strProvisionPassword = this.ProvisionPassword; //Terminal UserID şifresi 
            string strUserID = this.UserID;
            string strMerchantID = this.MerchantID;//Üye  işyeri Numarası 
            string strHostAddress = this.BankPOSURL;
            string strType = this.ChargeType;
            string strCurrencyCode = this.Currency;
            string strStoreKey = this.StoreKey3D; //3D Secure şifresi
            string strSuccessURL = this.SuccessURL;
            string strErrorURL = this.FailURL;
            string strInstallment = Request.Order.InstallmentCount.ToString(); //Taksit
            string strOrderID = Request.Order.ID;
            string strAmount = (Request.Order.Amount * 100).ToString().Replace(",", ".");
            string strIPAddress = Request.Order.CustomerIPAddress;
            if (strAmount.IndexOf(".") > -1)
                strAmount = strAmount.Substring(0, strAmount.IndexOf(".")); //İşlem Tutarı 1.00 TL için 100 gönderilmeli
            if (strIPAddress == "::1")
            {
                strIPAddress = "127.0.0.1";
            }

            string SecurityData = GetSHA1(strProvisionPassword + _strTerminalID).ToUpper();
            string HashData = GetSHA1(strTerminalID + strOrderID + strAmount + strSuccessURL + strErrorURL + strType + strInstallment + strStoreKey + SecurityData).ToUpper();

            string POSTData = "<form action='" + this.BankPOSURL3D + "' method='post' id='bankpostform' name='bankpostform'>";
            POSTData += "<input type='hidden' name='cardnumber' value='" + Request.Order.CreditCard.CardNumber + "'>";
            POSTData += "<input type='hidden' name='cardexpiredatemonth' value='" + Request.Order.CreditCard.FormattedMonth() + "'>";//“06”  olarak gönderilmeli 
            POSTData += "<input type='hidden' name='cardexpiredateyear' value='" + Request.Order.CreditCard.FormattedYear("yy") + "'>";//“17”  olarak gönderilmeli 
            POSTData += "<input type='hidden' name='cardcvv2' value='" + Request.Order.CreditCard.CVC + "'>";
            POSTData += "<input type='hidden' name='mode' value='" + strMode + "'>";
            POSTData += "<input type='hidden' name='secure3dsecuritylevel' Value='3D'>";
            POSTData += "<input type='hidden' name='apiversion' value='" + strVersion + "'>";
            POSTData += "<input type='hidden' name='terminalprovuserid' value='" + strProvUserID + "'>";
            POSTData += "<input type='hidden' name='terminaluserid' value='" + strUserID + "'>";
            POSTData += "<input type='hidden' name='terminalmerchantid' value='" + strMerchantID + "'>";
            POSTData += "<input type='hidden' name='txntype' value='" + strType + "'>";
            POSTData += "<input type='hidden' name='txnamount' value='" + strAmount + "'>";
            POSTData += "<input type='hidden' name='txncurrencycode' value='" + strCurrencyCode + "'>";
            POSTData += "<input type='hidden' name='txninstallmentcount' value='" + strInstallment + "'>";
            POSTData += "<input type='hidden' name='orderid' value='" + strOrderID + "'>";
            POSTData += "<input type='hidden' name='terminalid' value='" + this.TerminalID + "'>";//30691297 olarak kullanılmalı 9 haneli değer sadece securedata hesaplamasında kullanılmaktadır. 
            POSTData += "<input type='hidden' name='successurl' value='" + this.SuccessURL + "'>";
            POSTData += "<input type='hidden' name='errorurl' value='" + this.FailURL + "'>";
            POSTData += "<input type='hidden' name='customeremailaddress' value='" + Request.Order.CustomerEmailAddress + "'>";
            POSTData += "<input type='hidden' name='customeripaddress' value='" + strIPAddress + "'>";
            POSTData += "<input type='hidden' name='secure3dhash' value='" + HashData + "'>";
            POSTData += "</form><script>$(document).ready(function(){document.forms['bankpostform'].submit()});</script>";
            return POSTData;
        }

        public override PaymentResponse Charge(PaymentRequest Request)
        {
            string strMode = this.Mode;
            string strVersion = this.Version;
            string strTerminalID = this.TerminalID;
            string _strTerminalID = "0" + strTerminalID; //Başına 0 eklenerek 9 digite tamamlanmalıdır. 
            string strProvUserID = this.ProvisionUserID;
            string strProvisionPassword = this.ProvisionPassword; //Terminal UserID şifresi 
            string strUserID = this.UserID;
            string strMerchantID = this.MerchantID;//Üye  işyeri Numarası 
            string strHostAddress = this.BankPOSURL;
            string strType = this.ChargeType;
            string strCurrencyCode = this.Currency;

            string strCustomerName = Request.Order.CustomerName;
            string strIPAddress = Request.Order.CustomerIPAddress;
            string strEmailAddress = Request.Order.CustomerEmailAddress;
            string strOrderID = Request.Order.ID;
            string strNumber = Request.Order.CreditCard.CardNumber; //"5407099016729014";
            string strExpireDate = Request.Order.CreditCard.FormattedExpiryDate();// "0314";
            string strCVV2 = Request.Order.CreditCard.CVC.ToString();// "395";
            string strAmount = (Request.Order.Amount * 100).ToString().Replace(",", ".");
            if (strAmount.IndexOf(".") > -1)
                strAmount = strAmount.Substring(0, strAmount.IndexOf(".")); //İşlem Tutarı 1.00 TL için 100 gönderilmeli
            string strInstallment = Request.Order.InstallmentCount.ToString(); //Taksit
            if (strIPAddress == "::1")
            {
                strIPAddress = "127.0.0.1";
            }
            string strCardholderPresentCode = "0";
            if (this.Enable3D)
            {
                strCardholderPresentCode = "13";
            }
            string strMotoInd = "N";
            string SecurityData = GetSHA1(strProvisionPassword + _strTerminalID).ToUpper();
            string HashData = GetSHA1(strOrderID + strTerminalID + strNumber + strAmount + SecurityData).ToUpper();

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            System.Xml.XmlDeclaration dec = null;
            dec = doc.CreateXmlDeclaration("1.0", "ISO-8859-1", "yes");
            doc.AppendChild(dec);
            System.Xml.XmlElement GVPSRequest = null;
            GVPSRequest = doc.CreateElement("GVPSRequest");
            doc.AppendChild(GVPSRequest);
            System.Xml.XmlElement Mode = null;
            Mode = doc.CreateElement("Mode");
            Mode.AppendChild(doc.CreateTextNode(strMode));
            GVPSRequest.AppendChild(Mode);
            System.Xml.XmlElement Version = null;
            Version = doc.CreateElement("Version");
            Version.AppendChild(doc.CreateTextNode(strVersion));
            GVPSRequest.AppendChild(Version);
            System.Xml.XmlElement Terminal = null;
            Terminal = doc.CreateElement("Terminal");
            GVPSRequest.AppendChild(Terminal);
            System.Xml.XmlElement ProvUserID = null;
            ProvUserID = doc.CreateElement("ProvUserID");
            ProvUserID.AppendChild(doc.CreateTextNode(strProvUserID));
            Terminal.AppendChild(ProvUserID);
            System.Xml.XmlElement HashData_ = null;
            HashData_ = doc.CreateElement("HashData");
            HashData_.AppendChild(doc.CreateTextNode(HashData));
            Terminal.AppendChild(HashData_);
            System.Xml.XmlElement UserID = null;
            UserID = doc.CreateElement("UserID");
            UserID.AppendChild(doc.CreateTextNode(strUserID));
            Terminal.AppendChild(UserID);
            System.Xml.XmlElement ID = null;
            ID = doc.CreateElement("ID");
            ID.AppendChild(doc.CreateTextNode(strTerminalID));
            Terminal.AppendChild(ID);
            System.Xml.XmlElement MerchantID = null;
            MerchantID = doc.CreateElement("MerchantID");
            MerchantID.AppendChild(doc.CreateTextNode(strMerchantID));
            Terminal.AppendChild(MerchantID);
            System.Xml.XmlElement Customer = null;
            Customer = doc.CreateElement("Customer");
            GVPSRequest.AppendChild(Customer);
            System.Xml.XmlElement IPAddress = null;
            IPAddress = doc.CreateElement("IPAddress");
            IPAddress.AppendChild(doc.CreateTextNode(strIPAddress));
            Customer.AppendChild(IPAddress);
            System.Xml.XmlElement EmailAddress = null;
            EmailAddress = doc.CreateElement("EmailAddress");
            EmailAddress.AppendChild(doc.CreateTextNode(strEmailAddress));
            Customer.AppendChild(EmailAddress);
            System.Xml.XmlElement Card = null;
            Card = doc.CreateElement("Card");
            GVPSRequest.AppendChild(Card);
            System.Xml.XmlElement Number = null;
            Number = doc.CreateElement("Number");
            Number.AppendChild(doc.CreateTextNode(strNumber));
            Card.AppendChild(Number);
            System.Xml.XmlElement ExpireDate = null;
            ExpireDate = doc.CreateElement("ExpireDate");
            ExpireDate.AppendChild(doc.CreateTextNode(strExpireDate));
            Card.AppendChild(ExpireDate);
            System.Xml.XmlElement CVV2element = null;
            CVV2element = doc.CreateElement("CVV2");
            CVV2element.AppendChild(doc.CreateTextNode(strCVV2));
            Card.AppendChild(CVV2element);
            System.Xml.XmlElement Order = null;
            Order = doc.CreateElement("Order");
            GVPSRequest.AppendChild(Order);
            System.Xml.XmlElement OrderID = null;
            OrderID = doc.CreateElement("OrderID");
            OrderID.AppendChild(doc.CreateTextNode(strOrderID));
            Order.AppendChild(OrderID);
            System.Xml.XmlElement GroupID = null;
            GroupID = doc.CreateElement("GroupID");
            GroupID.AppendChild(doc.CreateTextNode(""));
            Order.AppendChild(GroupID);
            System.Xml.XmlElement Description = null;
            Description = doc.CreateElement("Description");
            Description.AppendChild(doc.CreateTextNode(""));
            Order.AppendChild(Description);
            System.Xml.XmlElement Transaction = null;
            Transaction = doc.CreateElement("Transaction");
            GVPSRequest.AppendChild(Transaction);
            System.Xml.XmlElement Type = null;
            Type = doc.CreateElement("Type");
            Type.AppendChild(doc.CreateTextNode(strType));
            Transaction.AppendChild(Type);
            System.Xml.XmlElement InstallmentCnt = null;
            InstallmentCnt = doc.CreateElement("InstallmentCnt");
            InstallmentCnt.AppendChild(doc.CreateTextNode(strInstallment));
            Transaction.AppendChild(InstallmentCnt);
            System.Xml.XmlElement Amount = null;
            Amount = doc.CreateElement("Amount");
            Amount.AppendChild(doc.CreateTextNode(strAmount));
            Transaction.AppendChild(Amount);
            System.Xml.XmlElement CurrencyCode = null;
            CurrencyCode = doc.CreateElement("CurrencyCode");
            CurrencyCode.AppendChild(doc.CreateTextNode(strCurrencyCode));
            Transaction.AppendChild(CurrencyCode);
            System.Xml.XmlElement CardholderPresentCode = null;
            CardholderPresentCode = doc.CreateElement("CardholderPresentCode");
            CardholderPresentCode.AppendChild(doc.CreateTextNode(strCardholderPresentCode));
            Transaction.AppendChild(CardholderPresentCode);
            System.Xml.XmlElement MotoInd = null;
            MotoInd = doc.CreateElement("MotoInd");
            MotoInd.AppendChild(doc.CreateTextNode(strMotoInd));
            Transaction.AppendChild(MotoInd);
            System.Xml.XmlElement _Description = null;
            _Description = doc.CreateElement("Description");
            _Description.AppendChild(doc.CreateTextNode(""));
            Transaction.AppendChild(_Description);
            System.Xml.XmlElement OriginalRetrefNum = null;
            OriginalRetrefNum = doc.CreateElement("OriginalRetrefNum");
            OriginalRetrefNum.AppendChild(doc.CreateTextNode(""));
            Transaction.AppendChild(OriginalRetrefNum);
            try
            {
                string data = "data=" + doc.OuterXml;
                var responseFromServer = strHostAddress.RequestURL(data, "POST", "application/x-www-form-urlencoded");
                var Result = this.GetPOSResponse(responseFromServer);
                Result.RawRequestData = data.Replace(Request.Order.CreditCard.CardNumber, Request.Order.CreditCard.CardNumber.Left(4) + "****").Replace(this.ProvisionUserID, "****").Replace(this.TerminalID, "****").Replace(this.MerchantID, "****").Replace(HashData, "****");
                Result.RawResponseData = responseFromServer;
                return Result;
            }
            catch (Exception ex)
            {
                var Result = new PaymentResponse();
                Result.Result = false;
                Result.ErrorCode = "001";
                Result.ErrorMessage = ex.Message + " " + ex.StackTrace;
                return Result;
            }
        }

        public override PaymentResponse Refund(PaymentCancellationRequest Request)
        {
            string strMode = this.Mode;
            string strVersion = this.Version;
            string strTerminalID = "0" + this.TerminalID;
            string _strTerminalID = strTerminalID; //Başına 0 eklenerek 9 digite tamamlanmalıdır. 
            string strUserID = this.UserID;
            string strMerchantID = this.MerchantID;//Üye  işyeri Numarası 
            string strProvUserID = this.RefundProvisionUserID;
            string strProvisionPassword = this.RefundProvisionPassword; //Terminal UserID şifresi 
            string strHostAddress = this.BankPOSURL;
            string strType = "void";
            string strCurrencyCode = this.Currency;

            string strOrderID = Request.OrderID;
            string strCustomerName = Request.CustomerName;
            string strIPAddress = Request.CustomerIPAddress;
            string strEmailAddress = Request.CustomerEmailAddress;
            string strAmount = (Request.Amount * 100).ToString().Replace(",", ".");
            if (strAmount.IndexOf(".") > -1)
                strAmount = strAmount.Substring(0, strAmount.IndexOf(".")); //İşlem Tutarı 1.00 TL için 100 gönderilmeli
            if (strIPAddress == "::1")
            {
                strIPAddress = "127.0.0.1";
            }
            string SecurityData = GetSHA1(strProvisionPassword + _strTerminalID).ToUpper();

            string HashData = GetSHA1(strOrderID + strTerminalID + strAmount + SecurityData).ToUpper();


            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            System.Xml.XmlDeclaration dec = null;
            dec = doc.CreateXmlDeclaration("1.0", "ISO-8859-1", "yes");
            doc.AppendChild(dec);
            System.Xml.XmlElement GVPSRequest = null;
            GVPSRequest = doc.CreateElement("GVPSRequest");
            doc.AppendChild(GVPSRequest);
            System.Xml.XmlElement Mode = null;
            Mode = doc.CreateElement("Mode");
            Mode.AppendChild(doc.CreateTextNode(strMode));
            GVPSRequest.AppendChild(Mode);
            System.Xml.XmlElement Version = null;
            Version = doc.CreateElement("Version");
            Version.AppendChild(doc.CreateTextNode(strVersion));
            GVPSRequest.AppendChild(Version);
            System.Xml.XmlElement Terminal = null;
            Terminal = doc.CreateElement("Terminal");
            GVPSRequest.AppendChild(Terminal);
            System.Xml.XmlElement ProvUserID = null;
            ProvUserID = doc.CreateElement("ProvUserID");
            ProvUserID.AppendChild(doc.CreateTextNode(strProvUserID));
            Terminal.AppendChild(ProvUserID);
            System.Xml.XmlElement HashData_ = null;
            HashData_ = doc.CreateElement("HashData");
            HashData_.AppendChild(doc.CreateTextNode(HashData));
            Terminal.AppendChild(HashData_);
            System.Xml.XmlElement UserID = null;
            UserID = doc.CreateElement("UserID");
            UserID.AppendChild(doc.CreateTextNode(strUserID));
            Terminal.AppendChild(UserID);
            System.Xml.XmlElement ID = null;
            ID = doc.CreateElement("ID");
            ID.AppendChild(doc.CreateTextNode(strTerminalID));
            Terminal.AppendChild(ID);
            System.Xml.XmlElement MerchantID = null;
            MerchantID = doc.CreateElement("MerchantID");
            MerchantID.AppendChild(doc.CreateTextNode(strMerchantID));
            Terminal.AppendChild(MerchantID);
            System.Xml.XmlElement Customer = null;
            Customer = doc.CreateElement("Customer");
            GVPSRequest.AppendChild(Customer);
            System.Xml.XmlElement IPAddress = null;
            IPAddress = doc.CreateElement("IPAddress");
            IPAddress.AppendChild(doc.CreateTextNode(strIPAddress));
            Customer.AppendChild(IPAddress);

            System.Xml.XmlElement Order = null;
            Order = doc.CreateElement("Order");
            GVPSRequest.AppendChild(Order);
            System.Xml.XmlElement OrderID = null;
            OrderID = doc.CreateElement("OrderID");
            OrderID.AppendChild(doc.CreateTextNode(strOrderID));
            Order.AppendChild(OrderID);

            System.Xml.XmlElement Transaction = null;
            Transaction = doc.CreateElement("Transaction");
            GVPSRequest.AppendChild(Transaction);
            System.Xml.XmlElement Type = null;
            Type = doc.CreateElement("Type");
            Type.AppendChild(doc.CreateTextNode(strType));
            Transaction.AppendChild(Type);

            System.Xml.XmlElement Amount = null;
            Amount = doc.CreateElement("Amount");
            Amount.AppendChild(doc.CreateTextNode(strAmount));
            Transaction.AppendChild(Amount);

            System.Xml.XmlElement OriginalRetrefNum = null;
            OriginalRetrefNum = doc.CreateElement("OriginalRetrefNum");
            OriginalRetrefNum.AppendChild(doc.CreateTextNode(Request.BankTransactionCode));
            Transaction.AppendChild(OriginalRetrefNum);
            try
            {
                string data = "data=" + doc.OuterXml;
                string responseFromServer = strHostAddress.RequestURL(data, "POST", "application/x-www-form-urlencoded");
                var Result = this.GetPOSResponse(responseFromServer);
                Result.RawRequestData = data;
                Result.RawResponseData = responseFromServer;
                return Result;
            }
            catch (Exception ex)
            {
                var Result = new PaymentResponse();
                Result.Result = false;
                Result.ErrorCode = "001";
                Result.ErrorMessage = ex.Message + " " + ex.StackTrace;
                return Result;
            }
        }

        public PaymentResponse GetPOSResponse(string xml)
        {
            var Result = new PaymentResponse();
            var doc = new System.Xml.XmlDocument();
            doc.LoadXml(xml);

            Result.Code = doc.GetElementsByTagName("Code").Item(0).InnerText;
            Result.ReferenceNumber = doc.GetElementsByTagName("RetrefNum").Item(0).InnerText;
            Result.CardType = doc.GetElementsByTagName("CardType").Item(0).InnerText;
            Result.CardNumberMasked = doc.GetElementsByTagName("CardNumberMasked").Item(0).InnerText;
            Result.ErrorMessage = doc.GetElementsByTagName("SysErrMsg").Item(0).InnerText;
            Result.HashData = doc.GetElementsByTagName("HashData").Item(0).InnerText;
            Result.Result = Result.Code == "00";
            return Result;
        }

        public Garanti()
        {
            this.BankPOSURL = "https://sanalposprovtest.garanti.com.tr/VPServlet";
            this.ChargeType = "preauth";
            this.Currency = "949";
            this.Version = "v0.01";
            this.Mode = "TEST";
        }
    }
}
