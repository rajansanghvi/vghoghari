using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VGhoghari.AppCodes.Business_Layer;
using VGhoghari.AppCodes.Utilities;
using VGhoghari.Models;


namespace VGhoghari.Controllers {
  public class UserController : Controller {

    /// <summary>
    /// End point to show the User Registration Page 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ActionResult Register() {
      if (Utility.IsUserLoggedIn) {
        return RedirectToAction("Index", "Home");
      }
      return View();
    }

    [HttpGet]
    public ActionResult Login() {
      if (Utility.IsUserLoggedIn) {
        return RedirectToAction("Index", "Home");
      }
      return View();
    }

    [HttpGet]
    public void Logout() {
      FormsAuthenticationUtils.SignOut();
      FormsAuthenticationUtils.RedirectToLoginPage();
    }

    [HttpGet]
    [Authorize(Roles = "user")]
    public ActionResult MyProfile() {
      if (!Utility.IsUserLoggedIn) {
        FormsAuthenticationUtils.RedirectToLoginPage();
      }
      UserTO userTO = UserBL.GetUserProfileOfCurrentUser();
      return View(userTO);
    }

    [HttpGet]
    [Authorize(Roles = "user")]
    public ActionResult EditProfile() {
      return View();
    }
  }
}