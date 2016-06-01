using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using xiaotao.Models;

namespace xiaotao.Areas.Dn.Controllers
{
   [Filters.StuLoginAuthorize]
   public class ClaimController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index(int? states, int pageIndex = 1, int pageSize = 8)
      {
         var sid = int.Parse(Session["Sid"].ToString());
         var orders = db.dn_claim.Where(o => o.student == sid).ToList();

         ViewBag.state1 = orders.Where(e => e.states == 1).Count();
         ViewBag.state2 = orders.Where(e => e.states == 2).Count();

         if (states != null)
         {
            orders = orders.Where(e => e.states == states).ToList();
         }

         var Count = orders.Count();

         var pageCount = (Count % pageSize == 0) ? (Count / pageSize) : (Count / pageSize + 1);

         var queryString = "";
         if (states != null)
         {
            queryString = "states=" + states + "&";
         }

         ViewData["pager"] = Filters.Tools.PagerDesign(pageIndex, pageCount, "/dn/claim/index?" + queryString + "pageIndex=");

         return View(orders.Skip(pageSize * (pageIndex - 1)).Take(pageSize));
      }

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         dn_claim dn_claim = db.dn_claim.Find(id);
         if (dn_claim == null)
         {
            return HttpNotFound();
         }
         return View(dn_claim);
      }

      public ActionResult Checkout(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         
         int sid = int.Parse(Session["Sid"].ToString());
         var goods = db.dn_goods.Find(id);

         if (goods == null)
         {
            return HttpNotFound();
         }
         ViewData["Address"] = db.xt_student_address.Where(e => e.student == sid).ToList();

         return View(goods);
      }

      public ActionResult Create()
      {
         ViewBag.goods = new SelectList(db.dn_goods, "id", "name");
         ViewBag.student = new SelectList(db.xt_student, "id", "email");
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create(int gid)
      {
         var goods = db.dn_goods.Find(gid);
         if(goods == null)
         {
            return HttpNotFound();
         }
         var claim = new dn_claim();
         claim.states = 1;
         claim.create_at = DateTime.Now;
         claim.goods = gid;
         claim.student = int.Parse(Session["Sid"].ToString());

         // 更新物品上架状态
         db.Entry(goods).State = EntityState.Unchanged;
         db.Entry(goods).Property(e => e.is_onsale).IsModified = true;
         goods.is_onsale = false;

         db.dn_claim.Add(claim);
         db.SaveChanges();
         
         return RedirectToAction("Index");
      }

      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         dn_claim dn_claim = db.dn_claim.Find(id);
         if (dn_claim == null)
         {
            return HttpNotFound();
         }
         ViewBag.goods = new SelectList(db.dn_goods, "id", "name", dn_claim.goods);
         ViewBag.student = new SelectList(db.xt_student, "id", "email", dn_claim.student);
         return View(dn_claim);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "id,states,create_at,goods,student")] dn_claim dn_claim)
      {
         if (ModelState.IsValid)
         {
            db.Entry(dn_claim).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         ViewBag.goods = new SelectList(db.dn_goods, "id", "name", dn_claim.goods);
         ViewBag.student = new SelectList(db.xt_student, "id", "email", dn_claim.student);
         return View(dn_claim);
      }

      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         dn_claim dn_claim = db.dn_claim.Find(id);
         if (dn_claim == null)
         {
            return HttpNotFound();
         }
         return View(dn_claim);
      }

      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         dn_claim dn_claim = db.dn_claim.Find(id);
         db.dn_claim.Remove(dn_claim);
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
