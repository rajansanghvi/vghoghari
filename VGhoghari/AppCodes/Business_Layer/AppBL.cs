using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VGhoghari.AppCodes.Data_Layer;

namespace VGhoghari.AppCodes.Business_Layer {
  public class AppBL {

    public static List<string> GetCountries() {
      return AppDL.GetAllCountries();
    }

    public static List<string> GetStates(string countryName) {
      return AppDL.GetStatesByCountry(countryName);
    }

    public static List<string> GetCities(string stateName) {
      return AppDL.GetCitiesByState(stateName);
    }
  }
}