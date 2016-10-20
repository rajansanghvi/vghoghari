using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VGhoghari.Controllers {
  public class HomeController : Controller {
    [HttpGet]
    public ActionResult Index() {
      return View();
    }

    public ActionResult AboutUs() {
      return View();
    }

    public ActionResult ContactUs() {
      return View();
    }

    public ActionResult Donate() {
      return View();
    }

    public ActionResult Forms() {
      return View();
    }

    public ActionResult Gallery() {
      return View();
    }
  }
}
