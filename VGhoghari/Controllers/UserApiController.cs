using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using VGhoghari.AppCodes.Business_Layer;
using VGhoghari.AppCodes.Utilities;
using VGhoghari.Models;

namespace VGhoghari.Controllers {
  public class UserApiController : ApiController {

    [HttpGet]
    public bool IsUsernameAvailable(string username) {
      return UserBL.IsUserNameAvailable(username);
    }

    [HttpPost]
    public IHttpActionResult RegisterUser(UserTO data) {

      int response = UserBL.RegisterUser(data);
      if (response == -1) {
        return BadRequest();
      }
      else if (response == 1) {
        return Conflict();
      }
      else if (response == 0) {
        return ResponseMessage(Request.CreateResponse(HttpStatusCode.Created));
      }
      return InternalServerError();
    }

    [HttpPost]
    public IHttpActionResult LoginUser(dynamic data) {
      string username = data.Username;
      string password = data.Password;
      bool isPersistent = data.IsPersistent;
      
      KeyValuePair<int, string> response = UserBL.LoginUser(username, password);

      if (response.Key == -1) {
        return BadRequest();
      }
      else if (response.Key == -2) {
        return Conflict();
      }
      else if (response.Key == 0) {
        FormsAuthenticationUtils.RedirectFromLoginPage(username.ToLower(), response.Value, isPersistent, null);
        return Ok();
      }
      return InternalServerError();
    }

    [HttpGet]
    [Authorize(Roles = "user")]
    public UserTO GetUserProfile() {
      return UserBL.GetUserProfileOfCurrentUser();
    }

    [HttpPost]
    [Authorize(Roles = "user")]
    public IHttpActionResult UpdateUserProfile(UserTO data) {
      int response = UserBL.UpdateUserProfileByUserCode(data);

      if (response == -1) {
        return BadRequest();
      }
      else if (response == 0) {
        return Ok();
      }
      return InternalServerError();
    }

    [HttpPost]
    [Authorize(Roles = "user")]
    public IHttpActionResult UploadProfileImage(string code) {
      // Checking no of files injected in Request object  
      if (HttpContext.Current.Request.Files.Count > 0) {
        try {
          //  Get all files from Request object 
          string responseFileName = string.Empty;
          HttpFileCollection files = HttpContext.Current.Request.Files;
          for (int i = 0; i < files.Count; i++) {
            //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
            //string filename = Path.GetFileName(Request.Files[i].FileName);  

            HttpPostedFile file = files[i];
            string fname = string.Empty;
            string fileName = string.Empty;

            // Checking for Internet Explorer  
            if (HttpContext.Current.Request.Browser.Browser.ToUpper() == "IE" || HttpContext.Current.Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER") {
              string[] testfiles = file.FileName.Split(new char[] { '\\' });
              fname = testfiles[testfiles.Length - 1];
            }
            else {
              fname = file.FileName;
            }

            fileName = string.Format("{0}_{1}", Guid.NewGuid().ToString(), fname);

            MediaTO media = new MediaTO();
            media.FileName = fileName;
            media.HumanisedFileName = fname;
            media.MimeType = file.ContentType;
            media.Path = Path.Combine(HttpContext.Current.Server.MapPath("~/AppData/profile_image/"), media.FileName);
            media.Size = file.ContentLength;

            KeyValuePair<int, string> responseData = UserBL.AttachProfileImageToUser(code, media);
            if (responseData.Key == -1) {
              return BadRequest();
            }
            else if (responseData.Key == 0) {
              // Get the complete folder path and store the file inside it.  
              file.SaveAs(media.Path);
              return Ok(responseData.Value);
            }
          }
          return Ok(string.Empty);
        }
        catch (Exception) {
          return InternalServerError();
        }
      }
      else {
        return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent, string.Empty));
      }
    }
  }
}
