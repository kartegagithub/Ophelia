using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ophelia.Net;
using Ophelia.Integration.Mailing.EuroMsg.EuroMsgPostService;
using Ophelia.Integration.Mailing.EuroMsg.EuroMsgAuthService;

namespace Ophelia.Integration.Mailing.EuroMsg
{
    public class MailClient : IMailClient
    {
        public int Port { get; set; }

        public string SMTPServer { get; set; }

        public string ServiceTicket { get; set; }

        public string FromName { get; set; }

        public string FromAddress { get; set; }

        public string ReplyAddress { get; set; }

        public string Subject { get; set; }

        public string HtmlBody { get; set; }

        public string Charset { get; set; }

        public string ToName { get; set; }

        public string ToEmailAddress { get; set; }

        public string PostType { get; set; }

        public EmAttachment[] Attachments { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool SendMail()
        {
            Post PostService = new Post();
            EmPostResult PostResult = PostService.PostHtmlWithType(ServiceTicket, FromName, FromAddress, ReplyAddress, Subject, HtmlBody, Charset, ToName, ToEmailAddress, Attachments, PostType);

            if (PostResult != null && PostResult.Code == "0")
                return true;

            return false;
        }

        public EmAuthResult Authenticate(string UserName, string Password)
        {
            Auth Authenticate = new Auth();
            EmAuthResult AuthenticateResult = Authenticate.Login(UserName, Password);
            return AuthenticateResult;
        }
    }
}
