using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using VGhoghari.AppCodes.Enum;
using VGhoghari.AppCodes.Utilities;
using VGhoghari.Models;

namespace VGhoghari.AppCodes.Data_Layer {
  public class MatrimonialDL {

    public static bool IsMyBiodata(string code) {
      const string sql = @"select 1
                          from
                          app_biodata_basic_infos
                          where
                          code = ?code
                          and
                          active = true
                          and
                          user_id = (select id from app_users where username = ?username);";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("code", code);
      dl.AddParam("username", Utility.CurrentUser);

      return dl.ExecuteSqlReturnScalar<int>(Utility.ConnectionString, sql) == 1 ? true : false;
    }

    public static BiodataTO FetchBasicInfo(string code) {
      const string sql = @"select b.code as code
                          , b.fullname as fullname
                          , b.gender as gender
                          , b.dob as dob
                          , b.age as age
                          , ifnull(b.birth_time, '') as birth_time
                          , b.native as native
                          , b.marital_status as marital_status
                          , ifnull(b.birth_place, '') as birth_place
                          , ifnull(b.about_me, '') as about_me
                          , b.approval_status as approval_status
                          , ifnull(c.landline_number, '') as landline_number
                          , c.mobile_number as mobile_number
                          , ifnull(c.email_id, '') as email_id
                          , ifnull(c.facebook_url, '') as facebook_url
                          , c.address as address
                          , ifnull(c.pincode, '') as pincode
                          , ifnull(c.city, '') as city
                          , ifnull(c.state, '') as state
                          , c.country as country
                          , c.residential_status as address_type
                          from
                          app_biodata_basic_infos b
                          left join
                          app_biodata_contact_infos c
                          on b.id = c.biodata_id
                          where
                          b.code = ?code
                          and
                          b.active = true
                          and
                          b.user_id = (select id from app_users where username = ?username and active = true);";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("code", code);
      dl.AddParam("username", Utility.CurrentUser);

      using (MySqlDataReader dr = dl.ExecuteSqlReturnReader(Utility.ConnectionString, sql)) {
        if (dr.Read()) {
          return new BiodataTO() {
            Code = dr.GetString("code"),
            BasicInfo = new BasicInfoTO() {
              Gender = (enGender)dr.GetInt32("gender"),
              Fullname = dr.GetString("fullname"),
              Dob = dr.GetDateTime("dob"),
              StringBirthTime = dr.GetString("birth_time"),
              Age = dr.GetInt32("age"),
              MaritalStatus = (enMaritalStatus)dr.GetInt32("marital_status"),
              Native = dr.GetString("native"),
              BirthPlace = dr.GetString("birth_place"),
              AboutMe = dr.GetString("about_me")
            },
            ContactInfo = new ContactInfoTO() {
              MobileNumber = dr.GetString("mobile_number"),
              LandlineNumber = dr.GetString("landline_number"),
              EmailId = dr.GetString("email_id"),
              FacebookUrl = dr.GetString("facebook_url"),
              Address = dr.GetString("address"),
              AddressType = (enAddressType)dr.GetInt32("address_type"),
              Country = dr.GetString("country"),
              State = dr.GetString("state"),
              City = dr.GetString("city"),
              Pincode = dr.GetString("pincode")
            }
          };
        }
      }
      return null;
    }

    public static int SaveBasicInfo(BiodataTO data) {
      using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required)) {
        string procedureName = @"save_basic_biodata_info";

        GlobalDL dl = new GlobalDL();
        dl.AddParam("a_code", data.Code);
        dl.AddParam("a_fullname", data.BasicInfo.Fullname);
        dl.AddParam("a_gender", data.BasicInfo.Gender);
        dl.AddParam("a_dob", data.BasicInfo.Dob);
        dl.AddParam("a_age", data.BasicInfo.Age);
        dl.AddParam("a_birthtime", data.BasicInfo.BirthTime);
        dl.AddParam("a_native", data.BasicInfo.Native);
        dl.AddParam("a_marital_status", data.BasicInfo.MaritalStatus);
        dl.AddParam("a_birth_place", data.BasicInfo.BirthPlace);
        dl.AddParam("a_about_me", data.BasicInfo.AboutMe);
        dl.AddParam("a_username", Utility.CurrentUser);
        dl.AddParam("a_landline_no", data.ContactInfo.LandlineNumber);
        dl.AddParam("a_mobile_no", data.ContactInfo.MobileNumber);
        dl.AddParam("a_email_id", data.ContactInfo.EmailId);
        dl.AddParam("a_facebook_url", data.ContactInfo.FacebookUrl);
        dl.AddParam("a_address", data.ContactInfo.Address);
        dl.AddParam("a_pincode", data.ContactInfo.Pincode);
        dl.AddParam("a_city", data.ContactInfo.City);
        dl.AddParam("a_state", data.ContactInfo.State);
        dl.AddParam("a_country", data.ContactInfo.Country);
        dl.AddParam("a_address_type", data.ContactInfo.AddressType);

        int id = dl.ExecuteProcedureReturnScalar<int>(Utility.ConnectionString, procedureName);
        ts.Complete();
        return id;
      }
    }
  }
}