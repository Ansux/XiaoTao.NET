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
   public class xt_studentController : Controller
  {
    private WebContext db = new WebContext();

    // GET: Admin/xt_student
    public ActionResult Index()
    {
      return View(db.xt_student.ToList());
    }

    // GET: Admin/xt_student/Details/5
    public ActionResult Details(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      xt_student xt_student = db.xt_student.Find(id);
      if (xt_student == null)
      {
        return HttpNotFound();
      }
      return View(xt_student);
    }

    // GET: Admin/xt_student/Create
    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "id,email,pwd")] xt_student xt_student)
    {
      if (ModelState.IsValid)
      {
        xt_student.create_at = DateTime.Now;
        xt_student.verify = false;
        xt_student.states = true;
        db.xt_student.Add(xt_student);
        db.SaveChanges();
        return RedirectToAction("Index");
      }

      return View(xt_student);
    }

    // GET: Admin/xt_student/Edit/5
    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      xt_student xt_student = db.xt_student.Find(id);
      if (xt_student == null)
      {
        return HttpNotFound();
      }
      return View(xt_student);
    }

    // POST: Admin/xt_student/Edit/5
    // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
    // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "id,email,pwd,avatar,sno,sname,voucher,sex,phone,create_at,update_at,verify,states")] xt_student xt_student)
    {
      if (ModelState.IsValid)
      {
        db.Entry(xt_student).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
      }
      return View(xt_student);
    }

    // GET: Admin/xt_student/Delete/5
    public ActionResult Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      xt_student xt_student = db.xt_student.Find(id);
      if (xt_student == null)
      {
        return HttpNotFound();
      }
      return View(xt_student);
    }

    // POST: Admin/xt_student/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      xt_student xt_student = db.xt_student.Find(id);
      db.xt_student.Remove(xt_student);
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
