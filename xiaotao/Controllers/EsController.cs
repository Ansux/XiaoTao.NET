using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace xiaotao.Controllers
{
  public class EsController : Controller
  {
    // GET: Web
    public ActionResult Index()
    {
      return Redirect("es/goodslist");
    }
  }
}