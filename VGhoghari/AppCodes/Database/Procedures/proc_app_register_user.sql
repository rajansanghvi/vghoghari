CREATE PROCEDURE vghoghari.`app_register_user`(
  a_code varchar(100)
  , a_authKey varchar(100)
  , a_username varchar(200)
  , a_hashedPassword varchar(500)
  , a_fullname varchar(500)
  , a_mobileNo varchar(200)
  , a_emailId varchar(200)
  , a_religion varchar(200)
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
      set @userId = 0;
      
      insert into app_users(code, auth_key, username, hashed_password, fullname, gender, dob, religion, 
      active, effective_from, effective_to, created_by, created_at, modified_by, modified_at)
      values
      (a_code, a_authKey, a_username, a_hashedPassword, a_fullname, 0, null, a_religion,
      1, CURRENT_DATE(), null, a_username, NOW(), a_username, NOW());
      
      select last_insert_id() into @userId;
      
      if @userId > 0 then
        insert into app_user_role_rel(user_id, role_id, active, effective_from, effective_to, created_by, created_at,
        modified_by, modified_at)
        values
        (@userId, (select id from app_roles where name = 'user' and active = true), 1, CURRENT_DATE(), null, 'system', NOW(), 'system', 
        NOW());
        
        insert into app_users_addl(user_id, landline_number, mobile_number, email_id, facebook_url, address, pincode, 
        city, state, country)
        values
        (@userId, null, a_mobileNo, a_emailId, null, null, null, null, null, null);  
      end if;
      
      select @userId;
    commit;
END;