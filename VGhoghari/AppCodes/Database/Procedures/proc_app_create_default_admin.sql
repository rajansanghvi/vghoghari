/*Procedure to create default user*/
CREATE PROCEDURE app_create_default_admin ()
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
    ((select UUID()), (select UUID()), 'rajansanghvi', (select MD5('RS12!@ps')), 'Rajan Sanghvi', 1, '1992-04-18', 'Jain',
    1, CURRENT_DATE(), null, 'system', NOW(), 'system', NOW());
    
    select LAST_INSERT_ID() INTO @userId;
    
    if @userId > 0 then
      insert into app_user_role_rel(user_id, role_id, active, effective_from, effective_to, created_by, created_at, modified_by, modified_at)
      values
      (@userId, 1, 1, CURRENT_DATE(), null, 'system', NOW(), 'system', NOW());
      
      insert into app_user_role_rel(user_id, role_id, active, effective_from, effective_to, created_by, created_at, modified_by, modified_at)
      values
      (@userId, 2, 1, CURRENT_DATE(), null, 'system', NOW(), 'system', NOW());
      
      insert into app_user_role_rel(user_id, role_id, active, effective_from, effective_to, created_by, created_at, modified_by, modified_at)
      values
      (@userId, 3, 1, CURRENT_DATE(), null, 'system', NOW(), 'system', NOW());
    end if;
  commit;
END;