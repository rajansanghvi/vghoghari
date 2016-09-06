/*Inital Script for Vghoghari Website with all the Required Tables and Initial Setup Data required
for the system to work*/

use vghoghari;

/*Roles tables defining the different roles available for the users of the application*/
create table if not exists app_roles
(
  id int(11) not null auto_increment primary key,
  code varchar(50) not null,
  name varchar(200) not null unique,
  description varchar(500) null,
  active tinyint(4) not null,
  effective_from date not null,
  effective_to date null,
  created_by varchar(200) not null,
  created_at datetime not null,
  modified_by varchar(200) not null,
  modified_at datetime not null
)engine InnoDB;

/*Inserting initial default roles during system setup*/
insert into app_roles
(code, name, description, active, effective_from, effective_to, created_by, created_at, modified_by, modified_at)
values
('super_admin', 'super_admin', 'Super Admin User with all rights including creating new admins or super admins' , 1, CURRENT_DATE(), null, 'system', NOW(), 'system', NOW());

insert into app_roles
(code, name, description, active, effective_from, effective_to, created_by, created_at, modified_by, modified_at)
values
('admin', 'admin', 'Admin User with all rights excluding creating new admins or super admins' , 1, CURRENT_DATE(), null, 'system', NOW(), 'system', NOW());

insert into app_roles
(code, name, description, active, effective_from, effective_to, created_by, created_at, modified_by, modified_at)
values
('user', 'user', 'Registered User with rights to access the website but not the admin panel' , 1, CURRENT_DATE(), null, 'system', NOW(), 'system', NOW());

/*Users Table having general info about all types of Users of the Website*/
create table if not exists app_users
(
	id int(11) not null auto_increment primary key,
  code varchar(100) not null,
  auth_key varchar(100) not null,
  username varchar(200) not null unique,
  hashed_password varchar(500) not null,
  fullname varchar(500) not null,
  gender tinyint(4) not null default 0,
  dob date null,
  religion varchar(200) null,
  profile_image varchar(255) null,
  active tinyint(4) not null,
  effective_from date not null,
  effective_to date null,
  created_by varchar(200) not null,
  created_at datetime not null,
  modified_by varchar(200) not null,
  modified_at datetime not null
)engine InnoDB;

/*User Role Relation to assign roles to each user*/
create table if not exists app_user_role_rel
(
	id int(11) not null auto_increment primary key,
  user_id int(11) not null,
  foreign key (user_id) references app_users(id)
  on delete cascade on update cascade,
  role_id int(11) not null,
  foreign key (role_id) references app_roles(id)
  on delete cascade on update cascade,
  active tinyint(4) not null,
  effective_from date not null,
  effective_to date null,
  created_by varchar(200) not null,
  created_at datetime not null,
  modified_by varchar(200) not null,
  modified_at datetime not null
)engine InnoDB;

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

/*Call the Procedure to create Rajan Sanghvi as first user with the Super Admin roles*/
CALL app_create_default_admin();

/*User's additional information like contact details or address*/
create table if not exists app_users_addl
(
  id int(11) not null auto_increment primary key,
  user_id int(11) not null,
  foreign key(user_id) references app_users(id)
  on delete cascade on update cascade,
  landline_number varchar(20) null,
  mobile_number varchar(20) null,
  email_id varchar(200) null,
  facebook_url varchar(255) null,
  address varchar(1000) null,
  pincode varchar(10) null,
  city varchar(200) null,
  state varchar(200) null,
  country varchar(200) null
)engine InnoDb;

/*Procedure to Register a new User*/
CREATE PROCEDURE vghoghari.`app_register_user`(
  code varchar(100)
  , authKey varchar(100)
  , username varchar(200)
  , hashedPassword varchar(500)
  , fullname varchar(500)
  , mobileNo varchar(200)
  , emailId varchar(200)
  , religion varchar(200)
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
      (code, authKey, username, hashedPassword, fullname, 0, null, religion,
      1, CURRENT_DATE(), null, username, NOW(), username, NOW());
      
      select last_insert_id() into @userId;
      
      if @userId > 0 then
        insert into app_user_role_rel(user_id, role_id, active, effective_from, effective_to, created_by, created_at,
        modified_by, modified_at)
        values
        (@userId, (select id from app_roles where name = 'user'), 1, CURRENT_DATE(), null, 'system', NOW(), 'system', 
        NOW());
        
        insert into app_users_addl(user_id, landline_number, mobile_number, email_id, facebook_url, address, pincode, 
        city, state, country)
        values
        (@userId, null, mobileNo, emailId, null, null, null, null, null, null);
        
      end if;
      
      select @userId;
    commit;
END;

/*Update User Profile*/
CREATE PROCEDURE vghoghari.`update_user_profile`(
  a_userCode varchar(100)
  , a_fullname varchar(500)
  , a_gender tinyint(4)
  , a_dob date
  , a_religion varchar(200)
  , a_landlineNo varchar(20)
  , a_mobileNo varchar(20)
  , a_emailId varchar(200)
  , a_facebookUrl varchar(255)
  , a_address varchar(1000)
  , a_pincode varchar(10)
  , a_city varchar(200)
  , a_state varchar(200)
  , a_country varchar(200)
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
  
    set @userId = 0;
    set @userAddlId = 0;
    
    select id into @userId FROM app_users where code = a_userCode and active = true;
    
    if @userId > 0 then
    
      update app_users
      set
      fullname = a_fullname
      , religion = a_religion
      , gender = a_gender
      , dob = a_dob
      , modified_by = a_username
      , modified_at = NOW()
      where
      id = @userId;
      
      select count(id) into @userAddlId FROM app_users_addl where user_id = @userId;
      
      if @userAddlId = 0 then
        insert into app_users_addl
        (user_id, landline_number, mobile_number, email_id, facebook_url, address, pincode, city, state, country)
        values
        (@userId, a_landlineNo, a_mobileNo, a_emailId, a_facebookUrl, a_address, a_pincode, a_city, a_state, a_country);
      else
        update app_users_addl
        set
        landline_number = a_landlineNo
        , mobile_number = a_mobileNo
        , email_id = a_emailId
        , facebook_url = a_facebookUrl
        , address = a_address
        , pincode = a_pincode
        , city = a_city
        , state = a_state
        , country = a_country
        where
        user_id = @userId;
      end if;
    end if;
    select @userId;
  commit;
END;

create table if not exists app_countries
(
  id int(11) not null auto_increment primary key,
  code int(11) not null,
  shortname varchar(3) not null,
  name varchar(200) not null,
  value varchar(200) null
)engine InnoDb;

create table if not exists app_states
(
  id int(11) not null auto_increment primary key,
  code int(11) not null,
  name varchar(200) not null,
  value varchar(200) null,
  country_id int(11) not null,
  foreign key(country_id) REFERENCES app_countries(id)
  on delete cascade on update CASCADE
)engine innoDb;

create table if not exists app_cities
(
  id int(11) not null auto_increment primary key,
  code int(11) not null,
  name varchar(200) not null,
  value varchar(200) null,
  state_id int(11) not null,
  foreign key(state_id) references app_states(id)
  on delete cascade on UPDATE CASCADE
)engine InnoDb;





/*Matrimonial section*/
/*1. For Basic Biodata Info and Admin Purpose*/
create table if not exists app_biodata_basic_infos
(
  id int(11) not null auto_increment primary key,
  code varchar(100) not null,
  fullname varchar(500) not null,  
  gender tinyint(4) not null,
  dob date not null,
  age tinyint(4) not null,
  birth_time time null,
  native varchar(200) not null,
  marital_status tinyint(4) not null,
  birth_place varchar(200) null,
  about_me varchar(1000) null,
  profile_image varchar(255) null,
  user_id  int(11) not null,
  foreign key(user_id) references app_users(id)
  on delete no action on update cascade,
  approval_status tinyint(4) not null,
  admin_action_by varchar(200) null,
  admin_action_at datetime null,
  admin_notes varchar(1000) null,
  active tinyint(4) not null,
  effective_from date not null,
  effective_to date null,
  created_by varchar(200) not null,
  created_at datetime not null,
  modified_by varchar(200) not null,
  modified_at  datetime not null
)engine InnoDB;

create table if not exists app_biodata_contact_infos
(
  id int(11) not null auto_increment primary key,
  biodata_id int(11) not null,
  foreign key(biodata_id) references app_biodata_basic_infos(id)
  on delete cascade on update cascade,
  landline_number varchar(20) null,
  mobile_number varchar(20) not null,
  email_id varchar(200) null,
  facebook_url varchar(255) null,
  address varchar(1000) not null,
  pincode varchar(10) null,
  city varchar(200) null,
  state varchar(200) null,
  country varchar(200) not null,
  residential_status tinyint(4) not null
)engine InnoDB;

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

create table app_biodata_religion_infos
(
  id int(11) not null auto_increment primary key,
  biodata_id int(11) not null,
  foreign key(biodata_id) references app_biodata_basic_infos(id)
  on delete cascade on update cascade,
  religion varchar(200) not null,
  caste varchar(200) null,
  subcaste varchar(200) null
)engine InnoDB;

create table if not exists app_biodata_social_infos
(
  id int(11) not null auto_increment primary key,
  biodata_id int(11) not null,
  foreign key(biodata_id) references app_biodata_basic_infos(id)
  on delete cascade on update cascade,
  manglik tinyint(4) not null,
  self_gothra varchar(200) null,
  maternal_gothra varchar(200) null,
  star_sign tinyint(4) null
)engine InnoDB;

create table if not exists app_biodata_physical_infos
(
  id int(11) not null auto_increment primary key,
  biodata_id int(11) not null,
  foreign key(biodata_id) references app_biodata_basic_infos(id)
  on delete cascade on update cascade,
  height_ft tinyint(4) not null,
  height_inch tinyint(4) not null,
  weight int(4) null,
  blood_group varchar(5) null,
  body_type tinyint(4) not null,
  complexion varchar(20) not null,
  optic tinyint(4) not null,
  diet varchar(20) not null,
  smoke varchar(20) not null,
  drink varchar(20) not null,
  deformity varchar(1000) null
)engine InnoDB;

create table if not exists app_biodata_education_infos
(
  id int(11) not null auto_increment primary key,
  biodata_id int(11) not null,
  foreign key(biodata_id) references app_biodata_basic_infos(id)
  on delete cascade on update cascade,
  education tinyint(4) not null,
  degrees_achieved varchar(1000) null,
  university_attended varchar(500) null
)engine InnoDB;

create table if not exists app_biodata_occupation_infos
(
  id int(11) not null auto_increment primary key,
  biodata_id int(11) not null,
  foreign key(biodata_id) references app_biodata_basic_infos(id)
  on delete cascade on update cascade,
  occupation tinyint(4) not null,
  profession varchar(255) null,
  occupation_at varchar(500) null,
  designation varchar(200) null,
  address varchar(1000) null
)engine InnoDB;

create table if not exists app_biodata_family_infos
(
  id int(11) not null auto_increment primary key,
  biodata_id int(11) not null,
  foreign key(biodata_id) references app_biodata_basic_infos(id)
  on delete cascade on update cascade,
  father_name varchar(500) not null,
  father_mobile_number varchar(20) not null,
  mother_name varchar(500) not null,
  mother_mobile_number varchar(20) null,
  grandfather_name varchar(500) not null,
  grandmother_name varchar(500) null,
  no_of_brothers tinyint(4) not null default 0,
  no_of_sisters tinyint(4) not null default 0,
  father_occupation tinyint(4) not null,
  father_profession varchar(255) null,
  father_occupation_at varchar(500) null,
  father_designation varchar(200) null,
  father_office_address varchar(1000) null,
  family_type tinyint(4) null,
  landline_number varchar(20) null,
  address varchar(1000) not null,
  city varchar(200) null,
  state varchar(200) null,
  country varchar(200) not null,
  residential_status tinyint(4) not null
)engine InnoDB;

create table app_biodata_mosal_infos
(
  id int(11) not null auto_increment primary key,
  biodata_id int(11) not null,
  foreign key(biodata_id) references app_biodata_basic_infos(id)
  on delete cascade on update cascade,
  uncle_name varchar(500) null,
  maternal_grandfather_name varchar(500) not null,
  maternal_grandmother_name varchar(500) null,
  native varchar(200) not null,
  contact_number varchar(20) null,
  address varchar(1000) null
)engine InnoDB;

create table app_biodata_sibbling_infos
(
  id int(11) not null auto_increment primary key,
  biodata_id int(11) not null,
  foreign key(biodata_id) references app_biodata_basic_infos(id)
  on delete cascade on update cascade,
  sibbling_name varchar(500) not null,
  sibbling_gender tinyint(4) not null,
  sibbling_in_law_name varchar(500) null,
  sibbling_in_law_native varchar(200) null,
  active tinyint(4) not null
)engine InnoDb;

create table app_biodata_other_infos
(
  id int(11) not null auto_increment primary key,
  biodata_id int(11) not null,
  foreign key(biodata_id) references app_biodata_basic_infos(id)
  on delete cascade on update cascade,
  hobbies varchar(1000) null,
  interest varchar(1000) null,
  expectation varchar(1000) null
)engine InnoDB;

/*End of matrimonial section*/


/*User contact Rel table storing contact details related to the Users*/
create table if not exists app_user_contact_rel
(
	id int(11) not null auto_increment primary key,
  user_id int(11) not null,
  contact_id int(11) not null,
  active tinyint(4) not null,
  effective_from date not null,
  effective_to date null,
  created_by varchar(200) not null,
  created_at datetime not null,
  modified_by varchar(200) not null,
  modified_at datetime not null,
  foreign key (user_id) references app_users(id)
  on delete cascade on update cascade,
  foreign key (contact_id) references app_contacts(id)
  on delete cascade on update cascade
)engine InnoDB;

/*Contact Table having contact details of users and any other part of the system related to contacts*/
create table if not exists app_contacts
(
	id int(11) not null auto_increment primary key,
  contact_person varchar(500) null,
  contact_value varchar(200) not null,
  contact_type tinyint(4) not null,
  active tinyint(4) not null,
  effective_from date not null,
  effective_to date null,
  created_by varchar(200) not null,
  created_at datetime not null,
  modified_by varchar(200) not null,
  modified_at datetime not null    
)engine InnoDB;

/*Address table*/
create table if not exists app_addresses
(
  id int(11) not null auto_increment primary key,
  address text null,
  pincode varchar(10) null,
  city varchar(255) null,
  state varchar(255) null,
  country varchar(255) null,
  address_status tinyint(4) null,
  address_type tinyint(4) null,
  active tinyint(4) not null,
  effective_from date not null,
  effective_to date null,
  created_by varchar(200) not null,
  created_at datetime not null,
  modified_by varchar(200) not null,
  modified_at datetime not null
)engine InnoDB;

/*Media table*/
create table if not exists app_media
(
  id int(11) not null auto_increment primary key,
  filename varchar(100) not null,
  humanised_filename varchar(255) null,
  path varchar(1000) null,
  mime_type varchar(255) not null,
  size int(11) not null default 0,
  active tinyint(4) not null,
  effective_from date not null,
  effective_to date null,
  created_by varchar(200) not null,
  created_at datetime not null,
  modified_by varchar(200) not null,
  modified_at datetime not null
)engine InnoDB;

/*Start of Event Section*/
/*Event Table*/
create table if not exists app_events
(
  id int(11) not null auto_increment primary key,
  code varchar(100) not null,
  title varchar(500) not null,
  short_description text not null,
  long_description mediumtext null,
  start_time datetime not null,
  end_time datetime null,
  venue varchar(255) not null,
  address varchar(1000) null,
  price int(6) not null default 0,
  capacity int(6) not null default 0,
  contact_person varchar(500) null,
  contact_number varchar(25) null,
  contact_email varchar(200) null,
  attending_count int(11) not null default 0,
  not_attending_count int(11) not null default 0,
  attending_maybe int(11) not null default 0,
  primary_image_id int(11) null,
  foreign key(primary_image_id) references app_media(id)
  on delete set null on update cascade,
  active tinyint(4) not null,
  effective_from date not null,
  effective_to date null,
  created_by varchar(200) not null,
  created_at datetime not null,
  modified_by varchar(200) not null,
  modified_at datetime not null  
)engine InnoDB;

/*Event Media Relation*/
create table if not exists app_event_media_rel
(
  id int(11) not null auto_increment primary key,
  event_id int(11) not null,
  foreign key(event_id) references app_events(id)
  on delete cascade on update cascade,
  media_id int(11) not null,
  foreign key(media_id) references app_media(id)
  on delete cascade on update cascade,
  active tinyint(4) not null,
  effective_from date not null,
  effective_to date null,
  created_by varchar(200) not null,
  created_at datetime not null,
  modified_by varchar(200) not null,
  modified_at datetime not null
)engine InnoDB;

/*Event Categories*/
create table if not exists app_event_categories
(
	id int(11) not null auto_increment primary key,
  name varchar(100) not null,
  description varchar(500) null,
  active tinyint(4) not null,
  effective_from date not null,
  effective_to date null,
  created_by varchar(200) not null,
  created_at datetime not null,
  modified_by varchar(200) not null,
  modified_at datetime not null
)engine InnoDB;

/*Add default event categories*/
insert into app_event_categories
(name, description, active, effective_from, effective_to, created_by, created_at, modified_by, modified_at )
values
('Business', 'Everything related to the Business and Profession', 1, CURRENT_DATE(), null, 'system', now(), 'system', now());

insert into app_event_categories
(name, description, active, effective_from, effective_to, created_by, created_at, modified_by, modified_at )
values
('Education', 'Everything related to the Education', 1, CURRENT_DATE(), null, 'system', now(), 'system', now());

insert into app_event_categories
(name, description, active, effective_from, effective_to, created_by, created_at, modified_by, modified_at )
values
('Social', 'Everything related to the Social Activities', 1, CURRENT_DATE(), null, 'system', now(), 'system', now());

/*Event Categories Rel*/
create table if not exists app_event_category_rel
(
	id int(11) not null auto_increment primary key,
  event_id int(11) not null,
  foreign key (event_id) references app_events(id)
  on delete cascade on update cascade,
  category_id int(11) not null,
  foreign key (category_id) references app_event_categories(id)
  on delete cascade on update cascade,
  active tinyint(4) not null,
  effective_from date not null,
  effective_to date null,
  created_by varchar(200) not null,
  created_at datetime not null,
  modified_by varchar(200) not null,
  modified_at datetime not null
)engine InnoDB;

/*User's Interest Count*/
create table if not exists app_event_interested_users
(
	id int(11) not null auto_increment primary key,
  user_id int(11) not null,
  foreign key (user_id) references app_users(id)
  on delete cascade on update cascade,
  event_id int(11) not null,
  foreign key (event_id) references app_events(id)
  on delete cascade on update cascade,
  interest_status tinyint(4) not null,
  active tinyint(4) not null,
  effective_from date not null,
  effective_to date null,
  created_by varchar(200) not null,
  created_at datetime not null,
  modified_by varchar(200) not null,
  modified_at datetime not null
)engine InnoDB;
/*End of event Section*/

/*Add event*/
CREATE PROCEDURE vghoghari.`app_add_event`(
code varchar(100)
, title varchar(500)
, shortDescription text
, description mediumtext
, startTime datetime
, endTime datetime
, venue varchar(255)
, address varchar(1000)
, contactPerson varchar(500)
, contactNumber varchar(25)
, contactEmail varchar(100)
, username varchar(200)
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
    set @eventId = 0;
    
    insert into app_events
    (code, title, short_description, long_description, start_time, end_time, venue,  address, contact_person, contact_number, contact_email, primary_image_id, attending_count, not_attending_count, attending_maybe,
    active, effective_from, effective_to, created_by, created_at, modified_by, modified_at)
    values
    (code, title, shortDescription, description, startTime, endTime, venue, address, contactPerson, contactNumber, contactEmail, null, 0, 0, 0,
    1, current_date(), null, username, now(), username, now());
    
    select LAST_INSERT_ID() into @eventId;
    
    select code FROM app_events where id = @eventId;
  commit;
END;