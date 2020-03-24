﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Reflection;
using Ophelia.Web.View.Mvc.Models;

namespace Ophelia.Web.View.Mvc.Controls.Binders
{
    public static class FieldMvcExtensions
    {
        public static MvcHtmlString CollectionBinderFieldFor<T, TModel, TEntity>(this FieldContainer<T> container, CollectionBinder.CollectionBinder<TModel, TEntity> binder, bool hideLabel = false) where T : class where TEntity : class where TModel : ListModel<TEntity>
        {
            var control = new Fields.CollectionBinderField<TModel, TEntity, T>(container, binder, hideLabel);
            binder.ParentDrawsLayout = true;
            container.AddField(control);
            return control.Render();
        }

        public static MvcHtmlString TextboxField<T>(this FieldContainer<T> container, string name, bool isRequired = false) where T : class { return container.TextboxField(name, "", isRequired); }
        public static MvcHtmlString TextboxField<T>(this FieldContainer<T> container, string name, string value, bool isRequired) where T : class { return container.TextboxField(name, value, isRequired, null); }
        public static MvcHtmlString TextboxField<T>(this FieldContainer<T> container, string name, string value, bool isRequired, object htmlAttributes) where T : class { return container.TextboxField(name, name, value, isRequired, htmlAttributes); }
        public static MvcHtmlString TextboxFieldFor<T>(this FieldContainer<T> container, Expression<Func<T, object>> expression, bool isRequired = false, object htmlAttributes = null) where T : class { return container.TextboxFieldFor("", expression, isRequired, htmlAttributes); }
        public static MvcHtmlString TextboxField<T>(this FieldContainer<T> container, string Text, string name, string value, bool isRequired, object htmlAttributes, bool canTranslateLabelText = true) where T : class
        {
            var control = new Fields.TextboxField<T>(container);
            control.DataControl.ID = name;
            control.DataControl.Name = name;
            control.DataControl.Value = value;
            control.HtmlAttributes = htmlAttributes;
            control.IsRequired = isRequired;
            control.CanTranslateLabelText = canTranslateLabelText;
            control.Text = Text;
            container.AddField(control);
            return control.Render();
        }
        public static MvcHtmlString TextboxFieldFor<T>(this FieldContainer<T> container, string Text, Expression<Func<T, object>> expression, bool isRequired = false, object htmlAttributes = null) where T : class
        {
            var control = new Fields.TextboxField<T>(container);
            control.Expression = expression;
            control.Text = Text;
            control.IsRequired = isRequired;
            control.HtmlAttributes = htmlAttributes;
            container.AddField(control);
            return control.Render();
        }

        public static MvcHtmlString LinkField<T>(this FieldContainer<T> container, string controlName, string labelText, string linkValue, string URL, object htmlAttributes = null) where T : class
        {
            var control = new Fields.LinkField<T>(container);
            control.ID = controlName;
            control.DataControl.URL = URL;
            control.DataControl.Name = controlName;
            control.DataControl.Text = linkValue;
            control.HtmlAttributes = htmlAttributes;
            control.Text = labelText;
            container.AddField(control);
            return control.Render();
        }

        public static MvcHtmlString NumberboxFieldFor<T>(this FieldContainer<T> container, Expression<Func<T, object>> expression, bool isRequired = false, object htmlAttributes = null, string format = "") where T : class { return container.NumberboxFieldFor("", expression, isRequired, htmlAttributes, format); }
        public static MvcHtmlString NumberboxFieldFor<T>(this FieldContainer<T> container, string Text, Expression<Func<T, object>> expression, bool isRequired = false, object htmlAttributes = null, string format = "") where T : class
        {
            var control = new Fields.NumberboxField<T>(container);
            control.Expression = expression;
            control.Text = Text;
            control.IsRequired = isRequired;
            control.HtmlAttributes = htmlAttributes;
            control.SetFormat(format, container.DecimalFormat, container.IntFormat);
            container.AddField(control);
            return control.Render();
        }

        public static MvcHtmlString NumberboxField<T>(this FieldContainer<T> container, string Text, string name, bool isRequired = false, object htmlAttributes = null, bool canTranslateLabelText = true, object value = null, string format = "") where T : class
        {
            var control = new Fields.NumberboxField<T>(container);
            control.DataControl.ID = name;
            control.DataControl.Name = name;
            control.Text = Text;
            control.SetFormat(format, container.DecimalFormat, container.IntFormat);
            control.CanTranslateLabelText = canTranslateLabelText;
            control.IsRequired = isRequired;
            control.HtmlAttributes = htmlAttributes;
            control.ExpressionValue = value;
            container.AddField(control);
            return control.Render();
        }

        public static MvcHtmlString DateFieldFor<T>(this FieldContainer<T> container, Expression<Func<T, object>> expression, DateTimeFormatType format = DateTimeFormatType.DateOnly, bool isRequired = false, object htmlAttributes = null, bool IsReadonly = false) where T : class { return container.DateFieldFor("", expression, format, isRequired, htmlAttributes, IsReadonly); }
        public static MvcHtmlString DateFieldFor<T>(this FieldContainer<T> container, string Text, Expression<Func<T, object>> expression, DateTimeFormatType format = DateTimeFormatType.DateOnly, bool isRequired = false, object htmlAttributes = null, bool IsReadonly = false) where T : class
        {
            var control = new Fields.DateField<T>(container);
            control.Expression = expression;
            control.Text = Text;
            control.IsRequired = isRequired;
            control.Format = format;
            control.Editable = !IsReadonly;
            control.HtmlAttributes = htmlAttributes;
            container.AddField(control);
            return control.Render();
        }

        public static MvcHtmlString DateField<T>(this FieldContainer<T> container, string Text, string name, DateTimeFormatType format = DateTimeFormatType.DateOnly, bool isRequired = false, object htmlAttributes = null, bool canTranslateLabelText = true, object value = null) where T : class
        {
            var control = new Fields.DateField<T>(container);
            control.DataControl.ID = name;
            control.DataControl.Name = name;
            control.Text = Text;
            control.CanTranslateLabelText = canTranslateLabelText;
            control.IsRequired = isRequired;
            control.Format = format;
            control.HtmlAttributes = htmlAttributes;
            control.ExpressionValue = value;
            container.AddField(control);
            return control.Render();
        }

        public static MvcHtmlString PasswordFieldFor<T>(this FieldContainer<T> container, Expression<Func<T, object>> expression, bool isRequired = false, object htmlAttributes = null) where T : class { return container.PasswordFieldFor("", expression, isRequired, htmlAttributes); }
        public static MvcHtmlString PasswordFieldFor<T>(this FieldContainer<T> container, string Text, Expression<Func<T, object>> expression, bool isRequired = false, object htmlAttributes = null) where T : class
        {
            var control = new Fields.PasswordField<T>(container);
            control.Expression = expression;
            control.Text = Text;
            control.IsRequired = isRequired;
            control.HtmlAttributes = htmlAttributes;
            container.AddField(control);
            return control.Render();
        }

        public static MvcHtmlString FilterboxFieldFor<T>(this FieldContainer<T> container, string ajaxURL, Expression<Func<T, object>> expression, Expression<Func<T, object>> selectedValueExpression, string valueMember = "ID", string DisplayMember = "Name", bool isRequired = false, object htmlAttributes = null, bool IsMultiple = false) where T : class { return container.FilterboxFieldFor("", ajaxURL, expression, selectedValueExpression, valueMember, DisplayMember, isRequired, htmlAttributes, IsMultiple); }
        public static MvcHtmlString FilterboxFieldFor<T>(this FieldContainer<T> container, string ajaxURL, Expression<Func<T, object>> expression, Expression<Func<T, object>> selectedValueExpression, Func<T, object> DisplayMemberExpression, string valueMember = "ID", bool isRequired = false, object htmlAttributes = null, bool IsMultiple = false) where T : class { return container.FilterboxFieldFor("", ajaxURL, expression, selectedValueExpression, valueMember, "", isRequired, htmlAttributes, IsMultiple, DisplayMemberExpression); }
        public static MvcHtmlString FilterboxFieldFor<T>(this FieldContainer<T> container, string Text, string ajaxURL, Expression<Func<T, object>> expression, Expression<Func<T, object>> selectedValueExpression, string valueMember = "ID", string DisplayMember = "Name", bool isRequired = false, object htmlAttributes = null, bool IsMultiple = false, Func<T, object> DisplayMemberExpression = null) where T : class
        {
            var control = new Fields.FilterboxField<T>(container);
            control.DataControl.IsMultiple = IsMultiple;
            control.Expression = expression;
            control.SelectedValueExpression = selectedValueExpression;
            control.ValueMember = valueMember;
            control.DisplayMemberExpression = DisplayMemberExpression;
            control.DisplayMember = DisplayMember;
            control.Text = Text;
            control.IsRequired = isRequired;
            control.HtmlAttributes = htmlAttributes;
            control.AjaxURL = ajaxURL;
            container.AddField(control);
            container.SetFieldProperties(control);
            return control.Render();
        }

        public static MvcHtmlString FilterboxField<T>(this FieldContainer<T> container, string Text, string name, string ajaxURL, object selectedValue, string valueMember = "ID", string DisplayMember = "Name", bool isRequired = false, object htmlAttributes = null, bool IsMultiple = false, bool CanTranslateLabelText = true, object selectListValue = null) where T : class
        {
            var control = new Fields.FilterboxField<T>(container);
            control.DataControl.ID = name;
            control.DataControl.Name = name;
            control.DataControl.IsMultiple = IsMultiple;
            control.SelectedValue = selectedValue;
            control.ValueMember = valueMember;
            control.DisplayMember = DisplayMember;
            control.Text = Text;
            control.IsRequired = isRequired;
            control.HtmlAttributes = htmlAttributes;
            control.AjaxURL = ajaxURL;
            control.CanTranslateLabelText = CanTranslateLabelText;
            control.SelectListValue = selectListValue;
            container.AddField(control);
            container.SetFieldProperties(control);
            return control.Render();
        }

        public static MvcHtmlString LabelFieldFor<T>(this FieldContainer<T> container, Expression<Func<T, object>> expression, object htmlAttributes = null, bool HasNullableControl = true) where T : class { return container.LabelFieldFor("", expression, htmlAttributes, HasNullableControl); }
        public static MvcHtmlString LabelFieldFor<T>(this FieldContainer<T> container, string Text, Expression<Func<T, object>> expression, object htmlAttributes = null, bool HasNullableControl = true) where T : class
        {
            var control = new Fields.LabelField<T>(container);
            control.Expression = expression;
            control.Text = Text;
            control.HtmlAttributes = htmlAttributes;
            container.AddField(control);
            if (HasNullableControl)
            {
                if (control.ExpressionValue != null && !string.IsNullOrEmpty(control.ExpressionValue.ToString()))
                    return control.Render();
            }
            else
                return control.Render();
            return new MvcHtmlString("");
        }
        public static MvcHtmlString LabelField<T>(this FieldContainer<T> container, string Text, string Value, object htmlAttributes = null, bool HasNullableControl = true) where T : class
        {
            var control = new Fields.LabelField<T>(container);
            control.Text = Text;
            control.ID = Text;
            control.Value = Value;
            control.HtmlAttributes = htmlAttributes;
            container.AddField(control);
            if (HasNullableControl)
            {
                if (!string.IsNullOrEmpty(Value))
                    return control.Render();
            }
            else
                return control.Render();
            return new MvcHtmlString("");
        }

        public static MvcHtmlString EnumLabelFieldFor<T>(this FieldContainer<T> container, Expression<Func<T, object>> expression, Type enumType, object htmlAttributes = null) where T : class { return container.EnumLabelFieldFor("", expression, enumType, htmlAttributes); }
        public static MvcHtmlString EnumLabelFieldFor<T>(this FieldContainer<T> container, string Text, Expression<Func<T, object>> expression, Type enumType, object htmlAttributes = null) where T : class
        {
            var control = new Fields.LabelField<T>(container);
            control.Expression = expression;
            control.Text = Text;
            control.EnumType = enumType;
            control.HtmlAttributes = htmlAttributes;
            container.AddField(control);
            return control.Render();
        }

        public static MvcHtmlString FileboxFieldFor<T>(this FieldContainer<T> container, Expression<Func<T, object>> expression, bool isRequired = false, object htmlAttributes = null) where T : class { return container.FileboxFieldFor("", expression, isRequired, htmlAttributes); }
        public static MvcHtmlString FileboxFieldFor<T>(this FieldContainer<T> container, string Text, Expression<Func<T, object>> expression, bool isRequired = false, object htmlAttributes = null) where T : class
        {
            var control = new Fields.FileboxField<T>(container);
            control.Expression = expression;
            control.Text = Text;
            control.IsRequired = isRequired;
            control.HtmlAttributes = htmlAttributes;
            container.AddField(control);
            return control.Render();
        }

        public static MvcHtmlString FileboxField<T>(this FieldContainer<T> container, string LabelText, string ControlID, object htmlAttributes = null) where T : class
        {
            var control = new Fields.FileboxField<T>(container);
            control.Text = LabelText;
            control.DataControl.ID = ControlID;
            control.DataControl.Name = ControlID;
            control.HtmlAttributes = htmlAttributes;
            container.AddField(control);
            return control.Render();
        }
        public static MvcHtmlString FileboxField<T>(this FieldContainer<T> container, string ControlID, object htmlAttributes = null) where T : class
        {
            return FileboxField(container, ControlID, ControlID, htmlAttributes);
        }

        public static MvcHtmlString RichTextFieldFor<T>(this FieldContainer<T> container, Expression<Func<T, object>> expression, bool isRequired = false, object htmlAttributes = null) where T : class { return container.RichTextFieldFor("", expression, isRequired, htmlAttributes); }
        public static MvcHtmlString RichTextFieldFor<T>(this FieldContainer<T> container, string Text, Expression<Func<T, object>> expression, bool isRequired = false, object htmlAttributes = null) where T : class
        {
            var control = new Fields.RichTextField<T>(container);
            control.Expression = expression;
            control.Text = Text;
            control.IsRequired = isRequired;
            control.HtmlAttributes = htmlAttributes;
            container.AddField(control);
            return control.Render();
        }

        public static MvcHtmlString CheckboxFieldFor<T>(this FieldContainer<T> container, Expression<Func<T, object>> expression, bool isRequired = false, object htmlAttributes = null, string DataOnText = "Yes", string DataOffText = "No") where T : class { return container.CheckboxFieldFor("", expression, isRequired, htmlAttributes, DataOnText, DataOffText); }
        public static MvcHtmlString CheckboxFieldFor<T>(this FieldContainer<T> container, string Text, Expression<Func<T, object>> expression, bool isRequired = false, object htmlAttributes = null, string DataOnText = "Yes", string DataOffText = "No") where T : class
        {
            var control = new Fields.CheckboxField<T>(container);
            control.Expression = expression;
            control.Text = Text;
            control.IsRequired = isRequired;
            control.HtmlAttributes = htmlAttributes;
            control.DataControl.DataOnText = DataOnText;
            control.DataControl.DataOffText = DataOffText;
            container.AddField(control);
            return control.Render();
        }

        public static MvcHtmlString CheckboxField<T>(this FieldContainer<T> container, string Text, string name, bool isRequired = false, object htmlAttributes = null, bool canTranslateLabelText = true, object value = null, string DataOnText = "Yes", string DataOffText = "No") where T : class
        {
            var control = new Fields.CheckboxField<T>(container);
            control.Text = Text;
            control.CanTranslateLabelText = canTranslateLabelText;
            control.DataControl.ID = name;
            control.DataControl.Name = name;
            control.IsRequired = isRequired;
            control.HtmlAttributes = htmlAttributes;
            control.ExpressionValue = value;
            control.DataControl.DataOnText = DataOnText;
            control.DataControl.DataOffText = DataOffText;
            container.AddField(control);
            return control.Render();
        }

        public static MvcHtmlString RadioboxFieldFor<T>(this FieldContainer<T> container, Expression<Func<T, object>> expression, bool isRequired = false, object htmlAttributes = null) where T : class { return container.RadioboxFieldFor("", expression, isRequired, htmlAttributes); }
        public static MvcHtmlString RadioboxFieldFor<T>(this FieldContainer<T> container, string Text, Expression<Func<T, object>> expression, bool isRequired = false, object htmlAttributes = null) where T : class
        {
            var control = new Fields.RadioboxField<T>(container);
            control.Expression = expression;
            control.Text = Text;
            control.IsRequired = isRequired;
            control.HtmlAttributes = htmlAttributes;
            container.AddField(control);
            return control.Render();
        }

        public static MvcHtmlString TextAreaFieldFor<T>(this FieldContainer<T> container, Expression<Func<T, object>> expression, bool isRequired = false, object htmlAttributes = null) where T : class { return container.TextAreaFieldFor("", expression, isRequired, htmlAttributes); }
        public static MvcHtmlString TextAreaFieldFor<T>(this FieldContainer<T> container, string Text, Expression<Func<T, object>> expression, bool isRequired = false, object htmlAttributes = null) where T : class
        {
            var control = new Fields.TextAreaField<T>(container);
            control.Expression = expression;
            control.Text = Text;
            control.IsRequired = isRequired;
            control.HtmlAttributes = htmlAttributes;
            container.AddField(control);
            return control.Render();
        }

        public static MvcHtmlString TextAreaField<T>(this FieldContainer<T> container, string Text, string name, string value = "", bool isRequired = false, object htmlAttributes = null, bool canTranslateLabelText = true) where T : class
        {
            var control = new Fields.TextAreaField<T>(container);
            control.DataControl.Value = value;
            control.CanTranslateLabelText = canTranslateLabelText;
            control.Text = Text;
            control.DataControl.ID = name;
            control.DataControl.Name = name;
            control.IsRequired = isRequired;
            control.HtmlAttributes = htmlAttributes;
            container.AddField(control);
            return control.Render();
        }

        public static MvcHtmlString SelectboxFieldFor<T>(this FieldContainer<T> container, Expression<Func<T, object>> expression, IEnumerable dataSource, bool isRequired = false, object htmlAttributes = null, string DefaultText = "", string DefaultValue = "", string DisplayMemberName = "Name", string ValueMemberName = "ID", bool IsMultiple = false) where T : class { return container.SelectboxFieldFor("", expression, dataSource, isRequired, htmlAttributes, DefaultText, DefaultValue, DisplayMemberName, ValueMemberName, IsMultiple); }
        public static MvcHtmlString SelectboxFieldFor<T>(this FieldContainer<T> container, string Text, Expression<Func<T, object>> expression, IEnumerable dataSource, bool isRequired = false, object htmlAttributes = null, string DefaultText = "", string DefaultValue = "", string DisplayMemberName = "Name", string ValueMemberName = "ID", bool IsMultiple = false) where T : class
        {
            var control = new Fields.SelectboxField<T>(container);
            control.Expression = expression;
            control.DataControl.DataSource = dataSource;
            control.DataControl.DisplayMemberName = DisplayMemberName;
            control.DataControl.ValueMemberName = ValueMemberName;
            control.DataControl.DefaultText = DefaultText;
            control.DataControl.DefaultValue = DefaultValue;
            control.DataControl.IsMultiple = IsMultiple;
            control.Text = Text;
            control.IsRequired = isRequired;
            control.HtmlAttributes = htmlAttributes;
            container.AddField(control);
            container.SetFieldProperties(control);
            return control.Render();
        }

        public static MvcHtmlString SelectboxField<T>(this FieldContainer<T> container, string name, string SelectedValue, IEnumerable dataSource, bool isRequired = false, object htmlAttributes = null, string DefaultText = "", string DefaultValue = "", string DisplayMemberName = "Name", string ValueMemberName = "ID", string IndentationMemberName = "", bool IsMultiple = false) where T : class
        {
            return SelectboxField(container, name, name, SelectedValue, dataSource, isRequired, htmlAttributes, true, DefaultText, DefaultValue, DisplayMemberName, ValueMemberName, IndentationMemberName, IsMultiple);
        }

        public static MvcHtmlString SelectboxField<T>(this FieldContainer<T> container, string Text, string name, string SelectedValue, IEnumerable dataSource, bool isRequired = false, object htmlAttributes = null, bool canTranslateLabelText = true, string DefaultText = "", string DefaultValue = "", string DisplayMemberName = "Name", string ValueMemberName = "ID", string IndentationMemberName = "", bool IsMultiple = false) where T : class
        {
            var control = new Fields.SelectboxField<T>(container);
            control.CanTranslateLabelText = canTranslateLabelText;
            control.DataControl.DataSource = dataSource;
            control.DataControl.SelectedValue = SelectedValue;
            control.DataControl.DisplayMemberName = DisplayMemberName;
            control.DataControl.IndentationMemberName = IndentationMemberName;
            control.DataControl.ValueMemberName = ValueMemberName;
            control.DataControl.DefaultText = DefaultText;
            control.DataControl.DefaultValue = DefaultValue;
            control.DataControl.IsMultiple = IsMultiple;
            control.Text = Text;
            control.DataControl.ID = name;
            control.DataControl.Name = name;
            control.IsRequired = isRequired;
            control.HtmlAttributes = htmlAttributes;
            container.AddField(control);
            container.SetFieldProperties(control);
            return control.Render();
        }

        public static MvcHtmlString EnumSelectboxFieldFor<T>(this FieldContainer<T> container, Expression<Func<T, object>> expression, Type enumType, bool isRequired = false, object htmlAttributes = null, string DefaultText = "", string DefaultValue = "") where T : class { return container.EnumSelectboxFieldFor("", expression, enumType, isRequired, htmlAttributes, DefaultText, DefaultValue); }
        public static MvcHtmlString EnumSelectboxFieldFor<T>(this FieldContainer<T> container, string Text, Expression<Func<T, object>> expression, Type enumType, bool isRequired = false, object htmlAttributes = null, string DefaultText = "", string DefaultValue = "") where T : class
        {
            var control = new Fields.SelectboxField<T>(container);
            control.Expression = expression;
            control.Text = Text;
            control.DataControl.DataSource = enumType.GetEnumSelectList(container.Client);
            control.DataControl.DefaultText = DefaultText;
            control.DataControl.DefaultValue = DefaultValue;
            control.DataControl.DisplayMemberName = "Text";
            control.DataControl.ValueMemberName = "Value";
            control.IsRequired = isRequired;
            control.HtmlAttributes = htmlAttributes;
            container.AddField(control);
            return control.Render();
        }
        public static Fields.BlankField<T> BlankField<T>(this FieldContainer<T> container, string Text, bool TranslateText = true) where T : class
        {
            var control = new Fields.BlankField<T>(container);
            if (TranslateText)
                control.Text = container.Client.TranslateText(Text);
            else
                control.Text = Text;
            control.ID = Text;
            container.AddField(control);
            return control;
        }
    }
}
