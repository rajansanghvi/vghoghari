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
      this.ProfileImage = string.Empty;

      this.BasicInfo = null;
      this.ContactInfo = null;

      this.ReligionInfo = null;
      this.SocialInfo = null;
      this.PhysicalInfo = null;

      this.EducationInfo = null;
      this.OccupationInfo = null;

      this.FamilyInfo = null;
      this.MosalInfo = null;

      this.FatherOccupationInfo = null;
      this.MotherOccupationInfo = null;

      this.AdditionalInfo = null;
    }

    [JsonIgnore]
    public int Id { get; set; }
    public string Code { get; set; }
    public string ProfileImage { get; set; }

    public BasicInfoTO BasicInfo { get; set; }
    public ContactInfoTO ContactInfo { get; set; }

    public ReligionInfoTO ReligionInfo { get; set; }
    public SocialInfoTO SocialInfo { get; set; }
    public PhysicalInfoTO PhysicalInfo { get; set; }

    public EducationInfoTO EducationInfo { get; set; }
    public OccupationInfoTO OccupationInfo { get; set; }

    public FamilyInfoTO FamilyInfo { get; set; }
    public MosalInfoTO MosalInfo { get; set; }

    public OccupationInfoTO FatherOccupationInfo { get; set; }
    public OccupationInfoTO MotherOccupationInfo { get; set; }

    public AdditionalBiodataInfoTO AdditionalInfo { get; set; }
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

  public class EducationInfoTO {

    public EducationInfoTO() {
      this.HighestEducation = enHighestEducation.UnSpecified;
      this.DegreesAchieved = string.Empty;
      this.DegreesList = new List<string>();
      this.UniversityAttended = string.Empty;
      this.AddlInfo = string.Empty;
    }

    public enHighestEducation HighestEducation { get; set; }
    public string DegreesAchieved { get; set; }
    public List<string> DegreesList { get; set; }
    public string UniversityAttended { get; set; }
    public string AddlInfo { get; set; }
  }

  public class OccupationInfoTO {

    public OccupationInfoTO() {
      this.Occupation = enOccupation.UnSpecified;
      this.ProfessionSector = string.Empty;
      this.OrganizationName = string.Empty;
      this.OrganizationAddress = string.Empty;
      this.Designation = string.Empty;
    }

    public enOccupation Occupation { get; set; }
    public string ProfessionSector { get; set; }
    public string OrganizationName { get; set; }
    public string Designation { get; set; }
    public string OrganizationAddress { get; set; }
  }

  public class FamilyInfoTO {

    public FamilyInfoTO() {
      this.FatherName = string.Empty;
      this.FatherMobileNumber = string.Empty;
      this.MotherName = string.Empty;
      this.MotherMobileNumber = string.Empty;
      this.GrandFatherName = string.Empty;
      this.GrandMotherName = string.Empty;
      this.NoOfBrothers = 0;
      this.NoOfSisters = 0;
      this.FamilyType = enFamilyType.UnSpecified;
      this.LandlineNumber = string.Empty;
      this.Address = string.Empty;
      this.City = string.Empty;
      this.State = string.Empty;
      this.Country = string.Empty;
      this.ResidenceStatus = enAddressType.UnSpecified;
    }

    public string FatherName { get; set; }
    public string FatherMobileNumber { get; set; }
    public string MotherName { get; set; }
    public string MotherMobileNumber { get; set; }
    public string GrandFatherName { get; set; }
    public string GrandMotherName { get; set; }
    public int NoOfBrothers { get; set; }
    public int NoOfSisters { get; set; }
    public enFamilyType FamilyType { get; set; }
    public string LandlineNumber { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public enAddressType ResidenceStatus { get; set; }
  }

  public class MosalInfoTO {

    public MosalInfoTO() {
      this.UncleName = string.Empty;
      this.GrandFatherName = string.Empty;
      this.GrandMotherName = string.Empty;
      this.Native = string.Empty;
      this.ContactNumber = string.Empty;
      this.Address = string.Empty;
    }

    public string UncleName { get; set; }
    public string GrandFatherName { get; set; }
    public string GrandMotherName { get; set; }
    public string Native { get; set; }
    public string ContactNumber { get; set; }
    public string Address { get; set; }
  }

  public class SibblingInfoTO {

    public SibblingInfoTO() {
      this.Code = string.Empty;
      this.Name = string.Empty;
      this.Gender = enGender.UnSpecified;
      this.Family = string.Empty;
      this.Native = string.Empty;
      this.GenderString = string.Empty;
    }

    public string Code { get; set; }
    public string Name { get; set; }
    public enGender Gender { get; set; }
    public string GenderString { get; set; }
    public string Family { get; set; }
    public string Native { get; set; }
  }

  public class AdditionalBiodataInfoTO {

    public AdditionalBiodataInfoTO() {
      this.Hobbies = string.Empty;
      this.Interest = string.Empty;
      this.Expectation = string.Empty;
    }

    public string Hobbies { get; set; }
    public string Interest { get; set; }
    public string Expectation { get; set; }
  }
}