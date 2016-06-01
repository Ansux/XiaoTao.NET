using System.Web.Mvc;
using xiaotao.Models;

namespace xiaotao.Filters
{
   public class StuVerifyAuthorize : ActionFilterAttribute
   {
      public override void OnActionExecuting(ActionExecutingContext filterContext)
      {
         if (filterContext.HttpContext.Session["Verify"] == null)
         {
            string Url = "~/mall/account/Security";
            filterContext.Result = new RedirectResult(Url, true);
         }
      }
   }
}