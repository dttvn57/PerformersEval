using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PerformersEval
{
    public class NotRequireSSLAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var response = filterContext.HttpContext.Response;

            if (request.IsSecureConnection && !request.IsLocal)
            {
                string redirectUrl = request.Url.ToString().Replace("https:", "http:");
                response.Redirect(redirectUrl);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}