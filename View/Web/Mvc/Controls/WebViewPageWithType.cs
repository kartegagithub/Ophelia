using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Web.View.Mvc.Controls
{
    public class WebViewPage<T>: System.Web.Mvc.WebViewPage<T>
    {
        public new Controls.HtmlHelper<T> Html { get; set; }

        public override void Execute()
        {
            
        }

        public override void InitHelpers()
        {
            base.InitHelpers();
            this.Html = new Controls.HtmlHelper<T>(ViewContext, this);
        }
    }
}
