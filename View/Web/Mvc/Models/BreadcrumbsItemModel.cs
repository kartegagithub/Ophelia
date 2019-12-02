using System;
using Ophelia.Web.View.Mvc.Html;
namespace Ophelia.Web.View.Mvc.Models
{
    public class BreadcrumbsItemModel
    {
        public string Text { get; set; }
        public string Title { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public object RouteValues { get; set; }
        public bool IsActive { get; set; }
        public Glyphicons Icon { get; set; }
    }
}
