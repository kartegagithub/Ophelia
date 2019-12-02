using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ophelia.Web.UI.Controls
{
    public class WebControl : System.Web.UI.WebControls.WebControl
    {
        public override string ID
        {
            get
            {
                return base.ID;
            }

            set
            {
                if (value.IndexOf(".") > -1)
                    base.ID = value.Replace(".", "_");
                else
                    base.ID = value;
            }
        }
        public virtual string Name { get; set; }
        public virtual object HtmlAttributes { get; set; }
        public virtual bool IsHidden { get; set; }
        public virtual System.IO.TextWriter Output { get; set; }
        public virtual Dictionary<string, string> EventHandlers { get; private set; }
        public virtual void AddEvent(string eventName, string functionName)
        {
            if (this.EventHandlers == null)
                this.EventHandlers = new Dictionary<string, string>();
            this.EventHandlers[eventName] = functionName;
        }

        public virtual string Draw()
        {
            StringBuilder sb = new StringBuilder();
            this.Output = new HtmlTextWriter(new System.IO.StringWriter(sb, System.Globalization.CultureInfo.InvariantCulture));
            this.RenderControl();
            return sb.ToString();
        }

        public virtual System.Web.UI.WebControls.WebControl FirstChild
        {
            get
            {
                if (this.Controls.Count > 0)
                    return (System.Web.UI.WebControls.WebControl)this.Controls[0];
                return null;
            }
        }
        public virtual System.Web.UI.WebControls.WebControl LastChild
        {
            get
            {
                if (this.Controls.Count > 0)
                    return (System.Web.UI.WebControls.WebControl)this.Controls[this.Controls.Count - 1];
                return null;
            }
        }
        public virtual void RenderControl()
        {
            this.RenderControlAsText(this.Output);
        }

        protected virtual void onBeforeRenderControl(System.IO.TextWriter writer)
        {

        }
        protected virtual void onRenderControl(System.IO.TextWriter writer)
        {

        }
        protected virtual void onAfterRenderControl(System.IO.TextWriter writer)
        {

        }
        public virtual void RenderControlAsText(System.IO.TextWriter writer)
        {
            this.RenderControl(new HtmlTextWriter(writer));
        }
        public override void RenderControl(HtmlTextWriter writer)
        {
            if (this.HtmlAttributes != null)
            {
                var type = this.HtmlAttributes.GetType();
                var props = type.GetProperties().ToDictionary(op => op.Name, op => op.GetValue(this.HtmlAttributes, null));
                foreach (var item in props)
                {
                    this.Attributes.Add(item.Key, Convert.ToString(item.Value));
                }
            }
            if (!string.IsNullOrEmpty(this.Name))
                this.Attributes.Add("name", this.Name);
            if (this.EventHandlers != null && this.EventHandlers.Count > 0)
            {
                foreach (var item in this.EventHandlers)
                {
                    this.Attributes.Add(item.Key, item.Value);
                }
            }
            if (this.IsHidden)
            {
                this.CssClass += " hidden";
            }
            this.onBeforeRenderControl(writer);
            this.onRenderControl(writer);
            base.RenderControl(writer);
            this.onAfterRenderControl(writer);
        }
        public WebControl() : base()
        {

        }
        public WebControl(HtmlTextWriterTag tag) : base(tag)
        {

        }
        public WebControl(string tag) : base(tag)
        {

        }
    }
}
