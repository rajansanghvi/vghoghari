DROP PROCEDURE IF EXISTS vghoghari.delete_sibbling_biodata_info;
CREATE PROCEDURE vghoghari.`delete_sibbling_biodata_info`(
  a_code varchar(100)
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
      
      set @sibblingCount = 0;
      
      select count(id) into @sibblingCount from app_biodata_sibbling_infos where code = a_sibbling_code and biodata_id = @biodataId;
      
      if @sibblingCount > 0 then
        update app_biodata_sibbling_infos
        set
        active = false
        where
        code = a_sibbling_code;
        
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
      else
        rollback;
      end if;      
      select @sibblingCount;
    else
      rollback;
    end if;
  commit;
END;
