using System.Threading;
namespace Ophelia.Web.View.Mvc.Controllers.Base
{
    public abstract class Controller : System.Web.Mvc.Controller
    {
        public Controller()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(ConfigurationManager.Culture);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(ConfigurationManager.Culture);
        }
        public HttpApplication Application { get { return (HttpApplication)this.HttpContext.ApplicationInstance; } set { } }
        public virtual Client Client { get { return this.Application.Client; } }
    }
}
