using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using xiaotao.Filters;
using xiaotao.Models;

namespace xiaotao.Areas.Mall.Controllers
{
   [Filters.StoreLoginAuthorize]
   public class StoreController : Controller
   {
      private WebContext db = new WebContext();

      #region OverView
      public ActionResult Index()
      {
         int id = int.Parse(Session["StoreId"].ToString());
         ViewData["orders"] = db.sp_order.Where(e => e.store == id && (e.states == 2 || e.states == 5)).ToList();
         var pros = db.sp_product.Where(e => e.store == id);
         ViewData["low_stock"] = pros.Where(e => e.stock < 10).ToList();
         return View();
      }
      #endregion

      #region 店铺详情
      public ActionResult Details()
      {
         var id = Session["StoreId"];
         xt_store xt_store = db.xt_store.Find(int.Parse(id.ToString()));
         if (xt_store == null)
         {
            return HttpNotFound();
         }
         return View(xt_store);
      }
      #endregion

      #region 店铺装修
      public ActionResult Setting()
      {
         return RedirectToAction("basic");
      }
      public ActionResult Basic()
      {
         var id = Session["StoreId"];
         xt_store xt_store = db.xt_store.Find(int.Parse(id.ToString()));
         if (xt_store == null)
         {
            return HttpNotFound();
         }
         return View(xt_store);
      }
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Basic(string name,string intro, string phone)
      {
         int sid = int.Parse(Session["StoreId"].ToString());
         var store = db.xt_store.Find(sid);
         db.Entry(store).State = EntityState.Unchanged;
         db.Entry(store).Property(e=>e.name).IsModified = true;
         db.Entry(store).Property(e=>e.intro).IsModified = true;
         db.Entry(store).Property(e=>e.phone).IsModified = true;
         db.Entry(store).Property(e=>e.update_at).IsModified = true;

         store.name = name;
         store.intro = intro;
         store.phone = phone;
         store.update_at = DateTime.Now;

         ViewBag.msg = "基本信息修改成功！";

         db.SaveChanges();
         saveLog(sid, 3, "基本信息修改");
         return View(store);
      }

      public ActionResult Avatar()
      {
         var id = Session["StoreId"];
         xt_store xt_store = db.xt_store.Find(int.Parse(id.ToString()));
         if (xt_store == null)
         {
            return HttpNotFound();
         }
         return View(xt_store);
      }
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Avatar(string name, string intro)
      {
         int sid = int.Parse(Session["StoreId"].ToString());
         var store = db.xt_store.Find(sid);
         db.Entry(store).State = EntityState.Unchanged;
         db.Entry(store).Property(e => e.name).IsModified = true;
         db.Entry(store).Property(e => e.intro).IsModified = true;
         db.Entry(store).Property(e => e.update_at).IsModified = true;

         store.name = name;
         store.intro = intro;
         store.update_at = DateTime.Now;

         db.SaveChanges();
         saveLog(sid, 3, "头像修改");
         return View(store);
      }

      public ActionResult Safe()
      {
         var id = Session["StoreId"];
         xt_store xt_store = db.xt_store.Find(int.Parse(id.ToString()));
         if (xt_store == null)
         {
            return HttpNotFound();
         }
         return View(xt_store);
      }
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Safe(string login_pwd, string email)
      {
         int sid = int.Parse(Session["StoreId"].ToString());
         var store = db.xt_store.Find(sid);
         db.Entry(store).State = EntityState.Unchanged;
         db.Entry(store).Property(e => e.login_pwd).IsModified = true;
         db.Entry(store).Property(e => e.email).IsModified = true;
         db.Entry(store).Property(e => e.update_at).IsModified = true;

         store.login_pwd = login_pwd;
         store.email = email;
         store.update_at = DateTime.Now;

         ViewBag.msg = "安全信息修改成功！";

         db.SaveChanges();
         saveLog(sid,3,"安全信息修改");
         return View(store);
      }

      public ActionResult Renzheng()
      {
         var id = Session["StoreId"];
         xt_store xt_store = db.xt_store.Find(int.Parse(id.ToString()));
         if (xt_store == null)
         {
            return HttpNotFound();
         }
         return View(xt_store);
      }
      #endregion

      #region 商家入驻申请
      [AllowAnonymous]
      public ActionResult Signup()
      {
         return View();
      }

      [AllowAnonymous]
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Signup([Bind(Include = "id,login_id,login_pwd,name,license,shopkeeper,id_no,id_cart,email,phone")] xt_store xt_store)
      {
         if (ModelState.IsValid)
         {
            // 处理图片上传
            if (Request.Files.Count > 0)
            {
               var img1 = Request.Files[0];
               if (img1 != null && img1.ContentLength > 0)
               {
                  var fileName = Filters.Tools.GetRandomFileName(img1);

                  var path = Path.Combine(Server.MapPath("~/Uploads/Stores/"), fileName);
                  img1.SaveAs(path);
                  xt_store.license = fileName;
               }

               var img2 = Request.Files[1];
               if (img2 != null && img2.ContentLength > 0)
               {
                  var fileName = Filters.Tools.GetRandomFileName(img2);

                  var path = Path.Combine(Server.MapPath("~/Uploads/Stores/"), fileName);
                  img2.SaveAs(path);
                  xt_store.id_cart = fileName;
               }
            }

            xt_store.create_at = DateTime.Now;
            xt_store.verify = false;
            xt_store.states = true;
            db.xt_store.Add(xt_store);
            db.SaveChanges();

            // 存入记录
            xt_store_log log = new xt_store_log();
            log.kind = 1;
            log.create_at = DateTime.Now;
            log.store = db.xt_store.FirstOrDefault(s => s.login_id == xt_store.login_id).id;
            db.xt_store_log.Add(log);
            db.SaveChanges();

            return RedirectToAction("SignupResult");
         }

         return View(xt_store);
      }

      [AllowAnonymous]
      public ActionResult SignupResult()
      {
         return View();
      }
      #endregion

      #region 商家登录
      [AllowAnonymous]
      public ActionResult Signin()
      {
         if(Session["StoreId"] != null)
         {
            return RedirectToAction("Index");
         }
         return View();
      }

      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public ActionResult Signin([Bind(Include = "login_id,login_pwd")] xt_store xt_store)
      {
         var store = db.xt_store.FirstOrDefault(s => s.login_id == xt_store.login_id && s.login_pwd == xt_store.login_pwd);
         if (store != null)
         {
            if (store.states == false)
            {
               ModelState.AddModelError("", "抱歉，您的账户已被禁用，请联系管理员处理！");
               return View(xt_store);
            }
            else if (store.verify == false)
            {
               ModelState.AddModelError("", "抱歉，您的账户已还未通过核验，请稍后再试！");
               return View(xt_store);
            }
            else
            {
               // 设置登录Session
               Session.Add("StoreId", store.id);
               Session.Add("StoreName", store.name);

               // 存入记录
               xt_store_log log = new xt_store_log();
               log.kind = 2;
               log.create_at = DateTime.Now;
               log.store = db.xt_store.FirstOrDefault(s => s.login_id == xt_store.login_id).id;
               db.xt_store_log.Add(log);
               db.SaveChanges();

               var ReturnUrl = Request.QueryString["ReturnUrl"];
               if (ReturnUrl != null)
               {
                  return Redirect(ReturnUrl);
               }

               return RedirectToAction("Index");
            }
         }
         else
         {
            ModelState.AddModelError("", "用户名和密码不匹配！");
         }

         return View(xt_store);
      }
      #endregion

      #region 退出登录
      public ActionResult Signout()
      {
         // Session.Clear();
         Session.Remove("StoreId");
         Session.Remove("StoreName");
         return RedirectToAction("Signin");
      }
      #endregion

      #region 订单管理
      public ActionResult Orders(int? states, int pageIndex = 1, int pageSize = 8)
      {
         int id = int.Parse(Session["StoreId"].ToString());
         var orders = db.sp_order.Where(s => s.store == id).OrderByDescending(e=>e.create_at).ToList();

         ViewBag.state1 = orders.Where(e => e.states == 1).Count();
         ViewBag.state2 = orders.Where(e => e.states == 2).Count();
         ViewBag.state3 = orders.Where(e => e.states == 3).Count();
         ViewBag.state5 = orders.Where(e => e.states == 5).Count();

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

         ViewData["pager"] = Filters.Tools.PagerDesign(pageIndex, pageCount, "/mall/store/orders?" + queryString + "pageIndex=");

         return View(orders.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList());
      }

      [HttpPost]
      public JsonResult GetOrders(int states, int pageIndex, int pageSize = 8)
      {
         int id = int.Parse(Session["StoreId"].ToString());
         var orders = db.sp_order.Where(s => s.store == id).ToList();

         var state1 = orders.Where(e => e.states == 1).Count();
         var state2 = orders.Where(e => e.states == 2).Count();
         var state3 = orders.Where(e => e.states == 3).Count();
         var state5 = orders.Where(e => e.states == 5).Count();

         if (states != 0)
         {
            orders = orders.Where(e => e.states == states).ToList();
         }
         if(pageIndex != 0)
         {
            orders = orders.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
         }

         var json = new
         {
            states = new
            {
               s1 = state1,
               s2 = state2,
               s3 = state3,
               s5 = state5,
            },
            orders = (from o in orders select new {
               id = o.id,
               amount = o.amount,
               is_pay = o.is_pay,
               states = o.states,
               receiver = o.receiver,
               addr = o.addr,
               phone = o.phone,
               create_at = o.create_at.ToString(),
               order_items = (from oi in o.sp_order_item select new
               {
                  id = oi.id,
                  product = oi.product,
                  proName = oi.sp_product.name,
                  proImg = oi.sp_product.ori_img,
                  price = oi.price,
                  number = oi.number,
                  is_comment = oi.is_comment,
                  comment = oi.comment,
                  is_reply = oi.is_reply,
                  reply = oi.reply
               }).ToArray()
            }).ToArray()
         };
         return Json(json);
      }

      #endregion

      #region 商品列表
      public ActionResult ProList(string type="all")
      {
         int id = int.Parse(Session["StoreId"].ToString());
         var products = db.sp_product.Where(s => s.store == id).OrderByDescending(e=>e.create_at).ToList();

         if(type == "isOnsale")
         {
            products = products.Where(e=>e.is_onsale==true).ToList();
         }else if(type == "isDelete")
         {
            products = products.Where(e => e.is_delete == true).ToList();
         }

         return View(products);
      }
      #endregion

      #region 统计
      public ActionResult Statistics()
      {
         return View();
      }

      /// <summary>
      /// 商品销量排行
      /// </summary>
      /// <returns></returns>
      public JsonResult ProductSales(DateTime? dt,int page=1)
      {
         int sid = int.Parse(Session["StoreId"].ToString());
         int year = 0;
         int month = 0;
         if (dt == null)
         {
            year = DateTime.Now.Year;
            month = DateTime.Now.Month;
         }
         else
         {
            year = Convert.ToDateTime(dt).Year;
            month = Convert.ToDateTime(dt).Month;
         }
         var orders = db.sp_order_item.Where(e => e.sp_order.store == sid && e.sp_order.create_at.Year==year&& e.sp_order.create_at.Month == month)
               .GroupBy(e => e.product)
               .Select(e => new { product = e.Key, sales = e.ToList() })
               .ToList();
         var showNum = 2;
         var json = new
         {
            counts = orders.Count(),
            proSales = (from o in orders.OrderByDescending(e => GetProductSale(e.sales)).Skip((page - 1) * showNum).Take(showNum)
                        select new
                        {
                           product = GetProduct(o.product),
                           sales = GetProductSale(o.sales)
                        })
         };
         return Json(json,JsonRequestBehavior.AllowGet);
      }

      public object GetProduct(int id)
      {
         var p = db.sp_product.Find(id);
         return new
         {
            id = p.id,
            name = p.name,
            ori_img = p.ori_img,
            price = p.price
         };
      }

      public int GetProductSale(List<sp_order_item> oi)
      {
         int sales = 0;
         foreach (var o in oi)
         {
            sales += o.number;
         }
         return sales;
      }

      /// <summary>
      /// 按日期进行销量统计
      /// </summary>
      /// <returns></returns>
      public JsonResult Sales()
      {
         var sid = int.Parse(Session["StoreId"].ToString());
         var orders = db.sp_order_item.Where(e => e.sp_order.store == sid).GroupBy(e => e.sp_order.create_at.Month)
            .Select(e => new { month = e.Key,orderNum = e.Count(), moneys = e.ToList() })
            .ToList();
         var json = (from o in orders
                     select new
                     {
                        month = o.month,
                        orderNum = o.orderNum,
                        moneys = GetMoneys(o.moneys)
                     });
         return Json(json,JsonRequestBehavior.AllowGet);
      }

      private decimal GetMoneys(List<sp_order_item> oi)
      {
         decimal moneys = 0;
         foreach(var o in oi)
         {
            moneys += o.number * o.price;
         }
         return moneys;
      }

      #endregion

      #region 发货
      public ActionResult Delivery(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }

         sp_order order = GetOrderById(id);

         if (order == null)
         {
            return HttpNotFound();
         }

         return View(order);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Delivery(int id)
      {
         sp_order order = GetOrderById(id);

         if (order == null)
         {
            return HttpNotFound();
         }

         db.Entry(order).State = EntityState.Modified;
         order.states = 3;
         order.deliver_time = DateTime.Now;

         // 商品已经发送出去，现在需要添加商品的销量
         var oitems = db.sp_order_item.Where(oi => oi.oid == order.id).ToList();
         foreach (var oi in oitems)
         {
            var pro = db.sp_product.Find(oi.product);
            db.Entry(pro).State = EntityState.Modified;
            pro.sales += oi.number;
         }
         db.SaveChanges();

         return RedirectToAction("Orders");
      }
      #endregion

      [HttpPost]
      public JsonResult GetComment(int id)
      {
         var ois = db.sp_order_item.Where(e=>e.oid==id);
         var json = (from oi in ois
                     select new
                     {
                        oid = oi.id,
                        product = oi.product,
                        rank = oi.rank,
                        comment = oi.comment
                     }).ToArray();

         return Json(json);
      }

      [HttpPost]
      public ActionResult Reply(int id)
      {
         var oitems = db.sp_order_item.Where(oi => oi.oid == id).ToList();
         foreach (var oi in oitems)
         {
            db.Entry(oi).State = EntityState.Modified;
            oi.is_reply = true;
            oi.reply = Request.Form["g_" + oi.id + "_reply"];
         }

         // 更新订单状态
         var order = db.sp_order.Find(id);
         db.Entry(order).State = EntityState.Modified;
         order.states = 6;

         db.SaveChanges();

         return Redirect(Request.QueryString["rawUrl"]);

      }

      public sp_order GetOrderById(int? id)
      {
         var sid = int.Parse(Session["StoreId"].ToString());
         sp_order order = db.sp_order.Where(o => o.id == id && o.store == sid).FirstOrDefault();

         return order;
      }

      [AllowAnonymous]
      public JsonResult IsExistLoginID(string login_id)
      {
         bool flag = true;
         if (db.xt_store.Where(e => e.login_id == login_id).Count() != 0)
         {
            flag = false;
         }
         return Json(flag, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// 验证用户身份
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public bool Valid(int id)
      {
         return true;
      }

      /// <summary>
      /// 保存操作日志
      /// </summary>
      /// <param name="sid"></param>
      /// <param name="kind"></param>
      public void saveLog(int sid,byte kind,string remark)
      {
         var log = new xt_store_log();
         log.store = sid;
         log.kind = kind;
         log.remark = remark;
         log.create_at = DateTime.Now;

         db.xt_store_log.Add(log);
         db.SaveChanges();
      }
   }
}