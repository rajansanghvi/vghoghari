using System.Web.Http;
using System.Web.Mvc;

namespace VGhoghari.Areas.Admin {
  public class AdminAreaRegistration : AreaRegistration {
    public override string AreaName {
      get {
        return "Admin";
      }
    }

    public override void RegisterArea(AreaRegistrationContext context) {

      context.Routes.MapHttpRoute(
          name: "AdminEventApi",
          routeTemplate: "Admin/api/{controller}/{action}/{id}",
          defaults: new { id = RouteParameter.Optional }
      );

      context.MapRoute(
          "Admin_default",
          "Admin/{controller}/{action}/{id}",
          new { controller = "Home", action = "Index", id = UrlParameter.Optional }
      );
    }
  }
}