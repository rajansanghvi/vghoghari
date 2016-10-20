using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VGhoghari.AppCodes.Enum;

namespace VGhoghari.Models {
  public class UserTO {

    public UserTO() {
      this.Code = string.Empty;
      this.AuthKey = string.Empty;
      this.Username = string.Empty;
      this.Password = string.Empty;
      this.ConfirmPassword = string.Empty;
      this.HashedPassword = string.Empty;
      this.Fullname = string.Empty;
      this.Gender = enGender.Un_Specified;
      this.Dob = null;
      this.MobileNumber = string.Empty;
      this.EmailId = string.Empty;
      this.LandlineNumber = string.Empty;
      this.FacebookUrl = string.Empty;
      this.Religion = string.Empty;
      this.Address = string.Empty;
      this.Pincode = string.Empty;
      this.City = string.Empty;
      this.State = string.Empty;
      this.Country = string.Empty;
      this.ProfileImage = string.Empty;
      this.EffectiveFrom = DateTime.Now;
    }

    public string Code { get; set; }
    [JsonIgnore]
    public string AuthKey { get; set; }

    public string Username { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string HashedPassword { get; set; }

    public string Fullname { get; set; }
    public enGender Gender { get; set; }
    public DateTime? Dob { get; set; }
   

    public string MobileNumber { get; set; }
    public string EmailId { get; set; }
    public string LandlineNumber { get; set; }
    public string FacebookUrl { get; set; }
    
    public string Religion { get; set; }

    public string Address { get; set; }
    public string Pincode { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }

    public string ProfileImage { get; set; }

    public DateTime EffectiveFrom { get; set; }
  }
}