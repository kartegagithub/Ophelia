using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ophelia.Web.UI.Controls
{
    public class Image : WebControl
    {
        public string Source { get; set; }

        protected override void onBeforeRenderControl(TextWriter writer)
        {
            this.Attributes.Add("src", this.Source);
            base.onBeforeRenderControl(writer);
        }
        public Image() : base(HtmlTextWriterTag.Img)
        {

        }
    }
}
