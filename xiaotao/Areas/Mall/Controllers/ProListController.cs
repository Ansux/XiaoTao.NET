using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using xiaotao.Controllers;
using xiaotao.Filters;
using xiaotao.Models;

namespace xiaotao.Areas.Mall.Controllers
{
   public class ProListController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index(string sort = "default", int pageIndex = 1, int pageSize = 8)
      {
         var Products = db.sp_product.Include(s => s.sp_brand).Include(s => s.sp_category).Include(s => s.xt_store);
         if (sort == "sales")
         {
            Products = Products.OrderByDescending(e => e.sales);
         }
         else if (sort == "price")
         {
            Products = Products.OrderByDescending(e => e.price);
         }

         var Count = Products.Count();

         var pageCount = (Count % pageSize == 0) ? (Count / pageSize) : (Count / pageSize + 1);

         ViewData["pager"] = ToolsController.PagerDesign(pageIndex, pageCount, "/mall/prolist/index?sort=" + sort + "&pageIndex=");

         ViewData["miniPager"] = ToolsController.MiniPagerDesign(pageIndex, pageCount, "/mall/prolist/index?sort=" + sort + "&pageIndex=");

         return View(Products.ToList().Skip(pageSize * (pageIndex - 1)).Take(pageSize));
      }

      public ActionResult Search(string kw = "", string sort = "default", int pageIndex = 1, int pageSize = 8)
      {
         var Products = db.sp_product.Where(e => e.name.Contains(kw)).Include(s => s.sp_brand).Include(s => s.sp_category).Include(s => s.xt_store);

         if (sort == "sales")
         {
            Products = Products.OrderByDescending(e => e.sales);
         }
         else if (sort == "price")
         {
            Products = Products.OrderByDescending(e => e.price);
         }

         var Count = Products.Count();

         var pageCount = (Count % pageSize == 0) ? (Count / pageSize) : (Count / pageSize + 1);

         ViewData["pager"] = ToolsController.PagerDesign(pageIndex, pageCount, "/mall/prolist/index?kw=" + kw + "&sort=" + sort + "&pageIndex=");

         ViewData["miniPager"] = ToolsController.MiniPagerDesign(pageIndex, pageCount, "/mall/prolist/index?kw=" + kw + "&sort=" + sort + "&pageIndex=");

         ViewBag.kw = kw;
         return View(Products.ToList().Skip(pageSize * (pageIndex - 1)).Take(pageSize));
      }

      public ActionResult Store(int? id, string sort = "default", int pageIndex = 1, int pageSize = 8)
      {
         var Products = db.sp_product.Where(e => e.store==id).Include(s => s.sp_brand).Include(s => s.sp_category).Include(s => s.xt_store);

         if (sort == "sales")
         {
            Products = Products.OrderByDescending(e => e.sales);
         }
         else if (sort == "price")
         {
            Products = Products.OrderByDescending(e => e.price);
         }

         var Count = Products.Count();

         var pageCount = (Count % pageSize == 0) ? (Count / pageSize) : (Count / pageSize + 1);

         ViewData["pager"] = ToolsController.PagerDesign(pageIndex, pageCount, "/mall/prolist//store?id=" + id + "&sort=" + sort + "&pageIndex=");

         ViewData["miniPager"] = ToolsController.MiniPagerDesign(pageIndex, pageCount, "/mall/prolist/store?id=" + id + "&sort=" + sort + "&pageIndex=");

         ViewBag.id = id;
         return View(Products.ToList().Skip(pageSize * (pageIndex - 1)).Take(pageSize));
      }

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         sp_product sp_product = db.sp_product.Find(id);
         if (sp_product == null)
         {
            return HttpNotFound();
         }
         return View(sp_product);
      }

   }
}
