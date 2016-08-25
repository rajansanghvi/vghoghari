using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Xml;

namespace VGhoghari.AppCodes.Utilities {
  public class FormsAuthenticationUtils {
    public static void RedirectFromLoginPage(string username, string commaSeperatedRoles, bool createPersistentCookie, string cookiePath) {
      SetAuthCookie(username, commaSeperatedRoles, createPersistentCookie, cookiePath);
      //string redirectURL = FormsAuthentication.GetRedirectUrl(username, createPersistentCookie);
      //HttpContext.Current.Response.Redirect(redirectURL);
    }

    public static void SetAuthCookie(string username, string commaSeperatedRoles, bool createPersistentCookie, string cookiePath) {
      FormsAuthenticationTicket ticket = CreateAuthenticationTicket(username, commaSeperatedRoles, createPersistentCookie, cookiePath);
      string encryptedTicket = FormsAuthentication.Encrypt(ticket);

      if (!FormsAuthentication.CookiesSupported) {
        //If the authentication ticket is specified not to use cookie, set it in the Uri
        FormsAuthentication.SetAuthCookie(encryptedTicket, createPersistentCookie);
      }
      else {
        HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

        if (ticket.IsPersistent) {
          authCookie.Expires = ticket.Expiration;
        }

        HttpContext.Current.Response.Cookies.Add(authCookie);
      }
    }

    private static FormsAuthenticationTicket CreateAuthenticationTicket(string username, string commaSeperatedRoles, bool createPersistentCookie, string cookiePath) {
      string path = cookiePath == null ? FormsAuthentication.FormsCookiePath : cookiePath;
      int expirationMinutes = GetCookieTimeoutValue();

      FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
          1,
          username,
          DateTime.Now,
          DateTime.Now.AddMinutes(expirationMinutes),
          createPersistentCookie,
          commaSeperatedRoles,
          path
        );

      return ticket;
    }

    private static int GetCookieTimeoutValue() {
      int timeout = 30; //default timeout is 30 minutes
      XmlDocument webConfig = new XmlDocument();
      webConfig.Load(HttpContext.Current.Server.MapPath(@"~\web.config"));
      XmlNode node = webConfig.SelectSingleNode("/configuration/system.web/authentication/forms");

      if (node != null && node.Attributes["timeout"] != null) {
        timeout = int.Parse(node.Attributes["timeout"].Value);
      }

      return timeout;
    }

    public static void AttachRolesToUser() {
      IPrincipal user = HttpContext.Current.User;

      if (user != null && user.Identity.IsAuthenticated && user.Identity is FormsIdentity) {
        FormsIdentity id = (FormsIdentity)user.Identity;
        FormsAuthenticationTicket ticket = (id.Ticket);

        if (!FormsAuthentication.CookiesSupported) {
          //If cookie is not supported for forms authentication, then the authentication ticket
          // is stored in the Url, which is encrypted. So, decrypt it
          ticket = FormsAuthentication.Decrypt(id.Ticket.Name);
        }

        // Get the stored user-data, in this case, user roles
        if (!string.IsNullOrEmpty(ticket.UserData)) {
          HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(id, ticket.UserData.Split(','));
        }
      }
    }

    public static void SignOut() {
      FormsAuthentication.SignOut();
    }

    public static void RedirectToLoginPage() {
      FormsAuthentication.RedirectToLoginPage();
    }
  }
}