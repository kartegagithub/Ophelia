﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ophelia.Web.UI.Controls;
using System.Web.Mvc;
using System.Linq.Expressions;
using Ophelia.Web.View.Mvc.Models;

namespace Ophelia.Web.View.Mvc.Controls.Binders.CollectionBinder.Columns
{
    public class ImageColumn<TModel, T> : BaseColumn<TModel, T> where T:class where TModel : ListModel<T>
    {
        public override object GetValue(T item)
        {
            var value = base.GetValue(item);
            if(value != null)
            {
                var image = new Image();
                image.HtmlAttributes = this.HtmlAttributes;
                image.Source = this.Binder.Client.GetImagePath(value.ToString(), true);
                image.CssClass = "img-responsive preview";
                return image.Draw();
            }
            return "";
        }
        public ImageColumn(CollectionBinder<TModel,T> binder, string Name) : base(binder, Name)
        {
            
        }
    }
}
