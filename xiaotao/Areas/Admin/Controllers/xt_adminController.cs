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
  public class xt_adminController : Controller
  {
    private WebContext db = new WebContext();

    // GET: Admin/xt_admin
    public ActionResult Index()
    {
      var xt_admin = db.xt_admin.Include(x => x.xt_role);
      return View(xt_admin.ToList());
    }

    // GET: Admin/xt_admin/Details/5
    public ActionResult Details(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      xt_admin xt_admin = db.xt_admin.Find(id);
      if (xt_admin == null)
      {
        return HttpNotFound();
      }
      return View(xt_admin);
    }

    // GET: Admin/xt_admin/Create
    public ActionResult Create()
    {
      ViewBag.role = new SelectList(db.xt_role, "id", "name");
      return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "id,login_id,login_pwd,avatar,real_name,sex,email,phone,role")] xt_admin xt_admin)
    {
      if (ModelState.IsValid)
      {
        xt_admin.create_at = DateTime.Now;
        db.xt_admin.Add(xt_admin);
        db.SaveChanges();
        return RedirectToAction("Index");
      }

      ViewBag.role = new SelectList(db.xt_role, "id", "name", xt_admin.role);
      return View(xt_admin);
    }

    // GET: Admin/xt_admin/Edit/5
    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      xt_admin xt_admin = db.xt_admin.Find(id);
      if (xt_admin == null)
      {
        return HttpNotFound();
      }
      ViewBag.role = new SelectList(db.xt_role, "id", "name", xt_admin.role);
      return View(xt_admin);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "id,login_id,login_pwd,avatar,real_name,sex,email,phone,role")] xt_admin xt_admin)
    {
      if (ModelState.IsValid)
      {
        db.Entry(xt_admin).State = EntityState.Modified;
        xt_admin.update_at = DateTime.Now;
        db.SaveChanges();
        return RedirectToAction("Index");
      }
      ViewBag.role = new SelectList(db.xt_role, "id", "name", xt_admin.role);
      return View(xt_admin);
    }

    // GET: Admin/xt_admin/Delete/5
    public ActionResult Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      xt_admin xt_admin = db.xt_admin.Find(id);
      if (xt_admin == null)
      {
        return HttpNotFound();
      }
      return View(xt_admin);
    }

    // POST: Admin/xt_admin/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      xt_admin xt_admin = db.xt_admin.Find(id);
      db.xt_admin.Remove(xt_admin);
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
