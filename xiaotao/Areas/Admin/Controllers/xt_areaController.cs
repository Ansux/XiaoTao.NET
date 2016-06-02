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
   public class xt_areaController : Controller
  {
    private WebContext db = new WebContext();

    // GET: Admin/xt_area
    public ActionResult Index()
    {
      return View(db.xt_area.ToList());
    }

    // GET: Admin/xt_area/Details/5
    public ActionResult Details(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      xt_area xt_area = db.xt_area.Find(id);
      if (xt_area == null)
      {
        return HttpNotFound();
      }
      return View(xt_area);
    }

    // GET: Admin/xt_area/Create
    public ActionResult Create()
    {
      return View();
    }

    // POST: Admin/xt_area/Create
    // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
    // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "id,name")] xt_area xt_area)
    {
      if (ModelState.IsValid)
      {
        db.xt_area.Add(xt_area);
        db.SaveChanges();
        return RedirectToAction("Index");
      }

      return View(xt_area);
    }

    // GET: Admin/xt_area/Edit/5
    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      xt_area xt_area = db.xt_area.Find(id);
      if (xt_area == null)
      {
        return HttpNotFound();
      }
      return View(xt_area);
    }

    // POST: Admin/xt_area/Edit/5
    // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
    // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "id,name")] xt_area xt_area)
    {
      if (ModelState.IsValid)
      {
        db.Entry(xt_area).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
      }
      return View(xt_area);
    }

    // GET: Admin/xt_area/Delete/5
    public ActionResult Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      xt_area xt_area = db.xt_area.Find(id);
      if (xt_area == null)
      {
        return HttpNotFound();
      }
      return View(xt_area);
    }

    // POST: Admin/xt_area/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      xt_area xt_area = db.xt_area.Find(id);
      db.xt_area.Remove(xt_area);
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
