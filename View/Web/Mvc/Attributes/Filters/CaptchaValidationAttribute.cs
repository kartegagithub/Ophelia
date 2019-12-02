using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using Ophelia.Web.View.Mvc.Html;

namespace Ophelia.Web.View.Mvc.Attributes
{
    public class CaptchaValidationAttribute : ActionFilterAttribute
    {
        public bool AlwaysShow { get; set; }
        public int ErrorCount { get; set; }
        public CaptchaValidationAttribute(bool alwaysShow, int errorCount)
        {
            this.AlwaysShow = alwaysShow;
            this.ErrorCount = errorCount;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpRequestBase request = filterContext.RequestContext.HttpContext.Request;
            Captcha captcha = new Captcha();
            captcha.CaptchaModel.AlwaysShow = this.AlwaysShow;
            captcha.CaptchaModel.ErrorCount = this.ErrorCount;
            if (captcha.Enabled())
            {
                var InputCaptcha = request.Form["CaptchaCode"];
                var CaptchaSessionKey = request.Form["CKey"];
                if (!captcha.CheckCaptcha(InputCaptcha, CaptchaSessionKey))
                    ((Controller)filterContext.Controller).ModelState.AddModelError("CaptchaError", captcha.CaptchaModel.AlertMessage.Message);
            }
        }
    }
}
