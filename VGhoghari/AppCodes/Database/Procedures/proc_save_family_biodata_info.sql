DROP PROCEDURE IF EXISTS vghoghari.save_family_biodata_info;
CREATE PROCEDURE vghoghari.`save_family_biodata_info`(
  a_code varchar(100)
  , a_father_name varchar(500)
  , a_father_mobile_number varchar(20)
  , a_mother_name varchar(500)
  , a_mother_mobile_number varchar(20)
  , a_grandfather_name varchar(500)
  , a_grandmother_name varchar(500)
  , a_no_of_brothers tinyint(4)
  , a_no_of_sisters tinyint(4)
  , a_family_type tinyint(4)
  , a_landline_number varchar(20)
  , a_address varchar(1000)
  , a_city varchar(200)
  , a_state varchar(200)
  , a_country varchar(200)
  , a_residential_status tinyint(4)
  , a_uncle_name varchar(500)
  , a_maternal_grandfather_name varchar(500)
  , a_maternal_grandmother_name varchar(500)
  , a_native varchar(200)
  , a_contact_number varchar(20)
  , a_mosal_address varchar(1000)
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
      
      set @familyCount = 0;
      select count(id) into @familyCount from app_biodata_family_infos where biodata_id = @biodataId;
      
      if @familyCount > 0 then
        update app_biodata_family_infos
        set
        father_name = a_father_name
        , father_mobile_number = a_father_mobile_number
        , mother_name = a_mother_name
        , mother_mobile_number = a_mother_mobile_number
        , grandfather_name =  a_grandfather_name
        , grandmother_name = a_grandmother_name
        , no_of_brothers = a_no_of_brothers
        , no_of_sisters = a_no_of_sisters
        , family_type = a_family_type
        , landline_number = a_landline_number
        , address = a_address
        , city = a_city
        , state = a_state
        , country = a_country
        , residential_status = a_residential_status
        where
        biodata_id = @biodataId;
      else
        insert into app_biodata_family_infos
        (biodata_id, father_name, father_mobile_number, mother_name, mother_mobile_number,
        grandfather_name, grandmother_name, no_of_brothers, no_of_sisters, family_type, landline_number,
        address, city, state, country, residential_status)
        values
        (@biodataId, a_father_name, a_father_mobile_number, a_mother_name, a_mother_mobile_number,
        a_grandfather_name, a_grandmother_name, a_no_of_brothers, a_no_of_sisters, a_family_type, a_landline_number,
        a_address, a_city, a_state, a_country, a_residential_status);
      end if;
      
      set @mosalCount = 0;
      select count(id) into @mosalCount from app_biodata_mosal_infos where biodata_id = @biodataId;
      
      if @mosalCount > 0 then
        update app_biodata_mosal_infos
        set
        uncle_name = a_uncle_name
        , maternal_grandfather_name = a_maternal_grandfather_name
        , maternal_grandmother_name = a_maternal_grandmother_name
        , native = a_native
        , contact_number = a_contact_number
        , address = a_mosal_address
        where
        biodata_id = @biodataId;
      else
        insert into app_biodata_mosal_infos
        (biodata_id, uncle_name, maternal_grandfather_name, maternal_grandmother_name, native, contact_number, address)
        values
        (@biodataId, a_uncle_name, a_maternal_grandfather_name, a_maternal_grandmother_name, a_native, a_contact_number, a_mosal_address);
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
