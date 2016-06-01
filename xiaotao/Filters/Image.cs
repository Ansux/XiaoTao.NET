using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace xiaotao.Filters
{
   public class ImageTools
   {
      /// <summary>
      /// 图片裁剪
      /// </summary>
      /// <param name="b"></param>
      /// <param name="StartX"></param>
      /// <param name="StartY"></param>
      /// <param name="iWidth"></param>
      /// <param name="iHeight"></param>
      /// <returns></returns>
      public static Bitmap CutImage(Image b, int StartX, int StartY, int iWidth, int iHeight)
      {
         if (b == null)
         {
            return null;
         }
         int w = b.Width;
         int h = b.Height;
         if (StartX >= w || StartY >= h)
         {
            // 开始截取坐标过大时，结束处理 
            return null;
         }
         if (StartX + iWidth > w)
         {
            // 宽度过大时只截取到最大大小 
            iWidth = w - StartX;
         }
         if (StartY + iHeight > h)
         {
            // 高度过大时只截取到最大大小 
            iHeight = h - StartY;
         }
         try
         {
            // 图片裁剪
            Bitmap temp = new Bitmap(iWidth, iHeight);
            Graphics g = Graphics.FromImage(temp);
            g.DrawImage(b, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(StartX, StartY, iWidth, iHeight), GraphicsUnit.Pixel);
            g.Dispose();

            // 图片缩小
            Bitmap outPut = new Bitmap(100, 100);
            Graphics g1 = Graphics.FromImage(outPut);
            g1.InterpolationMode = InterpolationMode.Default;
            g1.DrawImage(temp, new Rectangle(0, 0, 100, 100), new Rectangle(0, 0, iWidth, iHeight), GraphicsUnit.Pixel);

            g1.Dispose();
            return outPut;

         }
         catch
         {
            return null;
         }
      }

      /// <summary>
      /// 保存Bitmap文件
      /// </summary>
      /// <param name="b"></param>
      /// <param name="subPath"></param>
      /// <returns></returns>
      public static string UploadBitmap(Bitmap b,string subPath)
      {
         // 日期子路径
         string datePath = DateTime.Now.ToString("yyyyMM") + "/";
         // 随机文件名
         Random ran = new Random();
         string RandKey = ran.Next(100, 999).ToString();
         var fileName = TimeStamp().ToString() + RandKey + ".jpg";

         try
         {
            // 创建路径
            string directoryPath = HttpContext.Current.Server.MapPath("~/Uploads/" + subPath + "/" + datePath);
            if (!Directory.Exists(directoryPath))//不存在这个文件夹就创建这个文件夹 
            {
               Directory.CreateDirectory(directoryPath);
            }

            // 保存文件
            b.Save(HttpContext.Current.Server.MapPath("~/Uploads/" + subPath + "/" + datePath + fileName));
            return datePath + fileName;
         }
         catch (Exception ex)
         {
            return null;
         }
      }

      /// <summary>
      /// 保存提交的图片
      /// </summary>
      /// <param name="uploadFile"></param>
      /// <param name="subPath"></param>
      /// <returns></returns>
      public static uploadResult UploadImage(HttpPostedFileBase uploadFile,string subPath)
      {
         string fileName = uploadFile.FileName;
         int fileSize = uploadFile.ContentLength;
         string fileExt = Path.GetExtension(fileName).ToLower();
         string message = "";
         var flag = false;
         if (!(fileExt == ".png" || fileExt == ".gif" || fileExt == ".jpg" || fileExt == ".jpeg"))
         {
            message = "图片类型只能为gif,png,jpg,jpeg";
         }
         else
         {
            if (fileSize > (2 * 1024 * 1024))
            {
               message = "图片大小不能超过2M";
            }
            else
            {
               // 日期子路径
               string datePath = DateTime.Now.ToString("yyyyMM") + "/";
               // 随机文件名
               Random ran = new Random();
               string RandKey = ran.Next(100, 999).ToString();
               var newFileName = TimeStamp().ToString() + RandKey + fileExt;

               try
               {
                  // 创建路径
                  string directoryPath = HttpContext.Current.Server.MapPath("~/Uploads/"+ subPath + "/" + datePath);
                  if (!Directory.Exists(directoryPath))//不存在这个文件夹就创建这个文件夹 
                  {
                     Directory.CreateDirectory(directoryPath);
                  }

                  // 保存文件
                  uploadFile.SaveAs(Path.Combine(directoryPath, newFileName));

                  // 检测是否为合法的图片文件，若不合法则删除
                  try
                  {
                     Image img = Image.FromFile(directoryPath+newFileName);
                     flag = true;
                     img.Dispose();
                     message = datePath + newFileName;
                  }
                  catch (Exception e)
                  {
                     // 删除掉非图片文件
                     string ori = directoryPath + newFileName;
                     if (File.Exists(ori))
                     {
                        File.Delete(ori);
                     }
                     message = "不是标准的图片文件！";
                  }
               }
               catch (Exception ex)
               {
                  message = ex.Message;
               }
            }
         }

         return new uploadResult() { flag = flag, msg = message };
      }

      /// <summary>
      /// 时间戳
      /// </summary>
      /// <returns></returns>
      public static long TimeStamp()
      {
         DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));

         System.Threading.Thread.Sleep(1);
         DateTime nowTime = DateTime.Now;

         long unixTime = (long)Math.Round((nowTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);

         return unixTime;
      }

      /// <summary>
      /// 通过时间戳和随机数生成随机的文件名
      /// </summary>
      /// <param name="file"></param>
      /// <returns></returns>
      public static string GetRandomFileName(HttpPostedFileBase file)
      {
         Random ran = new Random();
         string RandKey = ran.Next(100, 999).ToString();

         var extension = Path.GetExtension(Path.GetFileName(file.FileName));
         var fileName = TimeStamp().ToString() + RandKey + extension;

         return fileName;
      }

   }

   /// <summary>
   /// 图片上传返回对象
   /// </summary>
   public class uploadResult
   {
      public bool flag { get; set; }
      public string msg { get; set; }
   }
}