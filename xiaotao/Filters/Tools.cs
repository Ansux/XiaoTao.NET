using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace xiaotao.Filters
{
   public class Tools
   {
      public static string PagerDesign(int pageIndex, int pageCount, string link)
      {
         var pager = "";

         var preLink = pageIndex == 1 ? "#" : (link + (pageIndex - 1));
         var pre = "<li " + ((pageIndex == 1) ? "class=disabled" : "") + ">" +
                     "<a href=" + preLink + " aria-label='Previous'>" +
                        "<span aria-hidden='true'>&laquo;</span>" +
                     "</a>" +
                   "</li>";

         var nextLink = pageIndex == pageCount ? "#" : (link + (pageIndex + 1));
         var next = "<li " + ((pageIndex == pageCount) ? "class=disabled" : "") + ">" +
                     "<a href=" + nextLink + " aria-label='Next'>" +
                        "<span aria-hidden='true'>&raquo;</span>" +
                     "</a>" +
                    "</li>";

         var repeat = "";
         int start, end;

         if (pageCount <= 5)
         {
            start = 1;
            end = pageCount;
         }
         else
         {
            if (pageIndex >= 3 && (pageCount - pageIndex > 2))
            {
               start = pageIndex - 2;
               end = pageIndex + 2;
            }
            else if (pageCount - pageIndex <= 2)
            {
               start = pageCount - 4;
               end = pageCount;
            }
            else
            {
               start = 1;
               end = 5;
            }
         }

         for (int i = start; i <= end; i++)
         {
            repeat += "<li " + ((pageIndex == i) ? "class=active" : "") + ">" +
                        "<a href=" + link + i + ">" + i + "</a>" +
                      "</li>";
         }

         if (pageCount > 1)
         {
            if(pageIndex > 1 && pageIndex < pageCount)
            {
               pager = pre + repeat + next;
            }
            else if (pageIndex == 1)
            {
               pager = repeat + next;
            }
            else if (pageIndex == pageCount)
            {
               pager = pre + repeat;
            }
         }

         return pager;
      }

      /// <summary>
      /// 获取时间戳
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

         //if (Directory.Exists(spath))
         //{
         //}
         //else
         //{
         //   DirectoryInfo directoryInfo = new DirectoryInfo(spath);
         //   directoryInfo.Create();
         //}

         return fileName;
      }

      /// <summary>
      /// MD5加密
      /// </summary>
      /// <param name="pwd"></param>
      /// <returns></returns>
      public static string MD5(string pwd)
      {
         MD5 md5 = new MD5CryptoServiceProvider();
         byte[] palindata = Encoding.Default.GetBytes(pwd);//将要加密的字符串转换为字节数组
         byte[] encryptdata = md5.ComputeHash(palindata);//将字符串加密后也转换为字符数组
         return Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为加密字符串
      }
   }
}