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
using System.Web.Script.Serialization;
using xiaotao.Areas.Mall.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using xiaotao.Filters;

namespace xiaotao.Areas.Mall.Controllers
{
   public class AccountController : Controller
   {

      private WebContext db = new WebContext();

      #region Overview
      public ActionResult Index()
      {
         var sid = int.Parse(Session["Sid"].ToString());
         var sp_order = db.sp_order.Where(o => o.buyer == sid && o.states < 4);
         return View(sp_order.ToList());
      }
      #endregion

      #region 学生登录
      public ActionResult Signin()
      {
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Signin(string user, string pwd)
      {
         if (ModelState.IsValid)
         {
            var student = GetStu(user,pwd);
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pwd))
            {
               return View();
            }
            if (student != null)
            {
               // 保存用户登录日志
               SaveLoginLog(student);
               // 如需返回上一个页面，则通过获取链接中的返回地址参数
               var ReturnUrl = Request.QueryString["ReturnUrl"];
               if (ReturnUrl != null)
               {
                  var url = ReturnUrl;
                  if (Request.QueryString.Count > 1)
                  {
                     foreach (var q in Request.QueryString)
                     {
                        if (q.ToString() != "ReturnUrl")
                        {
                           url += "&" + q.ToString() + "=" + Request.QueryString[q.ToString()];
                        }
                     }
                  }
                  return Redirect(url);
               }

               return RedirectToAction("Index");
            }
            ModelState.AddModelError("error", "账号和密码不匹配.");
         }

         return View();
      }

      [HttpPost]
      public JsonResult SigninByAjax(string user, string pwd)
      {
         var student = GetStu(user, pwd);

         if (student != null)
         {
            SaveLoginLog(student);
            return Json(true);
         }
         return Json(false);
      }

      private xt_student GetStu(string user, string pwd)
      {
         var student = new xt_student();
         // 检测用户是否是使用Email登录
         var md5Pwd = Tools.MD5(pwd);
         if (user.Contains("@"))
         {
            student = db.xt_student.Where(e => e.email == user && e.pwd == md5Pwd).SingleOrDefault();
         }
         else
         {
            var sno = int.Parse(user);
            student = db.xt_student.Where(e => e.sno == sno && e.pwd == md5Pwd).SingleOrDefault();
         }
         return student;
      }

      public void SaveLoginLog(xt_student student)
      {
         Session.Add("Sid", student.id);
         Session.Add("Semail", student.email);
         Session.Add("Savatar", student.avatar);
         Session.Add("Sno", student.sno);
         Session.Add("Sname", student.sname);

         Session.Add("CartItems", db.sp_cart_item.Where(e => e.student == student.id).Count());

         // 用户实名认证状态
         if (student.verify == true)
         {
            Session.Add("Verify", student.verify);
         }

         SaveStuLog(student.id,1);
         
      }

      #endregion

      #region 学生注册
      public ActionResult Signup()
      {
         return View();
      }

      public JsonResult IsExistEmail(string email)
      {
         bool flag = true;
         if(db.xt_student.Where(e=>e.email==email).Count() != 0)
         {
            flag = false;
         }
         return Json(flag,JsonRequestBehavior.AllowGet);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Signup(xt_student stu)
      {
         if (ModelState.IsValid)
         {
            try
            {
               stu.create_at = DateTime.Now;
               stu.verify = false;
               stu.states = true;

               // MD5
               stu.pwd = Tools.MD5(stu.pwd);
               db.xt_student.Add(stu);
               db.SaveChanges();

               // 将注册成功载入日志
               SaveStuLog(stu.id, 2);

               return RedirectToAction("Signin");
            }
            catch
            {
               ModelState.AddModelError("", "注册失败.");
            }
         }
         return View(stu);
      }

      #endregion

      #region 检测用户是否存在
      public JsonResult Validate(int sno)
      {
         var stu = from s in db.xt_student
                   where s.sno == sno
                   select s;
         if (stu != null)
         {
            return Json(false, JsonRequestBehavior.AllowGet);
         }
         return Json(true, JsonRequestBehavior.AllowGet);
      }
      #endregion

      #region 完善个人信息
      public ActionResult Complete()
      {
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Complete(xt_student stu)
      {
         return View();
      }

      #endregion

      #region 退出登录
      public ActionResult Signout()
      {
         Session.Remove("Sid");
         Session.Remove("Semail");
         Session.Remove("Savatar");
         Session.Remove("Sno");
         Session.Remove("Sname");
         Session.Remove("CartItems");

         if (Session["Verify"] != null)
         {
            Session.Remove("Verify");
         }
         return Redirect("/");
      }
      #endregion

      #region 设置中心
      public ActionResult Setting()
      {
         if (Session["Sid"] == null)
         {
            return RedirectToAction("Signin");
         }
         else
         {
            var stu = db.xt_student.Find(int.Parse(Session["Sid"].ToString()));
            if (stu != null)
            {
               return View(stu);
            }
            else
            {
               return HttpNotFound();
            }
         }
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Setting(xt_student stu)
      {
         db.Entry(stu).State = EntityState.Modified;
         db.Entry(stu).Property(e => e.create_at).IsModified = false;
         stu.update_at = DateTime.Now;
         db.SaveChanges();
         return View();
      }

      public ActionResult Avatar()
      {
         if (Session["Sid"] == null)
         {
            return RedirectToAction("Signin");
         }
         else
         {
            var stu = db.xt_student.Find(int.Parse(Session["Sid"].ToString()));
            if (stu != null)
            {
               return View(stu);
            }
            else
            {
               return HttpNotFound();
            }
         }
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Avatar(int? id)
      {
         var avatar_data = Request.Form["avatar_data"];
         JavaScriptSerializer json = new JavaScriptSerializer();
         ImageEntity image = json.Deserialize<ImageEntity>(avatar_data);

         var file = Request.Files[0];
         var fileName = Tools.GetRandomFileName(file);

         file.SaveAs(Server.MapPath("~/Uploads/account/" + fileName));
         Image bigImg = Image.FromFile(Server.MapPath("~/Uploads/account/" + fileName));

         var img = ImageTools.CutImage(bigImg, (int)image.x, (int)image.y, (int)image.width, (int)image.height);

         var stu = db.xt_student.Find(int.Parse(Session["Sid"].ToString()));
         db.Entry(stu).State = EntityState.Unchanged;
         db.Entry(stu).Property(e => e.avatar).IsModified = true;

         var fileName1 = ImageTools.UploadBitmap(img, "account/avatar");
         stu.avatar = fileName1;

         db.SaveChanges();
         Session.Add("Savatar", fileName1);

         bigImg.Dispose();
         img.Dispose();

         string ori = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/account/" + fileName);
         if (System.IO.File.Exists(ori))
         {
            System.IO.File.Delete(ori);
         }

         return Json(true);
      }

      public JsonResult IsExistPhone(string phone)
      {
         var flag = true;
         if(db.xt_student.Where(e=>e.phone == phone).Count() != 0)
         {
            flag = false;
         }
         return Json(flag,JsonRequestBehavior.AllowGet);
      }

      #endregion

      #region 安全中西
      [Filters.StuLoginAuthorize]
      public ActionResult Security()
      {
         var sid = int.Parse(Session["Sid"].ToString());
         var stu = db.xt_student.Find(sid);
         if (stu == null)
         {
            return HttpNotFound();
         }

         if (stu.verify == false)
         {
            var certification = db.xt_stu_certification.Where(e => e.student == sid && e.valid_at == null).FirstOrDefault();
            if (certification != null)
            {
               ViewBag.isValiding = true;
            }
         }
         return View(stu);

      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Security(int sno, string sname)
      {
         int sid = int.Parse(Session["Sid"].ToString());

         // 检测是否该学生是否已存在数据库
         var stu = db.xt_student.SingleOrDefault(e => e.sno == sno);
         var sm = db.xt_stu_certification.Where(e => e.sno == sno && e.is_pass == false);
         if (stu == null && sm.Count() == 0)
         {
            var certification = new xt_stu_certification();
            certification.student = sid;
            certification.sno = sno;
            certification.sname = sname;
            certification.create_at = DateTime.Now;
            certification.is_pass = false;

            if (Request.Files.Count > 0)
            {
               var file = Request.Files[0];
               if (file != null && file.ContentLength > 0)
               {
                  uploadResult ur = ImageTools.UploadImage(file, "Account/Security");
                  if (ur.flag == true)
                  {
                     certification.voucher = ur.msg;
                     db.xt_stu_certification.Add(certification);
                     db.SaveChanges();
                     return RedirectToAction("Security");
                  }
                  else
                  {
                     ModelState.AddModelError("error", ur.msg);
                  }
               }
            }

         }
         else
         {
            ModelState.AddModelError("error", "此学号已被注册！");
         }

         return View(db.xt_student.Find(sid));
      } 
      #endregion

      #region 学生地址管理
      [Filters.StuLoginAuthorize]
      public ActionResult Address(int? id)
      {
         var sid = int.Parse(Session["Sid"].ToString());
         ViewData["address"] = db.xt_student_address.Where(e => e.student == sid).ToList();
         if (id != null)
         {
            var addr = db.xt_student_address.Find(id);
            ViewBag.area = new SelectList(db.xt_area, "id", "name", addr.area);
            return View(addr);
         }

         ViewBag.area = new SelectList(db.xt_area, "id", "name");
         return View();
      }

      [Filters.StuLoginAuthorize]
      public ActionResult AddrSetDefault(int id)
      {
         int sid = int.Parse(Session["Sid"].ToString());

         var addr = db.xt_student_address.Find(id);

         var defaultAddr = db.xt_student_address.Where(e => e.student == sid && e.is_default == true).FirstOrDefault();
         if (defaultAddr != null)
         {
            db.Entry(defaultAddr).State = EntityState.Unchanged;
            db.Entry(defaultAddr).Property(e => e.is_default).IsModified = true;
            defaultAddr.is_default = false;
         }

         db.Entry(addr).State = EntityState.Unchanged;
         db.Entry(addr).Property(e => e.is_default).IsModified = true;
         addr.is_default = true;

         db.SaveChanges();

         return RedirectToAction("Address");
      }

      [HttpPost]
      public ActionResult SaveAddress([Bind(Include = "id,area,addr,receiver,phone,remark,is_default,student")] xt_student_address xt_student_address)
      {
         int sid = int.Parse(Session["Sid"].ToString());

         // 判断此地址是否是默认的
         if (xt_student_address.is_default == true)
         {
            var defaultAddr = db.xt_student_address.Where(e => e.student == sid && e.is_default == true).FirstOrDefault();
            if (defaultAddr != null && defaultAddr.id != xt_student_address.id)
            {
               db.Entry(defaultAddr).State = EntityState.Unchanged;
               db.Entry(defaultAddr).Property(e => e.is_default).IsModified = true;
               defaultAddr.is_default = false;
            }
            else if (defaultAddr != null && defaultAddr.id == xt_student_address.id)
            {
               db.Entry(xt_student_address).State = EntityState.Modified;
               xt_student_address.student = sid;
            }
         }

         // 判断是否为修改
         if (xt_student_address.id > 0)
         {
            db.Entry(xt_student_address).State = EntityState.Modified;
            xt_student_address.student = sid;
         }
         else
         {
            xt_student_address.student = sid;
            db.xt_student_address.Add(xt_student_address);
         }

         db.SaveChanges();

         return RedirectToAction("Address");
      } 
      #endregion

      [HttpPost]
      public JsonResult IsExistSno(int sno)
      {
         bool isExist = true;

         var stu = db.xt_student.Where(e => e.sno == sno);
         var certification = db.xt_stu_certification.Where(e => e.sno == sno && e.is_pass == false);
         if (stu.Count() != 0 || certification.Count() != 0)
         {
            isExist = false;
         }

         return Json(isExist);
      }

      /// <summary>
      /// 添加操作日志
      /// </summary>
      /// <param name="sid"></param>
      /// <param name="type"></param>
      private void SaveStuLog(int sid,byte type)
      {
         xt_student_log log = new xt_student_log();
         log.student = sid;
         log.kind = type;
         log.create_at = DateTime.Now;
         db.xt_student_log.Add(log);
         db.SaveChanges();
      }
   }
}