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
  public class xt_student_addressController : Controller
  {
    private WebContext db = new WebContext();

    // GET: Admin/xt_student_address
    public ActionResult Index()
    {
      var xt_student_address = db.xt_student_address.Include(x => x.xt_area).Include(x => x.xt_student);
      return View(xt_student_address.ToList());
    }

    // GET: Admin/xt_student_address/Details/5
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

    // GET: Admin/xt_student_address/Create
    public ActionResult Create()
    {
      ViewBag.area = new SelectList(db.xt_area, "id", "name");
      ViewBag.student = new SelectList(db.xt_student, "id", "email");
      return View();
    }

    // POST: Admin/xt_student_address/Create
    // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
    // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "id,area,addr,receiver,phone,remark,is_default,student")] xt_student_address xt_student_address)
    {
      if (ModelState.IsValid)
      {
        db.xt_student_address.Add(xt_student_address);
        db.SaveChanges();
        return RedirectToAction("Index");
      }

      ViewBag.area = new SelectList(db.xt_area, "id", "name", xt_student_address.area);
      ViewBag.student = new SelectList(db.xt_student, "id", "email", xt_student_address.student);
      return View(xt_student_address);
    }

    // GET: Admin/xt_student_address/Edit/5
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
      ViewBag.student = new SelectList(db.xt_student, "id", "email", xt_student_address.student);
      return View(xt_student_address);
    }

    // POST: Admin/xt_student_address/Edit/5
    // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
    // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
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
      ViewBag.student = new SelectList(db.xt_student, "id", "email", xt_student_address.student);
      return View(xt_student_address);
    }

    // GET: Admin/xt_student_address/Delete/5
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

    // POST: Admin/xt_student_address/Delete/5
    [HttpPost]
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
