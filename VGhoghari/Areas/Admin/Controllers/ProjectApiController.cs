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
  public class ProjectApiController : ApiController {
    [HttpPost]
    [Authorize(Roles = "admin")]
    public IHttpActionResult SaveProject(dynamic data) {
      if(!Utility.isUserActive) {
        return Unauthorized();
      }

      string code = data.Code;
      
      string title = data.Title;
      string shortDescription = data.ShortDescription;
      string description = data.Description;
      
      string contactPerson = data.ContactPerson;
      string contactNumber = data.ContactNumber;
      string contactEmail = data.ContactEmail;
      string bannerImageData = data.BannnerImageData;

      ProjectTO projectDetails = new ProjectTO();
      
      projectDetails.Code = code;
      projectDetails.ContactEmail = contactEmail;
      projectDetails.ContactNumber = contactNumber;
      projectDetails.ContactPerson = contactPerson;
     
     
      projectDetails.Description = description;
     
      projectDetails.ShortDescription = shortDescription;
     
      projectDetails.Title = title;
     

      int response = ProjectBL.SaveProject(bannerImageData, projectDetails);

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
    public ProjectTO GetProjectDetails(string code) {
      return ProjectBL.GetProjectByCode(code);
    }
  }
}
