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

namespace xiaotao.Areas.Admin.Controllers
{
   [Filters.AdminAuthorize]
   public class es_goodsController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index(string sort = "default", int pageIndex = 1, int pageSize = 8)
      {
         var goods = db.es_goods.Include(e => e.es_category).Include(e => e.xt_student);

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

         ViewData["pager"] = ToolsController.PagerDesign(pageIndex, pageCount, "/admin/es_goods/index?sort=" + sort + "&pageIndex=");

         return View(goods.ToList().Skip(pageSize * (pageIndex - 1)).Take(pageSize));
      }

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_goods es_goods = db.es_goods.Find(id);
         if (es_goods == null)
         {
            return HttpNotFound();
         }
         return View(es_goods);
      }

      public ActionResult Create()
      {
         ViewBag.category = new SelectList(db.es_category, "id", "name");
         ViewBag.seller = new SelectList(db.xt_student, "id", "email");
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "id,name,price,ori_img,thumb_img,is_new,is_onsale,is_delete,create_at,update_at,category,seller")] es_goods es_goods)
      {
         if (ModelState.IsValid)
         {
            db.es_goods.Add(es_goods);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         ViewBag.category = new SelectList(db.es_category, "id", "name", es_goods.category);
         ViewBag.seller = new SelectList(db.xt_student, "id", "email", es_goods.seller);
         return View(es_goods);
      }

      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_goods es_goods = db.es_goods.Find(id);
         if (es_goods == null)
         {
            return HttpNotFound();
         }
         ViewBag.category = new SelectList(db.es_category, "id", "name", es_goods.category);
         ViewBag.seller = new SelectList(db.xt_student, "id", "email", es_goods.seller);
         return View(es_goods);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "id,name,price,ori_img,thumb_img,is_new,is_onsale,is_delete,create_at,update_at,category,seller")] es_goods es_goods)
      {
         if (ModelState.IsValid)
         {
            db.Entry(es_goods).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         ViewBag.category = new SelectList(db.es_category, "id", "name", es_goods.category);
         ViewBag.seller = new SelectList(db.xt_student, "id", "email", es_goods.seller);
         return View(es_goods);
      }

      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_goods es_goods = db.es_goods.Find(id);
         if (es_goods == null)
         {
            return HttpNotFound();
         }
         return View(es_goods);
      }

      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         es_goods es_goods = db.es_goods.Find(id);
         db.es_goods.Remove(es_goods);
         db.SaveChanges();
         return RedirectToAction("Index");
      }

      protected override void Dispose(bool disposing)
      {
         if (disposing)
         {
            db.Dispose();
         }
         base.Dispose(disposing);
      }
   }
}
