using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VGhoghari.Controllers {
  public class UserController : Controller {

    /// <summary>
    /// End point to show the User Registration Page 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ActionResult Register() {
      return View();
    }
  }
}