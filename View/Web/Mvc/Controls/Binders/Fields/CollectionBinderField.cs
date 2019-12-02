﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ophelia.Web.UI.Controls;
using Ophelia.Web.View.Mvc.Models;
using Ophelia;
using Ophelia.Extensions;
using System.Web.UI;
using System.Web.Mvc;

namespace Ophelia.Web.View.Mvc.Controls.Binders.Fields
{
    public class CollectionBinderField<TModel, TEntity, T> : BaseField<T> where T : class where TEntity : class where TModel: ListModel<TEntity>
    {
        public new CollectionBinder.CollectionBinder<TModel, TEntity> DataControl { get { return (CollectionBinder.CollectionBinder<TModel, TEntity>)base.DataControl; } set { base.DataControl = value; } }

        protected override WebControl CreateDataControl()
        {
            return null;
        }
        public override MvcHtmlString Render()
        {
            if (this.Visible)
            {
                this.onBeforeRenderControl(this.Output);
                this.DataControl.RenderContent();
            }
            return new MvcHtmlString("");
        }
        protected override void onBeforeRenderControl(TextWriter writer)
        {
            base.onBeforeRenderControl(writer);
        }
        public CollectionBinderField(FieldContainer<T> FieldContainer, CollectionBinder.CollectionBinder<TModel, TEntity> binder, bool hideLabel = false) :base(FieldContainer)
        {
            this.DataControl = binder;
            this.DataControlParent.Controls.Add(this.DataControl);
            this.ID = this.DataControl.ID;
            this.Text = this.DataControl.Title;
            this.LabelControl.Visible = !hideLabel;
            this.CanTranslateLabelText = false;
        }
    }
}
