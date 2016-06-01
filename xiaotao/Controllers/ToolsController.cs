using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace xiaotao.Controllers
{
   public class ToolsController : Controller
   {
      public ActionResult Index()
      {
         return View();
      }

      /// <summary>
      /// 底部分页导航
      /// </summary>
      /// <param name="pageIndex"></param>
      /// <param name="pageCount"></param>
      /// <param name="link"></param>
      /// <returns></returns>
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
            if (pageIndex > 1 && pageIndex < pageCount)
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
      /// 迷你分页导航
      /// </summary>
      /// <param name="pageIndex"></param>
      /// <param name="PageCount"></param>
      /// <param name="link"></param>
      /// <returns></returns>
      public static string MiniPagerDesign(int pageIndex, int PageCount, string link)
      {
         StringBuilder sb = new StringBuilder();
         sb.Append("<div class='mini-pager'>");

         string PreAttr = pageIndex == 1 ? "class='pre disabled'" : "class='pre' href='" + link + (pageIndex - 1) + "'";
         // 上一页
         sb.Append("<a " + PreAttr + "><span class='glyphicon glyphicon-chevron-left'></span></a>");

         sb.Append("<span class='text'><b>" + pageIndex + "</b><em>/</em><i>" + PageCount + "</i></span>");

         string NextAttr = pageIndex == PageCount ? "class='next disabled'" : "class='next' href='" + link + (pageIndex + 1) + "'";
         // 下一页
         sb.Append("<a " + NextAttr + "><span class='glyphicon glyphicon-chevron-right'></span></a>");

         sb.Append("</div>");
         return sb.ToString();
      }
   }
}