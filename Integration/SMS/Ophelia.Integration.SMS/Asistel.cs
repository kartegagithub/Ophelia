using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Integration.SMS
{
    public class Asistel: SMSClient
    {
        private AsistelService.SmsWebService Service { get; set; }

        /// <summary>
        /// Send SMS to single number. Returns
        /// 100 SMS sunucuya başarıyla yüklendi
        /// 101 SMSC arızası
        /// 102 SMS Gateway Arızası
        /// 103 SMS Gönderim kotası aşılmış
        /// 104 Kullanıcı adı-Parola yanlış
        /// 105 Firma Kodu Aktif değil
        /// 106 gsmNo[] veya smsText[] boş olamaz.
        /// 107 SMS metniniz 1 karakterden uzun olmalı
        /// 108 Tarih formatı yanlış
        /// 109 Alfanumeric Başlık Hatalı veya Onaysız
        /// 110 Bilinmeyen Hata
        /// 120 Sunucu Meşgul
        /// 121 Geçersiz Gsm Listesi Format -90XXXXXXXXXX- 12 hane TelNo olmalı
        /// 122 Geçersiz Format 90XXXXXXXXXX olmalı
        /// </summary>
        /// <param name="number">12 Digits receiver's number (true: 90XXXXXXXXXX, false: 05XXXXXXXXX,5XXXXXXXXX,5XX XXXXXXX,5XX XXX XX XX)</param>
        /// <param name="message">SMS content</param>
        /// <param name="senderDisplayName">Message Title (Approved By Asistel, alfaNumeric)</param>
        /// <param name="senderNumber">Message Sender Number (Approved By Asistel, chargedParty)</param>
        /// <returns></returns>
        public override string SendSMS(string number, string message, string senderDisplayName, string senderNumber)
        {
            return this.SendSMS(new string[] { number }, new string[] { message }, senderDisplayName, senderNumber, DateTime.Now, true);
        }

        /// <summary>
        /// Send SMS to single number. Returns
        /// 100 SMS sunucuya başarıyla yüklendi
        /// 101 SMSC arızası
        /// 102 SMS Gateway Arızası
        /// 103 SMS Gönderim kotası aşılmış
        /// 104 Kullanıcı adı-Parola yanlış
        /// 105 Firma Kodu Aktif değil
        /// 106 gsmNo[] veya smsText[] boş olamaz.
        /// 107 SMS metniniz 1 karakterden uzun olmalı
        /// 108 Tarih formatı yanlış
        /// 109 Alfanumeric Başlık Hatalı veya Onaysız
        /// 110 Bilinmeyen Hata
        /// 120 Sunucu Meşgul
        /// 121 Geçersiz Gsm Listesi Format -90XXXXXXXXXX- 12 hane TelNo olmalı
        /// 122 Geçersiz Format 90XXXXXXXXXX olmalı
        /// </summary>
        /// <param name="number">Array of 12 Digits receiver's number (true: 90XXXXXXXXXX, false: 05XXXXXXXXX,5XXXXXXXXX,5XX XXXXXXX,5XX XXX XX XX)</param>
        /// <param name="message">SMS content</param>
        /// <param name="senderDisplayName">Message Title (Approved By Asistel, alfaNumeric)</param>
        /// <param name="senderNumber">Message Sender Number (Approved By Asistel, chargedParty)</param>
        /// <param name="sendingDate">Date to send sms</param>
        /// <param name="singleMessageForAllRecipients">True if a single message will be sent to all recipients</param>
        /// <returns></returns>
        public override string SendSMS(string[] number, string[] message, string senderDisplayName, string senderNumber, DateTime sendingDate, bool singleMessageForAllRecipients)
        {
            this.SetServiceURL();
            return this.Service.SmsGonder(this.UserName, this.Password, number, message, sendingDate.ToString("ddMMyyyyHHmmss"), senderDisplayName, senderNumber, !singleMessageForAllRecipients);
        }

        /// <summary>
        /// 104 Kullanıcı adı-Parola yanlış
        /// 109 Alfanumeric Başlık Bulunamadı
        /// baslik1|baslik2|baslik3|baslik4|baslik5 | ayırıcı karakteri ile(ASCII = ALT + 124) birbirinden ayrılmış 1 ila 5 arasında Alfanumerik başlık bilgisi.
        /// 110 Bilinmeyen Hata
        /// 120 Sunucu Meşgul
        /// </summary>
        /// <returns></returns>
        public override string GetSMSDisplayName()
        {
            this.SetServiceURL();
            return this.Service.BaslikOgren(this.UserName, this.Password);
        }

        /// <summary>
        /// 0 Rapor Bulunamadı.
        /// 1 SMS gönderilecek
        /// 2 SMS gönderiliyor
        /// 3 SMS beklemede
        /// 11 SMS aboneye iletildi.
        /// 12-13-14-15-16 SMS Başarısız
        /// 110 Bilinmeyen Hata
        /// 120 Sunucu Meşgul
        /// </summary>
        /// <param name="number">Raporu istenecek Telefon Numarası</param>
        /// <param name="transactionID">Raporu istenecek Telefon Numarasının gönderim anında verilen transactionId si.</param>
        /// <returns></returns>
        public AsistelService.KurumsalSms GetSMSReport(string number, string transactionID)
        {
            return this.GetSMSReport(new string[] { number }, transactionID);
        }

        /// <summary>
        /// 0 Rapor Bulunamadı.
        /// 1 SMS gönderilecek
        /// 2 SMS gönderiliyor
        /// 3 SMS beklemede
        /// 11 SMS aboneye iletildi.
        /// 12-13-14-15-16 SMS Başarısız
        /// 110 Bilinmeyen Hata
        /// 120 Sunucu Meşgul
        /// </summary>
        /// <param name="numbers">Raporu istenecek Telefon Numaraları</param>
        /// <param name="transactionID">Raporu istenecek Telefon Numarasının gönderim anında verilen transactionId si.</param>
        /// <returns></returns>
        public AsistelService.KurumsalSms GetSMSReport(string[] numbers, string transactionID)
        {
            this.SetServiceURL();
            return this.Service.GetSmsReport(this.UserName, this.Password, transactionID, numbers);
        }

        /// <summary>
        /// TransactionId1, ToplamNumara1, başarılıToplam1, başarısızToplam1, beklemedeToplam1 | TransactionId2, ToplamNumara2, başarılıToplam2, başarısızToplam2, beklemedeToplam2 …. TransactionIdN, ToplamNumaraN, başarılıToplamN, başarısızToplamN, beklemedeToplamN Detay parametresinin false yüklenmesi durumunda verilen tarih aralığında genel toplamları özet bilgi halinde öğrenebilirsiniz.
        /// Format örneğin:
        /// 1789941,561,541,16,4|1726334,5,4,0,1|17236330,1,1,0,0
        /// Örnekte verilen tarih aralığına göre 3 ayrı transactionId dönmüş.İlk 1789941 nolu ID yi incelersek
        /// TransactionId = 1789941
        /// Toplam Numara : 561
        /// İletilen Mesaj Sayısı : 541
        /// Başarısız Mesaj Sayısı : 16
        /// Rapor Beklenen Mesaj Sayısı : 4
        /// Ayırıcı Karakterler, (virgül)
        /// 
        /// telefonNo1,durum|telefonNo2,durum2| …|telefonNoN,durumN
        /// Detay parametresine true yüklemeniz durumunda tarih aralığında gönderilen tüm telefon numaraları ve sms durumlarının listesini alırsınız. Geri dönen format örneğin,
        /// 905556667788,1|905059998877,2|905052724861,0
        /// Şeklinde olursa,
        /// 1. numara da durum = 1 iletilmiş
        /// 2. numarada durum = 2 başarısız
        /// 3. numarada durum = 0 beklemede
        /// Durum kodlarını yorumlarken
        /// 0: Beklemede
        /// 1: Başarılı
        /// 2: Başarısız
        /// Olarak değerlendiriniz.
        /// Ayırıcı Karakterler, ve | işaretidir.
        /// 
        /// 104 Kullanıcı Adı veya Parola Hatalı
        /// 105 Firma Aktif Değil
        /// 108 Gönderilen tarih formatı hatalı
        /// 110 Bilinmeyen Hata
        /// 111 Rapor Yok.
        /// 120 Sunucu Meşgul
        /// </summary>
        /// <param name="startDate">Alınmak istenen raporun başlangıç tarihi</param>
        /// <param name="endDate">Alınmak istenen raporun bitiş tarihi.</param>
        /// <param name="detailed">Detaylı rapor almak için true, özet toplam raporu almak için false</param>
        /// <returns></returns>
        public string GetSMSReport(DateTime startDate, DateTime endDate, bool detailed)
        {
            this.SetServiceURL();
            return this.Service.SorguTarih(this.UserName, this.Password, startDate.ToString("ddMMyyyyHHmmss"), endDate.ToString("ddMMyyyyHHmmss"), detailed);
        }

        /// <summary>
        /// TransactionId, ToplamNumara, başarılıToplam, başarısızToplam, beklemedeToplam
        /// Detay parametresinin false yüklenmesi durumunda verilen transactionId için genel toplamları özet bilgi halinde öğrenebilirsiniz.
        /// Format örneği:
        /// 1789941,561,541,16,4
        /// TransactionId = 1789941
        /// Toplam Numara : 561
        /// İletilen Mesaj Sayısı : 541
        /// Başarısız Mesaj Sayısı : 16
        /// Rapor Beklenen Mesaj Sayısı : 4
        /// Ayırıcı Karakterler, (virgül)
        /// 
        /// telefonNo1,durum|telefonNo2,durum2| …|telefonNoN,durumN
        /// Detay parametresine true yüklemeniz durumunda tüm telefon numaraları ve sms durumlarının listesini alırsınız. Geri dönen format örneğin,
        /// 905556667788,1|905059998877,2|905052724861,0
        /// Şeklinde olursa,
        /// 1. numara da durum = 1 iletilmiş
        /// 2. numarada durum = 2 başarısız
        /// 3. numarada durum = 0 beklemede
        /// Durum kodlarını yorumlarken
        /// 0: Beklemede
        /// 1: Başarılı
        /// 2: Başarısız
        /// Olarak değerlendiriniz.
        /// Ayırıcı Karakterler, ve | işaretidir.
        /// 
        /// 104 Kullanıcı Adı veya Parola Hatalı
        /// 105 Firma Aktif Değil
        /// 108 Gönderilen tarih formatı hatalı
        /// 110 Bilinmeyen Hata
        /// 111 Rapor Yok
        /// 120 Sunucu Meşgul
        /// </summary>
        /// <param name="transactionID">TransactionId değeri</param>
        /// <param name="detailed">Detaylı rapor almak için true, özet toplam raporu almak için false</param>
        /// <returns></returns>
        public string GetSMSReport(string transactionID, bool detailed)
        {
            this.SetServiceURL();
            return this.Service.SorguTransactionId(this.UserName, this.Password, Convert.ToInt32(transactionID), detailed);
        }
        private void SetServiceURL()
        {
            if (!string.IsNullOrEmpty(this.ServiceURL))
                this.Service.Url = this.ServiceURL;
        }
        public Asistel()
        {
            this.Service = new AsistelService.SmsWebService();
        }
    }
}
