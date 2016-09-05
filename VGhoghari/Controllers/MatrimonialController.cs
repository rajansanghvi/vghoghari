using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VGhoghari.AppCodes.Utilities;

namespace VGhoghari.Controllers {
  public class MatrimonialController : Controller {

    [HttpGet]
    [Authorize(Roles = "user")]
    public ActionResult AddBasicInfo() {
      if (!Utility.isUserActive) {
        return RedirectToAction("Logout", "User");
      }
      return View();
    }
  }
}