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
   public class xt_storeController : Controller
   {
      private WebContext db = new WebContext();

      // GET: Admin/xt_store
      public ActionResult Index()
      {
         return View(db.xt_store.ToList());
      }

      // 待核验的商铺
      public ActionResult Verify()
      {
         var stores = db.xt_store.Where(e => e.verify == false);
         return View(stores.ToList());
      }

      public ActionResult Check(int? id)
      {
         var store = db.xt_store.Find(id);
         return View(store);
      }

      [HttpPost]
      public ActionResult Check([Bind(Include = "id,verify")] xt_store xt_store)
      {
         if (ModelState.IsValid)
         {
            db.Entry(xt_store).State = EntityState.Modified;
            xt_store.update_at = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         return View(xt_store);
      }

      // GET: Admin/xt_store/Details/5
      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         xt_store xt_store = db.xt_store.Find(id);
         if (xt_store == null)
         {
            return HttpNotFound();
         }
         return View(xt_store);
      }

      // GET: Admin/xt_store/Create
      public ActionResult Create()
      {
         return View();
      }

      // POST: Admin/xt_store/Create
      // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
      // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "id,login_id,login_pwd,avatar,name,sex,email,phone,verify,states")] xt_store xt_store)
      {
         if (ModelState.IsValid)
         {
            xt_store.create_at = DateTime.Now;
            db.xt_store.Add(xt_store);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         return View(xt_store);
      }

      // GET: Admin/xt_store/Edit/5
      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         xt_store xt_store = db.xt_store.Find(id);
         if (xt_store == null)
         {
            return HttpNotFound();
         }
         return View(xt_store);
      }

      // POST: Admin/xt_store/Edit/5
      // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
      // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "id,login_id,login_pwd,avatar,name,sex,email,phone,verify,states")] xt_store xt_store)
      {
         if (ModelState.IsValid)
         {
            db.Entry(xt_store).State = EntityState.Modified;
            xt_store.update_at = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         return View(xt_store);
      }

      // GET: Admin/xt_store/Delete/5
      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         xt_store xt_store = db.xt_store.Find(id);
         if (xt_store == null)
         {
            return HttpNotFound();
         }
         return View(xt_store);
      }

      // POST: Admin/xt_store/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         xt_store xt_store = db.xt_store.Find(id);
         db.xt_store.Remove(xt_store);
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
