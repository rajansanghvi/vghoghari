using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using VGhoghari.AppCodes.Utilities;
using VGhoghari.Models;

namespace VGhoghari.AppCodes.Data_Layer {
  public class ProjectDL {
    public static bool IsProjectPresent(string code) {
      const string sql = @"select 1
                            from app_projects
                            where
                            code = ?code
                            and
                            active = true;";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("code", code);
      return dl.ExecuteSqlReturnScalar<int>(Utility.ConnectionString, sql) == 1 ? true : false;
    }

    public static int SaveProjectDetails(ProjectTO data) {
      using(TransactionScope ts = new TransactionScope(TransactionScopeOption.Required)) {
        const string procedureName = @"app_save_project";

        GlobalDL dl = new GlobalDL();
        dl.AddParam("a_code", data.Code);
        dl.AddParam("a_title", data.Title);
        dl.AddParam("a_short_description", data.ShortDescription);
        dl.AddParam("a_description", data.Description);
        dl.AddParam("a_contact_person", data.ContactPerson);
        dl.AddParam("a_contact_number", data.ContactNumber);
        dl.AddParam("a_contact_email", data.ContactEmail);
        dl.AddParam("a_banner_image", data.BannerImage);
        dl.AddParam("a_username", Utility.CurrentUser);

        int id = dl.ExecuteProcedureReturnScalar<int>(Utility.ConnectionString, procedureName);
        ts.Complete();
        return id;
      }
    }

    public static string FetchBannerNameByCode(string code) {
      const string sql = @"select banner_image
                          from app_projects
                          where code = ?code
                          and
                          active = true;";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("code", code);

      return dl.ExecuteSqlReturnScalar<string>(Utility.ConnectionString, sql);
    }

    public static ProjectTO FetchProjectByCode(string code) {
      const string sql = @"select ifnull(title, '') as title
                          , ifnull(short_description, '') as short_description
                          , ifnull(long_description, '') as long_description
                          , ifnull(contact_person, '') as contact_person
                          , ifnull(contact_number, '') as contact_number
                          , ifnull(contact_email, '') as contact_email
                          , ifnull(banner_image, '') as banner_image
                        from app_projects
                        where
                        code = ?code
                        and active = true;";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("code", code);

      using(MySqlDataReader dr = dl.ExecuteSqlReturnReader(Utility.ConnectionString, sql)) {
        if(dr.Read()) {
          return new ProjectTO() {
            Title = dr.GetString("title"),
            ShortDescription = dr.GetString("short_description"),
            Description = dr.GetString("long_description"),
            ContactPerson = dr.GetString("contact_person"),
            ContactNumber = dr.GetString("contact_number"),
            ContactEmail = dr.GetString("contact_email"),
            BannerImage = dr.GetString("banner_image")
          };
        }
      }
      return null;
    }

    public static int CountActiveProjects() {
      const string sql = @"select 
                          count(id)
                          from app_projects
                          where
                          active = true;";

      GlobalDL dl = new GlobalDL();

      return dl.ExecuteSqlReturnScalar<int>(Utility.ConnectionString, sql);
    }

    public static List<ProjectTO> FetchProjects(int? offset, int? limit) {
      const string sql = @"select
                             ifnull(code, '') as code    
                          ,  ifnull(title, '') as title
                          , ifnull(short_description, '') as short_description
                          , ifnull(long_description, '') as long_description
                          , ifnull(contact_person, '') as contact_person
                          , ifnull(contact_number, '') as contact_number
                          , ifnull(contact_email, '') as contact_email
                          , ifnull(banner_image, '') as banner_image
                        from app_projects
                        where
                        active = true
                        order by modified_at desc";

      GlobalDL dl = new GlobalDL();
      List<ProjectTO> projects = new List<ProjectTO>();

      string query = sql;
      if(limit != null) {
        query += @" limit ?limit offset ?offset";
        dl.AddParam("offset", offset ?? 0);
        dl.AddParam("limit", limit.Value);
      }

      query += @";";

      using(MySqlDataReader dr = dl.ExecuteSqlReturnReader(Utility.ConnectionString, query)) {
        while(dr.Read()) {
          projects.Add(new ProjectTO() {
            Code = dr.GetString("code"),
            Title = dr.GetString("title"),
            ShortDescription = dr.GetString("short_description"),
            Description = dr.GetString("long_description"),
            ContactPerson = dr.GetString("contact_person"),
            ContactNumber = dr.GetString("contact_number"),
            ContactEmail = dr.GetString("contact_email"),
            BannerImage = dr.GetString("banner_image")
          });
        }
      }
      return projects;
    }
  }
}