using System.Web.Mvc;
using xiaotao.Models;

namespace xiaotao.Filters
{
   public class StuLoginAuthorize : ActionFilterAttribute
   {
      public override void OnActionExecuting(ActionExecutingContext filterContext)
      {
         if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
               || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
         {
            return;
         }

         if (filterContext.HttpContext.Session["Sid"] == null)
         {
            string redirectOnSuccess = filterContext.HttpContext.Request.RawUrl;
            string redirectUrl = string.Format("?ReturnUrl={0}", redirectOnSuccess);
            string loginUrl = "~/mall/account/signin" + redirectUrl;
            filterContext.Result = new RedirectResult(loginUrl, true);
         }
      }
   }
}