using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ophelia.Web.View.Mvc.Models
{
    public class DynamicListItemModel
    {
        public dynamic ViewBag { get; private set; }

        public DynamicListItemModel()
        {
            ViewBag = new ExpandoObject();
        }

        public DynamicListItemModel(object anonymousObject)
        {
            IDictionary<string, object> anonymousDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(anonymousObject);
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var item in anonymousDictionary)
                expando.Add(item);
            ViewBag = expando as ExpandoObject;
        }
    }
}