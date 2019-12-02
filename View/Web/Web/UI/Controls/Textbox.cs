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
    public class Textbox : WebControl
    {
        public string Type { get; set; }
        public string Value { get; set; }

        protected override void onBeforeRenderControl(TextWriter writer)
        {
            this.Attributes.Add("value", this.Value);
            this.Attributes.Add("type", this.Type);
            this.Attributes.Add("autocomplete", "off");
            base.onBeforeRenderControl(writer);
        }

        public Textbox() : base(HtmlTextWriterTag.Input)
        {
            this.Type = "text";
            this.CssClass = "form-control";
        }
    }
}
