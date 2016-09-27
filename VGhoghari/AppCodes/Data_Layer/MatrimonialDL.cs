using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using VGhoghari.AppCodes.Enum;
using VGhoghari.AppCodes.Utilities;
using VGhoghari.Models;

namespace VGhoghari.AppCodes.Data_Layer {
  public class MatrimonialDL {

    public static bool IsMyBiodata(string code) {
      const string sql = @"select 1
                          from
                          app_biodata_basic_infos
                          where
                          code = ?code
                          and
                          active = true
                          and
                          user_id = (select id from app_users where username = ?username);";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("code", code);
      dl.AddParam("username", Utility.CurrentUser);

      return dl.ExecuteSqlReturnScalar<int>(Utility.ConnectionString, sql) == 1 ? true : false;
    }

    public static BiodataTO FetchBasicInfo(string code) {
      const string sql = @"select ifnull(b.code, '') as code
                          , ifnull(b.fullname, '') as fullname
                          , ifnull(b.gender, 0) as gender
                          , b.dob as dob
                          , ifnull(b.age, 0) as age
                          , ifnull(b.birth_time, '') as birth_time
                          , ifnull(b.native, '') as native
                          , ifnull(b.marital_status, 0) as marital_status
                          , ifnull(b.birth_place, '') as birth_place
                          , ifnull(b.about_me, '') as about_me
                          , ifnull(b.approval_status, 0) as approval_status
                          , ifnull(c.landline_number, '') as landline_number
                          , ifnull(c.mobile_number, '') as mobile_number
                          , ifnull(c.email_id, '') as email_id
                          , ifnull(c.facebook_url, '') as facebook_url
                          , ifnull(c.address, '') as address
                          , ifnull(c.pincode, '') as pincode
                          , ifnull(c.city, '') as city
                          , ifnull(c.state, '') as state
                          , ifnull(c.country, '') as country
                          , ifnull(c.residential_status, 0) as address_type
                          from
                          app_biodata_basic_infos b
                          left join
                          app_biodata_contact_infos c
                          on b.id = c.biodata_id
                          where
                          b.code = ?code
                          and
                          b.active = true
                          and
                          b.user_id = (select id from app_users where username = ?username and active = true);";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("code", code);
      dl.AddParam("username", Utility.CurrentUser);

      using (MySqlDataReader dr = dl.ExecuteSqlReturnReader(Utility.ConnectionString, sql)) {
        if (dr.Read()) {
          return new BiodataTO() {
            Code = dr.GetString("code"),
            BasicInfo = new BasicInfoTO() {
              Gender = (enGender)dr.GetInt32("gender"),
              Fullname = dr.GetString("fullname"),
              Dob = dr.GetDateTime("dob"),
              StringBirthTime = dr.GetString("birth_time"),
              Age = dr.GetInt32("age"),
              MaritalStatus = (enMaritalStatus)dr.GetInt32("marital_status"),
              Native = dr.GetString("native"),
              BirthPlace = dr.GetString("birth_place"),
              AboutMe = dr.GetString("about_me")
            },
            ContactInfo = new ContactInfoTO() {
              MobileNumber = dr.GetString("mobile_number"),
              LandlineNumber = dr.GetString("landline_number"),
              EmailId = dr.GetString("email_id"),
              FacebookUrl = dr.GetString("facebook_url"),
              Address = dr.GetString("address"),
              AddressType = (enAddressType)dr.GetInt32("address_type"),
              Country = dr.GetString("country"),
              State = dr.GetString("state"),
              City = dr.GetString("city"),
              Pincode = dr.GetString("pincode")
            }
          };
        }
      }
      return null;
    }

    public static int SaveBasicInfo(BiodataTO data) {
      using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required)) {
        string procedureName = @"save_basic_biodata_info";

        GlobalDL dl = new GlobalDL();
        dl.AddParam("a_code", data.Code);
        dl.AddParam("a_fullname", data.BasicInfo.Fullname);
        dl.AddParam("a_gender", data.BasicInfo.Gender);
        dl.AddParam("a_dob", data.BasicInfo.Dob);
        dl.AddParam("a_age", data.BasicInfo.Age);
        dl.AddParam("a_birthtime", data.BasicInfo.BirthTime);
        dl.AddParam("a_native", data.BasicInfo.Native);
        dl.AddParam("a_marital_status", data.BasicInfo.MaritalStatus);
        dl.AddParam("a_birth_place", data.BasicInfo.BirthPlace);
        dl.AddParam("a_about_me", data.BasicInfo.AboutMe);
        dl.AddParam("a_username", Utility.CurrentUser);
        dl.AddParam("a_landline_no", data.ContactInfo.LandlineNumber);
        dl.AddParam("a_mobile_no", data.ContactInfo.MobileNumber);
        dl.AddParam("a_email_id", data.ContactInfo.EmailId);
        dl.AddParam("a_facebook_url", data.ContactInfo.FacebookUrl);
        dl.AddParam("a_address", data.ContactInfo.Address);
        dl.AddParam("a_pincode", data.ContactInfo.Pincode);
        dl.AddParam("a_city", data.ContactInfo.City);
        dl.AddParam("a_state", data.ContactInfo.State);
        dl.AddParam("a_country", data.ContactInfo.Country);
        dl.AddParam("a_address_type", data.ContactInfo.AddressType);

        int id = dl.ExecuteProcedureReturnScalar<int>(Utility.ConnectionString, procedureName);
        ts.Complete();
        return id;
      }
    }

    public static BiodataTO FetchPersonalInfo(string code) {
      const string sql = @"select
                          if(r.religion, '') as religion
                          , ifnull(r.caste, '') as caste
                          , ifnull(r.subcaste, '') as subcaste
                          , ifnull(s.manglik, 0) as manglik
                          , ifnull(s.self_gothra, '') as self_gothra
                          , ifnull(s.maternal_gothra, '') as maternal_gothra
                          , ifnull(s.star_sign, 0) as star_sign
                          , ifnull(p.height_ft, 0) as height_ft
                          , ifnull(p.height_inch, -1) as height_inch
                          , ifnull(p.weight, 0) as weight
                          , ifnull(p.blood_group, '') as blood_group
                          , ifnull(p.body_type, 0) as body_type
                          , ifnull(p.complexion, '') as complexion
                          , ifnull(p.optic, 0) as optics
                          , ifnull(p.diet, '') as diet
                          , ifnull(p.smoke, '') as smoke
                          , ifnull(p.drink, '') as drink
                          , ifnull(deformity, '') as deformity
                          from
                          app_biodata_basic_infos b
                          left join
                          app_biodata_religion_infos r
                          on b.id = r.biodata_id
                          left join
                          app_biodata_social_infos s
                          on b.id = s.biodata_id
                          left join
                          app_biodata_physical_infos p
                          on b.id = p.biodata_id
                          where
                          b.code = ?code
                          and
                          b.active = true
                          and b.user_id = (select id from app_users where username = ?username and active = true);";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("code", code);
      dl.AddParam("username", Utility.CurrentUser);

      using(MySqlDataReader dr = dl.ExecuteSqlReturnReader(Utility.ConnectionString, sql)) {
        if(dr.Read()) {
          return new BiodataTO() {
            ReligionInfo = new ReligionInfoTO() {
              Religion = dr.GetString("religion"),
              Caste = dr.GetString("caste"),
              SubCaste = dr.GetString("subcaste")
            },
            SocialInfo = new SocialInfoTO() {
              Manglik = (enBoolean) dr.GetInt32("manglik"),
              SelfGothra = dr.GetString("self_gothra"),
              MaternalGothra = dr.GetString("maternal_gothra"),
              StarSign = (enStarSign) dr.GetInt32("star_sign")
            },
            PhysicalInfo = new PhysicalInfoTO() {
              HeightFt = dr.GetInt32("height_ft"),
              HeightInch = dr.GetInt32("height_inch"),
              Weight = dr.GetInt32("weight"),
              BloodGroup = dr.GetString("blood_group"),
              BodyType = (enBodyType) dr.GetInt32("body_type"),
              Complexion = dr.GetString("complexion"),
              Optics = (enBoolean) dr.GetInt32("optics"),
              Diet = dr.GetString("diet"),
              Smoke = dr.GetString("smoke"),
              Drink = dr.GetString("drink"),
              Deformity = dr.GetString("deformity")
            }
          };
        }
      }
      return null;
    }

    public static int SavePersonalInfo(BiodataTO data) {
      using(TransactionScope ts = new TransactionScope(TransactionScopeOption.Required)) {
        string procedureName = @"save_personal_biodata_info";

        GlobalDL dl = new GlobalDL();
        dl.AddParam("a_code", data.Code);

        dl.AddParam("a_religion", data.ReligionInfo.Religion);
        dl.AddParam("a_caste", data.ReligionInfo.Caste);
        dl.AddParam("a_sub_caste", data.ReligionInfo.SubCaste);

        dl.AddParam("a_manglik", data.SocialInfo.Manglik);
        dl.AddParam("a_self_gothra", data.SocialInfo.SelfGothra);
        dl.AddParam("a_maternal_gothra", data.SocialInfo.MaternalGothra);
        dl.AddParam("a_star_sign", data.SocialInfo.StarSign);

        dl.AddParam("a_height_ft", data.PhysicalInfo.HeightFt);
        dl.AddParam("a_height_inch", data.PhysicalInfo.HeightInch);
        dl.AddParam("a_weight", data.PhysicalInfo.Weight);
        dl.AddParam("a_blood_group", data.PhysicalInfo.BloodGroup);
        dl.AddParam("a_body_type", data.PhysicalInfo.BodyType);
        dl.AddParam("a_complexion", data.PhysicalInfo.Complexion);
        dl.AddParam("a_optics", data.PhysicalInfo.Optics);
        dl.AddParam("a_diet", data.PhysicalInfo.Diet);
        dl.AddParam("a_smoke", data.PhysicalInfo.Smoke);
        dl.AddParam("a_drink", data.PhysicalInfo.Drink);
        dl.AddParam("a_deformity", data.PhysicalInfo.Deformity);
        dl.AddParam("a_username", Utility.CurrentUser);
        

        int id = dl.ExecuteProcedureReturnScalar<int>(Utility.ConnectionString, procedureName);
        ts.Complete();
        return id;
      }
    }

    public static BiodataTO FetchProfessionalInfo(string code) {
      const string sql = @"select
                          ifnull(e.education, 0) as education
                          , ifnull(e.degrees_achieved, '') as degrees_achieved
                          , ifnull(e.addl_info, '') as addl_info
                          , ifnull(e.university_attended, '') as university_attended
                          , ifnull(o.occupation, 0) as occupation
                          , ifnull(o.profession, '') as profession
                          , ifnull(o.occupation_at, '') as occupation_at
                          , ifnull(o.designation, '') as designation
                          , ifnull(o.address, '') as address
                          from
                          app_biodata_basic_infos b
                          left join
                          app_biodata_education_infos e
                          on b.id = e.biodata_id
                          left join
                          app_biodata_occupation_infos o
                          on b.id = o.biodata_id
                          where
                          b.code = ?code
                          and
                          b.active = true
                          and b.user_id = (select id from app_users where username = ?username and active = true);";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("code", code);
      dl.AddParam("username", Utility.CurrentUser);

      using(MySqlDataReader dr = dl.ExecuteSqlReturnReader(Utility.ConnectionString, sql)) {
        if(dr.Read()) {
          return new BiodataTO() {
            EducationInfo = new EducationInfoTO() {
              HighestEducation = (enHighestEducation) dr.GetInt32("education"),
              DegreesAchieved = dr.GetString("degrees_achieved"),
              DegreesList = dr.GetString("degrees_achieved").Split(',').ToList(),
              UniversityAttended = dr.GetString("university_attended"),
              AddlInfo = dr.GetString("addl_info")
            },
            OccupationInfo = new OccupationInfoTO() {
              Occupation = (enOccupation) dr.GetInt32("occupation"),
              ProfessionSector = dr.GetString("profession"),
              OrganizationName = dr.GetString("occupation_at"),
              OrganizationAddress = dr.GetString("address"),
              Designation = dr.GetString("designation")
            }
          };
        }
      }
      return null;
    }

    public static int SaveProfessionalInfo(BiodataTO data) {
      using(TransactionScope ts = new TransactionScope(TransactionScopeOption.Required)) {
        string procedureName = @"save_professional_biodata_info";

        GlobalDL dl = new GlobalDL();
        dl.AddParam("a_code", data.Code);

        dl.AddParam("a_education", data.EducationInfo.HighestEducation);
        dl.AddParam("a_degrees_achieved", data.EducationInfo.DegreesAchieved);
        dl.AddParam("a_university_attended", data.EducationInfo.UniversityAttended);
        dl.AddParam("a_addl_info", data.EducationInfo.AddlInfo);

        dl.AddParam("a_occupation", data.OccupationInfo.Occupation);
        dl.AddParam("a_professional_sector", data.OccupationInfo.ProfessionSector);
        dl.AddParam("a_organization_name", data.OccupationInfo.OrganizationName);
        dl.AddParam("a_designation", data.OccupationInfo.Designation);
        dl.AddParam("a_organization_addr", data.OccupationInfo.OrganizationAddress);

        dl.AddParam("a_username", Utility.CurrentUser);
        
        int id = dl.ExecuteProcedureReturnScalar<int>(Utility.ConnectionString, procedureName);
        ts.Complete();
        return id;
      }
    }

    public static BiodataTO FetchFamilyInfo(string code) {
      const string sql = @"select
                          ifnull(f.father_name, '') as father_name
                          , ifnull(f.father_mobile_number, '') as father_mobile_number
                          , ifnull(f.mother_name, '') as mother_name
                          , ifnull(f.mother_mobile_number, '') as mother_mobile_number
                          , ifnull(f.grandfather_name, '') as grandfather_name
                          , ifnull(f.grandmother_name, '') as grandmother_name
                          , ifnull(f.no_of_brothers, 0) as no_of_brothers
                          , ifnull(f.no_of_sisters, 0) as no_of_sisters
                          , ifnull(f.family_type, 0) as family_type
                          , ifnull(f.landline_number, '') as landline_number
                          , ifnull(f.address, '') as permanent_address
                          , ifnull(f.city, '') as city
                          , ifnull(f.state, '') as state
                          , ifnull(f.country, '') as country
                          , ifnull(f.residential_status, 0) as residential_status
                          , ifnull(m.uncle_name, '') as uncle_name
                          , ifnull(m.maternal_grandfather_name, '') as maternal_grandfather_name
                          , ifnull(m.maternal_grandmother_name, '') as maternal_grandmother_name
                          , ifnull(m.native, '') as maternal_native
                          , ifnull(m.contact_number, '') as contact_number
                          , ifnull(m.address, '') as mosal_address
                          from
                          app_biodata_basic_infos b
                          left join
                          app_biodata_family_infos f
                          on b.id = f.biodata_id
                          left join
                          app_biodata_mosal_infos m
                          on b.id = m.biodata_id
                          where
                          b.code = ?code
                          and
                          b.active = true
                          and b.user_id = (select id from app_users where username = ?username and active = true);";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("code", code);
      dl.AddParam("username", Utility.CurrentUser);

      using(MySqlDataReader dr = dl.ExecuteSqlReturnReader(Utility.ConnectionString, sql)) {
        if(dr.Read()) {
          return new BiodataTO() {
            FamilyInfo = new FamilyInfoTO() {
              Address = dr.GetString("permanent_address"),
              City = dr.GetString("city"),
              Country = dr.GetString("country"),
              FamilyType = (enFamilyType)dr.GetInt32("family_type"),
              FatherMobileNumber = dr.GetString("father_mobile_number"),
              FatherName = dr.GetString("father_name"),
              GrandFatherName = dr.GetString("grandfather_name"),
              GrandMotherName = dr.GetString("grandmother_name"),
              LandlineNumber = dr.GetString("landline_number"),
              MotherMobileNumber = dr.GetString("mother_mobile_number"),
              MotherName = dr.GetString("mother_name"),
              NoOfBrothers = dr.GetInt32("no_of_brothers"),
              NoOfSisters = dr.GetInt32("no_of_sisters"),
              ResidenceStatus = (enAddressType)dr.GetInt32("residential_status"),
              State = dr.GetString("state")
            },
            MosalInfo = new MosalInfoTO() {
              Address = dr.GetString("mosal_address"),
              ContactNumber = dr.GetString("contact_number"),
              GrandFatherName = dr.GetString("maternal_grandfather_name"),
              GrandMotherName = dr.GetString("maternal_grandmother_name"),
              Native = dr.GetString("maternal_native"),
              UncleName = dr.GetString("uncle_name")
            }
          };
        }
      }
      return null;
    }

    public static int SaveFamilyInfo(BiodataTO data) {
      using(TransactionScope ts = new TransactionScope(TransactionScopeOption.Required)) {
        string procedureName = @"save_family_biodata_info";

        GlobalDL dl = new GlobalDL();
        dl.AddParam("a_code", data.Code);

        dl.AddParam("a_father_name", data.FamilyInfo.FatherName);
        dl.AddParam("a_father_mobile_number", data.FamilyInfo.FatherMobileNumber);
        dl.AddParam("a_mother_name", data.FamilyInfo.MotherName);
        dl.AddParam("a_mother_mobile_number", data.FamilyInfo.MotherMobileNumber);
        dl.AddParam("a_grandfather_name", data.FamilyInfo.GrandFatherName);
        dl.AddParam("a_grandmother_name", data.FamilyInfo.GrandMotherName);
        dl.AddParam("a_no_of_brothers", data.FamilyInfo.NoOfBrothers);
        dl.AddParam("a_no_of_sisters", data.FamilyInfo.NoOfSisters);
        dl.AddParam("a_family_type", data.FamilyInfo.FamilyType);
        dl.AddParam("a_landline_number", data.FamilyInfo.LandlineNumber);
        dl.AddParam("a_address", data.FamilyInfo.Address);
        dl.AddParam("a_city", data.FamilyInfo.City);
        dl.AddParam("a_state", data.FamilyInfo.State);
        dl.AddParam("a_country", data.FamilyInfo.Country);
        dl.AddParam("a_residential_status", data.FamilyInfo.ResidenceStatus);

        dl.AddParam("a_uncle_name", data.MosalInfo.UncleName);
        dl.AddParam("a_maternal_grandfather_name", data.MosalInfo.GrandFatherName);
        dl.AddParam("a_maternal_grandmother_name", data.MosalInfo.GrandMotherName);
        dl.AddParam("a_native", data.MosalInfo.Native);
        dl.AddParam("a_contact_number", data.MosalInfo.ContactNumber);
        dl.AddParam("a_mosal_address", data.MosalInfo.Address);

        dl.AddParam("a_username", Utility.CurrentUser);

        int id = dl.ExecuteProcedureReturnScalar<int>(Utility.ConnectionString, procedureName);
        ts.Complete();
        return id;
      }
    }

    public static BiodataTO FetchFamilyOccupationDetails(string code) {
      const string sql = @"select
                          ifnull(fo.father_occupation, 0) as father_occupation
                          , ifnull(fo.father_profession, '') as father_profession
                          , ifnull(fo.father_occupation_at, '') as father_occupation_at
                          , ifnull(fo.father_designation, '') as father_designation
                          , ifnull(fo.father_occupation_address, '') as father_occupation_address
                          , ifnull(fo.mother_occupation, 0) as mother_occupation
                          , ifnull(fo.mother_profession, '') as mother_profession
                          , ifnull(fo.mother_occupation_at, '') as mother_occupation_at
                          , ifnull(fo.mother_designation, '') as mother_designation
                          , ifnull(fo.mother_occupation_address, '') as mother_occupation_address
                          from
                          app_biodata_basic_infos b
                          left join
                          app_biodata_family_occupation_infos fo
                          on b.id = fo.biodata_id
                          where
                          b.code = ?code
                          and
                          b.active = true
                          and b.user_id = (select id from app_users where username = ?username and active = true);";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("code", code);
      dl.AddParam("username", Utility.CurrentUser);

      using(MySqlDataReader dr = dl.ExecuteSqlReturnReader(Utility.ConnectionString, sql)) {
        if(dr.Read()) {
          return new BiodataTO() {
            FatherOccupationInfo = new OccupationInfoTO() {
              Occupation = (enOccupation) dr.GetInt32("father_occupation"),
              ProfessionSector = dr.GetString("father_profession"),
              OrganizationName = dr.GetString("father_occupation_at"),
              OrganizationAddress = dr.GetString("father_occupation_address"),
              Designation = dr.GetString("father_designation")
            },
            MotherOccupationInfo = new OccupationInfoTO() {
              Occupation = (enOccupation) dr.GetInt32("mother_occupation"),
              ProfessionSector = dr.GetString("mother_profession"),
              OrganizationName = dr.GetString("mother_occupation_at"),
              OrganizationAddress = dr.GetString("mother_occupation_address"),
              Designation = dr.GetString("mother_designation")
            }
          };
        }
      }
      return null;
    }

    public static int SaveFamilyOccupationDetails(BiodataTO data) {
      using(TransactionScope ts = new TransactionScope(TransactionScopeOption.Required)) {
        string procedureName = @"save_family_occupation_biodata_info";

        GlobalDL dl = new GlobalDL();
        dl.AddParam("a_code", data.Code);

        dl.AddParam("a_father_occupation", data.FatherOccupationInfo.Occupation);
        dl.AddParam("a_father_profession", data.FatherOccupationInfo.ProfessionSector);
        dl.AddParam("a_father_occupation_at", data.FatherOccupationInfo.OrganizationName);
        dl.AddParam("a_father_designation", data.FatherOccupationInfo.Designation);
        dl.AddParam("a_father_occupation_address", data.FatherOccupationInfo.OrganizationAddress);

        dl.AddParam("a_mother_occupation", data.MotherOccupationInfo.Occupation);
        dl.AddParam("a_mother_profession", data.MotherOccupationInfo.ProfessionSector);
        dl.AddParam("a_mother_occupation_at", data.MotherOccupationInfo.OrganizationName);
        dl.AddParam("a_mother_designation", data.MotherOccupationInfo.Designation);
        dl.AddParam("a_mother_occupation_address", data.MotherOccupationInfo.OrganizationAddress);

        dl.AddParam("a_username", Utility.CurrentUser);

        int id = dl.ExecuteProcedureReturnScalar<int>(Utility.ConnectionString, procedureName);
        ts.Complete();
        return id;
      }
    }

    public static List<SibblingInfoTO> FetchSillingInfos(string code) {
      const string sql = @"select
                          ifnull(s.code, '') as sibbling_code
                          , ifnull(s.sibbling_name, '') as sibbling_name
                          , ifnull(s.sibbling_gender, 0) as sibbling_gender
                          , ifnull(s.sibbling_in_law_name, '') as sibbling_in_law_name
                          , ifnull(s.sibbling_in_law_native, '') as sibbling_in_law_native
                          from
                          app_biodata_basic_infos b
                          left join
                          app_biodata_sibbling_infos s
                          on b.id = s.biodata_id
                          where
                          b.code = ?code
                          and
                          b.active = true
                          and b.user_id = (select id from app_users where username = ?username and active = true)
                          and s.active = true;";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("code", code);
      dl.AddParam("username", Utility.CurrentUser);

      List<SibblingInfoTO> sibblingInfos = new List<SibblingInfoTO>();

      using(MySqlDataReader dr = dl.ExecuteSqlReturnReader(Utility.ConnectionString, sql)) {
        while(dr.Read()) {
          sibblingInfos.Add(new SibblingInfoTO() {
            Code = dr.GetString("sibbling_code"),
            Name = dr.GetString("sibbling_name"),
            Gender = (enGender) dr.GetInt32("sibbling_gender"),
            Family = dr.GetString("sibbling_in_law_name"),
            Native = dr.GetString("sibbling_in_law_native"),
            GenderString = dr.GetInt32("sibbling_gender") != 0 ? ((enGender) dr.GetInt32("sibbling_gender")).ToString() : string.Empty
          });
        }
      }
      return sibblingInfos;
    }

    public static int SaveSibblingInfo(string code, SibblingInfoTO data) {
      using(TransactionScope ts = new TransactionScope(TransactionScopeOption.Required)) {
        const string procedure = @"save_sibbling_biodata_info";

        GlobalDL dl = new GlobalDL();
        dl.AddParam("a_code", code);
        dl.AddParam("a_name", data.Name);
        dl.AddParam("a_gender", data.Gender);
        dl.AddParam("a_family_name", data.Family);
        dl.AddParam("a_native", data.Native);
        dl.AddParam("a_sibbling_code", data.Code);
        dl.AddParam("a_username", Utility.CurrentUser);

        int id = dl.ExecuteProcedureReturnScalar<int>(Utility.ConnectionString, procedure);
        ts.Complete();
        return id;
      }
    }

    public static int DeleteSibblingInfo(string code, string sibblingCode) {
      using(TransactionScope ts = new TransactionScope(TransactionScopeOption.Required)) {
        const string procedureName = @"delete_sibbling_biodata_info";

        GlobalDL dl = new GlobalDL();
        dl.AddParam("a_code", code);
        dl.AddParam("a_sibbling_code", sibblingCode);
        dl.AddParam("a_username", Utility.CurrentUser);

        int count = dl.ExecuteProcedureReturnScalar<int>(Utility.ConnectionString, procedureName);
        ts.Complete();
        return count;
      }
    }

    public static BiodataTO FetchAdditionDetails(string code) {
      const string sql = @"select
                          ifnull(b.profile_image, '') as profile_image
                          , ifnull(o.hobbies, '') as hobbies
                          , ifnull(o.interest, '') as interest
                          , ifnull(o.expectation, '') as expectations
                          from
                          app_biodata_basic_infos b
                          left join
                          app_biodata_other_infos o
                          on b.id = o.biodata_id
                          where
                          b.code = ?code
                          and
                          b.active = true
                          and b.user_id = (select id from app_users where username = ?username and active = true);";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("code", code);
      dl.AddParam("username", Utility.CurrentUser);

      using(MySqlDataReader dr = dl.ExecuteSqlReturnReader(Utility.ConnectionString, sql)) {
        if(dr.Read()) {
          return new BiodataTO() {
            ProfileImage = dr.GetString("profile_image"),
            AdditionalInfo = new AdditionalBiodataInfoTO() {
              Hobbies = dr.GetString("hobbies"),
              Interest = dr.GetString("interest"),
              Expectation = dr.GetString("expectations")
            }
          };
        }
      }
      return null;
    }

    public static string GetProfileImageNameByCode(string code) {
      const string sql = @"select ifnull(profile_image, '') 
                          from
                          app_biodata_basic_infos
                          where
                          code = ?code
                          and
                          active = true";

      GlobalDL dl = new GlobalDL();
      dl.AddParam("code", code);

      string fileName = dl.ExecuteSqlReturnScalar<string>(Utility.ConnectionString, sql);
      return fileName;
    }

    public static int UpdateProfileImageByCode(string code, string fileName) {
      using(TransactionScope ts =  new TransactionScope()) {
        const string sql = @"update app_biodata_basic_infos
                            set
                            profile_image = ?fileName
                            , approval_status = 1
                            , modified_by = ?username
                            , modified_at = now()
                            where code = ?code
                            and active = true;";

        GlobalDL dl = new GlobalDL();
        dl.AddParam("fileName", fileName);
        dl.AddParam("username", Utility.CurrentUser);
        dl.AddParam("code", code);

        int rowsAffected = dl.ExecuteSqlNonQuery(Utility.ConnectionString, sql);
        return rowsAffected;
      }
    }

   public static int SaveAdditionalInfo(BiodataTO data) {
      using(TransactionScope ts = new TransactionScope(TransactionScopeOption.Required)) {
        const string procedure = @"save_additional_biodata_info";

        GlobalDL dl = new GlobalDL();
        dl.AddParam("a_code", data.Code);
        dl.AddParam("a_hobbies", data.AdditionalInfo.Hobbies);
        dl.AddParam("a_interest", data.AdditionalInfo.Interest);
        dl.AddParam("a_expectation", data.AdditionalInfo.Expectation);        
        dl.AddParam("a_username", Utility.CurrentUser);

        int id = dl.ExecuteProcedureReturnScalar<int>(Utility.ConnectionString, procedure);
        ts.Complete();
        return id;
      }
    }
  }
}