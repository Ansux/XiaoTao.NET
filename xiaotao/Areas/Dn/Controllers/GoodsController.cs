using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using xiaotao.Models;

namespace xiaotao.Areas.Dn.Controllers
{
   [Filters.StuLoginAuthorize]
   public class GoodsController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index()
      {
         var sid = int.Parse(Session["Sid"].ToString());
         var dn_goods = db.dn_goods.Where(e=>e.donor == sid).Include(d => d.dn_category).Include(d => d.xt_student);
         return View(dn_goods.ToList());
      }

      public ActionResult Claim()
      {
         var sid = int.Parse(Session["Sid"].ToString());
         var claims = db.dn_claim.Where(e => e.dn_goods.donor == sid);
         return View(claims.ToList());
      }

      [AllowAnonymous]
      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         dn_goods dn_goods = db.dn_goods.Find(id);
         if (dn_goods == null)
         {
            return HttpNotFound();
         }

         var claim = db.dn_claim.SingleOrDefault(e=>e.goods==id && e.states>=1);
         if (claim != null)
         {
            ViewBag.isClaim = true;
         }

         return View(dn_goods);
      }

      public ActionResult Create()
      {
         ViewBag.category = new SelectList(db.dn_category, "id", "name");
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "id,name,ori_img,is_anonymous,claim_type,claim_addr,category")] dn_goods goods)
      {
         if (ModelState.IsValid)
         {
            if (Request.Files.Count > 0)
            {
               var file = Request.Files[0];
               if (file != null && file.ContentLength > 0)
               {
                  var fileName = Path.GetFileName(file.FileName);
                  var path = Path.Combine(Server.MapPath("~/Uploads/Products/"), fileName);
                  file.SaveAs(path);
                  goods.ori_img = fileName;
               }
            }

            goods.claim_addr = (goods.claim_type == 2) ? goods.claim_addr : null;

            goods.is_onsale = true;
            goods.is_delete = false;
            goods.create_at = DateTime.Now;
            goods.donor = int.Parse(Session["Sid"].ToString());

            db.dn_goods.Add(goods);
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         
         return View(goods);
      }

      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         dn_goods dn_goods = db.dn_goods.Find(id);
         if (dn_goods == null)
         {
            return HttpNotFound();
         }
         ViewBag.category = new SelectList(db.dn_category, "id", "name", dn_goods.category);
         ViewBag.donor = new SelectList(db.xt_student, "id", "email", dn_goods.donor);
         return View(dn_goods);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "id,name,ori_img,thumb_img,is_anonymous,is_onsale,is_delete,create_at,update_at,category,donor")] dn_goods dn_goods)
      {
         if (ModelState.IsValid)
         {
            db.Entry(dn_goods).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         ViewBag.category = new SelectList(db.dn_category, "id", "name", dn_goods.category);
         ViewBag.donor = new SelectList(db.xt_student, "id", "email", dn_goods.donor);
         return View(dn_goods);
      }

      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         dn_goods dn_goods = db.dn_goods.Find(id);
         if (dn_goods == null)
         {
            return HttpNotFound();
         }
         return View(dn_goods);
      }

      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         dn_goods dn_goods = db.dn_goods.Find(id);
         db.dn_goods.Remove(dn_goods);
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
