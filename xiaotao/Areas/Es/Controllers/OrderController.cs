using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using xiaotao.Models;

namespace xiaotao.Areas.Es.Controllers
{
   [Filters.StuLoginAuthorize]
   public class OrderController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index(int? states, int pageIndex = 1, int pageSize = 8)
      {
         var sid = int.Parse(Session["Sid"].ToString());
         var orders = db.es_order.Where(e => e.buyer == sid).Include(e => e.es_goods).Include(e => e.xt_student).ToList();

         ViewBag.state1 = orders.Where(e => e.states == 1).Count();
         ViewBag.state2 = orders.Where(e => e.states == 2).Count();
         ViewBag.state3 = orders.Where(e => e.states == 3).Count();

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

         ViewData["pager"] = Filters.Tools.PagerDesign(pageIndex, pageCount, "/es/order/index?" + queryString + "pageIndex=");

         return View(orders.Skip(pageSize * (pageIndex - 1)).Take(pageSize));
      }

      public ActionResult Checkout(int id)
      {
         int sid = int.Parse(Session["Sid"].ToString());
         var goods = db.es_goods.Find(id);
         ViewData["Address"] = db.xt_student_address.Where(e => e.student == sid).ToList();

         return View(goods);
      }

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_order es_order = db.es_order.Find(id);
         if (es_order == null)
         {
            return HttpNotFound();
         }
         return View(es_order);
      }

      public ActionResult Create()
      {
         ViewBag.goods = new SelectList(db.es_goods, "id", "name");
         ViewBag.buyer = new SelectList(db.xt_student, "id", "email");
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create(int addr, int gid)
      {
         var goods = db.es_goods.Find(gid);
         if (goods == null)
         {
            return HttpNotFound();
         }

         var order = new es_order();
         order.goods = gid;
         order.amount = goods.price;
         order.states = 1;
         order.create_at = DateTime.Now;
         order.is_pay = false;
         order.buyer = int.Parse(Session["Sid"].ToString());

         var address = db.xt_student_address.Find(addr);
         order.receiver = address.receiver;
         order.addr = address.xt_area.name + "区" + address.addr;
         order.phone = address.phone;

         db.es_order.Add(order);

         // 将二手物品状态改为下架
         db.Entry(goods).State = EntityState.Unchanged;
         db.Entry(goods).Property(e => e.is_onsale).IsModified = true;
         goods.is_onsale = false;

         db.SaveChanges();
         return RedirectToAction("Index");
      }

      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_order es_order = db.es_order.Find(id);
         if (es_order == null)
         {
            return HttpNotFound();
         }
         ViewBag.goods = new SelectList(db.es_goods, "id", "name", es_order.goods);
         ViewBag.buyer = new SelectList(db.xt_student, "id", "email", es_order.buyer);
         return View(es_order);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "id,amount,is_pay,states,receiver,addr,create_at,goods,buyer")] es_order es_order)
      {
         if (ModelState.IsValid)
         {
            db.Entry(es_order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         ViewBag.goods = new SelectList(db.es_goods, "id", "name", es_order.goods);
         ViewBag.buyer = new SelectList(db.xt_student, "id", "email", es_order.buyer);
         return View(es_order);
      }

      #region 取消订单
      [HttpPost]
      public JsonResult Cancel(int? id)
      {
         if (id == null)
         {
            return Json(false);
         }
         es_order order = db.es_order.Find(id);
         if (order == null)
         {
            return Json(false);
         }

         var goods = db.es_goods.Find(order.goods);
         db.Entry(goods).State = EntityState.Unchanged;
         db.Entry(goods).Property(e => e.is_onsale).IsModified = true;
         goods.is_onsale = true;

         db.es_order.Remove(order);
         db.SaveChanges();

         return Json(true);
      }
      #endregion

      #region 删除订单（PS：修改订单状态，不在订单列表中显示）
      [HttpPost]
      public JsonResult Delete(int? id)
      {
         if (id == null)
         {
            return Json(false);
         }
         es_order order = db.es_order.Find(id);
         if (order == null)
         {
            return Json(false);
         }

         db.Entry(order).State = EntityState.Modified;
         order.states = 5;
         db.SaveChanges();

         return Json(true);
      }
      #endregion

      #region 订单支付
      public ActionResult Pay(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }

         es_order order = GetOrder(id);

         if (order == null)
         {
            return HttpNotFound();
         }

         return View(order);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Pay(int id)
      {
         es_order order = GetOrder(id);

         if (order == null)
         {
            return HttpNotFound();
         }

         db.Entry(order).State = EntityState.Modified;
         order.is_pay = true;
         order.states = 2;

         db.SaveChanges();
         return RedirectToAction("Index");
      }
      #endregion

      #region 订单完成
      public ActionResult Finish(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         es_order order = GetOrder(id);

         if (order == null)
         {
            return HttpNotFound();
         }

         return View(order);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Finish(int id)
      {
         es_order order = GetOrder(id);

         if (order == null)
         {
            return HttpNotFound();
         }

         db.Entry(order).State = EntityState.Modified;
         order.states = 4;

         db.SaveChanges();
         return RedirectToAction("Index");
      }
      #endregion

      #region 获取订单信息
      public es_order GetOrder(int? id)
      {
         var sid = int.Parse(Session["Sid"].ToString());
         es_order order = db.es_order.Where(o => o.id == id && o.buyer == sid).FirstOrDefault();

         return order;
      }
      #endregion
   }
}
