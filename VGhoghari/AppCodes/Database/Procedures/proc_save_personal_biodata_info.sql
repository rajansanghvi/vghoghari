CREATE PROCEDURE vghoghari.`save_personal_biodata_info`(
  a_code varchar(100)
  , a_religion varchar(200)
  , a_caste varchar(200)
  , a_sub_caste varchar(200)
  , a_manglik tinyint(4)
  , a_self_gothra varchar(200)
  , a_maternal_gothra varchar(200)
  , a_star_sign tinyint(4)
  , a_height_ft tinyint(4)
  , a_height_inch tinyint(4)
  , a_weight tinyint(4)
  , a_blood_group varchar(5)
  , a_body_type tinyint(4)
  , a_complexion varchar(20)
  , a_optics tinyint(4)
  , a_diet varchar(20)
  , a_smoke varchar(20)
  , a_drink varchar(20)
  , a_deformity varchar(1000)
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
        
      set @religionCount = 0;
      select count(id) into @religionCount from app_biodata_religion_infos where biodata_id = @biodataId;
        
      if @religionCount > 0 then
        update app_biodata_religion_infos
        set
        religion = a_religion
        , caste =  a_caste
        , subcaste =a_sub_caste
        where
        biodata_id = @biodataId;  
      else
        insert into app_biodata_religion_infos
        (biodata_id, religion, caste, subcaste)
        values
        (@biodataId, a_religion, a_caste, a_sub_caste);
      end if;
      
      set @socialCount = 0;
      select count(id) into @socialCount from app_biodata_social_infos where biodata_id = @biodataId;
      
      if @socialCount > 0 then
        update app_biodata_social_infos
        set
        manglik = a_manglik
        , self_gothra = a_self_gothra
        , maternal_gothra = a_maternal_gothra
        , star_sign = a_star_sign
        where
        biodata_id = @biodataId;
      else
        insert into app_biodata_social_infos
        (biodata_id, manglik, self_gothra, maternal_gothra, star_sign)
        values
        (@biodataId, a_manglik, a_self_gothra, a_maternal_gothra, a_star_sign);
      end if;
      
      set @physicalCount = 0;
      select count(id) into @physicalCount from app_biodata_physical_infos where biodata_id = @biodataId;
      
      if @physicalCount > 0 then
        update app_biodata_physical_infos
        set
        height_ft = a_height_ft
        , height_inch = a_height_inch
        , weight = a_weight
        , blood_group = a_blood_group
        , body_type = a_body_type
        , complexion = a_complexion
        , optic = a_optics
        , diet = a_diet
        , smoke = a_smoke
        , drink = a_drink
        , deformity = a_deformity
        where
        biodata_id = @biodataId;
      else
        insert into app_biodata_physical_infos
        (biodata_id, height_ft, height_inch, weight, blood_group, body_type, complexion, optic, diet, smoke, drink, deformity)
        values
        (@biodataId, a_height_ft, a_height_inch, a_weight, a_blood_group, a_body_type, a_complexion, a_optics, a_diet, a_smoke, a_drink, a_deformity);
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