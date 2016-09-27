CREATE PROCEDURE vghoghari.`save_family_occupation_biodata_info`(
  a_code varchar(100)
  , a_father_occupation tinyint(4)
  , a_father_profession varchar(500)
  , a_father_occupation_at varchar(500)
  , a_father_designation varchar(500)
  , a_father_occupation_address varchar(1000)
  , a_mother_occupation tinyint(4)
  , a_mother_profession varchar(500)
  , a_mother_occupation_at varchar(500)
  , a_mother_designation varchar(500)
  , a_mother_occupation_address varchar(1000)
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
      
      set @familyOccupationCount = 0;
      select count(id) into @familyOccupationCount from app_biodata_family_occupation_infos where biodata_id = @biodataId;
      
      if @familyOccupationCount > 0 then
        update app_biodata_family_occupation_infos
        set
        father_occupation = a_father_occupation
        , father_profession = a_father_profession
        , father_occupation_at = a_father_occupation_at
        , father_designation = a_father_designation
        , father_occupation_address = a_father_occupation_address
        , mother_occupation = a_mother_occupation
        , mother_profession = a_mother_profession
        , mother_occupation_at = a_mother_occupation_at
        , mother_designation = a_mother_designation
        , mother_occupation_address = a_mother_occupation_address
        where
        biodata_id = @biodataId;
      else
        insert into app_biodata_family_occupation_infos
        (biodata_id, father_occupation, father_profession, father_occupation_at, father_designation, father_occupation_address
        , mother_occupation, mother_profession, mother_occupation_at, mother_designation, mother_occupation_address)
        values
        (@biodataId, a_father_occupation, a_father_profession, a_father_occupation_at, a_father_designation, a_father_occupation_address
        , a_mother_occupation, a_mother_profession, a_mother_occupation_at, a_mother_designation, a_mother_occupation_address);
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
