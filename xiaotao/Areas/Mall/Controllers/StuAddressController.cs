using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using xiaotao.Models;

namespace xiaotao.Areas.Mall.Controllers
{
   [Filters.StuLoginAuthorize]
   public class StuAddressController : Controller
   {
      private WebContext db = new WebContext();

      // GET: Mall/StuAddress
      public ActionResult Index()
      {
         var xt_student_address = db.xt_student_address.Include(x => x.xt_area).Include(x => x.xt_student);
         return View(xt_student_address.ToList());
      }

      // GET: Mall/StuAddress/Details/5
      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         xt_student_address xt_student_address = db.xt_student_address.Find(id);
         if (xt_student_address == null)
         {
            return HttpNotFound();
         }
         return View(xt_student_address);
      }

      // GET: Mall/StuAddress/Create
      public ActionResult Create()
      {
         ViewBag.area = new SelectList(db.xt_area, "id", "name");
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "id,area,addr,receiver,phone,remark,is_default")] xt_student_address address)
      {
         if (ModelState.IsValid)
         {
            var student = int.Parse(Session["Sid"].ToString());
            if (address.is_default == true)
            {
               // 将原有的默认地址取消掉。
               var addr = db.xt_student_address.Where(a => a.student == student).SingleOrDefault(a => a.is_default == true);
               if (addr != null)
               {
                  addr.is_default = false;
                  db.SaveChanges();
               }
            }

            address.student = student;
            db.xt_student_address.Add(address);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         ViewBag.area = new SelectList(db.xt_area, "id", "name", address.area);
         return View(address);
      }

      // GET: Mall/StuAddress/Edit/5
      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         xt_student_address xt_student_address = db.xt_student_address.Find(id);
         if (xt_student_address == null)
         {
            return HttpNotFound();
         }
         ViewBag.area = new SelectList(db.xt_area, "id", "name", xt_student_address.area);
         ViewBag.student = new SelectList(db.xt_student, "sno", "pwd", xt_student_address.student);
         return View(xt_student_address);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "id,area,addr,receiver,phone,remark,is_default,student")] xt_student_address xt_student_address)
      {
         if (ModelState.IsValid)
         {
            db.Entry(xt_student_address).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         ViewBag.area = new SelectList(db.xt_area, "id", "name", xt_student_address.area);
         ViewBag.student = new SelectList(db.xt_student, "sno", "pwd", xt_student_address.student);
         return View(xt_student_address);
      }

      // GET: Mall/StuAddress/Delete/5
      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         xt_student_address xt_student_address = db.xt_student_address.Find(id);
         if (xt_student_address == null)
         {
            return HttpNotFound();
         }
         return View(xt_student_address);
      }

      // POST: Mall/StuAddress/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         xt_student_address xt_student_address = db.xt_student_address.Find(id);
         db.xt_student_address.Remove(xt_student_address);
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
