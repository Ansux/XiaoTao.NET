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
         ViewBag.F1 = db.sp_product.Where(e => e.category == 6).Take(4).ToList();
         ViewBag.F2 = db.sp_product.Where(e => e.category == 11).Take(4).ToList();
         ViewBag.F3 = db.sp_product.Where(e => e.category == 12).Take(4).ToList();
         return View();
      }
   }
}