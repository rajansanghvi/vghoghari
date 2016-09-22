DROP PROCEDURE IF EXISTS vghoghari.save_professional_biodata_info;
CREATE PROCEDURE vghoghari.`save_professional_biodata_info`(
  a_code varchar(100)
  , a_education tinyint(4)
  , a_degrees_achieved varchar(1000)
  , a_university_attended varchar(500)
  , a_addl_info varchar(1000)
  , a_occupation tinyint(4)
  , a_professional_sector varchar(500)
  , a_organization_name varchar(500)
  , a_designation varchar(500)
  , a_organization_addr varchar(1000)
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
      
      set @educationCount = 0;
      select count(id) into @educationCount from app_biodata_education_infos where biodata_id = @biodataId;
      
      if @educationCount > 0 then
        update app_biodata_education_infos
        set
        education = a_education
        , degrees_achieved = a_degrees_achieved
        , addl_info = a_addl_info
        , university_attended = a_university_attended
        where
        biodata_id = @biodataId;
      else
        insert into app_biodata_education_infos
        (biodata_id, education, degrees_achieved, addl_info, university_attended)
        values
        (@biodataId, a_education, a_degrees_achieved, a_addl_info, a_university_attended);
      end if;
      
       set @occupationCount = 0;
      select count(id) into @occupationCount from app_biodata_occupation_infos where biodata_id = @biodataId;
      
      if @occupationCount > 0 then
        update app_biodata_occupation_infos
        set
        occupation = a_occupation
        , profession = a_professional_sector
        , occupation_at = a_organization_name
        , designation = a_designation
        , address = a_organization_addr
        where
        biodata_id = @biodataId;
      else
        insert into app_biodata_occupation_infos
        (biodata_id, occupation, profession, occupation_at, designation, address)
        values
        (@biodataId, a_occupation, a_professional_sector, a_organization_name, a_designation, a_organization_addr);
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
