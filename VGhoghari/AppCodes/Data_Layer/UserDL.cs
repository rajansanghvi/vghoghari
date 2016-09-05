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
  public class UserDL {

    public static bool isUserActive(string username) {
      const string sql = @"select 1
                          from app_users
                          where
                          username = ?username
                          and
                          active = true;";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("username", username);

      return dl.ExecuteSqlReturnScalar<int>(Utility.ConnectionString, sql) == 1 ? true : false;
    }

    public static bool IsUsernameAvailable(string username) {
      const string sql = @"select 1
                          from app_users
                          where
                          username = ?username;";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("username", username);

      /* 
       * Query returns 1 if username exists in DB. If 1 then username is not available hence return false else return true
       */
      return dl.ExecuteSqlReturnScalar<int>(Utility.ConnectionString, sql) == 1 ? false : true;
    }

    public static int RegisterUser(UserTO data) {
      const string procedureName = @"app_register_user";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("code", data.Code);
      dl.AddParam("authKey", data.AuthKey);
      dl.AddParam("username", data.Username);
      dl.AddParam("hashedPassword", data.HashedPassword);
      dl.AddParam("fullname", data.Fullname);
      dl.AddParam("mobileNo", data.MobileNumber);
      dl.AddParam("emailId", data.EmailId);
      dl.AddParam("religion", data.Religion);

      int id = dl.ExecuteProcedureReturnScalar<int>(Utility.ConnectionString, procedureName);
      return id;
    }

    public static int ValidateLoginCredentials(string username, string hashedPassword) {
      const string sql = @"select id
                          from app_users
                          where
                          username = ?username
                          and
                          hashed_password = ?hashedPassword
                          and 
                          active = true;";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("username", username);
      dl.AddParam("hashedPassword", hashedPassword);

      return dl.ExecuteSqlReturnScalar<int>(Utility.ConnectionString, sql);
    }

    public static string GetRolesByUserId(int id) {
      const string sql = @"select IFNULL(group_concat(name), '') as roles
                          from
                          app_roles r
                          left join
                          app_user_role_rel urr
                          on r.id = urr.role_id
                          where
                          urr.user_id = ?userId
                          and
                          urr.active = true;";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("userId", id);

      return dl.ExecuteSqlReturnScalar<string>(Utility.ConnectionString, sql);
    }

    public static UserTO GetUserProfileByUsername(string username) {
      const string sql = @"select u.code as code
                          , u.username as username
                          , u.fullname as fullname
                          , ifnull(u.gender, 0) as gender
                          , ifnull(u.dob, '1870-01-01') as dob
                          , ifnull(u.religion, '') as religion
                          , ifnull(u.profile_image, '') as profile_image
                          , ifnull(ua.landline_number, '') as landline_number
                          , ifnull(ua.mobile_number, '') as mobile_number
                          , ifnull(ua.email_id, '') as email_id
                          , ifnull(ua.facebook_url, '') as facebook_url
                          , ifnull(ua.address, '') as address
                          , ifnull(ua.pincode, '') as pincode
                          , ifnull(ua.city, '') as city
                          , ifnull(ua.state, '') as state
                          , ifnull(ua.country, '') as country
                          , effective_from as effective_from
                          from
                          app_users u
                          left join
                          app_users_addl ua
                          on 
                          u.id = ua.user_id
                          where u.username = ?username
                          and
                          u.active = true;";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("username", username);

      using (MySqlDataReader dr = dl.ExecuteSqlReturnReader(Utility.ConnectionString, sql)) {
        if (dr.Read()) {
          return new UserTO() {
            Code = dr.GetString("code"),
            Username = dr.GetString("username"),
            Fullname = dr.GetString("fullname"),
            Gender = (enGender)dr.GetInt32("gender"),
            Dob = dr.GetDateTime("dob"),
            Religion = dr.GetString("religion"),
            ProfileImage = dr.GetString("profile_image"),
            LandlineNumber = dr.GetString("landline_number"),
            MobileNumber = dr.GetString("mobile_number"),
            EmailId = dr.GetString("email_id"),
            FacebookUrl = dr.GetString("facebook_url"),
            Address = dr.GetString("address"),
            Pincode = dr.GetString("pincode"),
            City = dr.GetString("city"),
            State = dr.GetString("state"),
            Country = dr.GetString("country"),
            EffectiveFrom = dr.GetDateTime("effective_from")
          };
        }
      }
      return null;
    }

    public static void UpdateUserProfileByUsercode(UserTO data) {
      using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required)) {
        const string procedureName = @"update_user_profile";

        GlobalDL dl = new GlobalDL();
        dl.AddParam("a_userCode", data.Code);
        dl.AddParam("a_fullname", data.Fullname);
        dl.AddParam("a_religion", data.Religion);
        dl.AddParam("a_dob", data.Dob);
        dl.AddParam("a_gender", data.Gender);
        dl.AddParam("a_landlineNo", data.LandlineNumber);
        dl.AddParam("a_mobileNo", data.MobileNumber);
        dl.AddParam("a_emailId", data.EmailId);
        dl.AddParam("a_facebookUrl", data.FacebookUrl);
        dl.AddParam("a_address", data.Address);
        dl.AddParam("a_pincode", data.Pincode);
        dl.AddParam("a_city", data.City);
        dl.AddParam("a_state", data.State);
        dl.AddParam("a_country", data.Country);
        dl.AddParam("a_username", Utility.CurrentUser);

        dl.ExecuteProcedureNonQuery(Utility.ConnectionString, procedureName);
        ts.Complete();
      }
    }

    public static string AttachProfileImageToUser(string code, string fileName) {
      using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required)) {
        const string sql = @"update app_users
                            set profile_image = ?profileImage
                            where code = ?userCode;

                            select profile_image
                            from app_users
                            where code = ?userCode;";

        GlobalDL dl = new GlobalDL();
        dl.AddParam("userCode", code);
        dl.AddParam("profileImage", fileName);

        string responseFileName = dl.ExecuteSqlReturnScalar<string>(Utility.ConnectionString, sql);
        ts.Complete();
        return responseFileName;
      }
    }

    public static string GetProfileImageUsingUserCode(string code) {
      const string sql = @"select ifnull(profile_image, '') as profile_image
                          from app_users
                          where
                          code = ?userCode
                          and
                          active = true;";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("userCode", code);

      return dl.ExecuteSqlReturnScalar<string>(Utility.ConnectionString, sql);
    }
  }
}