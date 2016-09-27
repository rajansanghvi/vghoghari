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

    [HttpGet]
    [Authorize(Roles = "user")]
    public BiodataTO GetProfessionalInfo(string code) {
      return MatrimonialBL.GetProfessionalInfo(code);
    }

    [HttpPost]
    [Authorize(Roles = "user")]
    public IHttpActionResult SaveProfessionalInfo(dynamic data) {
      if(!Utility.isUserActive) {
        return Unauthorized();
      }

      string code = data.Code;

      int education = data.Education;
      string[] degreesAchieved = data.DegreesAchieved.ToObject<string[]>();
      string universityAttended = data.UniversityAttended;
      string addlInfo = data.AddlInfo;

      int occupation = data.Occupation;
      string professionalSector = data.ProfessionalSector;
      string organizationName = data.OrganizationName;
      string designation = data.Designation;
      string organizationAddress = data.OrganizationAddress;

      EducationInfoTO educationInfo = new EducationInfoTO();
      educationInfo.HighestEducation = (enHighestEducation) education;
      educationInfo.DegreesAchieved = string.Join(",", degreesAchieved.ToList());
      educationInfo.UniversityAttended  = universityAttended;
      educationInfo.AddlInfo = addlInfo;

      OccupationInfoTO occupationInfo= new OccupationInfoTO();
      occupationInfo.Occupation = (enOccupation) occupation;
      occupationInfo.ProfessionSector = professionalSector;
      occupationInfo.OrganizationName = organizationName;
      occupationInfo.Designation = designation;
      occupationInfo.OrganizationAddress = organizationAddress;
      
      BiodataTO biodata = new BiodataTO();
      biodata.Code = code;
      biodata.EducationInfo = educationInfo;
      biodata.OccupationInfo= occupationInfo;

      KeyValuePair<int, string> response = MatrimonialBL.SaveProfessionalInfo(biodata);
      if(response.Key == -2) {
        return InternalServerError();
      }
      else if(response.Key == -1) {
        return BadRequest();
      }
      else if(response.Key == -3) {
        return ResponseMessage(Request.CreateResponse(HttpStatusCode.Forbidden));
      }
      else if(response.Key == 0) {
        return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, response.Value));
      }
      return InternalServerError();
    }

    [HttpGet]
    [Authorize(Roles = "user")]
    public BiodataTO GetFamilyInfo(string code) {
      return MatrimonialBL.GetFamilyInfo(code);
    }

    [HttpPost]
    [Authorize(Roles = "user")]
    public IHttpActionResult SaveFamilyInfo(dynamic data) {
      if(!Utility.isUserActive) {
        return Unauthorized();
      }

      string code = data.Code;

      string fatherName = data.FatherName;
      string fatherMobileNumber = data.FatherMobileNumber;
      string grandfatherName = data.GrandFatherName;
      string motherName = data.MotherName;
      string motherMobileNumber = data.MotherMobileNumber;
      string grandmotherName = data.GrandMotherName;
      int noOfBros = data.NoOfBrothers;
      int noOfSis = data.NoOfSisters;
      string landlineNumber = data.LandlineNumber;
      int familyType = data.FamilyType;
      string address = data.Address;
      string country = data.Country;
      string state = data.State;
      string city = data.City;
      int residenceStatus = data.ResidenceStatus;

      string uncleName = data.UncleName;
      string maternalGrandfatherName = data.MaternalGrandFatherName;
      string maternalGrandmotherName = data.MaternalGrandMotherName;
      string native = data.Native;
      string contactNumber = data.ContactNumber;
      string mosalAddress = data.MosalAddress;

      FamilyInfoTO familyInfo = new FamilyInfoTO();
      familyInfo.Address = address;
      familyInfo.City = city;
      familyInfo.Country = country;
      familyInfo.FamilyType = (enFamilyType) familyType;
      familyInfo.FatherMobileNumber = fatherMobileNumber;
      familyInfo.FatherName = fatherName;
      familyInfo.GrandFatherName = grandfatherName;
      familyInfo.GrandMotherName = grandmotherName;
      familyInfo.LandlineNumber = landlineNumber;
      familyInfo.MotherMobileNumber = motherMobileNumber;
      familyInfo.MotherName = motherName;
      familyInfo.NoOfBrothers = noOfBros;
      familyInfo.NoOfSisters = noOfSis;
      familyInfo.ResidenceStatus = (enAddressType) residenceStatus;
      familyInfo.State = state;

      MosalInfoTO mosalInfo = new MosalInfoTO();
      mosalInfo.Address = mosalAddress;
      mosalInfo.ContactNumber = contactNumber;
      mosalInfo.GrandFatherName = maternalGrandfatherName;
      mosalInfo.GrandMotherName = maternalGrandmotherName;
      mosalInfo.Native = native;
      mosalInfo.UncleName = uncleName;
      
      BiodataTO biodata = new BiodataTO();
      biodata.Code = code;
      biodata.FamilyInfo = familyInfo;
      biodata.MosalInfo = mosalInfo;

      KeyValuePair<int, string> response = MatrimonialBL.SaveFamilyInfo(biodata);
      if(response.Key == -2) {
        return InternalServerError();
      }
      else if(response.Key == -1) {
        return BadRequest();
      }
      else if(response.Key == -3) {
        return ResponseMessage(Request.CreateResponse(HttpStatusCode.Forbidden));
      }
      else if(response.Key == 0) {
        return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, response.Value));
      }
      return InternalServerError();
    }

    [HttpGet]
    [Authorize(Roles = "user")]
    public BiodataTO GetFamilyOccupationDetails(string code) {
      return MatrimonialBL.GetFamilyOccupationDetails(code);
    }

    [HttpPost]
    [Authorize(Roles = "user")]
    public IHttpActionResult SaveFamilyOccupationDetails(dynamic data) {
      if(!Utility.isUserActive) {
        return Unauthorized();
      }

      string code = data.Code;
      
      int fatherOccupation = data.FatherOccupation;
      string fatherProfessionalSector = data.FatherProfessionalSector;
      string fatherOrganizationName = data.FatherOrganizationName;
      string fatherDesignation = data.FatherDesignation;
      string fatherOrganizationAddress = data.FatherOrganizationAddress;

      int motherOccupation = data.MotherOccupation;
      string motherProfessionalSector = data.MotherProfessionalSector;
      string motherOrganizationName = data.MotherOrganizationName;
      string motherDesignation = data.MotherDesignation;
      string motherOrganizationAddress = data.MotherOrganizationAddress;
      
      OccupationInfoTO fatherOccupationInfo = new OccupationInfoTO();
      fatherOccupationInfo.Occupation = (enOccupation) fatherOccupation;
      fatherOccupationInfo.ProfessionSector = fatherProfessionalSector;
      fatherOccupationInfo.OrganizationName = fatherOrganizationName;
      fatherOccupationInfo.Designation = fatherDesignation;
      fatherOccupationInfo.OrganizationAddress = fatherOrganizationAddress;

      OccupationInfoTO motherOccupationInfo = new OccupationInfoTO();
      motherOccupationInfo.Occupation = (enOccupation) motherOccupation;
      motherOccupationInfo.ProfessionSector = motherProfessionalSector;
      motherOccupationInfo.OrganizationName = motherOrganizationName;
      motherOccupationInfo.Designation = motherDesignation;
      motherOccupationInfo.OrganizationAddress = motherOrganizationAddress;

      BiodataTO biodata = new BiodataTO();
      biodata.Code = code;
      biodata.FatherOccupationInfo = fatherOccupationInfo;
      biodata.MotherOccupationInfo = motherOccupationInfo;

      KeyValuePair<int, string> response = MatrimonialBL.SaveFamilyOccupationDetails(biodata);
      if(response.Key == -2) {
        return InternalServerError();
      }
      else if(response.Key == -1) {
        return BadRequest();
      }
      else if(response.Key == -3) {
        return ResponseMessage(Request.CreateResponse(HttpStatusCode.Forbidden));
      }
      else if(response.Key == 0) {
        return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, response.Value));
      }
      return InternalServerError();
    }

    [HttpGet]
    [Authorize(Roles = "user")]
    public List<SibblingInfoTO> GetSibblingInfo(string code) {
      return MatrimonialBL.GetSibblingInfo(code);
    }

    [HttpPost]
    [Authorize(Roles = "user")]
    public IHttpActionResult SaveSibblingInfo(dynamic data) {
      if(!Utility.isUserActive) {
        return Unauthorized();
      }

      string code = data.Code;

      int gender = data.Gender;
      string name = data.Name;
      string familyName= data.Family;
      string fatherDesignation = data.FatherDesignation;
      string native = data.Native;
      
      SibblingInfoTO sibblingInfo = new SibblingInfoTO();
      sibblingInfo.Family = familyName;
      sibblingInfo.Gender = (enGender) gender;
      sibblingInfo.GenderString = gender != 0 ? ((enGender)gender).ToString() : string.Empty;
      sibblingInfo.Name = name;
      sibblingInfo.Native = native;
        
      KeyValuePair<int, SibblingInfoTO> response = MatrimonialBL.SaveSibblingInfo(code, sibblingInfo);
      if(response.Key == -2) {
        return InternalServerError();
      }
      else if(response.Key == -1) {
        return BadRequest();
      }
      else if(response.Key == -3) {
        return ResponseMessage(Request.CreateResponse(HttpStatusCode.Forbidden));
      }
      else if(response.Key == 0) {
        return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, response.Value));
      }
      return InternalServerError();
    }

    [HttpPost]
    [Authorize(Roles = "user")]
    public IHttpActionResult DeleteSibblingInfo(dynamic data) {
      if(!Utility.isUserActive) {
        return Unauthorized();
      }

      string biodataCode = data.BiodataCode;
      string sibblingCode = data.SibblingCode;

      int response = MatrimonialBL.DeleteSibblingInfo(biodataCode, sibblingCode);
      if(response == -1) {
        return BadRequest();
      }
      else if(response == 0) {
        return Ok(sibblingCode);
      }
      else if (response == -3) {
        return ResponseMessage(Request.CreateResponse(HttpStatusCode.Forbidden));
      }
      return InternalServerError();
    }

    [HttpGet]
    [Authorize(Roles = "user")]
    public BiodataTO GetAdditionalDetails(string code) {
      return MatrimonialBL.GetAdditionalDetails(code);
    }

    [HttpPost]
    [Authorize(Roles = "user")]
    public IHttpActionResult SaveAdditionalInfo(dynamic data) {
      if(!Utility.isUserActive) {
        return Unauthorized();
      }

      string code = data.Code;

      string hobbies = data.Hobbies;
      string interest = data.Interest;
      string expectation = data.Expectations;
      string profileImageData = data.ProfileImageData;
      
      AdditionalBiodataInfoTO additionalInfo = new AdditionalBiodataInfoTO();
      additionalInfo.Hobbies = hobbies;
      additionalInfo.Interest = interest;
      additionalInfo.Expectation = expectation;

      BiodataTO biodata = new BiodataTO();
      biodata.Code = code;
      biodata.AdditionalInfo = additionalInfo;

      int response = MatrimonialBL.SaveAdditionalInfo(profileImageData, biodata);
      if(response == -2) {
        return InternalServerError();
      }
      else if(response == -1) {
        return BadRequest();
      }
      else if(response == -3) {
        return ResponseMessage(Request.CreateResponse(HttpStatusCode.Forbidden));
      }
      else if(response == 0) {
        return Ok();
      }
      return InternalServerError();
    }
  }
}
