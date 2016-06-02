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
   public class es_categoryController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index()
      {
         return View(db.es_category.ToList());
      }

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_category es_category = db.es_category.Find(id);
         if (es_category == null)
         {
            return HttpNotFound();
         }
         return View(es_category);
      }

      public ActionResult Create()
      {
         ViewBag.pid = new SelectList(db.es_category.Where(c => c.pid == 1 || c.pid == 0), "id", "name");
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "id,name,intro,pid")] es_category es_category)
      {
         if (ModelState.IsValid)
         {
            es_category.create_at = DateTime.Now;
            es_category.verify = true;
            db.es_category.Add(es_category);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         return View(es_category);
      }

      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_category es_category = db.es_category.Find(id);
         if (es_category == null)
         {
            return HttpNotFound();
         }
         return View(es_category);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "id,name,intro,create_at,pid,verify")] es_category es_category)
      {
         if (ModelState.IsValid)
         {
            db.Entry(es_category).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         return View(es_category);
      }

      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_category es_category = db.es_category.Find(id);
         if (es_category == null)
         {
            return HttpNotFound();
         }
         return View(es_category);
      }

      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         es_category es_category = db.es_category.Find(id);
         db.es_category.Remove(es_category);
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