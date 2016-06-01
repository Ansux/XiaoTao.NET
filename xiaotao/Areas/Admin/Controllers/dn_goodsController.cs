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
   public class dn_goodsController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index()
      {
         var dn_goods = db.dn_goods.Include(d => d.dn_category).Include(d => d.xt_student);
         return View(dn_goods.ToList());
      }

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         dn_goods dn_goods = db.dn_goods.Find(id);
         if (dn_goods == null)
         {
            return HttpNotFound();
         }
         return View(dn_goods);
      }

      public ActionResult Create()
      {
         ViewBag.category = new SelectList(db.dn_category, "id", "name");
         ViewBag.donor = new SelectList(db.xt_student, "id", "email");
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "id,name,ori_img,thumb_img,is_anonymous,is_onsale,is_delete,create_at,update_at,category,donor")] dn_goods dn_goods)
      {
         if (ModelState.IsValid)
         {
            db.dn_goods.Add(dn_goods);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         ViewBag.category = new SelectList(db.dn_category, "id", "name", dn_goods.category);
         ViewBag.donor = new SelectList(db.xt_student, "id", "email", dn_goods.donor);
         return View(dn_goods);
      }

      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         dn_goods dn_goods = db.dn_goods.Find(id);
         if (dn_goods == null)
         {
            return HttpNotFound();
         }
         ViewBag.category = new SelectList(db.dn_category, "id", "name", dn_goods.category);
         ViewBag.donor = new SelectList(db.xt_student, "id", "email", dn_goods.donor);
         return View(dn_goods);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "id,name,ori_img,thumb_img,is_anonymous,is_onsale,is_delete,create_at,update_at,category,donor")] dn_goods dn_goods)
      {
         if (ModelState.IsValid)
         {
            db.Entry(dn_goods).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         ViewBag.category = new SelectList(db.dn_category, "id", "name", dn_goods.category);
         ViewBag.donor = new SelectList(db.xt_student, "id", "email", dn_goods.donor);
         return View(dn_goods);
      }

      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         dn_goods dn_goods = db.dn_goods.Find(id);
         if (dn_goods == null)
         {
            return HttpNotFound();
         }
         return View(dn_goods);
      }

      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         dn_goods dn_goods = db.dn_goods.Find(id);
         db.dn_goods.Remove(dn_goods);
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
