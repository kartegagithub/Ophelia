using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Web.View.Mvc.Controls
{
    public class WebViewPage: System.Web.Mvc.WebViewPage
    {
        public new Controls.HtmlHelper<object> Html{get; set ;}

        public override void InitHelpers()
        {
            base.InitHelpers();
            this.Html = new Controls.HtmlHelper<object>(ViewContext, this);
        }
        public override void Execute()
        {
            
        }
    }
}
