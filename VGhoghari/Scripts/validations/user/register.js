var isValid = false;

$(document).ready(function () {

  $('#fullname').focusout(function () {
    fullnameValidation();
  });

  $('#username').focusout(function () {
    usernameValidation();
  });

  $('#password').focusout(function () {
    passwordValidation();
  });

  $('#confirm-password').focusout(function () {
    confirmPasswordValidation();
  });

  $('#mobile-no').focusout(function () {
    mobileNoValidation();
  });

  $('#email-id').focusout(function () {
    optionalEmailValidation();
  });

  $('#religion').focusout(function () {
    optionalReligionValidation();
  });

  $('#reset').on('click', function () {
    resetData();
  });

  $('#submit').on('click', function () {
    registerUser();
  });
});

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

function usernameValidation() {
  $('#username-error').empty();

  var errMsg = validateUserName($('#username').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#username-error').html(errMsg);
  }
  else {
    isUsernameAvailable($('#username').val().trim(), function (responseData) {
      if (responseData) {
        isValid = true;
      }
      else {
        isValid = false;
        $('#username-error').html('Username selected by you is already in use. Please select a new username. Please note usernames are case insensitive.');
      }
    });
  }
}

function passwordValidation() {
  $('#password-error').empty();

  var errMsg = validatePassword($('#password').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#password-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function confirmPasswordValidation() {
  $('#confirm-password-error').empty();

  var errMsg = validateConfirmPassword($('#confirm-password').val().trim(), $('#password').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#confirm-password-error').html(errMsg);
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

function isUsernameAvailable(username, callback) {
  var url = BASEURL + '/api/UserApi/IsUsernameAvailable?username=' + username;
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

function resetData() {
  $('#fullname').val('');
  $('#username').val('');
  $('#password').val('');
  $('#confirm-password').val('');
  $('#mobile-no').val('');
  $('#email-id').val('');
  $('#religion').val('');

  $('#fullname-error').empty();
  $('#username-error').empty();
  $('#password-error').empty();
  $('#confirm-password-error').empty();
  $('#mobile-no-error').empty();
  $('#email-id-error').empty();
  $('#religion-error').empty();
  $('#message').empty();
}

function registerUser() {
  if (isValid) {
    fullnameValidation();
    usernameValidation();
    passwordValidation();
    confirmPasswordValidation();
    mobileNoValidation();
    if (isFilled($('#email-id').val().trim())) {
      optionalEmailValidation();
    }
    if (isFilled($('#religion').val().trim())) {
      optionalReligionValidation();
    }

    if (isValid) {
      var dataObject = new Object();
      dataObject.Fullname = $('#fullname').val().trim();
      dataObject.Username = $('#username').val().trim();
      dataObject.Password = $('#password').val().trim();
      dataObject.ConfirmPassword = $('#confirm-password').val().trim();
      dataObject.MobileNumber = $('#mobile-no').val().trim();
      dataObject.EmailId = $('#email-id').val().trim();
      dataObject.Religion = $('#religion').val().trim();

      var url = BASEURL + '/api/UserApi/RegisterUser';
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
          409: function () {
            $('#message').html('<div class="alert alert-error fade in">Username selected by you is already in use. Please select a new username. Please note usernames are case insensitive.</div>');
          },
          201: function () {
            $(location).attr('href', BASEURL + '/User/Login');
          }
        }
      });
    }
    else {
      $('#message').html('<div class="alert alert-error fade in">There are certain invalid or empty fields. Please fill in all the required information correctly and try again.</div>');
    }
  }
  else {
    $('#message').append('<div class="alert alert-error fade in">There are certain invalid or empty fields. Please fill in all the required information correctly and try again.</div>');
  }
}