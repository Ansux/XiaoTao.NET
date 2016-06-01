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
   public class xt_student_logController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index()
      {
         var xt_student_log = db.xt_student_log.Include(x => x.xt_student);
         return View(xt_student_log.ToList());
      }

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         xt_student_log xt_student_log = db.xt_student_log.Find(id);
         if (xt_student_log == null)
         {
            return HttpNotFound();
         }
         return View(xt_student_log);
      }

      public ActionResult Create()
      {
         ViewBag.student = new SelectList(db.xt_student, "id", "email");
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "id,kind,create_at,student")] xt_student_log xt_student_log)
      {
         if (ModelState.IsValid)
         {
            db.xt_student_log.Add(xt_student_log);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         ViewBag.student = new SelectList(db.xt_student, "id", "email", xt_student_log.student);
         return View(xt_student_log);
      }

      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         xt_student_log xt_student_log = db.xt_student_log.Find(id);
         if (xt_student_log == null)
         {
            return HttpNotFound();
         }
         ViewBag.student = new SelectList(db.xt_student, "id", "email", xt_student_log.student);
         return View(xt_student_log);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "id,kind,create_at,student")] xt_student_log xt_student_log)
      {
         if (ModelState.IsValid)
         {
            db.Entry(xt_student_log).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         ViewBag.student = new SelectList(db.xt_student, "id", "email", xt_student_log.student);
         return View(xt_student_log);
      }

      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         xt_student_log xt_student_log = db.xt_student_log.Find(id);
         if (xt_student_log == null)
         {
            return HttpNotFound();
         }
         return View(xt_student_log);
      }

      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         xt_student_log xt_student_log = db.xt_student_log.Find(id);
         db.xt_student_log.Remove(xt_student_log);
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
