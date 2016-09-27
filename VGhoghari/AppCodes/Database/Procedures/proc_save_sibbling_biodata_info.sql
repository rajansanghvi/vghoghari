CREATE PROCEDURE vghoghari.`save_sibbling_biodata_info`(
  a_code varchar(100)
  , a_name varchar(500)
  , a_gender tinyint(4)
  , a_family_name varchar(500)
  , a_native varchar(200)
  , a_sibbling_code varchar(100)
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
    set @biodataCount = 0;
    select count(id) into @biodataCount from app_biodata_basic_infos where code = a_code; 
    
    if @biodataCount > 0 then
      set @biodataId = 0;
      select id into @biodataId from app_biodata_basic_infos where code = a_code;
      
      set @newSibblingId = 0;
      
      insert into app_biodata_sibbling_infos
      (biodata_id, sibbling_name, sibbling_gender, sibbling_in_law_name, sibbling_in_law_native, active, code)
      values
      (@biodataId, a_name, a_gender, a_family_name, a_native, true, a_sibbling_code);
      
      select LAST_INSERT_ID() into @newSibblingId;
      
      set @status = 0;
      select approval_status into @status from app_biodata_basic_infos where id = @biodataId;
      
      if @status > 0 then
        set @status = 1;
      end if;
        
      update app_biodata_basic_infos
      set
      approval_status = @status
      , modified_by = a_username
      , modified_at = now()
      where
      id = @biodataId;
      
      select @newSibblingId;
    else
      rollback;
    end if;
  commit;
END;
