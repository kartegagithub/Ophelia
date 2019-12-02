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
    public class CheckboxField<T> : BaseField<T> where T : class
    {
        public new Checkbox DataControl { get { return (Checkbox)base.DataControl; } set { base.DataControl = value; } }

        protected override WebControl CreateDataControl()
        {
            return new Checkbox();
        }
        protected override void onBeforeRenderControl(TextWriter writer)
        {
            base.onBeforeRenderControl(writer);
            this.DataControl.CssClass = "checkbox";
            this.DataControl.Attributes.Add("data-on-text", this.Client.TranslateText(this.DataControl.DataOnText));
            this.DataControl.Attributes.Add("data-off-text", this.Client.TranslateText(this.DataControl.DataOffText));
            this.DataControl.Attributes.Add("data-size", "small");
            if (this.ExpressionValue != null)
            {
                if (Convert.ToString(this.ExpressionValue).IsNumeric())
                {
                    this.DataControl.Checked = Convert.ToInt32(this.ExpressionValue) > 0;
                }
                else
                {
                    this.DataControl.Checked = Convert.ToBoolean(this.ExpressionValue);
                }
            }
            this.HasValue = this.DataControl.Checked;
        }
        public CheckboxField(FieldContainer<T> FieldContainer) :base(FieldContainer)
        {
            
        }
    }
}
