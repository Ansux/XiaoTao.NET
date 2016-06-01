using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using xiaotao.Models;

namespace xiaotao.Areas.Mall.Controllers
{
   [Filters.StuLoginAuthorize]
   public class OrderController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index(int? states, int pageIndex = 1, int pageSize = 8)
      {
         var sid = int.Parse(Session["Sid"].ToString());
         var orders = db.sp_order.Where(o => o.buyer == sid).OrderByDescending(e=>e.create_at).ToList();

         ViewBag.state1 = orders.Where(e => e.states == 1).Count();
         ViewBag.state2 = orders.Where(e => e.states == 2).Count();
         ViewBag.state3 = orders.Where(e => e.states == 3).Count();
         ViewBag.state4 = orders.Where(e => e.states == 4).Count();

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

         ViewData["pager"] = Filters.Tools.PagerDesign(pageIndex, pageCount, "/mall/order/index?" + queryString + "pageIndex=");

         return View(orders.Skip(pageSize * (pageIndex - 1)).Take(pageSize));
      }

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         sp_order sp_order = db.sp_order.Find(id);
         if (sp_order == null)
         {
            return HttpNotFound();
         }
         return View(sp_order);
      }

      #region 创建用户订单
      public ActionResult Create()
      {
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create(int addr)
      {
         var student = int.Parse(Session["Sid"].ToString());
         // 获取订单中的用户地址Id，重新组装
         var orderAddr = db.xt_student_address.Find(addr);
         var receiver = orderAddr.receiver;
         var address = orderAddr.xt_area.name + "区" + orderAddr.addr;
         var phone = orderAddr.phone;

         var type = Request.Form["type"];
         var flag = true;

         using (var trans = db.Database.BeginTransaction())
         {
            try
            {
               if (type == "single")
               {
                  int pid = int.Parse(Request.Form["product"]);
                  int num = int.Parse(Request.Form["number"]);
                  var product = db.sp_product.Find(pid);
                  if (product.stock < num)
                  {
                     flag = true;
                     ModelState.AddModelError("error", "抱歉，您所选购的商品<" + product.name + ">库存不足。");
                     return View();
                  }
                  var oid = CreateOrder(product.price * num, receiver, address, phone, product.store);
                  CreateOrderItem(oid, product.id, product.price, num, false);
                  return RedirectToAction("index");
               }
               else
               {
                  var formCartIds = Request.Form["cartItem"];
                  string[] cartIds = formCartIds.Split(',');
                  int[] ids = Array.ConvertAll<string, int>(cartIds, s => int.Parse(s));
                  var cartItems = db.sp_cart_item.Where(ci => ids.Contains(ci.id));

                  foreach (var c in cartItems)
                  {
                     if (c.number > c.sp_product.stock)
                     {
                        ModelState.AddModelError("error", "抱歉，您所选购的商品<" + c.sp_product.name + ">库存不足。");
                        flag = false;
                     }
                  }
                  if (flag == false)
                  {
                     ViewData["error"] = false;
                     return View();
                  }

                  // 购物清单按商铺归类
                  List<int> stores = new List<int>();
                  foreach (var item in cartItems)
                  {
                     stores.Add(item.sp_product.xt_store.id);
                  }
                  stores = stores.Distinct().ToList();

                  // 按店铺商品创建订单
                  foreach (var item in stores)
                  {
                     var carts = cartItems.Where(ci => ci.sp_product.store == item);
                     decimal amount = 0;
                     foreach (var cart in carts)
                     {
                        amount += Convert.ToDecimal(cart.sp_product.price * cart.number);
                     }
                     // 创建订单
                     var oid = CreateOrder(amount, receiver, address, phone, item);
                     // 将订单商品记录入库，并删除购物车，并减少相应的商品库存
                     foreach (var cart in carts)
                     {
                        // 创建订单条目
                        CreateOrderItem(oid, cart.product, cart.sp_product.price, cart.number, true);
                        // 删除购物车条目
                        db.sp_cart_item.Remove(cart);
                        Session["CartItems"] = int.Parse(Session["CartItems"].ToString()) - 1;
                     }
                     db.SaveChanges();
                  }
               }
               trans.Commit();
            }
            catch (Exception)
            {
               trans.Rollback();
            }
            return RedirectToAction("index");
         }
      }

      /// <summary>
      /// 生成订单
      /// </summary>
      /// <param name="amount"></param>
      /// <param name="receiver"></param>
      /// <param name="addr"></param>
      /// <param name="phone"></param>
      /// <param name="store"></param>
      /// <returns></returns>
      public int CreateOrder(decimal amount, string receiver, string addr, string phone, int store)
      {
         var student = int.Parse(Session["Sid"].ToString());
         var order = new sp_order();
         order.amount = amount;
         order.receiver = receiver;
         order.addr = addr;
         order.phone = phone;
         order.is_pay = false;
         order.states = 1;

         order.create_at = DateTime.Now;
         order.store = store;
         order.buyer = student;

         db.sp_order.Add(order);
         db.SaveChanges();
         return order.id;
      }

      /// <summary>
      /// 生成订单商品条目
      /// </summary>
      /// <param name="oid"></param>
      /// <param name="product"></param>
      /// <param name="price"></param>
      /// <param name="number"></param>
      public void CreateOrderItem(int oid, int product, decimal price, int number, bool hasMore)
      {
         db.sp_order_item.Add(new sp_order_item { oid = oid, product = product, price = price, number = number, is_comment = false, is_reply = false });

         // 减少商品库存
         var goods = db.sp_product.Find(product);
         goods.stock -= number;

         if (hasMore == false)
         {
            db.SaveChanges();
         }
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult CreateSingleOrder(int pid, int number, int addr)
      {
         var flag = true;
         var product = db.sp_product.Find(pid);
         if (number > product.stock)
         {
            ModelState.AddModelError("error", "抱歉，您所选购的商品库存不足。");
            flag = false;
         }

         var address = db.xt_student_address.Find(addr);
         CreateOrder(product.price * number, address.receiver, address.addr, address.phone, product.store);
         return View();

      }
      #endregion

      #region 取消订单
      [HttpPost]
      public JsonResult Cancel(int? id)
      {
         if (id == null)
         {
            return Json(false);
         }
         sp_order order = db.sp_order.Find(id);
         if (order == null)
         {
            return Json(false);
         }
         var oitems = db.sp_order_item.Where(oi => oi.oid == id).ToList();
         foreach (var oi in oitems)
         {
            // 恢复相应的商品库存
            var pro = db.sp_product.Find(oi.product);
            db.Entry(pro).State = EntityState.Modified;
            pro.stock += oi.number;

            // 删除相应的订单商品条目
            db.sp_order_item.Remove(oi);
         }
         db.sp_order.Remove(order);
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
         sp_order order = db.sp_order.Find(id);
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

         sp_order order = GetOrder(id);

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
         sp_order order = GetOrder(id);

         if (order == null)
         {
            return HttpNotFound();
         }

         db.Entry(order).State = EntityState.Modified;
         order.is_pay = true;
         order.states = 2;
         order.pay_time = DateTime.Now;

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
         sp_order order = GetOrder(id);

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
         sp_order order = GetOrder(id);

         if (order == null)
         {
            return HttpNotFound();
         }

         db.Entry(order).State = EntityState.Modified;
         order.states = 4;
         order.finish_time = DateTime.Now;

         db.SaveChanges();
         return RedirectToAction("Index");
      }
      #endregion

      #region 订单评价
      public ActionResult Comment(int? id)
      {
         var oitems = db.sp_order_item.Where(oi => oi.oid == id).ToList();
         ViewBag.Oid = id;
         return View(oitems);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Comment()
      {
         var oid = int.Parse(Request.Form["oid"]);
         var oitems = db.sp_order_item.Where(oi => oi.oid == oid).ToList();
         foreach (var oi in oitems)
         {
            db.Entry(oi).State = EntityState.Modified;
            oi.is_comment = true;
            oi.rank = int.Parse(Request.Form["g_" + oi.id + "-[rank]"]);
            oi.comment = Request.Form["g_" + oi.id + "-[comment]"];
         }

         // 更新订单状态
         var order = db.sp_order.Find(oid);
         db.Entry(order).State = EntityState.Modified;
         order.states = 5;

         db.SaveChanges();

         return RedirectToAction("Index");
      }

      #endregion

      #region 获取订单信息
      public sp_order GetOrder(int? id)
      {
         var sid = int.Parse(Session["Sid"].ToString());
         sp_order order = db.sp_order.Where(o => o.id == id && o.buyer == sid).FirstOrDefault();

         return order;
      }
      #endregion
   }
}
