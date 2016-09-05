using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VGhoghari.AppCodes.Enum;

namespace VGhoghari.Models {
  public class BiodataTO {

    public BiodataTO() {
      this.Id = 0;
      this.Code = string.Empty;
      this.BasicInfo = null;
      this.ContactInfo = null;
    }

    [JsonIgnore]
    public int Id { get; set; }
    public string Code { get; set; }

    public BasicInfoTO BasicInfo { get; set; }
    public ContactInfoTO ContactInfo { get; set; }
  }

  public class BasicInfoTO {

    public BasicInfoTO() {
      this.Gender = enGender.UnSpecified;
      this.Fullname = string.Empty;
      this.Dob = new DateTime(1870, 01, 01);
      this.BirthTime = null;
      this.StringBirthTime = string.Empty;
      this.Age = 0;
      this.MaritalStatus = enMaritalStatus.UnSpecified;
      this.Native = string.Empty;
      this.BirthPlace = string.Empty;
      this.AboutMe = string.Empty;
    }

    public enGender Gender { get; set; }
    public string Fullname { get; set; }
    public DateTime Dob { get; set; }
    public DateTime? BirthTime { get; set; }
    public string StringBirthTime { get; set; }
    public int Age { get; set; }
    public enMaritalStatus MaritalStatus { get; set; }
    public string Native { get; set; }
    public string BirthPlace { get; set; }
    public string AboutMe { get; set; }
  }

  public class ContactInfoTO {

    public ContactInfoTO() {
      this.MobileNumber = string.Empty;
      this.LandlineNumber = string.Empty;
      this.EmailId = string.Empty;
      this.FacebookUrl = string.Empty;
      this.Address = string.Empty;
      this.AddressType = enAddressType.UnSpecified;
      this.Country = string.Empty;
      this.State = string.Empty;
      this.City = string.Empty;
      this.Pincode = string.Empty;
    }

    public string MobileNumber { get; set; }
    public string LandlineNumber { get; set; }
    public string EmailId { get; set; }
    public string FacebookUrl { get; set; }
    public string Address { get; set; }
    public enAddressType AddressType { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public string Pincode { get; set; }
  }
}