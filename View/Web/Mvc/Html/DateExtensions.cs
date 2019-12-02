using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Ophelia.Web.View.Mvc.Html
{
    public static class DateExtensions
    {
        public static MvcHtmlString DateTextboxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, DateTime>> expression, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var model = DateTime.MinValue;
            if (htmlHelper.ViewData.Model != null)
            {
                model = expression.Compile().Invoke(htmlHelper.ViewData.Model);
            }
            if (model <= DateTime.MinValue || model >= DateTime.MaxValue)
            {
                return htmlHelper.TextBox(ExpressionHelper.GetExpressionText(expression), "", htmlAttributes);
            }
            else
            {
                return htmlHelper.TextBox(ExpressionHelper.GetExpressionText(expression), model.ToString("dd/MM/yyyy"), htmlAttributes);
            }
        }

        public static MvcHtmlString DateTextboxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, string>> expression, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var model = "";
            if (htmlHelper.ViewData.Model != null)
            {
                model = expression.Compile().Invoke(htmlHelper.ViewData.Model);
            }
            if (string.IsNullOrEmpty(model))
                return htmlHelper.TextBox(ExpressionHelper.GetExpressionText(expression), "", htmlAttributes);

            try
            {
                DateTime time = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + model);
                return htmlHelper.TextBox(ExpressionHelper.GetExpressionText(expression), time.ToShortTimeString(), htmlAttributes);
            }
            catch (Exception)
            {
                return htmlHelper.TextBox(ExpressionHelper.GetExpressionText(expression), "", htmlAttributes);
            }
        }
    }
}
