using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ophelia.Web.UI.Controls;
using Ophelia;
using Ophelia.Extensions;
using System.Web.UI;

namespace Ophelia.Web.View.Mvc.Controls.Binders.Fields
{
    public class BlankField<T> : BaseField<T> where T : class
    {
        public new Textbox DataControl { get { return (Textbox)base.DataControl; } set { base.DataControl = value; } }

        protected override WebControl CreateDataControl()
        {
            return new Label();
        }
        protected override void onBeforeRenderControl(TextWriter writer)
        {
            
        }
        protected override void onRenderControl(TextWriter writer)
        {
            
        }
        protected override void RenderContents(HtmlTextWriter writer)
        {
            
        }
        public BlankField(FieldContainer<T> FieldContainer) :base(FieldContainer)
        {
            
        }
    }
}
