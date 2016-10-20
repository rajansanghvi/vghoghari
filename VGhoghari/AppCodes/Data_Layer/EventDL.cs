using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using VGhoghari.AppCodes.Utilities;
using VGhoghari.Models;

namespace VGhoghari.AppCodes.Data_Layer {
  public class EventDL {

    public static bool CategoryExists(string name) {
      const string sql = @"select 1
                          from
                          app_event_categories
                          where lower(name) = ?name
                          and active = true;";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("name", name.ToLower());

      return dl.ExecuteSqlReturnScalar<int>(Utility.ConnectionString, sql) == 1 ? true : false;
    }
    
    public static int SaveCategory(string name, string description) {
      using(TransactionScope ts = new TransactionScope(TransactionScopeOption.Required)) {
        const string sql = @"insert into app_event_categories
                            (name, description, active, effective_from, effective_to, created_by, created_at, modified_by, modified_at)
                            values
                            (?name, ?description, 1, CURRENT_DATE(), null, ?username, now(), ?username, now());
                            select last_insert_id();";

        GlobalDL dl = new GlobalDL();
        dl.AddParam("name", name);
        dl.AddParam("description", description);
        dl.AddParam("username", Utility.CurrentUser);

        int id = dl.ExecuteSqlReturnScalar<int>(Utility.ConnectionString, sql);
        ts.Complete();
        return id;
      }
    }

    public static int CountCategories() {
      const string sql = @"select count(id)
                            from 
                            app_event_categories
                            where active = true;";

      GlobalDL dl = new GlobalDL();

      return dl.ExecuteSqlReturnScalar<int>(Utility.ConnectionString, sql);
    }

    /// <summary>
    /// Used to manage categories.
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public static List<EventCategoryTO> FetchCategories(int? offset, int? limit) {
      const string sql = @"select ifnull(name, '') as name
                          , ifnull(description, '') as description
                          from
                          app_event_categories
                          where active = true
                          order by id desc";

      GlobalDL dl = new GlobalDL();

      string query = sql;
      if(limit != null) {
        query += @" limit ?limit offset ?offset";
        dl.AddParam("offset", offset ?? 0);
        dl.AddParam("limit", limit.Value);
      }

      query += @";";
      
      List<EventCategoryTO> categories = new List<EventCategoryTO>();

      using(MySqlDataReader dr = dl.ExecuteSqlReturnReader(Utility.ConnectionString, query)) {
        while(dr.Read()) {
          categories.Add(new EventCategoryTO() {
            Name = dr.GetString("name"),
            Description = dr.GetString("description")
          });
        }
      }
      return categories;
    }

    public static int DeleteCategory(string name) {
      using(TransactionScope ts = new TransactionScope(TransactionScopeOption.Required)) {
        const string sql = @"update app_event_categories
                              set
                              active = false
                              where
                              lower(name) = ?name
                              and active = true; ";


        GlobalDL dl = new GlobalDL();
        dl.AddParam("name", name.ToLower());

        int rowsAffected = dl.ExecuteSqlNonQuery(Utility.ConnectionString, sql);
        ts.Complete();
        return rowsAffected;
      }
    }

    /// <summary>
    /// This method is used for filling dropdown in save event
    /// </summary>
    /// <returns></returns>
    public static List<string> FetchCategoryNames() {
      const string sql = @"select ifnull(name, '') as name
                          from app_event_categories
                          where active = true";

      GlobalDL dl = new GlobalDL();
      List<string> categories = new List<string>();
      using(MySqlDataReader dr = dl.ExecuteSqlReturnReader(Utility.ConnectionString, sql)) {
        while(dr.Read()) {
          categories.Add(dr.GetString("name"));
        }
      }
      return categories;
    }

    public static string FetchBannerNameByCode(string code) {
      const string sql = @"select banner_image
                          from app_events
                          where code = ?code
                          and
                          active = true;";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("code", code);

      return dl.ExecuteSqlReturnScalar<string>(Utility.ConnectionString, sql);
    }

    public static int SaveEventDetails(EventTO data) {
      using(TransactionScope ts = new TransactionScope(TransactionScopeOption.Required)) {
        const string procedureName = @"app_save_event";

        GlobalDL dl = new GlobalDL();
        dl.AddParam("a_code", data.Code);
        dl.AddParam("a_title", data.Title);
        dl.AddParam("a_short_description", data.ShortDescription);
        dl.AddParam("a_description", data.Description);
        dl.AddParam("a_start_time", data.StartDate);
        dl.AddParam("a_end_time", data.EndDate);
        dl.AddParam("a_cost_per_person", data.CostPerPerson);
        dl.AddParam("a_total_capacity", data.TotalCapacity);
        dl.AddParam("a_venue", data.Venue);
        dl.AddParam("a_city", data.City);
        dl.AddParam("a_state", data.State);
        dl.AddParam("a_country", data.Country);
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

    public static bool IsEventPresent(string code) {
      const string sql = @"select 1
                            from app_events
                            where
                            code = ?code
                            and
                            active = true;";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("code", code);
      return dl.ExecuteSqlReturnScalar<int>(Utility.ConnectionString, sql) == 1 ? true :  false;
    }

    public static int InActivateEventTagRelation(string code) {
      using(TransactionScope ts = new TransactionScope(TransactionScopeOption.Required)) {
        const string sql = @"update app_event_category_rel
                              set
                              active = false
                              , modified_by = ?username
                              , modified_at = now()
                              where event_id = (select id from app_events where code = ?code and active = true);";

        GlobalDL dl = new GlobalDL();
        dl.AddParam("code", code);
        dl.AddParam("username", Utility.CurrentUser);

        int rowsAffected = dl.ExecuteSqlNonQuery(Utility.ConnectionString, sql);
        ts.Complete();
        return rowsAffected;
      }
    }

    public static int AddCategoriesToEvent(int id, List<string> tags) {
      using(TransactionScope ts = new TransactionScope(TransactionScopeOption.Required)) {
        string prepareSqlStatement = string.Empty;
        GlobalDL dl = new GlobalDL();

        for(int i = 0; i < tags.Count; i++) {
          prepareSqlStatement += @"insert into app_event_category_rel
                                (event_id, category_id, active, effective_from, effective_to, created_by, created_at, modified_by, modified_at)
                                values
                                (?id, (select id FROM app_event_categories where lower(name) = ?categoryName" + i + @"), 1, CURRENT_DATE(), null, ?username, NOW(), ?username, now());";

          dl.AddParam("categoryName" + i, tags[i].ToLower());
        }

        dl.AddParam("id", id);
        dl.AddParam("username", Utility.CurrentUser);

        int noOfRowsAffected = dl.ExecuteSqlNonQuery(Utility.ConnectionString, prepareSqlStatement);
        ts.Complete();
        return noOfRowsAffected;
      }
    }

    public static EventTO FetchEventByCode(string code) {
      const string sql = @"select ifnull(title, '') as title
                          , ifnull(short_description, '') as short_description
                          , ifnull(long_description, '') as long_description
                          , ifnull(start_time, '') as start_time
                          , ifnull(end_time, '') as end_time
                          , ifnull(cost_per_person, '') as cost_per_person
                          , ifnull(capacity, '') as capacity
                          , ifnull(venue, '') as venue
                          , ifnull(city, '') as city
                          , ifnull(state, '') as state
                          , ifnull(country, '') as country
                          , ifnull(contact_person, '') as contact_person
                          , ifnull(contact_number, '') as contact_number
                          , ifnull(contact_email, '') as contact_email
                          , ifnull(banner_image, '') as banner_image
                          , ifnull(group_concat(ec.name), '') as tags
                        from app_events e
                        join
                        app_event_category_rel ecr
                        on 
                        e.id = ecr.event_id
                        join
                        app_event_categories ec
                        on
                        ecr.category_id = ec.id
                        where
                        code = ?code
                        and e.active = true
                        and ecr.active = true
                        and ec.active = true;";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("code", code);

      using(MySqlDataReader dr = dl.ExecuteSqlReturnReader(Utility.ConnectionString, sql)) {
        if(dr.Read()) {
          return new EventTO() {
            Title = dr.GetString("title"),
            ShortDescription = dr.GetString("short_description"),
            Description = dr.GetString("long_description"),
            FormattedStartDate = dr.GetString("start_time"),
            StartDate = dr.GetDateTime("start_time"),
            FormattedEndDate = dr.GetString("end_time"),
            EndDate = dr.GetDateTime("end_time"),
            CostPerPerson = dr.GetInt32("cost_per_person"),
            TotalCapacity = dr.GetInt32("capacity"),
            Venue = dr.GetString("venue"),
            City = dr.GetString("city"),
            State = dr.GetString("state"),
            Country = dr.GetString("country"),
            ContactPerson = dr.GetString("contact_person"),
            ContactNumber = dr.GetString("contact_number"),
            ContactEmail = dr.GetString("contact_email"),
            BannerImage = dr.GetString("banner_image"),
            Tags = dr.GetString("tags"),
            TagList = dr.GetString("tags").Split(',').ToList()
          };
        }
      }
      return null;
    }

    public static int CountUpcomingEvents() {
      const string sql = @"select 
                          count(id)
                          from app_events
                          where
                          start_time > now()
                          and
                          end_time > now()
                          and
                          active = true;";

      GlobalDL dl = new GlobalDL();

      return dl.ExecuteSqlReturnScalar<int>(Utility.ConnectionString, sql);
    }

    public static List<EventTO> FetchUpcomingEvents(int? offset, int? limit) {
      const string sql = @"select 
                          ifnull(code, '') as code
                          , ifnull(title, '') as title
                          , ifnull(short_description, '') as short_description
                          , ifnull(start_time, '') as start_time
                          , ifnull(end_time, '') as end_time
                          , ifnull(city, '') as city
                          , ifnull(state, '') as state
                          , ifnull(country, '') as country
                          , ifnull(banner_image, '') as banner_image
                          from app_events
                          where
                          start_time > now()
                          and
                          end_time > now()
                          and
                          active = true
                          order by start_time";

      GlobalDL dl = new GlobalDL();

      string query = sql;
      if(limit != null) {
        query += @" limit ?limit offset ?offset";
        dl.AddParam("offset", offset ?? 0);
        dl.AddParam("limit", limit.Value);
      }

      query += @";";

      List<EventTO> events = new List<EventTO>();

      using(MySqlDataReader dr = dl.ExecuteSqlReturnReader(Utility.ConnectionString, query)) {
        while(dr.Read()) {
          events.Add(new EventTO() {
            Title = dr.GetString("title"),
            Code = dr.GetString("code"),
            ShortDescription = dr.GetString("short_description"),
            FormattedStartDate = dr.GetString("start_time"),
            StartDate = dr.GetDateTime("start_time"),
            FormattedEndDate = dr.GetString("end_time"),
            EndDate = dr.GetDateTime("end_time"),
            City = dr.GetString("city"),
            State = dr.GetString("state"),
            Country = dr.GetString("country"),
            BannerImage = dr.GetString("banner_image")
          });
        }
      }
      return events;
    }

    public static int CountPreviousEvents() {
      const string sql = @"select 
                          count(id)
                          from app_events
                          where
                          start_time < now()
                          and
                          end_time < now()
                          and
                          active = true;";

      GlobalDL dl = new GlobalDL();

      return dl.ExecuteSqlReturnScalar<int>(Utility.ConnectionString, sql);
    }

    public static List<EventTO> FetchPreviousEvents(int? offset, int? limit) {
      const string sql = @"select 
                          ifnull(code, '') as code
                          , ifnull(title, '') as title
                          , ifnull(short_description, '') as short_description
                          , ifnull(start_time, '') as start_time
                          , ifnull(end_time, '') as end_time
                          , ifnull(city, '') as city
                          , ifnull(state, '') as state
                          , ifnull(country, '') as country
                          , ifnull(banner_image, '') as banner_image
                          from app_events
                          where
                          start_time < now()
                          and
                          end_time < now()
                          and
                          active = true
                          order by end_time desc";

      GlobalDL dl = new GlobalDL();

      string query = sql;
      if(limit != null) {
        query += @" limit ?limit offset ?offset";
        dl.AddParam("offset", offset ?? 0);
        dl.AddParam("limit", limit.Value);
      }

      query += @";";

      List<EventTO> events = new List<EventTO>();

      using(MySqlDataReader dr = dl.ExecuteSqlReturnReader(Utility.ConnectionString, query)) {
        while(dr.Read()) {
          events.Add(new EventTO() {
            Title = dr.GetString("title"),
            Code = dr.GetString("code"),
            ShortDescription = dr.GetString("short_description"),
            FormattedStartDate = dr.GetString("start_time"),
            StartDate = dr.GetDateTime("start_time"),
            FormattedEndDate = dr.GetString("end_time"),
            EndDate = dr.GetDateTime("end_time"),
            City = dr.GetString("city"),
            State = dr.GetString("state"),
            Country = dr.GetString("country"),
            BannerImage = dr.GetString("banner_image")
          });
        }
      }
      return events;
    }

    public static int CountOngoingEvents() {
      const string sql = @"select 
                          count(id)
                          from app_events
                          where
                          start_time <= now()
                          and
                          end_time >= now()
                          and
                          active = true;";

      GlobalDL dl = new GlobalDL();

      return dl.ExecuteSqlReturnScalar<int>(Utility.ConnectionString, sql);
    }

    public static List<EventTO> FetchOngoingEvents(int? offset, int? limit) {
      const string sql = @"select 
                          ifnull(code, '') as code
                          , ifnull(title, '') as title
                          , ifnull(short_description, '') as short_description
                          , ifnull(start_time, '') as start_time
                          , ifnull(end_time, '') as end_time
                          , ifnull(city, '') as city
                          , ifnull(state, '') as state
                          , ifnull(country, '') as country
                          , ifnull(banner_image, '') as banner_image
                          from app_events
                          where
                          start_time <= now()
                          and
                          end_time >= now()
                          and
                          active = true
                          order by end_time";

      GlobalDL dl = new GlobalDL();

      string query = sql;
      if(limit != null) {
        query += @" limit ?limit offset ?offset";
        dl.AddParam("offset", offset ?? 0);
        dl.AddParam("limit", limit.Value);
      }

      query += @";";

      List<EventTO> events = new List<EventTO>();

      using(MySqlDataReader dr = dl.ExecuteSqlReturnReader(Utility.ConnectionString, query)) {
        while(dr.Read()) {
          events.Add(new EventTO() {
            Title = dr.GetString("title"),
            Code = dr.GetString("code"),
            ShortDescription = dr.GetString("short_description"),
            FormattedStartDate = dr.GetString("start_time"),
            StartDate = dr.GetDateTime("start_time"),
            FormattedEndDate = dr.GetString("end_time"),
            EndDate = dr.GetDateTime("end_time"),
            City = dr.GetString("city"),
            State = dr.GetString("state"),
            Country = dr.GetString("country"),
            BannerImage = dr.GetString("banner_image")
          });
        }
      }
      return events;
    }

    public static int DeleteEvents(string code) {
      using(TransactionScope ts = new TransactionScope(TransactionScopeOption.Required)) {
        const string sql = @"update app_events
                            set
                            active = false
                            , modified_by = ?username
                            , modified_at = now()
                            where code = ?code";

        GlobalDL dl = new GlobalDL();
        dl.AddParam("code", code);
        dl.AddParam("username", Utility.CurrentUser);

        int rowsAffected = dl.ExecuteSqlNonQuery(Utility.ConnectionString, sql);
        ts.Complete();
        return rowsAffected;
      }
    }

    public static List<EventTO> FetchNearestActiveUpcomingEvents(int count) {
      const string sql = @"select ifnull(code, '') as code
                          , ifnull(title, '') as title
                          , ifnull(short_description, '') as short_description
                          , ifnull(start_time, '') as start_time
                          , ifnull(end_time, '') as end_time
                          , ifnull(venue, '') as venue
                          , ifnull(city, '') as city
                          , ifnull(state, '') as state
                          , ifnull(country, '') as country
                          from
                          app_events
                          where
                          start_time > NOW()
                          and
                          active = true
                          order by start_time
                          limit ?count;";

      List<EventTO> events = new List<EventTO>();

      GlobalDL dl = new GlobalDL();
      dl.AddParam("count", count);

      using(MySqlDataReader dr = dl.ExecuteSqlReturnReader(Utility.ConnectionString, sql)) {
        while(dr.Read()) {
          events.Add(new EventTO() {
            Code = dr.GetString("code"),
            Title = dr.GetString("title"),
            ShortDescription = dr.GetString("short_description"),
            StartDate = dr.GetDateTime("start_time"),
            EndDate = dr.GetDateTime("end_time"),
            FormattedStartDate = dr.GetString("start_time"),
            FormattedEndDate = dr.GetString("end_time"),
            Venue = dr.GetString("venue"),
            City = dr.GetString("city"),
            State = dr.GetString("state"),
            Country = dr.GetString("country")
          });
        }
        return events;
      }
    }
  }
}