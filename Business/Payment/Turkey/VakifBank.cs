using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _PosnetDotNetModule;
using Ophelia.Web.Extensions;
using System.Web;
using System.Xml;
using System.Net;
using System.IO;

namespace Ophelia.Business.Payment.Turkey
{
    public class VakifBank : POS
    {
        public string XCIP { get; set; }

        public VakifBank()
        {
            this.BankPOSURL = "https://subesiz.vakifbank.com.tr/vpos724v3/";
        }

        public override PaymentResponse Charge(PaymentRequest Request)
        {
            var Response = new PaymentResponse();

            try
            {
                byte[] b = new byte[5000];
                System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("ISO-8859-9");
                string AmountInString = Request.Order.Amount.ToString().Replace(".", "").Replace(",", "");
                AmountInString = String.Format("{0:0000000000.00}", Convert.ToInt32(AmountInString)).Replace(",", "");

                string InstallmentInString = "";
                if (Request.Order.InstallmentCount <= 0)
                    InstallmentInString = "00";
                else
                    InstallmentInString = String.Format("{0:00}", Request.Order.InstallmentCount);

                string YearInString = Request.Order.CreditCard.Year.ToString().Substring(2, 2);
                string MonthInString = string.Format("{0:00}", Request.Order.CreditCard.Month);

                string provizyonMesaji = "kullanici=" + this.AccountName + "&sifre=" + this.AccountPassword + "&islem=PRO&uyeno=" + this.ClientID + "&posno=" + this.TerminalID + "&kkno=" + Request.Order.CreditCard.CardNumber + "&gectar=" + YearInString + MonthInString + "&cvc=" + string.Format("{0:000}", Request.Order.CreditCard.CVC) + "&tutar=" + AmountInString + "&provno=000000&taksits=" + InstallmentInString + "&islemyeri=I&uyeref=" + Ophelia.Utility.Randomize() + "&vbref=0&khip=" + HttpContext.Current.Request.GetUserHostAddress() + "&xcip=" + this.XCIP;

                b.Initialize();
                b = Encoding.ASCII.GetBytes(provizyonMesaji);

                WebRequest h1 = (WebRequest)HttpWebRequest.Create(this.BankPOSURL + "?" + provizyonMesaji);
                h1.Method = "GET";

                WebResponse wr = h1.GetResponse();
                Stream s2 = wr.GetResponseStream();

                byte[] buffer = new byte[10000];
                int len = 0, r = 1;
                while (r > 0)
                {
                    r = s2.Read(buffer, len, 10000 - len);
                    len += r;
                }
                s2.Close();
                var sonuc = encoding.GetString(buffer, 0, len).Replace("\r", "").Replace("\n", "");

                String ApproveCode, RefCode;
                XmlNode node = null;
                XmlDocument _msgTemplate = new XmlDocument();
                _msgTemplate.LoadXml(sonuc);
                node = _msgTemplate.SelectSingleNode("//Cevap/Msg/Kod");
                ApproveCode = node.InnerText.ToString();

                Response.RawRequestData = provizyonMesaji.Replace(Request.Order.CreditCard.CardNumber, Request.Order.CreditCard.CardNumber.Left(4) + "****");
                if (ApproveCode == "00")
                {
                    node = _msgTemplate.SelectSingleNode("//Cevap/Msg/ProvNo");
                    RefCode = node.InnerText.ToString();
                    Response.Code = ApproveCode;
                    Response.Result = true;
                    Response.ReferenceNumber = RefCode;
                }
                else
                {
                    Response.Result = false;
                    Response.ErrorCode = ApproveCode;
                    Response.ErrorMessage = ApproveCode;
                }
                Response.RawResponseData = sonuc;
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
