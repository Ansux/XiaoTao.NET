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
  public class sp_categoryController : Controller
  {
    private WebContext db = new WebContext();

    public ActionResult Index()
    {
      return View(db.sp_category.ToList());
    }

    public ActionResult Details(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      sp_category sp_category = db.sp_category.Find(id);
      if (sp_category == null)
      {
        return HttpNotFound();
      }
      return View(sp_category);
    }

    public ActionResult Create()
    {
      ViewBag.pid = new SelectList(db.sp_category.Where(c => c.pid == 1 || c.pid == 0), "id", "name");
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "id,name,intro,pid")] sp_category sp_category)
    {
      if (ModelState.IsValid)
      {
        sp_category.create_at = DateTime.Now;
        sp_category.verify = true;
        db.sp_category.Add(sp_category);
        db.SaveChanges();
        return RedirectToAction("Index");
      }

      return View(sp_category);
    }

    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      sp_category sp_category = db.sp_category.Find(id);
      if (sp_category == null)
      {
        return HttpNotFound();
      }
      return View(sp_category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "id,name,intro,pid,verify")] sp_category sp_category)
    {
      if (ModelState.IsValid)
      {
        db.Entry(sp_category).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
      }
      return View(sp_category);
    }

    public ActionResult Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      sp_category sp_category = db.sp_category.Find(id);
      if (sp_category == null)
      {
        return HttpNotFound();
      }
      return View(sp_category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      sp_category sp_category = db.sp_category.Find(id);
      db.sp_category.Remove(sp_category);
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
