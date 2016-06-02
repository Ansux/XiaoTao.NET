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
   public class AccountController : Controller
   {
      private WebContext db = new WebContext();

      [Filters.AdminAuthorize]
      public ActionResult Index()
      {
         var xt_admin = db.xt_admin.Include(x => x.xt_role);
         return View(xt_admin.ToList());
      }

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

      public ActionResult Signin()
      {
         if (Session["AdminId"] != null)
         {
            return RedirectToAction("Index");
         }
         return View();
      }

      [HttpPost]
      public ActionResult Signin(string login_id, string login_pwd)
      {
         var admin = db.xt_admin.SingleOrDefault(e => e.login_id == login_id && e.login_pwd == login_pwd);
         if (admin != null)
         {
            Session["AdminId"] = admin.id;
            Session["AdminName"] = admin.real_name;
            return RedirectToAction("Index");
         }
         else
         {
            return View();
         }
      }

      [Filters.AdminAuthorize]
      public ActionResult Setting()
      {
         int aid = int.Parse(Session["AdminId"].ToString());
         var admin = db.xt_admin.Find(aid);
         
         return View(admin);
      }

      [Filters.AdminAuthorize]
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Setting([Bind(Include = "id,login_id,login_pwd,avatar,real_name,sex,email,phone")] xt_admin xt_admin)
      {
         if (ModelState.IsValid)
         {
            db.Entry(xt_admin).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         ViewBag.role = new SelectList(db.xt_role, "id", "name", xt_admin.role);
         return View(xt_admin);
      }
      
      public ActionResult Signout()
      {
         Session.Remove("AdminId");
         Session.Remove("AdminName");
         return RedirectToAction("Signin");
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
