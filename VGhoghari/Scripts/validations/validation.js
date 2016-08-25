﻿var fullNamePattern = /^([A-Za-z]{1,})([ ]{0,1})([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})?([ ]{0,1})?([A-Za-z]{1,})$/;
var usernamePattern = /^(?!.*\.\.)(?!.*\.$)[^\W][\w.]{0,29}$/;
var passwordPattern = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d!$%@#£€*?&]{8,16}$/;
var mobileNoPattern = /^(\+)?(91)?( )?[789]\d{9}$/;
var emailIdPattern = /^([\w\.\-_]+)?\w+@[\w-_]+(\.\w+){1,}$/;
var religionPattern = /^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;"',./?]{2,}$/;
var landlinePattern = /^[0-9]{3,5}[-][0-9]{8}$/;
var facebookPattern = /^((http|https):\/\/|)(www\.|)facebook\.com\/[a-zA-Z0-9.]{1,}$/;
var addressPattern = /^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;"',./?]{2,}$/;
var geographyPattern = /^(?=.*[a-zA-Z\d].*)[a-zA-Z\d !@#$%&*()\-_+=:;"',./?]{2,}$/;
var pincodePattern = /^\d{6}$/;
var validImageExtensions = ['.jpg', '.jpeg', '.gif', '.png'];

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
  else if (content.length < 10 || content.length > 20) {
    return 'This field should have a minimum of 10 digits and a maximum of 20 digits.';
  }
  else if (!content.match(mobileNoPattern)) {
    return 'This is an invalid mobile number. Please enter a mobile number in +91 7755661234 or 7755661234 format. Please note only Indian mobile numbers are supported.';
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

function validateOptionalLandline(content) {
  if (content !== undefined && content !== null && content !== '' && content.length > 0) {
    if (content.length < 12 || content.length > 14) {
      return 'This field should have a minimum of 12 digits and a maximum of 14 digits.';
    }
    else if (!content.match(landlinePattern)) {
      return 'This is an invalid landline number. Please enter a landline number in 022-25566778 format.';
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
    if (content.length !== 6) {
      return 'This field should have exactly 6 digits.';
    }
    else if (!content.match(pincodePattern)) {
      return 'This field can have only numbers [0-6] and can have exactly 6 digits.';
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