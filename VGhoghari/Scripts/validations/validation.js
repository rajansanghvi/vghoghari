var fullNamePattern = /^([A-Za-z]{1,})([ ]{0,1})([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})$/;
var usernamePattern = /^(?!.*\.\.)(?!.*\.$)[^\W][\w.]{0,29}$/;
var passwordPattern = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d!$%@#£€*?&]{8,16}$/;
var mobileNoPattern = /^(\+)?(\d){0,3}( )?\d{4,15}$/;
var emailIdPattern = /^([\w\.\-_]+)?\w+@[\w-_]+(\.\w+){1,}$/;
var religionPattern = /^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;"',./?]{2,}$/;
var landlinePattern = /^(\+)?(\d){0,3}( )?(\d){0,3}( )?\d{4,11}$/;
var facebookPattern = /^((http|https):\/\/|)(www\.|)facebook\.com\/[a-zA-Z0-9.]{1,}$/;
var addressPattern = /^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;"',./?]{2,}$/;
var geographyPattern = /^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;"',./?]{2,}$/;
var freeTextPattern = /^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;"',./?]{2,}$/;
var pincodePattern = /^\d{3,10}$/;
var validImageExtensions = ['.jpg', '.jpeg', '.gif', '.png'];

function validateDropdown(content) {
  if (content === undefined || content === null || content === '' || content.length === 0 || content == 0) {
    return 'This is a required field.';
  }
  return '';
}

function validateDropdownAllowingZero(content) {
  if (content === undefined || content === null || content === '' || content.length === 0 || content == -1) {
    return 'This is a required field.';
  }
  return '';
}

function validateFullName(content) {
  if (content === undefined || content === null || content === '' || content.length === 0) {
    return 'This is a required field.';
  }
  else if (content.length < 2 || content.length > 500) {
    return 'This field should have a minimum of 2 characters and a maximum of 500 characters.';
  }
  else if (!content.match(fullNamePattern)) {
    return 'This field can consits of only letters [A-Z or a-z] and spaces between them. It can not start or end with a space. It should have a minimum of 2 letters and a maximum of 4 words.';
  }
  return '';
}

function validateUserName(content) {
  if (content === undefined || content === null || content === '' || content.length === 0) {
    return 'This is a required field.';
  }
  else if (content.length > 30) {
    return 'This field can consists a maximum of 30 charaters.';
  }
  else if (!content.match(usernamePattern)) {
    return 'This field can consist of letters (A-Z or a-z), numbers (0-9), underscores(_) and periods(.). It cannot start or end with a period nor can have more than one period sequentially. It can have a maximum of 30 characters.';
  }
  return '';
}

function validatePassword(content) {
  if (content === undefined || content === null || content === '' || content.length === 0) {
    return 'This is a required field.';
  }
  else if (content.length < 8 || content.length > 16) {
    return 'This field should have a minimum of 8 characters and a maximum of 16 characters.';
  }
  else if (!content.match(passwordPattern)) {
    return 'This field should have 1 letter (A-Z or a-z) and 1 Number (0-9). It can optionally have special characters (@, $, !, %, *, ?, &, #, *). This field should have a minimum of 8 characters and a maximum of 16 characters.';
  }
  return '';
}

function validateConfirmPassword(content, passwordContent) {
  if (content === undefined || content === null || content === '' || content.length === 0) {
    return 'This is a required field.';
  }
  else if (content !== passwordContent) {
    return 'The password does not match the one entered above. Please re-enter to confirm your password.';
  }
  return '';
}

function validateMobileNo(content) {
  if (content === undefined || content === null || content === '' || content.length === 0) {
    return 'This is a required field.';
  }
  else if (content.length < 4 || content.length > 20) {
    return 'This field should have a minimum of 4 digits and a maximum of 20 digits.';
  }
  else if (!content.match(mobileNoPattern)) {
    return 'This is an invalid mobile number. Please enter a mobile number in +917755661234 or 7755661234 format. Please specify country code for numbers other than Indian Mobile Numbers.';
  }
  return '';
}

function validateOptionalEmailId(content) {
  if (content !== undefined && content !== null && content !== '' && content.length > 0) {
    if (content.length > 200) {
      return 'This field can consists of a maximum of 200 characters.';
    }
    else if (!content.match(emailIdPattern)) {
      return 'This is an invalid email-id. Please enter a valid email-id.';
    }
    return '';
  }
  return '';
}

function validateOptionalReligion(content) {
  if (content !== undefined && content !== null && content !== '' && content.length > 0) {
    if (content.length > 200) {
      return 'This field can consists of a maximum of 200 characters.';
    }
    else if (!content.match(religionPattern)) {
      return 'This field should have atleast one letter or a number. It can not consists of the following special charaters (`, ~, ^, , { }, [ ], <>, \\, |).';
    }
    return '';
  }
  return '';
}

function validateReligion(content) {
  if (content === undefined || content === null || content === '' || content.length === 0) {
    return 'This is a required field';
  }
  else if (content.length > 200) {
    return 'This field can consists of a maximum of 200 characters.';
  }
  else if (!content.match(religionPattern)) {
    return 'This field should have atleast one letter or a number. It can not consists of the following special charaters (`, ~, ^, , { }, [ ], <>, \\, |).';
   }
  return '';
}

function validateDob(content) {
  if (content === undefined || content === null || content === '' || content.length === 0) {
    return 'This is a required field.';
  }
  else {
    if (moment(content, 'YYYY-MM-DD').isValid()) {
      var dob = moment(content, 'YYYY-MM-DD');
      var now = moment();
      if (dob.isAfter(now)) {
        return 'You can not have a future date as your birth date. Please select a valid date of birth.';
      }
      return '';
    }
    return 'This field contains an invalid date. Please select a valid date using the available calendar option.';
  }
}

function validateOptionalDob(content) {
  if (content !== undefined && content !== null && content !== '' && content.length > 0) {
    if (moment(content, 'YYYY-MM-DD').isValid()) {
      var dob = moment(content, 'YYYY-MM-DD');
      var now = moment();
      if (dob.isAfter(now)) {
        return 'You can not have a future date as your birth date. Please select a valid date of birth.';
      }
      return '';
    }
    return 'This field contains an invalid date. Please select a valid date using the available calendar option.';
  }
  return '';
}

function validateAge(content) {
  var dob = moment(content, 'YYYY-MM-DD');
  var today = moment();

  var age = new Date(today - dob).getFullYear() - 1970;

  if (age < 18) {
    return 'You must be 18 years or above to register yourselves as a bride or a groom.';
  }
  else if (age > 100) {
    return 'It seems you have entered a wrong date. Please enter a valid date.';
  }
  else if (isNaN(age)) {
    return "This is an invalid age. Please peovide a valid date of birth to calculate your age.";
  }
  else {   
    return age;
  }
}

function validateOptionalLandline(content) {
  if (content !== undefined && content !== null && content !== '' && content.length > 0) {
    if (content.length < 4 || content.length > 20) {
      return 'This field should have a minimum of 4 digits and a maximum of 20 digits.';
    }
    else if (!content.match(landlinePattern)) {
      return 'This is an invalid landline number. Please enter a landline number in +91225566778 or 02225566778 format.';
    }
  }
  return '';
}

function validateOptionalFacebookUrl(content) {
  if (content !== undefined && content !== null && content !== '' && content.length > 0) {
    if (content.length > 255) {
      return 'This field can have a maximum of 255 characters.';
    }
    else if (!content.match(facebookPattern)) {
      return 'This is an invalid facebook url. Please enter a valid link to your facebook profile.';
    }
  }
  return '';
}

function validateAddress(content) {
  if (content === undefined || content === null || content === '' || content.length === 0) {
    return 'This is a required field.';
  }
  else if (content.length > 1000) {
    return 'This field can have a maximum of 1000 characters.';
  }
  else if (!content.match(addressPattern)) {
    return 'This field should have atleast one letter or a number. It can not consists of the following special charaters (`, ~, ^, , { }, [ ], <>, \\, |).';
  }
  return '';
}

function validateOptionalAddress(content) {
  if (content !== undefined && content !== null && content !== '' && content.length > 0) {
    if (content.length > 1000) {
      return 'This field can have a maximum of 1000 characters.';
    }
    else if (!content.match(addressPattern)) {
      return 'This field should have atleast one letter or a number. It can not consists of the following special charaters (`, ~, ^, , { }, [ ], <>, \\, |).';
    }
  }
  return '';
}

function validateGeography(content) {
  if (content === undefined || content === null || content === '' || content.length === 0) {
    return 'This is a required field.';
  }
  else if (content.length > 200) {
    return 'This field can consists of a maximum of 200 characters.';
  }
  else if (!content.match(geographyPattern)) {
    return 'This field should have atleast one letter or a number. It can not consists of the following special charaters (`, ~, ^, , { }, [ ], <>, \\, |).';
  }
  return '';
}

function validateOptionalGeography(content) {
  if (content !== undefined && content !== null && content !== '' && content.length > 0) {
    if (content.length > 200) {
      return 'This field can consists of a maximum of 200 characters.';
    }
    else if (!content.match(geographyPattern)) {
      return 'This field should have atleast one letter or a number. It can not consists of the following special charaters (`, ~, ^, , { }, [ ], <>, \\, |).';
    }
    return '';
  }
  return '';
}

function validateOptionalPincode(content) {
  if (content !== undefined && content !== null && content !== '' && content.length > 0) {
    if (content.length < 3 || content.length > 10) {
      return 'This field can consists of a minimum of 3 digits and a maximum of 10 digits.';
    }
    else if (!content.match(pincodePattern)) {
      return 'This field can have only numbers [0-9] and can have a minimum of 3 digits and a maximum of 10.';
    }
    return '';
  }
  return '';
}

function isFilled(content) {
  if (content === undefined || content === null || content === '' || content.length === 0) {
    return false;
  }
  return true;
}

function validateOptionalFreeText(content) {
  if (content !== undefined && content !== null && content !== '' && content.length > 0) {
    if (content.length > 1000) {
      return 'This field can consists of a maximum of 1000 characters.';
    }
    else if (!content.match(freeTextPattern)) {
      return 'This field should have atleast one letter or a number. It can not consists of the following special charaters (`, ~, ^, , { }, [ ], <>, \\, |).';
    }
    return '';
  }
  return '';
}

function validateOptionalTime(content) {
  if (content !== undefined && content !== null && content !== '' && content.length > 0) {
    if (!moment(content, 'HH:mm').isValid()) {
      return 'This is an invalid time. Please select a valid time using available time selector.' 
    }
  }
  return '';
}

function validateImageType(fileName) {
  if (fileName.length > 0) {
    var blnValid = false;
    for (var j = 0; j < validImageExtensions.length; j++) {
      var curExtension = validImageExtensions[j];
      if (fileName.substr(fileName.length - curExtension.length, curExtension.length).toLowerCase() === curExtension.toLowerCase()) {
        blnValid = true;
        break;
      }
    }
    return blnValid;
  }
  return false;
}

function validateOptionalNumberField(content) {
  if(content !== undefined && content !== null && content !== '' && content.length > 0) {
    if(isNaN(content)){
      return 'This is not a valid number.';
    }
  }
  return '';
}