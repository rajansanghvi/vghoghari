using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VGhoghari.AppCodes.Business_Layer;
using VGhoghari.AppCodes.Enum;
using VGhoghari.AppCodes.Utilities;
using VGhoghari.Models;

namespace VGhoghari.Controllers {
  public class MatrimonialApiController : ApiController {

    [HttpGet]
    [Authorize(Roles = "user")]
    public BiodataTO GetBasicInfo(string code) {
      return MatrimonialBL.GetBasicInfo(code);
    }

    [HttpPost]
    [Authorize(Roles = "user")]
    public IHttpActionResult SaveBasicInfo(dynamic data) {
      if (!Utility.isUserActive) {
        return Unauthorized();
      }

      string code = data.Code;

      int gender = data.Gender;
      string fullname = data.Fullname;
      string dob = data.Dob;
      string birthTime = data.BirthTime;
      int age = data.Age;
      int maritalStatus = data.MaritalStatus;
      string native = data.Native;
      string birthPlace = data.BirthPlace;
      string aboutMe = data.AboutMe;

      string mobileNumber = data.MobileNumber;
      string landlineNumber = data.LandlineNumber;
      string emailId = data.EmailId;
      string facebookUrl = data.FacebookUrl;
      string address = data.Address;
      int addressType = data.AddressType;
      string country = data.Country;
      string state = data.State;
      string city = data.City;
      string pincode = data.Pincode;

      BasicInfoTO basicInfo = new BasicInfoTO();
      basicInfo.Gender = (enGender)gender;
      basicInfo.Fullname = fullname;
      if (!string.IsNullOrWhiteSpace(dob)) {
        basicInfo.Dob = DateTime.Parse(dob);
      }

      if (!string.IsNullOrWhiteSpace(birthTime)) {
        basicInfo.BirthTime = DateTime.Parse(birthTime);
      }

      basicInfo.Age = age;
      basicInfo.MaritalStatus = (enMaritalStatus)maritalStatus;
      basicInfo.Native = native;
      basicInfo.BirthPlace = birthPlace;
      basicInfo.AboutMe = aboutMe;


      ContactInfoTO contactInfo = new ContactInfoTO();
      contactInfo.MobileNumber = mobileNumber;
      contactInfo.LandlineNumber = landlineNumber;
      contactInfo.EmailId = emailId;
      contactInfo.FacebookUrl = facebookUrl;
      contactInfo.Address = address;
      contactInfo.AddressType = (enAddressType)addressType;
      contactInfo.Country = country;
      contactInfo.State = state;
      contactInfo.City = city;
      contactInfo.Pincode = pincode;

      BiodataTO biodata = new BiodataTO();
      biodata.Code = code;
      biodata.BasicInfo = basicInfo;
      biodata.ContactInfo = contactInfo;

      KeyValuePair<int, string> response = MatrimonialBL.SaveBasicInfo(biodata);
      if (response.Key == -2) {
        return InternalServerError();
      }
      else if (response.Key == -1) {
        return BadRequest();
      }
      else if (response.Key == -3) {
        return ResponseMessage(Request.CreateResponse(HttpStatusCode.Forbidden));
      }
      else if (response.Key == 0) {
        return ResponseMessage(Request.CreateResponse(HttpStatusCode.Created, response.Value));
      }
      return InternalServerError();
    }

    [HttpGet]
    [Authorize(Roles = "user")]
    public BiodataTO GetPersonalInfo(string code) {
      return MatrimonialBL.GetPersonalInfo(code);
    }

    [HttpPost]
    [Authorize(Roles = "user")]
    public IHttpActionResult SavePersonalInfo(dynamic data) {
      if (!Utility.isUserActive) {
        return Unauthorized();
      }

      string code = data.Code;

      string religion = data.Religion;
      string caste = data.Caste;
      string subCaste = data.SubCaste;
      
      int manglik = data.Manglik;
      string selfGothra = data.SelfGothra;
      string maternalGothra = data.MaternalGothra;
      int starSign = data.StarSign;


      int heightFt = data.HeightFt;
      int heightInch = data.HeightInch;
      int weight = data.Weight;
      string bloodGroup = data.BloodGroup;
      int bodyType = data.BodyType;
      string complexion = data.Complexion;
      int optics = data.Optics;
      string diet = data.Diet;
      string smoke = data.Smoke;
      string drink = data.Drink;
      string deformity = data.Deformity;

      ReligionInfoTO religionInfo = new ReligionInfoTO();
      religionInfo.Religion = religion;
      religionInfo.Caste= caste;
      religionInfo.SubCaste= subCaste;

      SocialInfoTO socialInfo = new SocialInfoTO();
      socialInfo.Manglik = (enBoolean)manglik;
      socialInfo.SelfGothra = selfGothra;
      socialInfo.MaternalGothra = maternalGothra;
      socialInfo.StarSign = (enStarSign)starSign;

      PhysicalInfoTO physicalInfo = new PhysicalInfoTO();
      physicalInfo.HeightFt = heightFt;
      physicalInfo.HeightInch = heightInch;
      physicalInfo.Weight = weight;
      physicalInfo.BloodGroup = bloodGroup;
      physicalInfo.BodyType = (enBodyType)bodyType;
      physicalInfo.Complexion = complexion;
      physicalInfo.Optics = (enBoolean)optics;
      physicalInfo.Diet = diet;
      physicalInfo.Smoke = smoke;
      physicalInfo.Drink = drink;
      physicalInfo.Deformity = deformity;

      BiodataTO biodata = new BiodataTO();
      biodata.Code = code;
      biodata.ReligionInfo = religionInfo;
      biodata.SocialInfo= socialInfo;
      biodata.PhysicalInfo = physicalInfo;

      KeyValuePair<int, string> response = MatrimonialBL.SavePersonalInfo(biodata);
      if (response.Key == -2) {
        return InternalServerError();
      }
      else if (response.Key == -1) {
        return BadRequest();
      }
      else if (response.Key == -3) {
        return ResponseMessage(Request.CreateResponse(HttpStatusCode.Forbidden));
      }
      else if (response.Key == 0) {
        return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, response.Value));
      }
      return InternalServerError();
    }
  }
}
