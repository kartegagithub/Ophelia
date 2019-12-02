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
using System.Collections;
using System.Web;

namespace Ophelia.Web.View.Mvc.Controls.Binders.CollectionBinder.Columns
{
    public class BoolColumn<TModel, T> : BaseColumn<TModel, T> where T:class where TModel : ListModel<T>
    {
        public bool SingleSelection { get; set; }
        public bool CanSetValue { get; set; }
        public string DataControlCssClass { get; set; }
        public IEnumerable<SelectListItem> Datasource { get; set; }
        public Func<T, object> ComparisonFunction { get; set; }
        public override object GetValue(T item)
        {
            if(item != null)
            {
                var value = base.GetValue(item);
                if(value != null)
                {
                    var convertedValue = false;
                    if(bool.TryParse(value.ToString(), out convertedValue))
                    {
                        return this.Binder.Client.TranslateText(convertedValue.ToString());
                    }
                }
                return value;
            }
            return "";
        }

        public override WebControl GetEditableControl(T entity, object value, HttpRequest request)
        {
            if (this.AllowEdit)
            {
                var orginalValue = value;
                var identifierValue = this.IdentifierExpression.GetValue(entity);
                if(this.Datasource == null)
                {
                    if (this.SingleSelection)
                    {
                        var control = new Radio();
                        control.HtmlAttributes = this.HtmlAttributes;
                        control.ID = this.IdentifierKeyword + identifierValue;
                        control.Name = this.IdentifierKeyword;
                        control.Attributes.Add("data-identifier", Convert.ToString(identifierValue));
                        control.Attributes.Add("data-column", this.FormatColumnName());
                        if (this.CanSetValue)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(value)))
                                control.Value = Convert.ToString(value);
                            else if (!string.IsNullOrEmpty(Convert.ToString(identifierValue)))
                                control.Value = Convert.ToString(identifierValue);
                        }
                        control.CssClass = this.DataControlCssClass;
                        if (this.ComparisonFunction != null && entity != null)
                            control.Checked = Convert.ToBoolean(this.ComparisonFunction.Execute(entity));
                        return control;
                    }
                    else
                    {
                        var control = new Checkbox();
                        control.HtmlAttributes = this.HtmlAttributes;
                        control.ID = this.IdentifierKeyword + identifierValue;
                        control.Name = control.ID;
                        control.Attributes.Add("data-identifier", Convert.ToString(identifierValue));
                        control.Attributes.Add("data-column", this.FormatColumnName());
                        if (this.CanSetValue)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(value)))
                                control.Value = Convert.ToString(value);
                            else if (!string.IsNullOrEmpty(Convert.ToString(identifierValue)))
                                control.Value = Convert.ToString(identifierValue);
                        }
                        control.CssClass = this.DataControlCssClass;
                        if (this.ComparisonFunction != null && entity != null)
                            control.Checked = Convert.ToBoolean(this.ComparisonFunction.Execute(entity));
                        return control;
                    }
                }
                else
                {
                    var selectbox = new UI.Controls.Select();
                    selectbox.ID = this.IdentifierKeyword + identifierValue;
                    selectbox.Name = selectbox.ID;
                    selectbox.Attributes.Add("data-identifier", Convert.ToString(identifierValue));
                    selectbox.Attributes.Add("data-column", this.FormatColumnName());

                    selectbox.CssClass = "form-control";
                    selectbox.DataSource = this.Datasource;
                    selectbox.DefaultText = this.Binder.Client.TranslateText("Select");
                    selectbox.DefaultValue = "";
                    selectbox.DisplayMemberName = "Text";
                    selectbox.ValueMemberName = "Value";
                    if (value != null)
                        selectbox.SelectedValue = Convert.ToString(value);
                    return selectbox;
                }
            }
            return new Panel();
        }
        public BoolColumn(CollectionBinder<TModel,T> binder, string Name) : base(binder, Name)
        {
            this.CanSetValue = true;
        }
    }
}