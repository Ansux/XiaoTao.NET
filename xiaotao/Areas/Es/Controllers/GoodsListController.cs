using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using xiaotao.Controllers;
using xiaotao.Models;

namespace xiaotao.Areas.Es.Controllers
{
   public class GoodsListController : Controller
   {
      private WebContext db = new WebContext();
      public ActionResult Index(string sort = "default", int pageIndex = 1, int pageSize = 8)
      {
         var goods = db.es_goods.Where(e => e.is_onsale == true && e.is_delete == false).Include(e => e.es_category).Include(e => e.xt_student);

         if (sort == "times")
         {
            goods = goods.OrderByDescending(e => e.create_at);
         }
         else if (sort == "price")
         {
            goods = goods.OrderByDescending(e => e.price);
         }

         var Count = goods.Count();

         var pageCount = (Count % pageSize == 0) ? (Count / pageSize) : (Count / pageSize + 1);

         ViewData["pager"] = ToolsController.PagerDesign(pageIndex, pageCount, "/es/goodslist/index?sort=" + sort + "&pageIndex=");
         ViewData["miniPager"] = ToolsController.MiniPagerDesign(pageIndex, pageCount, "/es/goodslist/index?sort=" + sort + "&pageIndex=");

         return View(goods.ToList().Skip(pageSize * (pageIndex - 1)).Take(pageSize));
      }
   }
}