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

namespace xiaotao.Areas.Mall.Controllers
{
   [Filters.StoreLoginAuthorize]
   public class ProductController : Controller
   {
      private WebContext db = new WebContext();

      [AllowAnonymous]
      public ActionResult Index()
      {
         var sp_product = db.sp_product.Include(s => s.sp_brand).Include(s => s.sp_category).Include(s => s.xt_store);
         return View(sp_product.ToList());
      }

      [AllowAnonymous]
      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         sp_product sp_product = db.sp_product.Find(id);
         if (sp_product == null)
         {
            return HttpNotFound();
         }

         ViewBag.CommentCount = db.sp_order_item.Where(i => i.product == id && i.is_comment == true).Count();

         return View(sp_product);
      }

      #region 获取商品评论
      [HttpPost]
      [AllowAnonymous]
      public JsonResult GetRank(int id)
      {
         var ranks = db.sp_order_item.Where(oi=>oi.product == id && oi.is_comment == true);
         var rank_1 = ranks.Where(r => r.rank == 1).Count();
         var rank_2 = ranks.Where(r => r.rank == 2).Count();
         var rank_3 = ranks.Where(r => r.rank == 3).Count();
         var json = new
         {
            all = ranks.Count(),
            rank_1 = rank_1,
            rank_2 = rank_2,
            rank_3 = rank_3
         };
         return Json(json);
      }


      [HttpPost]
      [AllowAnonymous]
      public JsonResult GetCommentRecord(int id, int rank)
      {
         List<sp_order_item> record = new List<sp_order_item>();
         if(rank == 0)
         {
            record = db.sp_order_item.Where(i => i.product == id && i.is_comment == true).ToList();
         }else if (rank == 1 || rank == 2 || rank ==3)
         {
            record = db.sp_order_item.Where(i => i.product == id && i.rank == rank).ToList();
         }

         if (record.Count() == 0)
         {
            return Json(false);
         }
         else
         {
            var json = new
            {
               flag = true,
               comments = (from r in record
                           select new
                           {
                              id = r.id,
                              buyer = r.sp_order.xt_student.sname,
                              rank = r.rank,
                              comment = r.comment,
                              reply = r.reply
                           }).ToArray()
            };
            return Json(json);
         }
      } 
      #endregion

      #region 发布商品
      public ActionResult Create()
      {
         ViewBag.brand = new SelectList(db.sp_brand, "id", "name");
         // ViewBag.category = new SelectList(db.sp_category, "id", "name");

         ViewData["category"] = GetCategory(1);
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      [ValidateInput(false)]
      public ActionResult Create([Bind(Include = "id,name,price,stock,ori_img,is_onsale,category,brand")] sp_product sp_product)
      {
         if (ModelState.IsValid)
         {
            // 获取店铺Id
            int storeId = (int)Session["StoreId"];

            if (Request.Files.Count > 0)
            {
               var file = Request.Files[0];
               if (file != null && file.ContentLength > 0)
               {
                  var fileName = Filters.Tools.GetRandomFileName(file);

                  var path = Path.Combine(Server.MapPath("~/Uploads/Products/"), fileName);
                  file.SaveAs(path);
                  sp_product.ori_img = fileName;
               }
            }

            sp_product.detail = Request.Form["editorValue"].ToString();
            sp_product.create_at = DateTime.Now;
            sp_product.is_delete = false;
            sp_product.sales = 0;
            sp_product.store = storeId;
            db.sp_product.Add(sp_product);
            db.SaveChanges();
            return Redirect("/mall/store/ProList");
         }

         ViewBag.brand = new SelectList(db.sp_brand, "id", "name", sp_product.brand);
         ViewBag.category = new SelectList(db.sp_category, "id", "name", sp_product.category);
         ViewBag.store = new SelectList(db.xt_store, "id", "login_id", sp_product.store);
         return View(sp_product);
      }

      #endregion
            
      #region 编辑商品信息
      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         var sid = int.Parse(Session["StoreId"].ToString());
         sp_product sp_product = db.sp_product.Where(p => p.id == id && p.store == sid).FirstOrDefault();
         if (sp_product == null)
         {
            return HttpNotFound();
         }
         ViewBag.brand = new SelectList(db.sp_brand, "id", "name", sp_product.brand);
         // ViewBag.category = new SelectList(db.sp_category, "id", "name", sp_product.category);
         ViewData["category"] = GetCategory(sp_product.category);

         return View(sp_product);
      }

      [ValidateInput(false)]
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "id,name,price,stock,ori_img,is_onsale,category,brand")] sp_product sp_product)
      {
         try {
            // 获取店铺Id
            db.Entry(sp_product).State = EntityState.Modified;

            if (Request.Files.Count > 0)
            {
               var file = Request.Files[0];
               if (file != null && file.ContentLength > 0)
               {
                  var fileName = Filters.Tools.GetRandomFileName(file);

                  var path = Path.Combine(Server.MapPath("~/Uploads/Products/"), fileName);
                  file.SaveAs(path);
                  sp_product.ori_img = fileName;
               }
            }
            else
            {
               db.Entry(sp_product).Property(e => e.ori_img).IsModified = false;
            }

            db.Entry(sp_product).Property(e => e.sales).IsModified = false;
            db.Entry(sp_product).Property(e => e.is_delete).IsModified = false;
            db.Entry(sp_product).Property(e => e.create_at).IsModified = false;
            db.Entry(sp_product).Property(e => e.store).IsModified = false;

            sp_product.detail = Request.Form["editorValue"].ToString();
            db.SaveChanges();
            return Redirect("/mall/store/ProList");
         }
         catch
         {
            ViewBag.brand = new SelectList(db.sp_brand, "id", "name", sp_product.brand);
            ViewData["category"] = GetCategory(sp_product.category);
            return View(GetProduct(sp_product.id));
         }
      }
      #endregion

      public string GetCategory(int? cid)
      {
         var cates = db.sp_category.ToList();
         var category = "<option value='1'>顶级分类</option>";
         foreach (var c in cates.Where(c => c.pid == 1))
         {
            category += "<option value='" + c.id + "' " + (c.id == cid ? "selected" : "") + ">" + c.name + "</option>";
            foreach (var sb in cates.Where(s => s.pid == c.id))
            {
               category += "<option value='" + sb.id + "' " + (sb.id == cid ? "selected" : "") + ">-- " + sb.name + "</option>";
            }
         }
         return category;
      }

      public sp_product GetProduct(int id)
      {
         int sid = int.Parse(Session["StoreId"].ToString());
         return db.sp_product.SingleOrDefault(e=>e.store == sid && e.id == id);
      }

      #region 删除商品
      [HttpPost]
      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         sp_product sp_product = db.sp_product.Find(id);
         if (sp_product == null)
         {
            return HttpNotFound();
         }

         db.Entry(sp_product).State = EntityState.Unchanged;
         db.Entry(sp_product).Property(e => e.is_delete).IsModified = true;
         db.Entry(sp_product).Property(e => e.is_onsale).IsModified = true;
         sp_product.is_onsale = false;
         sp_product.is_delete = true;
         db.SaveChanges();

         return Json(true);
      }
      #endregion

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
