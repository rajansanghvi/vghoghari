CREATE PROCEDURE vghoghari.`save_additional_biodata_info`(
  a_code varchar(100)
  , a_hobbies varchar(1000)
  , a_interest varchar(1000)
  , a_expectation varchar(1000)
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
      
      set @additionInfoCount = 0;
      select count(id) into @additionInfoCount from app_biodata_other_infos where biodata_id = @biodataId;
      
      if @additionInfoCount > 0 then
        update app_biodata_other_infos
        set
        hobbies = a_hobbies
        , interest = a_interest
        , expectation = a_expectation
        where
        biodata_id = @biodataId;
      else
        insert into app_biodata_other_infos
        (biodata_id, hobbies, interest, expectation)
        values
        (@biodataId, a_hobbies, a_interest, a_expectation);
      end if;
      
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
      
      select @biodataId;
    else
      rollback;
    end if;
  commit;
END;