using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ophelia.Web.UI.Controls;
using Ophelia;
using Ophelia.Extensions;

namespace Ophelia.Web.View.Mvc.Controls.Binders.Fields
{
    public class SelectboxField<T> : BaseField<T> where T : class
    {
        public string NewURL { get; set; }
        public string ViewURL { get; set; }
        public bool AllowNew { get; set; }
        public bool AllowView { get; set; }
        public new Select DataControl { get { return (Select)base.DataControl; } set { base.DataControl = value; } }

        protected override WebControl CreateDataControl()
        {
            return new Select();
        }
        protected override void onBeforeRenderControl(TextWriter writer)
        {
            base.onBeforeRenderControl(writer);
            if (this.DefaultValue == null)
                this.DefaultValue = "-1";

            if (!this.DataControl.IsMultiple)
                this.DataControl.CssClass = "form-control simple-select select-menu-color select2-hidden-accessible";
            else
            {
                this.DataControl.CssClass = "form-control simple-select multiple-selection select2-hidden-accessible";
                this.DataControl.Attributes.Add("multiple", "multiple");
            }

            if (this.IsRequired && !string.IsNullOrEmpty(this.DataControl.CssClass) && this.DataControl.CssClass.IndexOf(" required") == -1)
            {
                this.DataControl.CssClass += " required";
                this.DataControl.Attributes.Add("aria-required", "true");
            }

            if (this.ExpressionValue != null)
                this.DataControl.SelectedValue = Convert.ToString(this.ExpressionValue);

            if (this.AllowNew)
            {
                this.DataControl.Attributes.Add("data-allow-new", "true");
                this.DataControl.Attributes.Add("data-new-url", this.NewURL);
            }
            if (this.AllowView)
            {
                this.DataControl.Attributes.Add("data-allow-view", "true");
                this.DataControl.Attributes.Add("data-view-url", this.ViewURL);
            }
            this.DataControl.Attributes.Add("data-clear", "true");
            this.HasValue = !string.IsNullOrEmpty(this.DataControl.SelectedValue) && this.DataControl.SelectedValue != Convert.ToString(this.DefaultValue);
        }
        public SelectboxField(FieldContainer<T> FieldContainer) : base(FieldContainer)
        {

        }
    }
}
