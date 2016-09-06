﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using VGhoghari.AppCodes.Data_Layer;
using VGhoghari.AppCodes.Enum;
using VGhoghari.Models;

namespace VGhoghari.AppCodes.Business_Layer {
  public class MatrimonialBL {

    public static bool IsMyBiodata(string code) {
      return MatrimonialDL.IsMyBiodata(code);
    }

    public static BiodataTO GetBasicInfo(string code) {
      return MatrimonialDL.FetchBasicInfo(code);
    }

    private static int ValidateBasicInfo(BiodataTO data) {
      if(data.BasicInfo.Gender == enGender.UnSpecified) {
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
      if(data.BasicInfo.MaritalStatus == enMaritalStatus.UnSpecified) {
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
      if(data.ContactInfo.AddressType == enAddressType.UnSpecified) {
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

      if(data.SocialInfo.Manglik == enBoolean.UnSpecified) {
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
      if(data.PhysicalInfo.BodyType == enBodyType.UnSpecified) {
        return -1;
      }
      if(string.IsNullOrWhiteSpace(data.PhysicalInfo.Complexion)) {
        return -1;
      }
      if(data.PhysicalInfo.Optics == enBoolean.UnSpecified) {
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
  }
}