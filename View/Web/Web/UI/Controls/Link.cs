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
    public class Link : WebControl
    {
        public string URL { get; set; }
        public string OnClick { get; set; }
        public bool NewWindow { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        protected override void onBeforeRenderControl(TextWriter writer)
        {
            if (!string.IsNullOrEmpty(this.URL))
                this.Attributes.Add("href", this.URL);
            else
                this.Attributes.Add("href", "#");
            if (!string.IsNullOrEmpty(this.OnClick))
            {
                this.Attributes.Add("onclick", this.OnClick);
            }
            if (!string.IsNullOrEmpty(this.Text) && string.IsNullOrEmpty(this.Title) && this.Text.IndexOf("<") == -1)
            {
                this.Title = this.Text;
            }
            if (!string.IsNullOrEmpty(this.Title))
            {
                this.Attributes.Add("title", this.Title);
            }
            if (this.NewWindow)
            {
                this.Attributes.Add("target", "_blank");
            }
            base.onBeforeRenderControl(writer);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.Write(this.Text);
            base.RenderContents(writer);
        }
        public Link() : base(HtmlTextWriterTag.A)
        {

        }
    }
}
