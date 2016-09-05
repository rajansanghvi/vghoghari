var isValid = false;

$(document).ready(function () {

  $('#username').focusout(function () {
    usernameValidation();
  });

  $('#password').focusout(function () {
    passwordValidation();
  });

  $('#reset').on('click', function () {
    resetData();
  });

  $('#submit').on('click', function () {
    loginUser();
  })
});

function usernameValidation() {
  $('#username-error').empty();

  var errMsg = validateUserName($('#username').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#username-error').html(errMsg);
  }
  else {
    isValid = true;
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

function resetData() {
  $('#username').val('');
  $('#password').val('');
  
  $('#username-error').empty();
  $('#password-error').empty();
  $('#message').empty();
}

function loginUser() {
  if (isValid) {
    usernameValidation();
    passwordValidation();

    if (isValid) {
      var dataObject = new Object();
      dataObject.Username = $('#username').val().trim();
      dataObject.Password = $('#password').val().trim();
      dataObject.IsPersistent = $('#persistent-login').is(':checked');

      var url = BASEURL + '/api/UserApi/LoginUser';
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
            $('#message').html('<div class="alert alert-error fade in">Invalid login credentials. Please use your username and password combination to log in to your account.</div>');
          },
          200: function () {
            $(location).attr('href', BASEURL + '/Home/Index');
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