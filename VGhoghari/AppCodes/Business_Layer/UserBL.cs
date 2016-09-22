using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using VGhoghari.AppCodes.Data_Layer;
using VGhoghari.AppCodes.Utilities;
using VGhoghari.Models;

namespace VGhoghari.AppCodes.Business_Layer {
  public class UserBL {

    public static bool isUserActive(string username) {
      return UserDL.isUserActive(username);
    }

    public static bool IsUserNameAvailable(string username) {
      return UserDL.IsUsernameAvailable(username.ToLower());
    }

    private static int ValidateRegistrationData(UserTO data) {
      if (string.IsNullOrWhiteSpace(data.Fullname)
        || data.Fullname.Length < 2
        || data.Fullname.Length > 500
        || !Regex.IsMatch(data.Fullname, @"^([A-Za-z]{1,})([ ]{0,1})([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})$")) {
        return -1;
      }

      if (string.IsNullOrWhiteSpace(data.Username)
        || data.Username.Length > 30
        || !Regex.IsMatch(data.Username, @"^(?!.*\.\.)(?!.*\.$)[^\W][\w.]{0,29}$")) {
        return -1;
      }
      else if (!IsUserNameAvailable(data.Username)) {
        return 1;
      }

      if (string.IsNullOrWhiteSpace(data.Password)
        || data.Password.Length < 8
        || data.Password.Length > 16
        || !Regex.IsMatch(data.Password, @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d!$%@#£€*?&]{8,16}$")) {
        return -1;
      }

      if (string.IsNullOrWhiteSpace(data.ConfirmPassword)
        || !string.Equals(data.ConfirmPassword, data.Password)) {
        return -1;
      }

      if (string.IsNullOrWhiteSpace(data.MobileNumber)
        || data.MobileNumber.Length < 4
        || data.MobileNumber.Length > 20
        || !Regex.IsMatch(data.MobileNumber, @"^(\+)?(\d){0,3}( )?\d{4,15}$")) {
        return -1;
      }

      if (!string.IsNullOrWhiteSpace(data.EmailId)) {
        if (data.EmailId.Length > 200
          || !Regex.IsMatch(data.EmailId, @"^([\w\.\-_]+)?\w+@[\w-_]+(\.\w+){1,}$")) {
          return -1;
        }
      }

      if (!string.IsNullOrWhiteSpace(data.Religion)) {
        if (data.Religion.Length > 200
          || !Regex.IsMatch(data.Religion, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      return 0;
    }

    public static int RegisterUser(UserTO data) {
      int validationResult = ValidateRegistrationData(data);

      if (validationResult == 0) {
        data.Code = Guid.NewGuid().ToString();
        data.AuthKey = Guid.NewGuid().ToString();

        data.Username = data.Username.ToLower();
        data.HashedPassword = Utility.GetMd5Hash(data.Password);

        int id = UserDL.RegisterUser(data);
        if (id <= 0) {
          return -2;
        }
      }

      return validationResult;
    }

    private static int ValidateLoginData(string username, string password) {
      if (string.IsNullOrWhiteSpace(username)
        || username.Length > 30
        || !Regex.IsMatch(username, @"^(?!.*\.\.)(?!.*\.$)[^\W][\w.]{0,29}$")) {
        return -1;
      }

      if (string.IsNullOrWhiteSpace(password)
        || password.Length < 8
        || password.Length > 16
        || !Regex.IsMatch(password, @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d!$%@#£€*?&]{8,16}$")) {
        return -1;
      }

      return 0;
    }

    public static KeyValuePair<int, string> LoginUser(string username, string password) {
      int validationResult = ValidateLoginData(username, password);

      if (validationResult == 0) {
        int userId = UserDL.ValidateLoginCredentials(username.ToLower(), Utility.GetMd5Hash(password));

        if (userId > 0) {
          string roles = UserDL.GetRolesByUserId(userId);
          return new KeyValuePair<int, string>(validationResult, roles);
        }

        /*-2 indicates that the credentials are not valid. hence not authenticate*/
        return new KeyValuePair<int, string>(-2, string.Empty);
      }

      /*if reaches here means validation result is -1 which means invalid data*/
      return new KeyValuePair<int, string>(validationResult, string.Empty);
    }

    public static UserTO GetUserProfileOfCurrentUser() {
      return UserDL.GetUserProfileByUsername(Utility.CurrentUser.ToLower());
    }

    private static int ValidateProfileData(UserTO data) {
      if (string.IsNullOrWhiteSpace(data.Fullname)
        || data.Fullname.Length < 2
        || data.Fullname.Length > 500
        || !Regex.IsMatch(data.Fullname, @"^([A-Za-z]{1,})([ ]{0,1})([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})$")) {
        return -1;
      }

      if (!string.IsNullOrWhiteSpace(data.Religion)) {
        if (data.Religion.Length > 200
          || !Regex.IsMatch(data.Religion, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      if (data.Dob.HasValue) {
        if (DateTime.Compare(data.Dob.Value.Date, DateTime.Today) > 0) {
          return -1;
        }
      }

      if (!string.IsNullOrWhiteSpace(data.LandlineNumber)) {
        if (data.LandlineNumber.Length < 4
          || data.LandlineNumber.Length > 20
          || !Regex.IsMatch(data.LandlineNumber, @"^(\+)?(\d){0,3}( )?(\d){0,3}( )?\d{4,11}$")) {
          return -1;
        }
      }

      if (string.IsNullOrWhiteSpace(data.MobileNumber)
        || data.MobileNumber.Length < 4
        || data.MobileNumber.Length > 20
        || !Regex.IsMatch(data.MobileNumber, @"^(\+)?(\d){0,3}( )?\d{4,15}$")) {
        return -1;
      }

      if (!string.IsNullOrWhiteSpace(data.EmailId)) {
        if (data.EmailId.Length > 200
          || !Regex.IsMatch(data.EmailId, @"^([\w\.\-_]+)?\w+@[\w-_]+(\.\w+){1,}$")) {
          return -1;
        }
      }

      if (!string.IsNullOrWhiteSpace(data.FacebookUrl)) {
        if (data.FacebookUrl.Length > 255
          || !Regex.IsMatch(data.FacebookUrl, @"^((http|https):\/\/|)(www\.|)facebook\.com\/[a-zA-Z0-9.]{1,}$")) {
          return -1;
        }
      }

      if (!string.IsNullOrWhiteSpace(data.Address)) {
        if (data.Address.Length > 1000
          || !Regex.IsMatch(data.Address, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      if (!string.IsNullOrWhiteSpace(data.Pincode)) {
        if (data.Pincode.Length != 6
          || !Regex.IsMatch(data.Pincode, @"^\d{6}$")) {
          return -1;
        }
      }

      if (!string.IsNullOrWhiteSpace(data.City)) {
        if (data.City.Length > 200
          || !Regex.IsMatch(data.City, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      if (!string.IsNullOrWhiteSpace(data.State)) {
        if (data.State.Length > 200
          || !Regex.IsMatch(data.State, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      if (!string.IsNullOrWhiteSpace(data.Country)) {
        if (data.Country.Length > 200
          || !Regex.IsMatch(data.Country, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      return 0;
    }
    
    public static int UpdateUserProfileByUserCode(UserTO data) {
      int validationResult = ValidateProfileData(data);

      if (validationResult == 0) {
        UserDL.UpdateUserProfileByUsercode(data);
        return validationResult;
      }

      return validationResult;
    }

    private static int ValidateProfileImage(MediaTO data) {
      List<string> mimeTypes = new List<string>()
      {
        "image/jpeg",
        "image/pjpeg",
        "image/gif",
        "image/png"
      };

      if (string.IsNullOrWhiteSpace(data.HumanisedFileName)) {
        return -1;
      }

      if (string.IsNullOrWhiteSpace(data.MimeType)
        || !mimeTypes.Contains(data.MimeType.ToLower())) {
        return -1;
      }

      if (data.Size <= 0) {
        return -1;
      }

      return 0;
    }

    public static KeyValuePair<int, string> AttachProfileImageToUser(string code, MediaTO data) {
      int validationResult = ValidateProfileImage(data);

      if (validationResult == 0) {
        string previousProfileImage = UserDL.GetProfileImageUsingUserCode(code);
        if (!string.IsNullOrWhiteSpace(previousProfileImage)) {
          string path = Path.Combine(HttpContext.Current.Server.MapPath("~/AppData/profile_image/"), previousProfileImage);
          if (File.Exists(path)) {
            File.Delete(path);
          }
        }
        string responseFileName =  UserDL.AttachProfileImageToUser(code, data.FileName);
        return new KeyValuePair<int, string>(validationResult, responseFileName);
      }
      return new KeyValuePair<int, string>(validationResult, string.Empty);
    }
  }
}