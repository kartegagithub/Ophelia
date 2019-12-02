using System.Web;
using System.Web.SessionState;
namespace Ophelia.Web
{
    public interface IClient
    {
        HttpApplicationState Application { get; }
        HttpSessionState Session { get; }
        HttpResponse Response { get; }
        HttpRequest Request { get; }
        string ComputerName { get; }
        string ApplicationName { get; set; }
        string UserHostAddress { get; }
        string SessionID { get; }
        long LoggedInUserCount { get; set; }
        void Disconnect();
    }
}
