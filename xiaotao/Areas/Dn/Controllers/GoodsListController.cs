using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using xiaotao.Models;

namespace xiaotao.Areas.Dn.Controllers
{
   public class GoodsListController : Controller
   {
      private WebContext db = new WebContext();
      public ActionResult Index(int pageIndex = 1, int pageSize = 8)
      {
         var goods = db.dn_goods.Where(e => e.is_onsale == true && e.is_delete == false).Include(e => e.dn_category).Include(e => e.xt_student);

         var Count = goods.Count();

         var pageCount = (Count % pageSize == 0) ? (Count / pageSize) : (Count / pageSize + 1);

         ViewData["pager"] = Filters.Tools.PagerDesign(pageIndex, pageCount, "/dn/goodslist/index?pageIndex=");

         return View(goods.ToList().Skip(pageSize * (pageIndex - 1)).Take(pageSize));
      }
   }
}