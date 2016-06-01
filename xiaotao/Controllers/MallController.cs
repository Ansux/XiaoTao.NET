using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using xiaotao.Models;

namespace xiaotao.Controllers
{
   public class MallController : Controller
   {
      WebContext db = new WebContext();
      // GET: Web
      public ActionResult Index()
      {
         //return Redirect("mall/prolist");
         return View(db.sp_product.ToList());
      }
   }
}