using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VGhoghari.AppCodes.Business_Layer;
using VGhoghari.AppCodes.Utilities;
using VGhoghari.Models;

namespace VGhoghari.Areas.Admin.Controllers {
  public class EventApiController : ApiController {

    [HttpGet]
    [Authorize(Roles = "admin")]
    public bool IsCategoryPresent(string category) {
      return EventBL.CategoryExists(category);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public IHttpActionResult SaveCategory(dynamic data) {
      if(!Utility.isUserActive) {
        return Unauthorized();
      }

      string name = data.Name;
      string description = data.Description;

      int response = EventBL.SaveCategory(name, description);

      if(response == -1) {
        return BadRequest();
      }
      else if (response == 0) {
        return ResponseMessage(Request.CreateResponse(HttpStatusCode.Created));
      }

      return InternalServerError();
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public List<String> GetEventCategories() {
      return EventBL.GetCategoryNames();
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public IHttpActionResult SaveEvent(dynamic data) {
      if(!Utility.isUserActive) {
        return Unauthorized();
      }

      string code = data.Code;
      string[] tags = data.Tags.ToObject<string[]>();
      string title = data.Title;
      string shortDescription = data.ShortDescription;
      string description = data.Description;
      string startDate = data.StartTime;
      string endDate = data.EndTime;
      int costPerPerson = data.CostPerPerson;
      int totalCapacity = data.TotalCapacity;
      string venue = data.Venue;
      string country = data.Country;
      string state = data.State;
      string city = data.City;
      string contactPerson = data.ContactPerson;
      string contactNumber = data.ContactNumber;
      string contactEmail = data.ContactEmail;
      string bannerImageData = data.BannnerImageData;

      EventTO eventDetails = new EventTO();

      DateTime parsedDate;
      if(!string.IsNullOrWhiteSpace(endDate)) {
        if(DateTime.TryParse(endDate, out parsedDate)) {
          eventDetails.EndDate = DateTime.Parse(endDate);
        }
        else {
          return BadRequest();
        }
      }

      if(DateTime.TryParse(startDate, out parsedDate)) {
        eventDetails.StartDate = DateTime.Parse(startDate);
      }
      else {
        return BadRequest();
      }

      eventDetails.Code = code;
      eventDetails.City = city;
      eventDetails.ContactEmail = contactEmail;
      eventDetails.ContactNumber = contactNumber;
      eventDetails.ContactPerson = contactPerson;
      eventDetails.CostPerPerson = costPerPerson;
      eventDetails.Country = country;
      eventDetails.Description = description;
      eventDetails.FormattedEndDate = endDate;
      eventDetails.FormattedStartDate = startDate;
      eventDetails.ShortDescription = shortDescription;
      eventDetails.State = state;
      eventDetails.TagList = tags.ToList();
      eventDetails.Tags = string.Join(",", tags.ToList());
      eventDetails.Title = title;
      eventDetails.TotalCapacity = totalCapacity;
      eventDetails.Venue = venue;

      int response = EventBL.SaveEvent(bannerImageData, eventDetails);

      if(response == -1) {
        return BadRequest();
      }
      else if(response == 0) {
        return Ok();
      }
      else if(response == -3) {
        return NotFound();
      }

      return InternalServerError();
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public EventTO GetEventDetails(string code) {
      return EventBL.GetEventByCode(code);
    }
  }
}
