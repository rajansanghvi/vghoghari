using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VGhoghari.AppCodes.Utilities;

namespace VGhoghari.AppCodes.Data_Layer {
  public class AppDL {

    public static List<string> GetAllCountries() {
      const string sql = @"select name
                          from app_countries
                          order by name;";

      GlobalDL dl = new GlobalDL();
      List<string> countries = new List<string>();

      using (MySqlDataReader dr = dl.ExecuteSqlReturnReader(Utility.ConnectionString, sql)) {
        while (dr.Read()) {
          countries.Add(dr.GetString("name"));
        }
      }
      return countries;
    }

    public static List<string> GetStatesByCountry(string countryName) {
      const string sql = @"select name
                            from
                            app_states
                            where
                            country_id = (select id from app_countries where name = ?countryName)
                            order by name;";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("countryName", countryName);
      List<string> states = new List<string>();

      using (MySqlDataReader dr = dl.ExecuteSqlReturnReader(Utility.ConnectionString, sql)) {
        while (dr.Read()) {
          states.Add(dr.GetString("name"));
        }
      }
      return states;
    }

    public static List<string> GetCitiesByState(string stateName) {
      const string sql = @"select name
                            from
                            app_cities
                            where
                            state_id = (select id from app_states where name = ?stateName)
                            order by name;";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("stateName", stateName);
      List<string> cities = new List<string>();

      using (MySqlDataReader dr = dl.ExecuteSqlReturnReader(Utility.ConnectionString, sql)) {
        while (dr.Read()) {
          cities.Add(dr.GetString("name"));
        }
      }
      return cities;
    }
  }
}