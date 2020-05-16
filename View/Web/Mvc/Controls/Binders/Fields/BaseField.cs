﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ophelia.Web.UI.Controls;
using System.Web.Mvc;
using System.Linq.Expressions;

namespace Ophelia.Web.View.Mvc.Controls.Binders.Fields
{
    public abstract class BaseField<T> : Panel where T : class
    {
        public Client Client
        {
            get
            {
                return this.FieldContainer.Client;
            }
        }
        public Expression<Func<T, object>> Expression { get; set; }
        public bool CanTranslateLabelText { get; set; }
        public bool IsRequired { get; set; }
        public bool Editable { get; set; }
        public string Text { get; set; }
        public FieldContainer<T> FieldContainer { get; private set; }
        public Panel DataControlParent { get; set; }
        public virtual WebControl DataControl { get; set; }
        public Label LabelControl { get; set; }
        public object ExpressionValue { get; set; }
        public object DefaultValue { get; set; }
        public bool HasValue { get; set; }
        public string HelpTip { get; set; }
        public string HelpClassName { get; set; }
        public string HelpNavigator { get; set; }
        public SearchHelp SearchHelp { get; set; }
        public virtual MvcHtmlString Render()
        {
            if (this.Visible)
                return new MvcHtmlString(base.Draw());
            else
                return new MvcHtmlString("");
        }

        protected override void onBeforeRenderControl(TextWriter writer)
        {
            if (this.HtmlAttributes != null)
            {
                var type = this.HtmlAttributes.GetType();
                var props = type.GetProperties().ToDictionary(op => op.Name, op => op.GetValue(this.HtmlAttributes, null));
                var newProps = new Dictionary<string, object>();
                var dataControlProps = new Dictionary<string, object>();
                foreach (var item in props)
                {
                    if (item.Key.StartsWith("datacontrol_"))
                    {
                        var key = item.Key.Replace("datacontrol_", "");
                        if (key == "class")
                        {
                            this.DataControl.CssClass += " " + item.Value;
                        }
                        else
                        {
                            this.DataControl.Attributes.Add(key, Convert.ToString(item.Value));
                        }
                    }
                    else
                    {
                        newProps[item.Key] = item.Value;
                    }
                }
                this.HtmlAttributes = newProps;
            }
            this.SetDataValue();
            this.CssClass = "form-group";
            if (!this.Editable)
                this.DataControl.Attributes.Add("readonly", "readonly");
            base.onBeforeRenderControl(writer);
        }
        protected override void onRenderControl(TextWriter writer)
        {
            if (this.HasValue)
                this.LabelControl.CssClass += " has-value";
            base.onRenderControl(writer);
        }
        internal object GetExpressionValue(Expression expression = null)
        {
            if (expression == null)
                expression = this.Expression;

            return expression.GetValue(this.FieldContainer.Entity, this.FieldContainer.CurrentLanguageID, this.FieldContainer.DefaultEntityProperties);
        }
        internal virtual void SetDataValue()
        {
            if (this.Expression != null && this.ExpressionValue == null)
            {
                this.DataControl.Name = this.Expression.Body.ParsePath();
                this.DataControl.ID = this.DataControl.Name.Replace(".", "_");
                this.ExpressionValue = this.GetExpressionValue();
                if (string.IsNullOrEmpty(this.Text))
                {
                    this.Text = this.DataControl.Name;
                    if (this.Text.IndexOf(".") > -1)
                        this.Text = this.Text.Right(this.Text.Length - this.Text.IndexOf(".") - 1);
                }
                if (!this.HasValue)
                    this.HasValue = this.ExpressionValue != null;

                if (!string.IsNullOrEmpty(this.Text))
                {
                    if (this.CanTranslateLabelText)
                        this.LabelControl.Text = this.Client.TranslateText(this.Text);
                    else
                        this.LabelControl.Text = this.Text;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(this.Text))
                {
                    if (this.CanTranslateLabelText)
                    {
                        this.LabelControl.Text = this.Client.TranslateText(this.Text);
                    }
                    else
                        this.LabelControl.Text = this.Text;
                }
            }
            if (!string.IsNullOrEmpty(this.LabelControl.Text) && this.IsRequired)
                this.LabelControl.Text += " (*)";

            if (this.IsRequired && !string.IsNullOrEmpty(this.DataControl.CssClass) && this.DataControl.CssClass.IndexOf(" required") == -1)
            {
                this.DataControl.CssClass += " required";
                this.DataControl.Attributes.Add("aria-required", "true");
            }
            if (!string.IsNullOrEmpty(this.HelpClassName) && (!string.IsNullOrEmpty(this.HelpTip)) || !string.IsNullOrEmpty(this.HelpNavigator))
            {
                this.LabelControl.Controls.Add(new System.Web.UI.WebControls.Literal() { Text = "<i class=\"" + this.HelpClassName + "\" data-help-tooltip=\"" + this.HelpTip + "\" data-help-navigator=\"" + this.HelpNavigator + "\"></i>" });
            }
            this.LabelControl.Text = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(this.LabelControl.Text, false);
        }
        protected abstract WebControl CreateDataControl();

        public BaseField(FieldContainer<T> FieldContainer)
        {
            this.FieldContainer = FieldContainer;
            this.DataControl = this.CreateDataControl();
            this.LabelControl = new Label();
            this.DataControlParent = new Panel();
            if (this.DataControl != null)
                this.DataControlParent.Controls.Add(this.DataControl);

            this.Controls.Add(this.LabelControl);
            this.Controls.Add(this.DataControlParent);

            this.LabelControl.CssClass = string.IsNullOrEmpty(this.FieldContainer.LabelCssClass) ? "control-label col-lg-3" : this.FieldContainer.LabelCssClass;
            this.DataControlParent.CssClass = string.IsNullOrEmpty(this.FieldContainer.DataControlParentCssClass) ? "col-lg-9" : this.FieldContainer.DataControlParentCssClass;
            this.Visible = true;
            this.Editable = true;
            this.CanTranslateLabelText = true;
        }
    }
}
