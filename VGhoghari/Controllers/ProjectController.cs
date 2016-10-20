using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VGhoghari.AppCodes.Business_Layer;
using VGhoghari.AppCodes.Utilities;
using VGhoghari.Models;
using PagedList;
using PagedList.Mvc;

namespace VGhoghari.Controllers {
  public class ProjectController : Controller {
    [HttpGet]
    public ActionResult List(int? page) {
      
      List<ProjectTO> projects = new List<ProjectTO>();

      int count = ProjectBL.CountProjects();
      if(count > 0) {
        projects = ProjectBL.GetAllProjects(page);
      }

      var pagedData = new StaticPagedList<ProjectTO>(projects, page ?? 1, ProjectBL.PROJECT_PAGE_SIZE, count);

      return View(pagedData);
    }

    [HttpGet]
    public ActionResult Details(string code, int? page) {
      
      ProjectTO details = ProjectBL.GetProjectByCode(code);
      if(details == null) {
        return RedirectToAction("List", "Project", new { page = page });
      }

      return View(details);
    }
  }
}