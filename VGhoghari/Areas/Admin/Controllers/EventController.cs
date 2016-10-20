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

namespace VGhoghari.Areas.Admin.Controllers {
  public class EventController : Controller {

    /// <summary>
    /// Api to Add new event category. Called from manage Category page or URL: /Admin/Event/Categories
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult Categories() {
      if(!Utility.isUserActive) {
        FormsAuthenticationUtils.SignOut();
        FormsAuthenticationUtils.RedirectToLoginPage();
      }
      return View();
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult ManageCategories(int? page) {
      if(!Utility.isUserActive) {
        FormsAuthenticationUtils.SignOut();
        FormsAuthenticationUtils.RedirectToLoginPage();
      }

      List<EventCategoryTO> categories = new List<EventCategoryTO>();
      int count = EventBL.CountCategories();

      if(count > 0) {
        categories = EventBL.GetCategories(page);
      }

      var pagedData = new StaticPagedList<EventCategoryTO>(categories, page ?? 1, EventBL.EVENT_CATEGORY_PAGE_SIZE, count);

      return View(pagedData);
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult DeleteCategory(string category, int? page) {
      if(!Utility.isUserActive) {
        FormsAuthenticationUtils.SignOut();
        FormsAuthenticationUtils.RedirectToLoginPage();
      }

      int response = EventBL.DeleteCategory(category);
      if(response != 0) {
        // redirect to interenal server error
      }
      return RedirectToAction("ManageCategories", "Event", new { page = page });
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult Save(string code) {
      return View();
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult Manage(string eventType, int? page) {

      if(!Utility.isUserActive) {
        FormsAuthenticationUtils.SignOut();
        FormsAuthenticationUtils.RedirectToLoginPage();
      }

      if(!string.IsNullOrWhiteSpace(eventType)) {
        eventType = eventType.ToLower();
      }

      List<EventTO> events = new List<EventTO>();
      int count = 0;
      int pageSize = 0;
      switch(eventType) {
        case "previous":
          ViewBag.EventType = "Previous";
          count = EventBL.CountPreviousEvents();
          pageSize = EventBL.PREVIOUS_EVENT_PAGE_SIZE;
          events = EventBL.GetPreviousEvents(page);
          break;
        case "ongoing":
          ViewBag.EventType = "Ongoing";
          count = EventBL.CountOngoingEvents();
          pageSize = EventBL.ONGOING_EVENT_PAGE_SIZE;
          events = EventBL.GetOngoingEvents(page);
          break;
        default:
          ViewBag.EventType = "Upcoming";
          count = EventBL.CountUpcomingEvents();
          pageSize = EventBL.UPCOMING_EVENT_PAGE_SIZE;
          events = EventBL.GetUpcomingEvents(page);
          break;
      }

      var pagedData = new StaticPagedList<EventTO>(events, page ?? 1, pageSize, count);

      return View(pagedData);
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult Delete(string code, int? page, string eventType) {
      if(!Utility.isUserActive) {
        FormsAuthenticationUtils.SignOut();
        FormsAuthenticationUtils.RedirectToLoginPage();
      }

      int rowsAffected = EventBL.DeleteEvent(code);
      if(rowsAffected != 1) {
        // Internal Server 
      }
      return RedirectToAction("Manage", "Event", new { page = page, eventType = eventType });
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult Details(string code, int? page, string eventType) {
      if(!Utility.isUserActive) {
        FormsAuthenticationUtils.SignOut();
        FormsAuthenticationUtils.RedirectToLoginPage();
      }

      EventTO eventDetails = EventBL.GetEventByCode(code);
      if(eventDetails == null) {
        return RedirectToAction("Manage", "Event", new { page = page, eventType = eventType });
      }

      return View(eventDetails);
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult CountEvents() {
      ViewBag.UpcomingEventCount = EventBL.CountUpcomingEvents();
      ViewBag.OngoingEventCount = EventBL.CountOngoingEvents();
      ViewBag.PreviousEventCount = EventBL.CountPreviousEvents();

      return PartialView("CountEvents");
    }

    [HttpGet]
    [Authorize(Roles = ("admin"))]
    public ActionResult UpcomingEventByCount(int count) {
      List<EventTO> events = EventBL.GetNearestUpcomingEvents(count);
      return PartialView(events);
    }

    [HttpGet]
    [Authorize(Roles = ("admin"))]
    public ActionResult ActiveEventCategories() {
      List<string> categories = EventBL.GetCategoryNames();
      return PartialView(categories);
    }
  }
}