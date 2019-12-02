using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Ophelia.Web.View.Mvc.Html
{
    public static class Analytics
    {
        public static IHtmlString GoogleAnalytics(this HtmlHelper htmlHelper)
        {
            if (!ConfigurationManager.GoogleAnalyticsEnabled || string.IsNullOrEmpty(ConfigurationManager.GoogleAnalyticsAccountID)) return null;
            var analyticsScript = new StringBuilder();
            analyticsScript.Append(@"<script type='text/javascript'>
                                    (function (i, s, o, g, r, a, m) {
                                        i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                                            (i[r].q = i[r].q || []).push(arguments)
                                        }, i[r].l = 1 * new Date(); a = s.createElement(o),
		                                m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
                                    })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');");
            analyticsScript.Append("ga('create', '").Append(ConfigurationManager.GoogleAnalyticsAccountID).Append("', '");
            analyticsScript.Append(ConfigurationManager.DomainUrl.Authority ?? "auto").Append("');");
            analyticsScript.Append("ga('send', 'pageview');</script>");
            return htmlHelper.Raw(analyticsScript.ToString());
        }
        public static IHtmlString GoogleTagManager(this HtmlHelper htmlHelper)
        {
            if (!ConfigurationManager.TagManagerEnabled || string.IsNullOrEmpty(ConfigurationManager.TagManagerAccountID)) return null;
            var tagManagerScript = new StringBuilder();
            tagManagerScript.Append(@"<noscript><iframe src='//www.googletagmanager.com/ns.html?id=");
            tagManagerScript.Append(ConfigurationManager.TagManagerAccountID);
            tagManagerScript.Append("' height='0' width='0' class='hide invisible'></iframe></noscript>");
            tagManagerScript.Append("<script type='text/javascript'>");
            tagManagerScript.Append(@"(function (w, d, s, l, i) { w[l] = w[l] || []; w[l].push({ 'gtm.start': new Date().getTime(), event: 'gtm.js' });
                                        var f = d.getElementsByTagName(s)[0],
                                        j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; 
                                        j.src='//www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);});");
            tagManagerScript.Append("(window, document, 'script', 'dataLayer', '").Append(ConfigurationManager.TagManagerAccountID).Append("');");
            tagManagerScript.Append("</script>");
            return htmlHelper.Raw(tagManagerScript.ToString());
        }
    }
}
