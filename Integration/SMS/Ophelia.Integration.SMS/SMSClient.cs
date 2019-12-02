using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Integration.SMS
{
    public abstract class SMSClient
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ServiceURL { get; set; }

        public virtual string SendSMS(string number, string message, string senderDisplayName, string senderNumber)
        {
            throw new NotImplementedException();
        }

        public virtual string SendSMS(string[] number, string[] message, string senderDisplayName, string senderNumber, DateTime sendingDate, bool singleMessageForAllRecipients)
        {
            throw new NotImplementedException();
        }

        public virtual string GetSMSDisplayName()
        {
            throw new NotImplementedException();
        }
        public static SMSClient Create(ClientType type, string UserName, string Password, string ServiceURL)
        {
            switch (type)
            {
                case ClientType.Asistel:
                    return new Asistel() { UserName = UserName, Password = Password, ServiceURL = ServiceURL };
                case ClientType.EuroMSG:
                    break;
            }
            throw new NotImplementedException();
        }
    }
    public enum ClientType
    {
        Asistel = 1,
        EuroMSG = 2
    }
}
