DROP PROCEDURE IF EXISTS vghoghari.save_basic_biodata_info;
CREATE PROCEDURE vghoghari.`save_basic_biodata_info`(
  a_code varchar(100)
  , a_gender TINYINT(4)
  , a_fullname varchar(500)
  , a_dob date
  , a_birthtime time
  , a_age tinyint(4)
  , a_marital_status tinyint(4)
  , a_native varchar(200)
  , a_birth_place varchar(200)
  , a_about_me varchar(1000)
  , a_mobile_no varchar(20)
  , a_landline_no varchar(20)
  , a_email_id varchar(200)
  , a_facebook_url varchar(255)
  , a_address varchar(1000)
  , a_address_type tinyint(4)
  , a_country varchar(200)
  , a_state varchar(200)
  , a_city varchar(200)
  , a_pincode varchar(10)
  , a_username varchar(200)
)
BEGIN
  declare exit handler for sqlexception
    begin
      rollback;
    end;
    
  declare exit handler for sqlwarning
    begin
      rollback;
    end;
    
  start transaction;
    
    set @userCount = 0;
    
    select count(id) into @userCount from app_users where username = a_username;
    
    if @userCount > 0 then
      set @userId = 0;
      select id into @userId from app_users where username = a_username;
      
      set @biodataCount = 0;
      
      select count(id) into @biodataCount from app_biodata_basic_infos where code = a_code; 
      
      if @biodataCount > 0 then
        set @biodataId = 0;
        select id into @biodataId from app_biodata_basic_infos where code = a_code; 
        
        set @status = 0;
        select approval_status into @status from app_biodata_basic_infos where id = @biodataId;
        
        if @status > 0 then
          set @status = 1;
        end if;
        
        update app_biodata_basic_infos
        set
        fullname = a_fullname
        , gender = a_gender
        , dob = a_dob
        , age = a_age
        , birth_time = a_birthtime
        , native = a_native
        , marital_status = a_marital_status
        , birth_place = a_birth_place
        , about_me = a_about_me
        , approval_status = @status
        , modified_by = a_username
        , modified_at = now()
        where
        id = @biodataId;
      
        update app_biodata_contact_infos
        set
        landline_number = a_landline_no
        , mobile_number = a_mobile_no
        , email_id = a_email_id
        , facebook_url = a_facebook_url
        , address = a_address
        , pincode = a_pincode
        , city = a_city
        , state = a_state
        , country = a_country
        , residential_status = a_address_type
        where
        biodata_id = @biodataId;
      else
        insert into app_biodata_basic_infos
        (code, fullname, gender, dob, age, birth_time, native, marital_status, birth_place, about_me, profile_image
        , user_id, approval_status, admin_action_by, admin_action_at, admin_notes, active, effective_from, effective_to
        , created_by, created_at, modified_by, modified_at)
        values
        (a_code, a_fullname, a_gender, a_dob, a_age, a_birthtime, a_native, a_marital_status, a_birth_place, a_about_me, null
        , @userId, 0, null, null, null, 1, CURDATE(), null, a_username, now(), a_username, now());
      
        select last_insert_id() into @biodataId;
      
        insert into app_biodata_contact_infos
        (biodata_id, landline_number, mobile_number, email_id, facebook_url, address, pincode, city
        , state, country, residential_status)
        values
        (@biodataId, a_landline_no, a_mobile_no, a_email_id, a_facebook_url, a_address, a_pincode, a_city
        , a_state, a_country, a_address_type);
      end if;  
      select @biodataId;
    else
      rollback;
    end if;
  commit;
END;
