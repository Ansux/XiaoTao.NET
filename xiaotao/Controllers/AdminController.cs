using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace xiaotao.Controllers
{
  public class AdminController : Controller
  {
    // GET: Admin
    public ActionResult Index()
    {
      return Redirect("admin/xt_student");
    }
  }
}