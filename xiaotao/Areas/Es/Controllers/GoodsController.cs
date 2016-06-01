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

namespace xiaotao.Areas.Es.Controllers
{
   [Filters.StuLoginAuthorize]
   public class GoodsController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index(int pageIndex = 1, int pageSize = 8)
      {
         var stu = int.Parse(Session["Sid"].ToString());
         var goods = db.es_goods.Where(e => e.seller == stu).Include(e => e.es_category).Include(e => e.xt_student);

         var Count = goods.Count();

         var pageCount = (Count % pageSize == 0) ? (Count / pageSize) : (Count / pageSize + 1);

         ViewData["pager"] = Filters.Tools.PagerDesign(pageIndex, pageCount, "/es/goods/index?pageIndex=");

         return View(goods.ToList().Skip(pageSize * (pageIndex - 1)).Take(pageSize));
      }

      public ActionResult Trade(int? states, int pageIndex = 1, int pageSize = 8)
      {
         var sid = int.Parse(Session["Sid"].ToString());

         var orders = db.es_order.Where(e => e.es_goods.seller == sid).ToList();

         ViewBag.state1 = orders.Where(e => e.states == 1).Count();
         ViewBag.state2 = orders.Where(e => e.states == 2).Count();
         ViewBag.state3 = orders.Where(e => e.states == 3).Count();
         ViewBag.state4 = orders.Where(e => e.states == 4).Count();

         if (states != null)
         {
            orders = orders.Where(e=>e.states == states).ToList();
         }
         
         var Count = orders.Count();

         var pageCount = (Count % pageSize == 0) ? (Count / pageSize) : (Count / pageSize + 1);

         ViewData["pager"] = Filters.Tools.PagerDesign(pageIndex, pageCount, "/es/goods/Traded?pageIndex=");

         return View(orders.Skip(pageSize * (pageIndex - 1)).Take(pageSize));
      }

      [AllowAnonymous]
      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_goods es_goods = db.es_goods.Find(id);
         if (es_goods == null)
         {
            return HttpNotFound();
         }

         var order = db.es_order.SingleOrDefault(e => e.goods == id && e.states >= 1);
         if (order != null)
         {
            ViewBag.isSold = true;
         }

         ViewBag.consultCount = db.es_consult.Where(e => e.goods == id).Count();
         return View(es_goods);
      }

      public ActionResult Create()
      {
         ViewBag.category = new SelectList(db.es_category, "id", "name");
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "id,name,price,ori_img,is_new,is_onsale,category")] es_goods goods)
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

            goods.create_at = DateTime.Now;
            goods.seller = int.Parse(Session["Sid"].ToString());
            goods.is_delete = false;

            db.es_goods.Add(goods);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         ViewBag.category = new SelectList(db.es_category, "id", "name", goods.category);
         return View(goods);
      }

      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_goods es_goods = GetGoods(id);
         if (es_goods == null)
         {
            return HttpNotFound();
         }
         ViewBag.category = new SelectList(db.es_category, "id", "name", es_goods.category);
         return View(es_goods);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "id,name,price,ori_img,is_new,is_onsale,category")] es_goods es_goods)
      {
         if (ModelState.IsValid)
         {
            db.Entry(es_goods).State = EntityState.Modified;

            if (es_goods.ori_img != null)
            {
               var file = Request.Files[0];
               if (file != null && file.ContentLength > 0)
               {
                  var fileName = Path.GetFileName(file.FileName);
                  var path = Path.Combine(Server.MapPath("~/Uploads/Products/"), fileName);
                  file.SaveAs(path);
                  es_goods.ori_img = fileName;
               }
            }
            else
            {
               db.Entry(es_goods).Property(e => e.ori_img).IsModified = false;
            }

            es_goods.update_at = DateTime.Now;

            db.Entry(es_goods).Property(e => e.create_at).IsModified = false;
            db.Entry(es_goods).Property(e => e.seller).IsModified = false;
            db.Entry(es_goods).Property(e => e.is_delete).IsModified = false;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         ViewBag.category = new SelectList(db.es_category, "id", "name", es_goods.category);
         return View(es_goods);
      }

      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_goods es_goods = db.es_goods.Find(id);
         if (es_goods == null)
         {
            return HttpNotFound();
         }
         return View(es_goods);
      }

      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         es_goods es_goods = db.es_goods.Find(id);
         db.es_goods.Remove(es_goods);
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

      private es_goods GetGoods(int? id)
      {
         int sid = int.Parse(Session["Sid"].ToString());
         return db.es_goods.Where(e => e.seller == sid).FirstOrDefault();
      }

      [AllowAnonymous]
      [HttpPost]
      public JsonResult GetConsult(int? id)
      {         
         var consults = db.es_consult.Where(e=>e.goods == id).ToList();
         var json = (from c in consults.Where(e=>e.ori_id==null)
                     select new
                     {
                        id = c.id,
                        writer = c.writer,
                        writerName = c.xt_student1.sno,
                        content = c.content,
                        ori_writer = c.ori_writer,
                        create_at = c.create_at.ToString(),
                        sub = (from s in consults.Where(e=>e.ori_id == c.id) select new {
                           id = s.id,
                           writer = s.writer,
                           writerName = s.xt_student1.sno,
                           content = s.content,
                           ori_writer = s.xt_student2.sno,
                           create_at = s.create_at.ToString(),
                        }).ToArray()
                     }).ToArray();
         return Json(json);
      }

      [AllowAnonymous]
      [HttpPost]
      public ActionResult SubmitConsult(string content,int gid,int? oid)
      {
         if (Session["Sid"] == null)
         {
            return Json(false);
         }

         es_consult consult = new es_consult();
         if(oid != null)
         {
            // 若是回复内容，则添加 顶级咨询条目和当前咨询条目的作者
            var cobj = db.es_consult.Find(oid);
            consult.ori_id = cobj.ori_id == null ? cobj.id : cobj.ori_id;
            consult.ori_writer = cobj.ori_writer == null ? cobj.writer : cobj.ori_writer;
         }

         consult.goods = gid;
         consult.writer = int.Parse(Session["Sid"].ToString());
         consult.content = content;
         consult.create_at = DateTime.Now;
         consult.is_show = true;

         db.es_consult.Add(consult);
         db.SaveChanges();

         var temp = db.es_consult.Find(consult.id);
         var json = new
         {
            id = temp.id,
            writer = temp.writer,
            content = temp.content,
            ori_writer = temp.ori_writer,
            create_at = temp.create_at.ToString()
         };
         return Json(json);
      }

   }
}
