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
   public class xt_stu_certificationController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index()
      {
         var xt_stu_certification = db.xt_stu_certification.Include(x => x.xt_student);
         return View(xt_stu_certification.ToList());
      }

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         xt_stu_certification xt_stu_certification = db.xt_stu_certification.Find(id);
         if (xt_stu_certification == null)
         {
            return HttpNotFound();
         }
         return View(xt_stu_certification);
      }

      public ActionResult Create()
      {
         ViewBag.student = new SelectList(db.xt_student, "id", "email");
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "id,sno,sname,voucher,is_pass,result,create_at,valid_at,student")] xt_stu_certification xt_stu_certification)
      {
         if (ModelState.IsValid)
         {
            db.xt_stu_certification.Add(xt_stu_certification);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         ViewBag.student = new SelectList(db.xt_student, "id", "email", xt_stu_certification.student);
         return View(xt_stu_certification);
      }

      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         xt_stu_certification xt_stu_certification = db.xt_stu_certification.Find(id);
         if (xt_stu_certification == null)
         {
            return HttpNotFound();
         }
         ViewBag.student = new SelectList(db.xt_student, "id", "email", xt_stu_certification.student);
         return View(xt_stu_certification);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "id,sno,sname,voucher,is_pass,result,create_at,valid_at,student")] xt_stu_certification xt_stu_certification)
      {
         if (ModelState.IsValid)
         {
            db.Entry(xt_stu_certification).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         ViewBag.student = new SelectList(db.xt_student, "id", "email", xt_stu_certification.student);
         return View(xt_stu_certification);
      }

      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         xt_stu_certification xt_stu_certification = db.xt_stu_certification.Find(id);
         if (xt_stu_certification == null)
         {
            return HttpNotFound();
         }
         return View(xt_stu_certification);
      }

      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         xt_stu_certification xt_stu_certification = db.xt_stu_certification.Find(id);
         db.xt_stu_certification.Remove(xt_stu_certification);
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

      public ActionResult Valid(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         var certification = db.xt_stu_certification.Find(id);
         if(certification != null)
         {
            var stu_db = db.xt_stuinfo_db.SingleOrDefault(e => e.sno == certification.sno && e.sname == certification.sname);
            if (stu_db != null)
            {
               ViewBag.exist = true;
               certification.is_pass = true;
            }
         }
         
         return View(certification);
      }

      [HttpPost]
      public ActionResult Valid(int id,bool is_pass,string result)
      {
         var certification = db.xt_stu_certification.Find(id);
         db.Entry(certification).State = EntityState.Unchanged;
         db.Entry(certification).Property(e => e.is_pass).IsModified = true;
         db.Entry(certification).Property(e => e.result).IsModified = true;
         db.Entry(certification).Property(e => e.valid_at).IsModified = true;
         certification.is_pass = is_pass;
         certification.result = result;
         certification.valid_at = DateTime.Now;

         // 更新学生表实名数据
         if (certification.is_pass == true)
         {
            var stu = db.xt_student.Find(certification.student);
            db.Entry(stu).State = EntityState.Unchanged;
            db.Entry(stu).Property(e => e.sno).IsModified = true;
            db.Entry(stu).Property(e => e.sname).IsModified = true;
            db.Entry(stu).Property(e => e.verify).IsModified = true;
            stu.sno = certification.sno;
            stu.sname = certification.sname;
            stu.verify = true;
         }

         db.SaveChanges();

         return RedirectToAction("Index");
      }
   }
}
