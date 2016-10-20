using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VGhoghari.AppCodes.Business_Layer;
using VGhoghari.Models;
using PagedList;
using PagedList.Mvc;

namespace VGhoghari.Controllers {
  public class EventController : Controller {
    [HttpGet]
    public ActionResult List(string eventType, int? page) {
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
    public ActionResult Details(string code, int? page, string eventType) {
      EventTO eventDetail = EventBL.GetEventByCode(code);

      if(eventDetail == null) {
        return RedirectToAction("List", "Event", new { page = page, eventType = eventType });
      }

      return View(eventDetail);
    }

    [HttpGet]
    public ActionResult UpcomingEventByCount(int count) {
      List<EventTO> events = EventBL.GetNearestUpcomingEvents(count);
      return PartialView(events);
    }

    [HttpGet]
    public ActionResult ActiveEventCategories() {
      List<string> categories = EventBL.GetCategoryNames();
      return PartialView(categories);
    }

    [HttpGet]
    public ActionResult TopUpcomingEvent(int count) {
      List<EventTO> events = EventBL.GetNearestUpcomingEvents(count);
      return PartialView(events);
    }

    [HttpGet]
    public ActionResult TopThreeUpcomingEvent(int count) {
      List<EventTO> events = EventBL.GetNearestUpcomingEvents(count);
      return PartialView(events);
    }
  }
}