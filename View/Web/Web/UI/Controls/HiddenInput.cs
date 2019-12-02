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
    public class HiddenInput : WebControl
    {
        public string Value { get; set; }

        protected override void onBeforeRenderControl(TextWriter writer)
        {
            this.Attributes.Add("value", this.Value);
            this.Attributes.Add("type", "hidden");
            base.onBeforeRenderControl(writer);
        }

        public HiddenInput() : base(HtmlTextWriterTag.Input)
        {
            
        }
    }
}
