using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VGhoghari.Areas.Admin.Controllers {
  public class HomeController : Controller {

    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult Index() {
      return View();
    }
  }
}