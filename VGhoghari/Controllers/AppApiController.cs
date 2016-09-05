using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VGhoghari.AppCodes.Business_Layer;

namespace VGhoghari.Controllers {
  public class AppApiController : ApiController {

    [HttpGet]
    [Authorize(Roles = "user")]
    public List<string> GetCountries() {
      return AppBL.GetCountries();
    }

    [HttpGet]
    [Authorize(Roles = "user")]
    public List<string> GetStates(string countryName) {
      return AppBL.GetStates(countryName);
    }

    [HttpGet]
    [Authorize(Roles = "user")]
    public List<string> GetCities(string stateName) {
      return AppBL.GetCities(stateName);
    }
  }
}
