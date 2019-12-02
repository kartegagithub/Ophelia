using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Net
{
    public interface IMailClient : IDisposable
    {
        string SMTPServer { get; set; }

        int Port { get; set; }

        string ServiceTicket { get; set; }

        string FromName { get; set; }

        string FromAddress { get; set; }

        string ReplyAddress { get; set; }

        string Subject { get; set; }

        string HtmlBody { get; set; }

        string Charset { get; set; }

        string ToName { get; set; }

        string ToEmailAddress { get; set; }

        string PostType { get; set; }

        bool SendMail();
    }
}
