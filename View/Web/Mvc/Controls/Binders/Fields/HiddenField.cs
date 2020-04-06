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
    public class HiddenField<T> : BaseField<T> where T : class
    {
        public new HiddenInput DataControl { get { return (HiddenInput)base.DataControl; } set { base.DataControl = value; } }

        protected override WebControl CreateDataControl()
        {
            return new HiddenInput();
        }
        public override string Draw()
        {
            if (this.ExpressionValue != null)
                this.DataControl.Value = Convert.ToString(this.ExpressionValue);

            this.DataControl.Value = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(this.DataControl.Value, false);

            return this.DataControl.Draw();
        }
        public HiddenField(FieldContainer<T> FieldContainer) :base(FieldContainer)
        {
            this.IsHidden = true;
        }
    }
}
