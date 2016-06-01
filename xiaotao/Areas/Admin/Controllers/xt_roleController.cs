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
  public class xt_roleController : Controller
  {
    private WebContext db = new WebContext();

    // GET: Admin/xt_role
    public ActionResult Index()
    {
      return View(db.xt_role.ToList());
    }

    // GET: Admin/xt_role/Details/5
    public ActionResult Details(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      xt_role xt_role = db.xt_role.Find(id);
      if (xt_role == null)
      {
        return HttpNotFound();
      }
      return View(xt_role);
    }

    // GET: Admin/xt_role/Create
    public ActionResult Create()
    {
      return View();
    }

    // POST: Admin/xt_role/Create
    // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
    // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "id,name")] xt_role xt_role)
    {
      if (ModelState.IsValid)
      {
        db.xt_role.Add(xt_role);
        db.SaveChanges();
        return RedirectToAction("Index");
      }

      return View(xt_role);
    }

    // GET: Admin/xt_role/Edit/5
    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      xt_role xt_role = db.xt_role.Find(id);
      if (xt_role == null)
      {
        return HttpNotFound();
      }
      return View(xt_role);
    }

    // POST: Admin/xt_role/Edit/5
    // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
    // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "id,name")] xt_role xt_role)
    {
      if (ModelState.IsValid)
      {
        db.Entry(xt_role).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
      }
      return View(xt_role);
    }

    // GET: Admin/xt_role/Delete/5
    public ActionResult Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      xt_role xt_role = db.xt_role.Find(id);
      if (xt_role == null)
      {
        return HttpNotFound();
      }
      return View(xt_role);
    }

    // POST: Admin/xt_role/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      xt_role xt_role = db.xt_role.Find(id);
      db.xt_role.Remove(xt_role);
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
