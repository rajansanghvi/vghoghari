using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VGhoghari.AppCodes.Business_Layer;
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

    [HttpGet]
    [Authorize(Roles = "user")]
    public ActionResult AddPersonalInfo(string code) {
      if (!Utility.isUserActive) {
        return RedirectToAction("Logout", "User");
      }

      if (string.IsNullOrWhiteSpace(code)
        || !MatrimonialBL.IsMyBiodata(code)) {
        return RedirectToAction("Manage", "Matrimonial");
      }
      return View();
    }
  }
}