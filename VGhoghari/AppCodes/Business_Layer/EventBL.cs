using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using VGhoghari.AppCodes.Data_Layer;
using VGhoghari.Models;

namespace VGhoghari.AppCodes.Business_Layer {
  public class EventBL {

    public static int EVENT_CATEGORY_PAGE_SIZE = 10;
    public static int UPCOMING_EVENT_PAGE_SIZE = 9;
    public static int PREVIOUS_EVENT_PAGE_SIZE = 9;
    public static int ONGOING_EVENT_PAGE_SIZE = 9;

    public static bool CategoryExists(string name) {
      return EventDL.CategoryExists(name);
    }

    private static int ValidateCategoryData(string name, string description) {

      if(string.IsNullOrWhiteSpace(name)
        || name.Length > 100
        || !Regex.IsMatch(name, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
        return -1;
      }

      if(!string.IsNullOrWhiteSpace(description)) {
        if(description.Length > 500
          || !Regex.IsMatch(description, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      return 0;
    }

    public static int SaveCategory(string name, string description) {
      int validationResult = ValidateCategoryData(name, description);

      if(validationResult == 0) {
        int id = EventDL.SaveCategory(name, description);

        if(id > 0) {
          return 0;
        }
        else {
          //if reached here some database error
          return -2;
        }
      }

      //validation result is -1 if reached here
      return validationResult;
    }

    public static int CountCategories() {
      return EventDL.CountCategories();
    }

    public static List<EventCategoryTO> GetCategories(int? pageNumber) {
      int offset = ((pageNumber ?? 1) - 1) * EVENT_CATEGORY_PAGE_SIZE;
      return EventDL.FetchCategories(offset, EVENT_CATEGORY_PAGE_SIZE);
    }

    public static int DeleteCategory(string name) {
      int rowsAffected = EventDL.DeleteCategory(name);

      if(rowsAffected == 1) {
        return 0;
      }
      // if rows affected is 0, no such category found and if more than one, than some error as category is unique
      return -1;
    }

    public static List<string> GetCategoryNames() {
      return EventDL.FetchCategoryNames();
    }

    private static int ValidateEventDetails(string bannerImageData, EventTO data) {


      if(!string.IsNullOrWhiteSpace(data.Code)) {
        if(!EventDL.IsEventPresent(data.Code)) {
          return -3;
        }
      }

      if(data.TagList.Count <= 0) {
        return -1;
      }

      if(string.IsNullOrWhiteSpace(data.Title)
        || data.Title.Length > 500
        || !Regex.IsMatch(data.Title, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
        return -1;
      }

      if(string.IsNullOrWhiteSpace(data.ShortDescription)) {
        return -1;
      }

      if(data.EndDate.HasValue) {
        if(DateTime.Compare(data.StartDate, data.EndDate.Value) > 0) {
          return -1;
        }
      }

      if(!string.IsNullOrWhiteSpace(data.Venue)) {
        if(data.Venue.Length > 1000
          || !Regex.IsMatch(data.Venue, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      if(string.IsNullOrWhiteSpace(data.Country)) {
        return -1;
      }

      if(!string.IsNullOrWhiteSpace(data.ContactPerson)) {
        if(data.ContactPerson.Length < 2
          || data.ContactPerson.Length > 500
          || !Regex.IsMatch(data.ContactPerson, @"^([A-Za-z]{1,})([ ]{0,1})([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})$")) {
          return -1;
        }
      }

      if(!string.IsNullOrWhiteSpace(data.ContactNumber)) {
        if(data.ContactNumber.Length < 4
          || data.ContactNumber.Length > 20
          || (!Regex.IsMatch(data.ContactNumber, @"^(\+)?(\d){0,3}( )?\d{4,15}$") && !Regex.IsMatch(data.ContactNumber, @"^(\+)?(\d){0,3}( )?(\d){0,3}( )?\d{4,11}$"))) {
          return -1;
        }
      }

      if(!string.IsNullOrWhiteSpace(data.ContactEmail)) {
        if(data.ContactEmail.Length > 200
          || !Regex.IsMatch(data.ContactEmail, @"^([\w\.\-_]+)?\w+@[\w-_]+(\.\w+){1,}$")) {
          return -1;
        }
      }

      if(string.IsNullOrWhiteSpace(data.Code)) {
        if(string.IsNullOrWhiteSpace(bannerImageData)) {
          return -1;
        }
      }

      return 0;
    }

    public static int SaveEvent(string bannerImageData, EventTO data) {

      int validationResponse = ValidateEventDetails(bannerImageData, data);

      if(validationResponse == 0) {

        if(!string.IsNullOrWhiteSpace(bannerImageData)) {
          string fileName = Guid.NewGuid().ToString();
          data.BannerImage = fileName + ".jpg";

          string imageData = bannerImageData.Split(',')[1];

          try {
            string directory = HttpContext.Current.Server.MapPath("~/AppData/events/");
            if(!Directory.Exists(directory)) {
              Directory.CreateDirectory(directory);
            }

            string path = Path.Combine(directory, data.BannerImage);
            File.WriteAllBytes(path, Convert.FromBase64String(imageData));

            if(!string.IsNullOrWhiteSpace(data.Code)) {
              string oldFileName = EventDL.FetchBannerNameByCode(data.Code);
              if(!string.IsNullOrWhiteSpace(oldFileName)) {
                string oldFilePath = Path.Combine(directory, oldFileName);
                if(File.Exists(oldFilePath)) {
                  File.Delete(oldFilePath);
                }
              }
            }
          }
          catch(Exception e) {
            return -2;
          }
        }

        if(string.IsNullOrWhiteSpace(data.Code)) {
          data.Code = Guid.NewGuid().ToString();
        }
        else {
          int rowsAffected = EventDL.InActivateEventTagRelation(data.Code);
          if(rowsAffected <= 0) {
            return -2;
          }
        }

        int response = EventDL.SaveEventDetails(data);
        if(response > 0) {
          int rowsAffected = EventDL.AddCategoriesToEvent(response, data.TagList);
          if(rowsAffected != data.TagList.Count) {
            return -2;
          }
          return 0;
        }
        else {
          return -2;
        }
      }
      // if reached here data validation error or if event not present
      return validationResponse;
    }

    public static EventTO GetEventByCode(string code) {
      return EventDL.FetchEventByCode(code);
    }

    public static int CountUpcomingEvents() {
      return EventDL.CountUpcomingEvents();
    }

    public static List<EventTO> GetUpcomingEvents(int? pageNumber) {
      int offset = ((pageNumber ?? 1) - 1) * UPCOMING_EVENT_PAGE_SIZE;
      return EventDL.FetchUpcomingEvents(offset, UPCOMING_EVENT_PAGE_SIZE);
    }

    public static int CountPreviousEvents() {
      return EventDL.CountPreviousEvents();
    }

    public static List<EventTO> GetPreviousEvents(int? pageNumber) {
      int offset = ((pageNumber ?? 1) - 1) * PREVIOUS_EVENT_PAGE_SIZE;
      return EventDL.FetchPreviousEvents(offset, PREVIOUS_EVENT_PAGE_SIZE);
    }

    public static int CountOngoingEvents() {
      return EventDL.CountOngoingEvents();
    }

    public static List<EventTO> GetOngoingEvents(int? pageNumber) {
      int offset = ((pageNumber ?? 1) - 1) * ONGOING_EVENT_PAGE_SIZE;
      return EventDL.FetchOngoingEvents(offset, ONGOING_EVENT_PAGE_SIZE);
    }

    public static int DeleteEvent(string code) {
      return EventDL.DeleteEvents(code);
    }

    public static List<EventTO> GetNearestUpcomingEvents(int count) {
      return EventDL.FetchNearestActiveUpcomingEvents(count);
    }
  }
}