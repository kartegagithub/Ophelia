using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Ophelia.Web.View.Mvc.Models
{
    public class DynamicViewModel
    {
        public dynamic ViewBag { get; private set; }

        private DynamicViewModel() { }

        public static DynamicViewModel Create(object anonymousObject)
        {
            IDictionary<string, object> anonymousDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(anonymousObject);
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var item in anonymousDictionary)
                expando.Add(item);

            return new DynamicViewModel { ViewBag = expando as ExpandoObject };
        }
    }
}
