using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ophelia.Web.UI.Controls
{
    public class Checkbox : WebControl
    {
        public bool Checked { get; set; }
        public string Value { get; set; }
        public string DataOnText { get; set; }
        public string DataOffText { get; set; }

        protected override void onBeforeRenderControl(TextWriter writer)
        {
            if (this.Checked)
                this.Attributes.Add("checked", "checked");
            if (!string.IsNullOrEmpty(this.Value))
                this.Attributes.Add("value", this.Value);
            this.Attributes.Add("type", "checkbox");
            base.onBeforeRenderControl(writer);
        }

        public Checkbox() : base(HtmlTextWriterTag.Input)
        {

        }
    }
}
