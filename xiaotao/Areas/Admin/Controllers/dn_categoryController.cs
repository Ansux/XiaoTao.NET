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
   public class dn_categoryController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index()
      {
         return View(db.dn_category.ToList());
      }

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         dn_category dn_category = db.dn_category.Find(id);
         if (dn_category == null)
         {
            return HttpNotFound();
         }
         return View(dn_category);
      }

      public ActionResult Create()
      {
         ViewBag.pid = new SelectList(db.dn_category.Where(c => c.pid == 1 || c.pid == 0), "id", "name");
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "id,name,intro,pid")] dn_category category)
      {
         if (ModelState.IsValid)
         {
            category.verify = true;
            category.create_at = DateTime.Now;

            db.dn_category.Add(category);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         return View(category);
      }

      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         dn_category dn_category = db.dn_category.Find(id);
         if (dn_category == null)
         {
            return HttpNotFound();
         }
         return View(dn_category);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "id,name,intro,create_at,pid,verify")] dn_category dn_category)
      {
         if (ModelState.IsValid)
         {
            db.Entry(dn_category).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         return View(dn_category);
      }

      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         dn_category dn_category = db.dn_category.Find(id);
         if (dn_category == null)
         {
            return HttpNotFound();
         }
         return View(dn_category);
      }

      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         dn_category dn_category = db.dn_category.Find(id);
         db.dn_category.Remove(dn_category);
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
