var isValid = false;

$(document).ready(function () {

  var userCode = '';

  $('#dob').datetimepicker({
    format: 'YYYY-MM-DD',
    useCurrent: false,
    maxDate: moment(),
    icons: {
      time: 'fa fa-clock-o',
      date: 'fa fa-calendar',
      up: 'fa fa-chevron-up',
      down: 'fa fa-chevron-down',
      previous: 'fa fa-chevron-left',
      next: 'fa fa-chevron-right',
      today: 'fa fa-home',
      clear: 'fa fa-trash',
      close: 'fa fa-trash'
    }
  });

  $(document).on('change', ':file', function () {
    var input = $(this),
        numFiles = input.get(0).files ? input.get(0).files.length : 1,
        label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
    if (validateImageType(label)) {
      $('#image-upload-info').removeClass('text-danger');
      uploadProfileImage(userCode);
    }
    else {
      $('#image-upload-info').addClass('text-danger');
    }
    input.trigger('fileselect', [numFiles, label]);
  });

  $(':file').on('fileselect', function (event, numFiles, label) {
    var input = $(this).parents('.input-group').find(':text'),
        log = numFiles > 1 ? numFiles + ' files selected' : label;

    if (input.length) {
      input.val(log);
    } else {
      if (log) alert(log);
    }
  });

  fetchUserProfileDetails(function (userDetails) {
    if (userDetails === undefined || userDetails === null) {
      $(location).attr('href', BASEURL + '/User/Login');
    }
    else {
      userCode = userDetails.Code;
      fillForm(userDetails);
    }
  });

  $('#fullname').focusout(function () {
    fullnameValidation();
  });

  $('#religion').focusout(function () {
    optionalReligionValidation();
  });

  $('#dob').focusout(function () {
    optionalDobValidation();
  });

  $('#landline-no').focusout(function () {
    optionalLandlineValidation();
  });

  $('#mobile-no').focusout(function () {
    mobileNoValidation();
  });

  $('#email-id').focusout(function () {
    optionalEmailValidation();
  });

  $('#facebook-url').focusout(function () {
    optionalFacebookUrlValidation();
  });

  $('#address').focusout(function () {
    optionalAddressValidation();
  });

  $('#pincode').focusout(function () {
    optionalPincodeValidation();
  });

  $('#city').focusout(function () {
    optionalCityValidation();
  });

  $('#state').focusout(function () {
    optionalStateValidation();
  });

  $('#country').focusout(function () {
    optionalCountryValidation();
  });

  $('#submit').on('click', function () {
    updateProfile(userCode);
  });
});

function fetchUserProfileDetails(callback) {
  var url = BASEURL + '/api/UserApi/GetUserProfile';

  $.ajax({
    url: url,
    type: 'GET',
    dataType: 'json',
    contentType: 'application/json',
    success: function (responseData) {
      callback(responseData);
    }
  });
}

function fillForm(userDetails) {

  if (userDetails.ProfileImage !== undefined && userDetails.ProfileImage !== null && userDetails.ProfileImage !== '' && userDetails.ProfileImage.length > 0) {
    $('#profile-image').attr("src", BASEURL + "/AppData/profile_image/" + userDetails.ProfileImage);
  }

  $('#fullname').val(userDetails.Fullname);
  $('#religion').val(userDetails.Religion);
  $('#gender').val(userDetails.Gender);

  if (userDetails.Dob === undefined || userDetails.Dob === null || moment(userDetails.Dob).year() === 1870) {
    $('#dob').val('');
  }
  else {
    $('#dob').data("DateTimePicker").date(moment(userDetails.Dob, 'YYYY-MM-DD'));
  }

  $('#landline-no').val(userDetails.LandlineNumber);
  $('#mobile-no').val(userDetails.MobileNumber);
  $('#email-id').val(userDetails.EmailId);
  $('#facebook-url').val(userDetails.FacebookUrl);

  $('#address').val(userDetails.Address);
  $('#pincode').val(userDetails.Pincode);
  $('#city').val(userDetails.City);
  $('#state').val(userDetails.State);
  $('#country').val(userDetails.Country);
}

function fullnameValidation() {
  $('#fullname-error').empty();

  var errMsg = validateFullName($('#fullname').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#fullname-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function optionalReligionValidation() {
  $('#religion-error').empty();

  var errMsg = validateOptionalReligion($('#religion').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#religion-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function optionalDobValidation() {
  $('#dob-error').empty();

  var errMsg = validateOptionalDob($('#dob').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#dob-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function optionalLandlineValidation() {
  $('#landline-no-error').empty();

  var errMsg = validateOptionalLandline($('#landline-no').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#landline-no-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function mobileNoValidation() {
  $('#mobile-no-error').empty();

  var errMsg = validateMobileNo($('#mobile-no').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#mobile-no-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function optionalEmailValidation() {
  $('#email-id-error').empty();

  var errMsg = validateOptionalEmailId($('#email-id').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#email-id-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function optionalFacebookUrlValidation() {
  $('#facebook-url-error').empty();

  var errMsg = validateOptionalFacebookUrl($('#facebook-url').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#facebook-url-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function optionalAddressValidation() {
  $('#address-error').empty();

  var errMsg = validateOptionalAddress($('#address').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#address-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function optionalPincodeValidation() {
  $('#pincode-error').empty();

  var errMsg = validateOptionalPincode($('#pincode').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#pincode-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function optionalCityValidation() {
  $('#city-error').empty();

  var errMsg = validateOptionalGeography($('#city').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#city-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function optionalStateValidation() {
  $('#state-error').empty();

  var errMsg = validateOptionalGeography($('#state').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#state-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function optionalCountryValidation() {
  $('#country-error').empty();

  var errMsg = validateOptionalGeography($('#country').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#country-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function updateProfile(userCode) {
  if (isValid) {
    fullnameValidation();
    
    if (isFilled($('#religion').val().trim())) {
      optionalReligionValidation();
    }

    if (isFilled($('#dob').val().trim())) {
      optionalDobValidation();
    }

    if (isFilled($('#landline-no').val().trim())) {
      optionalLandlineValidation();
    }

    mobileNoValidation();

    if (isFilled($('#email-id').val().trim())) {
      optionalEmailValidation();
    }
    
    if (isFilled($('#facebook-url').val().trim())) {
      optionalFacebookUrlValidation();
    }

    if (isFilled($('#address').val().trim())) {
      optionalAddressValidation();
    }

    if (isFilled($('#pincode').val().trim())) {
      optionalPincodeValidation();
    }

    if (isFilled($('#city').val().trim())) {
      optionalCityValidation();
    }

    if (isFilled($('#state').val().trim())) {
      optionalStateValidation();
    }

    if (isFilled($('#country').val().trim())) {
      optionalCountryValidation();
    }

    if (isValid) {
      var dataObject = new Object();
      dataObject.Code = userCode;
      dataObject.Fullname = $('#fullname').val().trim();
      dataObject.Religion = $('#religion').val().trim();
      dataObject.Gender = $('#gender').val().trim();
      dataObject.Dob = $('#dob').val().trim();
      dataObject.LandlineNumber = $('#landline-no').val().trim();
      dataObject.MobileNumber = $('#mobile-no').val().trim();
      dataObject.EmailId = $('#email-id').val().trim();
      dataObject.FacebookUrl = $('#facebook-url').val().trim();
      dataObject.Address = $('#address').val().trim();
      dataObject.Pincode = $('#pincode').val().trim();
      dataObject.City = $('#city').val().trim();
      dataObject.State = $('#state').val().trim();
      dataObject.Country = $('#country').val().trim();

      var url = BASEURL + '/api/UserApi/UpdateUserProfile';
      $.ajax({
        url: url,
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(dataObject),
        statusCode: {
          400: function () {
            $('#message').html('<div class="alert alert-error fade in">There are certain invalid or empty fields. Please fill in all the required information correctly and try again.</div>');
          },
          200: function () {
            $(location).attr('href', BASEURL + '/User/MyProfile');
          }
        }
      });
    }
    else {
      $('#message').html('<div class="alert alert-error fade in">There are certain invalid or empty fields. Please fill in all the required information correctly and try again.</div>');
    }
  }
  else {
    $('#message').html('<div class="alert alert-error fade in">There are certain invalid or empty fields. Please fill in all the required information correctly and try again.</div>');
  }
}

function uploadProfileImage(userCode) {
  if (window.FormData !== undefined) {

    var fileUpload = $("#user-profile-image").get(0);
    var files = fileUpload.files;

    // Create FormData object  
    var fileData = new FormData();

    // Looping over all files and add it to FormData object  
    for (var i = 0; i < files.length; i++) {
      fileData.append(files[i].name, files[i]);
    }

    url = BASEURL + '/api/UserApi/UploadProfileImage?code=' + userCode;
    $.ajax({
      url: url,
      type: "POST",
      contentType: false, // Not to set any content header  
      processData: false, // Not to process data  
      data: fileData,
       statusCode: {
        400: function () {
          $('#message').html('<div class="alert alert-error fade in">The image uploaded is not valid. Please try again with a different image.</div>');
        },
        200: function (responseData) {
          $('#profile-image').attr('src', BASEURL + '/AppData/profile_image/' + responseData);
        },
        204: function () {
          $('#message').html('<div class="alert alert-error fade in">No profile image uploaded. Please select a valid image in either .jpg, .jpeg, .gif or .png format.</div>');
        }
      }
    });
  }
  else {
    $('#image-upload-info').html('Your browser does not support modern file upload functionality. Please update your browser to the latest version and try again.').addClass('text-danger');
  }
}