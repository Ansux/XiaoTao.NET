using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using xiaotao.Models;

namespace xiaotao.Areas.Admin.Controllers
{
   [Filters.AdminAuthorize]
   public class xt_store_logController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index(int pageIndex = 1, int pageSize = 12)
      {
         var logs = db.xt_store_log.Include(x => x.xt_store);

         var Count = logs.Count();

         var pageCount = (Count % pageSize == 0) ? (Count / pageSize) : (Count / pageSize + 1);

         ViewData["pager"] = Filters.Tools.PagerDesign(pageIndex, pageCount, "/admin/xt_store_log/index?pageIndex=");

         return View(logs.ToList().Skip(pageSize * (pageIndex - 1)).Take(pageSize));
      }

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         xt_store_log xt_store_log = db.xt_store_log.Find(id);
         if (xt_store_log == null)
         {
            return HttpNotFound();
         }
         return View(xt_store_log);
      }

      public ActionResult Create()
      {
         ViewBag.store = new SelectList(db.xt_store, "id", "login_id");
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "id,kind,create_at,store")] xt_store_log xt_store_log)
      {
         if (ModelState.IsValid)
         {
            db.xt_store_log.Add(xt_store_log);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         ViewBag.store = new SelectList(db.xt_store, "id", "login_id", xt_store_log.store);
         return View(xt_store_log);
      }

      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         xt_store_log xt_store_log = db.xt_store_log.Find(id);
         if (xt_store_log == null)
         {
            return HttpNotFound();
         }
         ViewBag.store = new SelectList(db.xt_store, "id", "login_id", xt_store_log.store);
         return View(xt_store_log);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "id,kind,create_at,store")] xt_store_log xt_store_log)
      {
         if (ModelState.IsValid)
         {
            db.Entry(xt_store_log).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         ViewBag.store = new SelectList(db.xt_store, "id", "login_id", xt_store_log.store);
         return View(xt_store_log);
      }

      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         xt_store_log xt_store_log = db.xt_store_log.Find(id);
         if (xt_store_log == null)
         {
            return HttpNotFound();
         }
         return View(xt_store_log);
      }

      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         xt_store_log xt_store_log = db.xt_store_log.Find(id);
         db.xt_store_log.Remove(xt_store_log);
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
