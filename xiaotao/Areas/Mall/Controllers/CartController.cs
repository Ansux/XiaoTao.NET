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
   public class CartController : Controller
   {
      private WebContext db = new WebContext();

      public ActionResult Index()
      {
         return View();
      }

      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         sp_cart_item sp_cart_item = db.sp_cart_item.Find(id);
         if (sp_cart_item == null)
         {
            return HttpNotFound();
         }
         return View(sp_cart_item);
      }

      #region 商品详情->加入购物车
      public ActionResult Create(int pid,int number)
      {
         try
         {
            var sid = int.Parse(Session["Sid"].ToString());

            var cartItem = db.sp_cart_item.FirstOrDefault(e => e.product == pid && e.student == sid);
            if (cartItem != null)
            {
               cartItem.number += number;
            }
            else
            {
               var ci = new sp_cart_item();
               ci.create_at = DateTime.Now;
               ci.product = pid;
               ci.number = number;
               ci.student = sid;

               db.sp_cart_item.Add(ci);

               Session["CartItems"] = int.Parse(Session["CartItems"].ToString()) + 1;

            }
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         catch
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
      }
      #endregion

      #region 删除购物车条目
      [HttpPost]
      public JsonResult Delete(int id)
      {
         var cartItem = db.sp_cart_item.Find(id);
         db.sp_cart_item.Remove(cartItem);
         db.SaveChanges();

         Session["CartItems"] = int.Parse(Session["CartItems"].ToString()) - 1;
         return Json(true, JsonRequestBehavior.AllowGet);
      }
      #endregion

      #region 变更（增减）购物车商品数量
      [HttpPost]
      public JsonResult Modify(string action, int id, int number=1)
      {
         var res = true;
         var cartItem = db.sp_cart_item.Find(id);
         if (cartItem != null)
         {
            if (action == "add")
            {
               if (cartItem.sp_product.stock > 0)
               {
                  cartItem.number += 1;
               }
               else
               {
                  res = false;
               }
            }
            else if (action == "minus")
            {
               if (cartItem.number > 1)
               {
                  cartItem.number -= 1;
               }
               else
               {
                  res = false;
               }
            }else if(action == "modify")
            {
               cartItem.number = number;
            }
            db.SaveChanges();
         }
         else
         {
            res = false;
         }

         return Json(res, JsonRequestBehavior.AllowGet);
      }
      #endregion

      #region 获取购物车题目列表 Json
      [HttpPost]
      public JsonResult GetCartList()
      {
         var sid = int.Parse(Session["Sid"].ToString());
         List<sp_cart_item> clist = db.sp_cart_item.Where(ci => ci.student == sid).ToList();

         // 按店铺归类购物车商品
         var stores = clist;
         List<int> storeList = new List<int>();
         foreach (var item in stores)
         {
            storeList.Add(item.sp_product.store);
         }
         storeList = storeList.Distinct().ToList();

         var json = new
         {
            isCheck = false,
            stores = (from s in storeList
                      select new CartStores()
                      {
                         sid = s,
                         sname = db.xt_store.Find(s).name,
                         isCheck = false,
                         proItems = (from g in clist
                                     where g.sp_product.store == s
                                     select new CartItems()
                                     {
                                        cid = g.id,
                                        proId = g.sp_product.id,
                                        proName = g.sp_product.name,
                                        number = Convert.ToInt32(g.number),
                                        price = g.sp_product.price,
                                        thumb = g.sp_product.ori_img,
                                        subPrice = g.sp_product.price * Convert.ToInt32(g.number),
                                        stock = Convert.ToInt32(g.sp_product.stock),
                                        isCheck = false
                                     }).ToArray()
                      }).ToArray()
         };
         return Json(json, JsonRequestBehavior.AllowGet);
      }
      #endregion

      #region 购物车结算，按店铺归类，去重
      public ActionResult Settle()
      {
         return RedirectToAction("Index");
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      [Filters.StuVerifyAuthorize]
      public ActionResult Settle(sp_product sp)
      {
         string singleCheckout = Request.Form["type"];
         if(singleCheckout == "single")
         {
            // 从商品详情页直接购买结算。
            int pid = int.Parse(Request.Form["pid"]);
            int number = int.Parse(Request.Form["number"]);

            var product = db.sp_product.Find(pid);

            ViewData["product"] = product;
            ViewBag.storeId = product.store;
            ViewBag.storeName = product.xt_store.name;
            ViewBag.number = number;
            ViewBag.type = 1;
            return View();
         }
         else
         {
            // 从购物车进行结算
            string formIds = Request.Form["product"];
            if (!string.IsNullOrEmpty(formIds))
            {
               string[] idArray = formIds.Split(',');
               int[] ids = Array.ConvertAll<string, int>(idArray, s => int.Parse(s));
               var cartItems = db.sp_cart_item.Where(ci => ids.Contains(ci.id));

               // 按店铺归类购物车商品
               var stores = cartItems;
               List<SettleStores> storeList = new List<SettleStores>();
               foreach (var item in stores)
               {
                  SettleStores obj = new SettleStores();
                  obj.sid = item.sp_product.xt_store.id;
                  obj.sname = item.sp_product.xt_store.name;
                  storeList.Add(obj);
               }
               // 去重复的数据
               ViewData["storeList"] = storeList.Where((x, i) => storeList.FindIndex(z => z.sid == x.sid) == i).ToList();
               ViewData["cartItems"] = cartItems.ToList();
               return View();
            }
         }
         
         return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }

      [HttpPost]
      [Filters.StuVerifyAuthorize]
      public ActionResult SingleSettle(int pid,int number)
      {
         int sid = int.Parse(Session["Sid"].ToString());

         var product = db.sp_product.Find(pid);

         ViewBag.number = number;

         ViewBag.storeId = product.store;
         ViewBag.storeName = product.xt_store.name;

         return View(product);
      }
      #endregion

      #region 获取用户地址列表
      public JsonResult GetAddrList()
      {
         var stuId = int.Parse(Session["Sid"].ToString());
         // 用户地址
         var addrs = db.xt_student_address.Where(a => a.student == stuId).ToList();
         var json = new
         {
            addrs = (from a in addrs
                     select new
                     {
                        id = a.id,
                        receiver = a.receiver,
                        area = a.area,
                        areaName = a.xt_area.name,
                        addr = a.addr,
                        phone = a.phone,
                        is_default = a.is_default
                     }).ToArray()
         };
         return Json(json, JsonRequestBehavior.AllowGet);
      }
      #endregion

      #region 获取地址区域，用于动态添加用户地址
      public JsonResult GetAreas()
      {
         var areas = db.xt_area.ToList();
         var json = new
         {
            areas = (from a in areas
                     select new
                     {
                        id = a.id,
                        name = a.name
                     }).ToArray()
         };
         return Json(json, JsonRequestBehavior.AllowGet);
      }
      #endregion

      #region 保存用户动态添加的收货地址
      [HttpPost]
      public ActionResult AddrSave([Bind(Include = "id,area,addr,receiver,phone,remark,is_default")] xt_student_address address)
      {
         if (ModelState.IsValid)
         {
            var student = int.Parse(Session["Sid"].ToString());

            if (address.is_default == true)
            {
               // 将原有的默认地址取消掉。
               var addr = db.xt_student_address.Where(a => a.student == student && a.is_default == true).FirstOrDefault();
               if (addr != null)
               {
                  addr.is_default = false;
                  db.Entry(addr).State = EntityState.Modified;
                  db.SaveChanges();
               }
            }

            if (address.id <= 0)
            {
               address.student = student;
               db.xt_student_address.Add(address);
               db.SaveChanges();
               var a = db.xt_student_address.Find(address.id);

               var json = new
               {
                  id = a.id,
                  receiver = a.receiver,
                  area = a.area,
                  areaName = db.xt_area.Find(a.area).name,
                  addr = a.addr,
                  phone = a.phone,
                  is_default = a.is_default
               };
               return Json(json);
            }
            else
            {
               address.student = student;
               db.Entry(address).State = EntityState.Modified;
               db.SaveChanges();
               return Json(true);
            }

         }
         return Json(false);
      } 
      #endregion
   }
}
