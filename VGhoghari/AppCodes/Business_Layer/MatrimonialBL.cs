using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using VGhoghari.AppCodes.Data_Layer;
using VGhoghari.AppCodes.Enum;
using VGhoghari.Models;

namespace VGhoghari.AppCodes.Business_Layer {
  public class MatrimonialBL {

    public static int MY_BIODATA_LIST_PAGE_SIZE = 5;

    public static int BIODATA_LIST_PAGE_SIZE = 9;

    public static bool IsMyBiodata(string code) {
      return MatrimonialDL.IsMyBiodata(code);
    }

    public static BiodataTO GetBasicInfo(string code) {
      return MatrimonialDL.FetchBasicInfo(code);
    }

    private static int ValidateBasicInfo(BiodataTO data) {
      if(data.BasicInfo.Gender == enGender.Un_Specified) {
        return -1;
      }
      if(string.IsNullOrWhiteSpace(data.BasicInfo.Fullname)
        || data.BasicInfo.Fullname.Length < 2
        || data.BasicInfo.Fullname.Length > 500
        || !Regex.IsMatch(data.BasicInfo.Fullname, @"^([A-Za-z]{1,})([ ]{0,1})([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})$")) {
        return -1;
      }
      if(DateTime.Compare(data.BasicInfo.Dob.Date, new DateTime(1870, 01, 01).Date) == 0
        || DateTime.Compare(data.BasicInfo.Dob.Date, DateTime.Today) > 0) {
        return -1;
      }
      else {
        DateTime today = DateTime.Today;
        int age = today.Year - data.BasicInfo.Dob.Year;

        if(data.BasicInfo.Dob > today.AddYears(-age)) {
          age--;
        }

        if(age < 18 || age > 100) {
          return -1;
        }
      }
      if(data.BasicInfo.Age < 18
        || data.BasicInfo.Age > 100) {
        return -1;
      }
      if(data.BasicInfo.MaritalStatus == enMaritalStatus.Un_Specified) {
        return -1;
      }
      if(string.IsNullOrWhiteSpace(data.BasicInfo.Native)
        || data.BasicInfo.Native.Length > 200
        || !Regex.IsMatch(data.BasicInfo.Native, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
        return -1;
      }
      if(!string.IsNullOrWhiteSpace(data.BasicInfo.BirthPlace)) {
        if(data.BasicInfo.BirthPlace.Length > 200
          || !Regex.IsMatch(data.BasicInfo.BirthPlace, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }
      if(!string.IsNullOrWhiteSpace(data.BasicInfo.AboutMe)) {
        if(data.BasicInfo.AboutMe.Length > 1000
          || !Regex.IsMatch(data.BasicInfo.AboutMe, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      if(string.IsNullOrWhiteSpace(data.ContactInfo.MobileNumber)
        || data.ContactInfo.MobileNumber.Length < 4
        || data.ContactInfo.MobileNumber.Length > 20
        || !Regex.IsMatch(data.ContactInfo.MobileNumber, @"^(\+)?(\d){0,3}( )?\d{4,15}$")) {
        return -1;
      }
      if(!string.IsNullOrWhiteSpace(data.ContactInfo.LandlineNumber)) {
        if(data.ContactInfo.LandlineNumber.Length < 4
          || data.ContactInfo.LandlineNumber.Length > 20
          || !Regex.IsMatch(data.ContactInfo.LandlineNumber, @"^(\+)?(\d){0,3}( )?(\d){0,3}( )?\d{4,11}$")) {
          return -1;
        }
      }
      if(!string.IsNullOrWhiteSpace(data.ContactInfo.EmailId)) {
        if(data.ContactInfo.EmailId.Length > 200
          || !Regex.IsMatch(data.ContactInfo.EmailId, @"^([\w\.\-_]+)?\w+@[\w-_]+(\.\w+){1,}$")) {
          return -1;
        }
      }
      if(!string.IsNullOrWhiteSpace(data.ContactInfo.FacebookUrl)) {
        if(data.ContactInfo.FacebookUrl.Length > 255
          || !Regex.IsMatch(data.ContactInfo.FacebookUrl, @"^((http|https):\/\/|)(www\.|)facebook\.com\/[a-zA-Z0-9.]{1,}$")) {
          return -1;
        }
      }
      if(string.IsNullOrWhiteSpace(data.ContactInfo.Address)
        || data.ContactInfo.Address.Length > 1000
        || !Regex.IsMatch(data.ContactInfo.Address, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
        return -1;
      }
      if(data.ContactInfo.AddressType == enAddressType.Un_Specified) {
        return -1;
      }
      if(string.IsNullOrWhiteSpace(data.ContactInfo.Country)) {
        return -1;
      }
      if(!string.IsNullOrWhiteSpace(data.ContactInfo.Pincode)) {
        if(data.ContactInfo.Pincode.Length < 3
          || data.ContactInfo.Pincode.Length > 10
          || !Regex.IsMatch(data.ContactInfo.Pincode, @"^\d{3,10}$")) {
          return -1;
        }
      }

      return 0;
    }

    public static KeyValuePair<int, string> SaveBasicInfo(BiodataTO data) {
      int validationResponse = ValidateBasicInfo(data);
      if(validationResponse == 0) {
        if(string.IsNullOrWhiteSpace(data.Code)) {
          data.Code = Guid.NewGuid().ToString();
        }
        else {
          if(!MatrimonialDL.IsMyBiodata(data.Code)) {
            return new KeyValuePair<int, string>(-3, string.Empty);
          }
        }
        int id = MatrimonialDL.SaveBasicInfo(data);
        if(id > 0) {
          return new KeyValuePair<int, string>(validationResponse, data.Code);
        }
        // Some issue in saving details hence failed
        return new KeyValuePair<int, string>(-2, string.Empty);
      }

      //validation failed hence -1
      return new KeyValuePair<int, string>(validationResponse, string.Empty);
    }

    public static BiodataTO GetPersonalInfo(string code) {
      return MatrimonialDL.FetchPersonalInfo(code);
    }

    private static int ValidatePersonalInfo(BiodataTO data) {

      if(string.IsNullOrWhiteSpace(data.Code)) {
        return -1;
      }

      if(string.IsNullOrWhiteSpace(data.ReligionInfo.Religion)) {
        return -1;
      }
      if(string.IsNullOrWhiteSpace(data.ReligionInfo.Caste)) {
        return -1;
      }
      else {
        if(string.Equals(data.ReligionInfo.Caste.ToLower(), "other")) {
          if(string.IsNullOrWhiteSpace(data.ReligionInfo.SubCaste)
            || data.ReligionInfo.SubCaste.Length > 200
            || !Regex.IsMatch(data.ReligionInfo.SubCaste, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
            return -1;
          }
        }
      }

      if(data.SocialInfo.Manglik == enBoolean.Un_Specified) {
        return -1;
      }
      if(!string.IsNullOrWhiteSpace(data.SocialInfo.SelfGothra)) {
        if(data.SocialInfo.SelfGothra.Length > 200
          || !Regex.IsMatch(data.SocialInfo.SelfGothra, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }
      if(!string.IsNullOrWhiteSpace(data.SocialInfo.MaternalGothra)) {
        if(data.SocialInfo.MaternalGothra.Length > 200
          || !Regex.IsMatch(data.SocialInfo.MaternalGothra, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      if(data.PhysicalInfo.HeightFt <= 0) {
        return -1;
      }
      if(data.PhysicalInfo.HeightInch < 0) {
        return -1;
      }
      if(data.PhysicalInfo.Weight < 0) {
        return -1;
      }
      if(data.PhysicalInfo.BodyType == enBodyType.Un_Specified) {
        return -1;
      }
      if(string.IsNullOrWhiteSpace(data.PhysicalInfo.Complexion)) {
        return -1;
      }
      if(data.PhysicalInfo.Optics == enBoolean.Un_Specified) {
        return -1;
      }
      if(string.IsNullOrWhiteSpace(data.PhysicalInfo.Diet)) {
        return -1;
      }
      if(string.IsNullOrWhiteSpace(data.PhysicalInfo.Smoke)) {
        return -1;
      }
      if(string.IsNullOrWhiteSpace(data.PhysicalInfo.Drink)) {
        return -1;
      }
      if(!string.IsNullOrWhiteSpace(data.PhysicalInfo.Deformity)) {
        if(data.PhysicalInfo.Deformity.Length > 1000
          || !Regex.IsMatch(data.PhysicalInfo.Deformity, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }
      return 0;
    }

    public static KeyValuePair<int, string> SavePersonalInfo(BiodataTO data) {
      if(!MatrimonialDL.IsMyBiodata(data.Code)) {
        return new KeyValuePair<int, string>(-3, string.Empty);
      }

      int validationResponse = ValidatePersonalInfo(data);
      if(validationResponse == 0) {
        int id = MatrimonialDL.SavePersonalInfo(data);
        if(id > 0) {
          return new KeyValuePair<int, string>(validationResponse, data.Code);
        }
        // Some issue in saving details hence failed
        return new KeyValuePair<int, string>(-2, string.Empty);
      }

      //validation failed hence -1
      return new KeyValuePair<int, string>(validationResponse, string.Empty);
    }

    public static BiodataTO GetProfessionalInfo(string code) {
      return MatrimonialDL.FetchProfessionalInfo(code);
    }

    private static int ValidateProfessionalInfo(BiodataTO data) {

      if(string.IsNullOrWhiteSpace(data.Code)) {
        return -1;
      }

      if(data.EducationInfo.HighestEducation == enHighestEducation.Un_Specified) {
        return -1;
      }

      if(string.IsNullOrWhiteSpace(data.EducationInfo.DegreesAchieved)) {
        return -1;
      }

      if(!string.IsNullOrWhiteSpace(data.EducationInfo.UniversityAttended)) {
        if(data.EducationInfo.UniversityAttended.Length > 500
          || !Regex.IsMatch(data.EducationInfo.UniversityAttended, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      if(!string.IsNullOrWhiteSpace(data.EducationInfo.AddlInfo)) {
        if(data.EducationInfo.AddlInfo.Length > 1000
          || !Regex.IsMatch(data.EducationInfo.AddlInfo, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      if(data.OccupationInfo.Occupation == enOccupation.Un_Specified) {
        return -1;
      }

      if(string.IsNullOrWhiteSpace(data.OccupationInfo.ProfessionSector)) {
        return -1;
      }

      if(!string.IsNullOrWhiteSpace(data.OccupationInfo.OrganizationName)) {
        if(data.OccupationInfo.OrganizationName.Length > 500
          || !Regex.IsMatch(data.OccupationInfo.OrganizationName, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      if(!string.IsNullOrWhiteSpace(data.OccupationInfo.Designation)) {
        if(data.OccupationInfo.Designation.Length > 500
          || !Regex.IsMatch(data.OccupationInfo.Designation, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      if(!string.IsNullOrWhiteSpace(data.OccupationInfo.OrganizationAddress)) {
        if(data.OccupationInfo.OrganizationAddress.Length > 1000
          || !Regex.IsMatch(data.OccupationInfo.OrganizationAddress, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      return 0;
    }

    public static KeyValuePair<int, string> SaveProfessionalInfo(BiodataTO data) {
      if(!MatrimonialDL.IsMyBiodata(data.Code)) {
        return new KeyValuePair<int, string>(-3, string.Empty);
      }

      int validationResponse = ValidateProfessionalInfo(data);
      if(validationResponse == 0) {
        int id = MatrimonialDL.SaveProfessionalInfo(data);
        if(id > 0) {
          return new KeyValuePair<int, string>(validationResponse, data.Code);
        }
        // Some issue in saving details hence failed
        return new KeyValuePair<int, string>(-2, string.Empty);
      }

      //validation failed hence -1
      return new KeyValuePair<int, string>(validationResponse, string.Empty);
    }

    public static BiodataTO GetFamilyInfo(string code) {
      return MatrimonialDL.FetchFamilyInfo(code);
    }

    private static int ValidateFamilyInfo(BiodataTO data) {
      if(string.IsNullOrWhiteSpace(data.Code)) {
        return -1;
      }

      if(string.IsNullOrWhiteSpace(data.FamilyInfo.Address)
        || data.FamilyInfo.Address.Length > 1000
        || !Regex.IsMatch(data.FamilyInfo.Address, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
        return -1;
      }

      if(string.IsNullOrWhiteSpace(data.FamilyInfo.Country)) {
        return -1;
      }

      if(data.FamilyInfo.FamilyType == enFamilyType.Un_Specified) {
        return -1;
      }

      if(!string.IsNullOrWhiteSpace(data.FamilyInfo.FatherMobileNumber)) {
        if(data.FamilyInfo.FatherMobileNumber.Length < 4
          || data.FamilyInfo.FatherMobileNumber.Length > 20
          || !Regex.IsMatch(data.FamilyInfo.FatherMobileNumber, @"^(\+)?(\d){0,3}( )?\d{4,15}$")) {
          return -1;
        }
      }

      if(string.IsNullOrWhiteSpace(data.FamilyInfo.FatherName)
        || data.FamilyInfo.FatherName.Length < 2
        || data.FamilyInfo.FatherName.Length > 500
        || !Regex.IsMatch(data.FamilyInfo.FatherName, @"^([A-Za-z]{1,})([ ]{0,1})([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})$")) {
        return -1;
      }

      if(string.IsNullOrWhiteSpace(data.FamilyInfo.GrandFatherName)
        || data.FamilyInfo.GrandFatherName.Length < 2
        || data.FamilyInfo.GrandFatherName.Length > 500
        || !Regex.IsMatch(data.FamilyInfo.GrandFatherName, @"^([A-Za-z]{1,})([ ]{0,1})([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})$")) {
        return -1;
      }

      if(!string.IsNullOrWhiteSpace(data.FamilyInfo.GrandMotherName)) {
        if(data.FamilyInfo.GrandMotherName.Length < 2
        || data.FamilyInfo.GrandMotherName.Length > 500
        || !Regex.IsMatch(data.FamilyInfo.GrandMotherName, @"^([A-Za-z]{1,})([ ]{0,1})([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})$")) {
          return -1;
        }
      }


      if(!string.IsNullOrWhiteSpace(data.FamilyInfo.LandlineNumber)) {
        if(data.FamilyInfo.LandlineNumber.Length < 4
          || data.FamilyInfo.LandlineNumber.Length > 20
          || !Regex.IsMatch(data.FamilyInfo.LandlineNumber, @"^(\+)?(\d){0,3}( )?(\d){0,3}( )?\d{4,11}$")) {
          return -1;
        }
      }

      if(!string.IsNullOrWhiteSpace(data.FamilyInfo.MotherMobileNumber)) {
        if(data.FamilyInfo.MotherMobileNumber.Length < 4
          || data.FamilyInfo.MotherMobileNumber.Length > 20
          || !Regex.IsMatch(data.FamilyInfo.MotherMobileNumber, @"^(\+)?(\d){0,3}( )?\d{4,15}$")) {
          return -1;
        }
      }

      if(string.IsNullOrWhiteSpace(data.FamilyInfo.MotherName)
      || data.FamilyInfo.MotherName.Length < 2
      || data.FamilyInfo.MotherName.Length > 500
      || !Regex.IsMatch(data.FamilyInfo.MotherName, @"^([A-Za-z]{1,})([ ]{0,1})([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})$")) {
        return -1;
      }

      if(data.FamilyInfo.NoOfBrothers < 0) {
        return -1;
      }

      if(data.FamilyInfo.NoOfSisters < 0) {
        return -1;
      }

      if(data.FamilyInfo.ResidenceStatus == enAddressType.Un_Specified) {
        return -1;
      }

      if(string.IsNullOrWhiteSpace(data.FamilyInfo.FatherMobileNumber) && string.IsNullOrWhiteSpace(data.FamilyInfo.MotherMobileNumber) && string.IsNullOrWhiteSpace(data.FamilyInfo.LandlineNumber)) {
        return -1;
      }

      if(!string.IsNullOrWhiteSpace(data.MosalInfo.UncleName)) {
        if(data.MosalInfo.UncleName.Length < 2
          || data.MosalInfo.UncleName.Length > 500
          || !Regex.IsMatch(data.MosalInfo.UncleName, @"^([A-Za-z]{1,})([ ]{0,1})([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})$")) {
          return -1;
        }
      }

      if(string.IsNullOrWhiteSpace(data.MosalInfo.GrandFatherName)
        || data.MosalInfo.GrandFatherName.Length < 2
        || data.MosalInfo.GrandFatherName.Length > 500
        || !Regex.IsMatch(data.MosalInfo.GrandFatherName, @"^([A-Za-z]{1,})([ ]{0,1})([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})$")) {
        return -1;
      }

      if(!string.IsNullOrWhiteSpace(data.MosalInfo.GrandMotherName)) {
        if(data.MosalInfo.GrandMotherName.Length < 2
          || data.MosalInfo.GrandMotherName.Length > 500
          || !Regex.IsMatch(data.MosalInfo.GrandMotherName, @"^([A-Za-z]{1,})([ ]{0,1})([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})$")) {
          return -1;
        }
      }

      if(string.IsNullOrWhiteSpace(data.MosalInfo.Native)
        || data.MosalInfo.Native.Length > 200
        || !Regex.IsMatch(data.MosalInfo.Native, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
        return -1;
      }

      if(!string.IsNullOrWhiteSpace(data.MosalInfo.ContactNumber)) {
        if(data.MosalInfo.ContactNumber.Length < 4
          || data.MosalInfo.ContactNumber.Length > 20
          || (!Regex.IsMatch(data.MosalInfo.ContactNumber, @"^(\+)?(\d){0,3}( )?(\d){0,3}( )?\d{4,11}$")
                              && !Regex.IsMatch(data.MosalInfo.ContactNumber, @"^(\+)?(\d){0,3}( )?\d{4,15}$"))) {
          return -1;
        }
      }

      if(!string.IsNullOrWhiteSpace(data.FamilyInfo.Address)) {
        if(data.FamilyInfo.Address.Length > 1000
          || !Regex.IsMatch(data.FamilyInfo.Address, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }
      return 0;
    }

    public static KeyValuePair<int, string> SaveFamilyInfo(BiodataTO data) {
      if(!MatrimonialDL.IsMyBiodata(data.Code)) {
        return new KeyValuePair<int, string>(-3, string.Empty);
      }

      int validationResponse = ValidateFamilyInfo(data);
      if(validationResponse == 0) {
        int id = MatrimonialDL.SaveFamilyInfo(data);
        if(id > 0) {
          return new KeyValuePair<int, string>(validationResponse, data.Code);
        }
        // Some issue in saving details hence failed
        return new KeyValuePair<int, string>(-2, string.Empty);
      }

      //validation failed hence -1
      return new KeyValuePair<int, string>(validationResponse, string.Empty);
    }

    public static BiodataTO GetFamilyOccupationDetails(string code) {
      return MatrimonialDL.FetchFamilyOccupationDetails(code);
    }

    private static int ValidateFamilyOccupationDetails(BiodataTO data) {

      if(string.IsNullOrWhiteSpace(data.Code)) {
        return -1;
      }

      if(data.FatherOccupationInfo.Occupation == enOccupation.Un_Specified) {
        return -1;
      }

      if(string.IsNullOrWhiteSpace(data.FatherOccupationInfo.ProfessionSector)) {
        return -1;
      }

      if(!string.IsNullOrWhiteSpace(data.FatherOccupationInfo.OrganizationName)) {
        if(data.FatherOccupationInfo.OrganizationName.Length > 500
          || !Regex.IsMatch(data.FatherOccupationInfo.OrganizationName, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      if(!string.IsNullOrWhiteSpace(data.FatherOccupationInfo.Designation)) {
        if(data.FatherOccupationInfo.Designation.Length > 500
          || !Regex.IsMatch(data.FatherOccupationInfo.Designation, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      if(!string.IsNullOrWhiteSpace(data.FatherOccupationInfo.OrganizationAddress)) {
        if(data.FatherOccupationInfo.OrganizationAddress.Length > 1000
          || !Regex.IsMatch(data.FatherOccupationInfo.OrganizationAddress, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      if(data.MotherOccupationInfo.Occupation == enOccupation.Un_Specified) {
        return -1;
      }

      if(string.IsNullOrWhiteSpace(data.MotherOccupationInfo.ProfessionSector)) {
        return -1;
      }

      if(!string.IsNullOrWhiteSpace(data.MotherOccupationInfo.OrganizationName)) {
        if(data.MotherOccupationInfo.OrganizationName.Length > 500
          || !Regex.IsMatch(data.MotherOccupationInfo.OrganizationName, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      if(!string.IsNullOrWhiteSpace(data.MotherOccupationInfo.Designation)) {
        if(data.MotherOccupationInfo.Designation.Length > 500
          || !Regex.IsMatch(data.MotherOccupationInfo.Designation, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      if(!string.IsNullOrWhiteSpace(data.MotherOccupationInfo.OrganizationAddress)) {
        if(data.MotherOccupationInfo.OrganizationAddress.Length > 1000
          || !Regex.IsMatch(data.MotherOccupationInfo.OrganizationAddress, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      return 0;
    }

    public static KeyValuePair<int, string> SaveFamilyOccupationDetails(BiodataTO data) {
      if(!MatrimonialDL.IsMyBiodata(data.Code)) {
        return new KeyValuePair<int, string>(-3, string.Empty);
      }

      int validationResponse = ValidateFamilyOccupationDetails(data);
      if(validationResponse == 0) {
        int id = MatrimonialDL.SaveFamilyOccupationDetails(data);
        if(id > 0) {
          return new KeyValuePair<int, string>(validationResponse, data.Code);
        }
        // Some issue in saving details hence failed
        return new KeyValuePair<int, string>(-2, string.Empty);
      }

      //validation failed hence -1
      return new KeyValuePair<int, string>(validationResponse, string.Empty);
    }

    public static List<SibblingInfoTO> GetSibblingInfo(string code) {
      return MatrimonialDL.FetchSillingInfos(code);
    }

    private static int ValidateSibblingInfo(string code, SibblingInfoTO data) {
      if(string.IsNullOrWhiteSpace(code)) {
        return -1;
      }

      if(string.IsNullOrWhiteSpace(data.Name)
        || data.Name.Length < 2
        || data.Name.Length > 500
        || !Regex.IsMatch(data.Name, @"^([A-Za-z]{1,})([ ]{0,1})([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})$")) {
        return -1;
      }

      if(data.Gender == enGender.Un_Specified) {
        return -1;
      }

      if(!string.IsNullOrWhiteSpace(data.Family)) {
        if(data.Family.Length < 2
        || data.Family.Length > 500
        || !Regex.IsMatch(data.Family, @"^([A-Za-z]{1,})([ ]{0,1})([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})$")) {
          return -1;
        }
      }

      if(!string.IsNullOrWhiteSpace(data.Native)) {
        if(data.Name.Length > 200
        || !Regex.IsMatch(data.Native, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }
      return 0;
    }

    public static KeyValuePair<int, SibblingInfoTO> SaveSibblingInfo(string code, SibblingInfoTO data) {
      if(!MatrimonialDL.IsMyBiodata(code)) {
        return new KeyValuePair<int, SibblingInfoTO>(-3, null);
      }

      int validationResponse = ValidateSibblingInfo(code, data);
      if(validationResponse == 0) {
        data.Code = Guid.NewGuid().ToString();
        int id = MatrimonialDL.SaveSibblingInfo(code, data);
        if(id > 0) {
          return new KeyValuePair<int, SibblingInfoTO>(validationResponse, data);
        }
        // Some issue in saving details hence failed
        return new KeyValuePair<int, SibblingInfoTO>(-2, null);
      }
      //validation failed hence -1
      return new KeyValuePair<int, SibblingInfoTO>(validationResponse, null);
    }

    public static int DeleteSibblingInfo(string code, string sibblingCode) {
      if(!MatrimonialDL.IsMyBiodata(code)) {
        return -3;
      }

      int count = MatrimonialDL.DeleteSibblingInfo(code, sibblingCode);
      if(count == 0) {
        return -1;
      }
      else if(count > 1) {
        return -2;
      }
      return 0;
    }

    public static BiodataTO GetAdditionalDetails(string code) {
      return MatrimonialDL.FetchAdditionDetails(code);
    }

    private static int ValidateAdditionalInfo(string profileImageData, BiodataTO data) {
      if(string.IsNullOrWhiteSpace(data.Code)) {
        return -1;
      }

      if(!string.IsNullOrWhiteSpace(data.AdditionalInfo.Hobbies)) {
        if(data.AdditionalInfo.Hobbies.Length > 1000
          || !Regex.IsMatch(data.AdditionalInfo.Hobbies, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      if(!string.IsNullOrWhiteSpace(data.AdditionalInfo.Interest)) {
        if(data.AdditionalInfo.Interest.Length > 1000
          || !Regex.IsMatch(data.AdditionalInfo.Interest, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      if(!string.IsNullOrWhiteSpace(data.AdditionalInfo.Expectation)) {
        if(data.AdditionalInfo.Expectation.Length > 1000
          || !Regex.IsMatch(data.AdditionalInfo.Expectation, @"^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;""',./?]{2,}$")) {
          return -1;
        }
      }

      if(string.IsNullOrWhiteSpace(MatrimonialDL.FetchProfileImageNameByCode(data.Code))) {
        if(string.IsNullOrWhiteSpace(profileImageData)) {
          return -1;
        }
      }

      return 0;
    }

    public static int SaveAdditionalInfo(string profileImageData, BiodataTO data) {
      if(!MatrimonialDL.IsMyBiodata(data.Code)) {
        return -3;
      }

      int validationResponse = ValidateAdditionalInfo(profileImageData, data);
      if(validationResponse == 0) {

        if(!string.IsNullOrWhiteSpace(profileImageData)) {
          string fileName = Guid.NewGuid().ToString();
          data.ProfileImage = fileName + ".jpg";

          string imageData = profileImageData.Split(',')[1];
          string directory = HttpContext.Current.Server.MapPath("~/AppData/biodata/");
          try {
            if(!Directory.Exists(directory)) {
              Directory.CreateDirectory(directory);
            }
            string path = Path.Combine(directory, data.ProfileImage);
            File.WriteAllBytes(path, Convert.FromBase64String(imageData));
            string oldFileName = MatrimonialDL.FetchProfileImageNameByCode(data.Code);
            if(!string.IsNullOrWhiteSpace(oldFileName)) {
              string oldFilePath = Path.Combine(directory, oldFileName);
              if(File.Exists(oldFilePath)) {
                File.Delete(oldFilePath);
              }
            }
            int rowsAffected = MatrimonialDL.UpdateProfileImageByCode(data.Code, data.ProfileImage);
            if(rowsAffected != 1) {
              return -2;
            }
          }
          catch(Exception e) {
            return -2;
          }
        }

        int id = MatrimonialDL.SaveAdditionalInfo(data);
        if(id > 0) {
          return validationResponse;
        }
        // Some issue in saving details hence failed
        return -2;
      }
      //validation failed hence -1
      return validationResponse;
    }

    public static int CountMyBiodataList() {
      return MatrimonialDL.CountMyBiodataList();
    }

    public static List<BiodataTO> GetMyBiodataList(int? pageNumber) {

      int offset = (pageNumber ?? 1) - 1;
      return MatrimonialDL.FetchMyBiodataList(offset, MY_BIODATA_LIST_PAGE_SIZE);
    }

    public static BiodataTO GetMyBiodataDetails(string code) {
      BiodataTO biodata = MatrimonialDL.FetchBiodataByCode(code);
      if(biodata != null) {
        biodata.Sibblings = MatrimonialDL.FetchSibblingInfoByBiodataCode(code);
      }

      return biodata;
    }

    public static int CountMyBiodataByStatus(enApprovalStatus status) {
      return MatrimonialDL.CountMyBiodataByStatus(status);
    }

    public static List<BiodataTO> GetMyBiodataListByStatus(int? pageNumber, enApprovalStatus status) {

      int offset = ((pageNumber ?? 1) - 1) * MY_BIODATA_LIST_PAGE_SIZE;

      return MatrimonialDL.FetchMyBiodataListByStatus(offset, MY_BIODATA_LIST_PAGE_SIZE, status);
    }

    public static int DeleteMyBiodataByCode(string code) {
      return MatrimonialDL.DeleteMyBiodataByCode(code);
    }

    public static int CountAllActiveBiodatas() {
      return MatrimonialDL.CountTotalActiveBiodatas();
    }

    public static List<BiodataTO> GetAllBiodatas(int? pageNumber) {
      int offset = ((pageNumber ?? 1) - 1) * BIODATA_LIST_PAGE_SIZE;
      return MatrimonialDL.FetchAllBiodata(offset, BIODATA_LIST_PAGE_SIZE);
    }

    public static int CountOfMyActiveBiodata() {
      return MatrimonialDL.CountOfMyActiveBiodata();
    }

    public static int GetBiodataCountByStatus(enApprovalStatus status) {
      return MatrimonialDL.GetBiodataCountByStatus(status);
    }

    public static List<BiodataTO> GetBiodataListByStatus(int? pageNumber, enApprovalStatus status) {
      int offset = ((pageNumber ?? 1) - 1) * MY_BIODATA_LIST_PAGE_SIZE;

      return MatrimonialDL.GetBiodataListByStatus(offset, MY_BIODATA_LIST_PAGE_SIZE, status);
    }

    public static int UpdateApprovalStatus(string code, enApprovalStatus status) {
      return MatrimonialDL.UpdateApprovalStatus(code, status);
    }
  }
}