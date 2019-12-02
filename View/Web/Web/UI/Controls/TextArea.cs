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
    public class TextArea : WebControl
    {
        public string Value { get; set; }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.Write(this.Value);
            base.RenderContents(writer);
        }

        public TextArea() : base(HtmlTextWriterTag.Textarea)
        {
            this.CssClass = "form-control";
        }
    }
}
