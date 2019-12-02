using System.Text;
using System.Web.UI;
using Ophelia.Web.Application.Client;

namespace Ophelia.Web.View.Controls
{
    public class WebControl : System.Web.UI.Control
    {
        protected StringBuilder oContent;
        private HtmlTextWriterTag eTag = HtmlTextWriterTag.Div;
        private QueryString oQueryString;
        public string OnClick { get; set; }
        public bool Binded { get; set; }
        public virtual int Width { get; set; }
        public virtual string RequestData
        {
            get
            {
                if (this.Request != null && !string.IsNullOrEmpty(this.ID))
                {
                    return this.Request[this.ID];
                }
                else
                {
                    return null;
                }
            }
        }
        public QueryString QueryString
        {
            get
            {
                if (this.oQueryString == null)
                {
                    this.oQueryString = new QueryString(this.Request);
                }
                return this.oQueryString;
            }
        }
        public System.Web.HttpRequest Request
        {
            get { return System.Web.HttpContext.Current.Request; }
        }
        public System.Web.HttpResponse Response
        {
            get { return System.Web.HttpContext.Current.Response; }
        }
        protected virtual HtmlTextWriterTag Tag
        {
            get { return this.eTag; }
            set { this.eTag = value; }
        }
        public StringBuilder Content
        {
            get
            {
                if (this.oContent == null)
                    this.oContent = new StringBuilder();
                return this.oContent;
            }
        }
        public string StyleClass { get; set; }
        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Visible)
            {
                this.OnBeforeRender(writer);
                writer.WriteBeginTag(this.Tag.ToString());
                if (!string.IsNullOrEmpty(this.StyleClass))
                    writer.WriteAttribute(HtmlTextWriterAttribute.Class.ToString(), this.StyleClass);
                if (!string.IsNullOrEmpty(this.ID))
                {
                    writer.WriteAttribute(HtmlTextWriterAttribute.Id.ToString(), this.ID);
                    writer.WriteAttribute(HtmlTextWriterAttribute.Name.ToString(), this.ID);
                }
                if (this.Width > -1)
                    writer.WriteAttribute(HtmlTextWriterAttribute.Style.ToString(), "width:" + this.Width + "px;");
                if (!string.IsNullOrEmpty(this.OnClick))
                    writer.WriteAttribute(HtmlTextWriterAttribute.Onclick.ToString(), this.OnClick);
                this.OnAttributesRender(writer);
                writer.Write(HtmlTextWriter.TagRightChar);
                this.OnBeforeWriteContent(writer);
                if (this.oContent != null)
                    writer.Write(this.oContent.ToString());
                this.OnAfterWriteContent(writer);
                this.OnBeforeRenderChildren(writer);
                this.RenderChildren(writer);
                this.OnAfterRenderChildren(writer);
                writer.WriteEndTag(this.Tag.ToString());
                this.OnAfterRender(writer);
            }
        }

        protected virtual void OnAttributesRender(HtmlTextWriter writer)
        {
        }

        protected virtual void OnBeforeRender(HtmlTextWriter writer)
        {
        }

        protected virtual void OnAfterRender(HtmlTextWriter writer)
        {
        }

        protected virtual void OnBeforeRenderChildren(HtmlTextWriter writer)
        {
        }

        protected virtual void OnAfterRenderChildren(HtmlTextWriter writer)
        {
        }

        protected virtual void OnBeforeWriteContent(HtmlTextWriter writer)
        {
        }

        protected virtual void OnAfterWriteContent(HtmlTextWriter writer)
        {
        }
        public WebControl()
        {
            this.Width = -1;
        }
    }
}
