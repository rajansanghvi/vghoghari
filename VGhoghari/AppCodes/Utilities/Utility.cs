using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using VGhoghari.AppCodes.Business_Layer;

namespace VGhoghari.AppCodes.Utilities {
  public static class Utility {
    public const string DB_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

    public static string GetDateForSqlParam(DateTime dt) {
      return dt.ToString(DB_DATE_FORMAT, CultureInfo.InstalledUICulture);
    }

    public static string AddWildCard(string input) {
      if (string.IsNullOrWhiteSpace(input)) {
        return string.Empty;
      }
      const string WC = "%{0}%";
      return string.Format(WC, input);
    }

    public static string ConnectionString {
      get {
        return ConfigurationManager.ConnectionStrings["VGhoghariDbContext"].ConnectionString;
      }
    }

    internal static string GetMd5Hash(string input) {
      using (System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create()) {
        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++) {
          sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
      }
    }

    public static string CurrentUser {
      get {
        try {
          return Thread.CurrentPrincipal.Identity.Name;
        }
        catch (Exception) {
          return string.Empty;
        }
      }
    }

    public static bool IsUserLoggedIn {
      get {
        try {
          return Thread.CurrentPrincipal.Identity.IsAuthenticated;
        }
        catch (Exception) {
          return false;
        }
      }
    }

    public static bool isUserActive {
      get {
        try {
          return UserBL.isUserActive(CurrentUser);
        }
        catch (Exception) {
          return false;
        }
      }
    }

    public static bool isAdminUser {
      get {
        try {
          return UserBL.IsAdminUser(Utility.CurrentUser);
        }
        catch (Exception) {
          return false;
        }
      }
    }
  }
}