var isValid = false;
var isProfileImagePresent = false;
$(document).ready(function () {

  var code = getQueryStringValue('code');
  
  $(document).on('change', ':file', function () {
    var input = $(this),
        numFiles = input.get(0).files ? input.get(0).files.length : 1,
        label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
    console.log(input.get(0).files.length)
    if (validateImageType(label)) {
      isValid = true;
      $('#image-upload-info').removeClass('text-danger');
      // uploadProfileImage(userCode);
    }
    else {
      isValid = false;
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
  
  fetchAdditionalBiodataInfo(code, function (response) {
    if (response !== undefined && response !== null) {
      fillAdditionalInfo(response);
    }
    else {
      $(location).attr('href', BASEURL + '/Matrimonial/Manage');
    }
  });

  $('#hobbies').focusout(function () {
    hobbiesValidation();
  });

  $('#interest').focusout(function () {
    interestValidation();
  });

  $('#expectations').focusout(function () {
    expectationsValidation();
  });

  $('#reset').on('click', function () {
    resetData();
  });

  $('#submit').on('click', function () {
    saveAdditionalInfo(code);
  });
});

function fetchAdditionalBiodataInfo(code, callback) {
  var url = BASEURL + '/api/MatrimonialApi/GetAdditionalDetails?code=' + code;

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

function fillAdditionalInfo(responseData) {
  if (responseData.ProfileImage !== undefined && responseData.ProfileImage !== null && responseData.ProfileImage !== '' && responseData.ProfileImage.length > 0) {
    isProfileImagePresent = true;
    $('#biodata-profile-image').attr("src", BASEURL + "/AppData/biodata/" + responseData.ProfileImage);
  }

  $('#hobbies').val(responseData.AdditionalInfo.Hobbies);
  $('#interest').val(responseData.AdditionalInfo.Interest);
  $('#expectations').val(responseData.AdditionalInfo.Expectation);
}

function hobbiesValidation() {
  $('#hobbies-error').empty();

  var errMsg = validateOptionalFreeText($('#hobbies').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#hobbies-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function interestValidation() {
  $('#interest-error').empty();

  var errMsg = validateOptionalFreeText($('#interest').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#interest-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function expecationsValidation() {
  $('#expecations-error').empty();

  var errMsg = validateOptionalFreeText($('#expectations').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#expecations-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function resetData() {
  $('#profile-image').val('');
  $('#image-upload-info').removeClass('text-danger');
  $('#file-name').val('');

  $('#hobbies').val('');
  $('#interest').val('');
  $('#expectations').val('');

  $('#hobbies-error').empty();
  $('#interest-error').empty();
  $('#expectations-error').empty();
  
  $('#message').empty();
  $('#message').addClass('hide');
}

function readFileData(file, success, error) {
  var reader = new FileReader();

  reader.onload = function () {
    console.log(reader.result);
    success(reader.result);
  };
  reader.onerror = function (error) {
    console.log(error);
    error(error);
  };

  reader.readAsDataURL(file);
}

function saveAdditionalInfo(code) {
  hobbiesValidation();
  interestValidation();
  expecationsValidation();

  if (!isProfileImagePresent) {
    if ($('#profile-image').get(0).files.length === 0) {
      isValid = false;
      $('#image-upload-info').addClass('text-danger');
    }
    else {
      var label = $('#profile-image').val().replace(/\\/g, '/').replace(/.*\//, '');
      if (validateImageType(label)) {
        isValid = true;
        $('#image-upload-info').removeClass('text-danger');
      }
      else {
        isValid = false;
        $('#image-upload-info').addClass('text-danger');
      }
    }
  }
  else {
    if ($('#profile-image').get(0).files.length !== 0) {
      var label = $('#profile-image').val().replace(/\\/g, '/').replace(/.*\//, '');
      if (validateImageType(label)) {
        isValid = true;
        $('#image-upload-info').removeClass('text-danger');
      }
      else {
        isValid = false;
        $('#image-upload-info').addClass('text-danger');
      }
    }
  }

  if (isValid) {
    var dataObject = new Object();

    dataObject.Code = code;

    dataObject.Hobbies = $('#hobbies').val().trim();
    dataObject.Interest = $('#interest').val();
    dataObject.Expectations = $('#expectations').val().trim();

    if ($('#profile-image').get(0).files.length === 0) {
      dataObject.ProfileImageData = '';
      save(dataObject);
      return;
    }

    readFileData($('#profile-image').get(0).files[0],
      function (responseData) {
        dataObject.ProfileImageData = responseData;
        save(dataObject);
      },
      function (error) {
        $('#message').html('<strong> Error! </strong>Some Error while processing your profile image. Please refresh and try again.');
        $('#message').removeClass('hide');
      });
  }
  else {
    $('#message').html('<strong> Error! </strong>There are certain invalid or empty fields. Please fill in all the required information correctly and try again.');
    $('#message').removeClass('hide');
  }
}

function save(object) {
  var url = BASEURL + '/api/MatrimonialApi/SaveAdditionalInfo';

  $.ajax({
    url: url,
    type: 'POST',
    dataType: 'json',
    contentType: 'application/json',
    data: JSON.stringify(object),
    statusCode: {
      400: function () {
        $('#message').html('<strong> Error! </strong>There are certain invalid or empty fields. Please fill in all the required information correctly and try again.');
        $('#message').removeClass('hide');
      },
      200: function () {
        $(location).attr('href', BASEURL + '/Matrimonial/Manage');
      },
      401: function () {
        $(location).attr('href', BASEURL + '/User/Logout');
      },
      403: function () {
        $(location).attr('href', BASEURL + '/Matrimonial/Manage');
      }
    }
  });
}