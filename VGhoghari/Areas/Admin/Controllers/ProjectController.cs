using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VGhoghari.AppCodes.Utilities;
using VGhoghari.Models;
using PagedList;
using PagedList.Mvc;
using VGhoghari.AppCodes.Business_Layer;

namespace VGhoghari.Areas.Admin.Controllers {
  public class ProjectController : Controller {
    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult Save(string code) {
      return View();
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult Manage(int? page) {

      if(!Utility.isUserActive) {
        FormsAuthenticationUtils.SignOut();
        FormsAuthenticationUtils.RedirectToLoginPage();
      }
      
      List<ProjectTO> projects = new List<ProjectTO>();

      int count = ProjectBL.CountProjects();
      if(count > 0) {
        projects = ProjectBL.GetAllProjects(page);
      }

      var pagedData = new StaticPagedList<ProjectTO>(projects, page ?? 1, ProjectBL.PROJECT_PAGE_SIZE, count);

      return View(pagedData);
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult Details(string code, int? page) {
      if(!Utility.isUserActive) {
        FormsAuthenticationUtils.SignOut();
        FormsAuthenticationUtils.RedirectToLoginPage();
      }

      ProjectTO details = ProjectBL.GetProjectByCode(code);
      if(details == null) {
        return RedirectToAction("Manage", "Project", new { page = page });
      }

      return View(details);
    }
  }
}