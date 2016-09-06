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

      this.ReligionInfo = null;
      this.SocialInfo = null;
      this.PhysicalInfo = null;
    }

    [JsonIgnore]
    public int Id { get; set; }
    public string Code { get; set; }

    public BasicInfoTO BasicInfo { get; set; }
    public ContactInfoTO ContactInfo { get; set; }

    public ReligionInfoTO ReligionInfo { get; set; }
    public SocialInfoTO SocialInfo { get; set; }
    public PhysicalInfoTO PhysicalInfo { get; set; }
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

  public class ReligionInfoTO {

    public ReligionInfoTO() {
      this.Religion = string.Empty;
      this.Caste = string.Empty;
      this.SubCaste = string.Empty;
    }

    public string Religion { get; set; }
    public string Caste { get; set; }
    public string SubCaste { get; set; }
  }

  public class SocialInfoTO {

    public SocialInfoTO() {
      this.Manglik = enBoolean.UnSpecified;
      this.SelfGothra = string.Empty;
      this.MaternalGothra = string.Empty;
      this.StarSign = enStarSign.UnSpecified;
    }

    public enBoolean Manglik { get; set; }
    public string SelfGothra { get; set; }
    public string MaternalGothra { get; set; }
    public enStarSign StarSign { get; set; }
  }

  public class PhysicalInfoTO {

    public PhysicalInfoTO() {
      this.HeightFt = 0;
      this.HeightInch = -1;
      this.Weight = 0;
      this.BodyType = enBodyType.UnSpecified;
      this.BloodGroup = string.Empty;
      this.Complexion = string.Empty;
      this.Diet = string.Empty;
      this.Smoke = string.Empty;
      this.Drink = string.Empty;
      this.Optics = enBoolean.UnSpecified;
      this.Deformity = string.Empty;
    }

    public int HeightFt { get; set; }
    public int HeightInch { get; set; }
    public int Weight { get; set; }
    public enBodyType BodyType { get; set; }
    public string BloodGroup { get; set; }
    public string Complexion { get; set; }
    public string Diet { get; set; }
    public string Smoke { get; set; }
    public string Drink { get; set; }
    public enBoolean Optics { get; set; }
    public string Deformity { get; set; }
  }
}