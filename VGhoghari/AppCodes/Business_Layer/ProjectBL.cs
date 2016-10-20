using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using VGhoghari.AppCodes.Data_Layer;
using VGhoghari.Models;

namespace VGhoghari.AppCodes.Business_Layer {
  public class ProjectBL {

    public static int PROJECT_PAGE_SIZE = 9;

    private static int ValidateProjectDetails(string bannerImageData, ProjectTO data) {


      if(!string.IsNullOrWhiteSpace(data.Code)) {
        if(!ProjectDL.IsProjectPresent(data.Code)) {
          return -3;
        }
      }
      
      if(string.IsNullOrWhiteSpace(data.Title)
        || data.Title.Length > 500
        || !Regex.IsMatch(data.Title, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
        return -1;
      }

      if(string.IsNullOrWhiteSpace(data.ShortDescription)) {
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

    public static int SaveProject(string bannerImageData, ProjectTO data) {

      int validationResponse = ValidateProjectDetails(bannerImageData, data);

      if(validationResponse == 0) {

        if(!string.IsNullOrWhiteSpace(bannerImageData)) {
          string fileName = Guid.NewGuid().ToString();
          data.BannerImage = fileName + ".jpg";

          string imageData = bannerImageData.Split(',')[1];

          try {
            string directory = HttpContext.Current.Server.MapPath("~/AppData/projects/");
            if(!Directory.Exists(directory)) {
              Directory.CreateDirectory(directory);
            }

            string path = Path.Combine(directory, data.BannerImage);
            File.WriteAllBytes(path, Convert.FromBase64String(imageData));

            if(!string.IsNullOrWhiteSpace(data.Code)) {
              string oldFileName = ProjectDL.FetchBannerNameByCode(data.Code);
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
        
        int response = ProjectDL.SaveProjectDetails(data);
        if(response > 0) {
          return 0;
        }
        else {
          return -2;
        }
      }
      // if reached here data validation error or if event not present
      return validationResponse;
    }

    public static ProjectTO GetProjectByCode(string code) {
      return ProjectDL.FetchProjectByCode(code);
    }

    public static int CountProjects() {
      return ProjectDL.CountActiveProjects();
    }

    public static List<ProjectTO> GetAllProjects(int? pageNumber) {
      int offset = ((pageNumber ?? 1) - 1) * PROJECT_PAGE_SIZE;

      return ProjectDL.FetchProjects(offset, PROJECT_PAGE_SIZE);
    }
  }
}