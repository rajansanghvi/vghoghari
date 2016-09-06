var isValid = false;

$(document).ready(function () {

  var code = getQueryStringValue('code');
  
  //fetchPersonalBiodataInfo(code, function () {});

  $('#religion').on('hidden.bs.select', function (e) {
    religionValidation();
  });

  $('#caste').on('hidden.bs.select', function (e) {
    casteValidation();
    if ($('#caste').val() === 'Other') {
      $('#sub-caste').prop('disabled', false);
    }
    else {
      $('#sub-caste').val('');
      $('#sub-caste-error').empty();
      $('#sub-caste').prop('disabled', true);
    }
  });

  $('#sub-caste').focusout(function () {
    subCasteValidation();
  });

  $('#manglik').on('hidden.bs.select', function (e) {
    manglikValidation();
  });

  $('#self-gothra').focusout(function () {
    selfGotraValidation();
  });

  $('#maternal-gothra').focusout(function () {
    maternalGotraValidation();
  });

  $('#height-ft').on('hidden.bs.select', function (e) {
    heightFtValidation();
  });

  $('#height-inch').on('hidden.bs.select', function (e) {
    heightInchValidation();
  });

  $('#weight').focusout(function () {
    weightValidation();
  });

  $('#body-type').on('hidden.bs.select', function (e) {
    bodyTypeValidation();
  });

  $('#complexion').on('hidden.bs.select', function (e) {
    complexionValidation();
  });

  $('#optics').on('hidden.bs.select', function (e) {
    opticsValidation();
  });

  $('#diet').on('hidden.bs.select', function (e) {
    dietValidation();
  });

  $('#smoke').on('hidden.bs.select', function (e) {
    smokeValidation();
  });

  $('#drink').on('hidden.bs.select', function (e) {
    drinkValidation();
  });

  $('#deformity').focusout(function () {
    optionalDeformityValidation();
  });

  $('#reset').on('click', function () {
    resetData();
  });

  $('#submit').on('click', function () {
    savePersonalInfo(code);
  });
});

function fetchPersonalBiodataInfo(code, callback) {
  var url = BASEURL + '/api/MatrimonialApi/GetPersonalInfo?code=' + code;

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

function religionValidation() {
  $('#religion-error').empty();

  var errMsg = validateDropdown($('#religion').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#religion-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function casteValidation() {
  $('#caste-error').empty();

  var errMsg = validateDropdown($('#caste').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#caste-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function subCasteValidation() {
  $('#sub-caste-error').empty();

  var errMsg = validateReligion($('#sub-caste').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#sub-caste-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function manglikValidation() {
  $('#manglik-error').empty();

  var errMsg = validateDropdown($('#manglik').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#manglik-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function selfGotraValidation() {
  $('#self-gothra-error').empty();

  var errMsg = validateOptionalReligion($('#self-gothra').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#self-gothra-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function maternalGotraValidation() {
  $('#maternal-gothra-error').empty();

  var errMsg = validateOptionalReligion($('#maternal-gothra').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#maternal-gothra-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function heightFtValidation() {
  $('#height-ft-error').empty();

  var errMsg = validateDropdown($('#height-ft').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#height-ft-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function heightInchValidation() {
  $('#height-inch-error').empty();

  var errMsg = validateDropdownAllowingZero($('#height-inch').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#height-inch-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function weightValidation() {
  $('#weight-error').empty();

  var errMsg = validateOptionalNumberField($('#weight').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#weight-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function bodyTypeValidation() {
  $('#body-type-error').empty();

  var errMsg = validateDropdown($('#body-type').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#body-type-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function complexionValidation() {
  $('#complexion-error').empty();

  var errMsg = validateDropdown($('#complexion').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#complexion-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function opticsValidation() {
  $('#optics-error').empty();

  var errMsg = validateDropdown($('#optics').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#optics-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function dietValidation() {
  $('#diet-error').empty();

  var errMsg = validateDropdown($('#diet').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#diet-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function smokeValidation() {
  $('#smoke-error').empty();

  var errMsg = validateDropdown($('#smoke').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#smoke-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function drinkValidation() {
  $('#drink-error').empty();

  var errMsg = validateDropdown($('#drink').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#drink-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function optionalDeformityValidation() {
  $('#deformity-error').empty();

  var errMsg = validateOptionalFreeText($('#deformity').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#deformity-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function resetData() {
  $('#religion').selectpicker('val', 'Jain');
  $('#caste').selectpicker('val', '');

  $('#sub-caste').val('');
  $('#sub-caste').prop('disabled');

  $('#manglik').selectpicker('val', '0');

  $('#self-gothra').val('');
  $('#maternal-gothra').val('');

  $('#star-sign').selectpicker('val', '0');

  $('#height-ft').selectpicker('val', '0');
  $('#height-inch').selectpicker('val', '-1');

  $('#weight').val('');

  $('#blood-group').selectpicker('val', '');
  $('#body-type').selectpicker('val', '0');
  $('#complexion').selectpicker('val', '');
  $('#optics').selectpicker('val', '0');
  $('#diet').selectpicker('val', '');
  $('#smoke').selectpicker('val', '');
  $('#drink').selectpicker('val', '');

  $('#deformity').val('');

  $('#religion-error').empty();
  $('#caste-error').empty();
  $('#sub-caste-error').empty();
  $('#manglik-error').empty();
  $('#self-gothra-error').empty();
  $('#maternal-gothra-error').empty();
  $('#star-sign-error').empty();
  $('#height-ft-error').empty();
  $('#height-inch-error').empty();
  $('#weight-error').empty();
  $('#blood-group-error').empty();
  $('#body-type-error').empty();
  $('#complexion-error').empty();
  $('#optics-error').empty();
  $('#diet-error').empty();
  $('#smoke-error').empty();
  $('#drink-error').empty();
  $('#deformity-error').empty();
  $('#message').empty();
  $('#message').addClass('hide');
}

function savePersonalInfo() {
  religionValidation();
  casteValidation();

  if ($('#caste').val() === 'Other') {
    subCasteValidation();
  }

  manglikValidation();
  selfGotraValidation();
  maternalGotraValidation();
  heightFtValidation();
  heightInchValidation();
  weightValidation();
  bodyTypeValidation();
  complexionValidation();
  opticsValidation();
  dietValidation();
  smokeValidation();
  drinkValidation();
  optionalDeformityValidation();

  if (isValid) {
    var dataObject = new Object();

    dataObject.Code = code;

    dataObject.Religion = $('#religion').val().trim();
    dataObject.Caste = $('#caste').val().trim();
    dataObject.SubCaste = $('#sub-caste').val().trim();

    dataObject.Manglik = $('#manglik').val().trim();
    dataObject.SelfGothra = $('#self-gothra').val().trim();
    dataObject.MaternalGothra = $('#maternal-gothra').val().trim();
    dataObject.StarSign = $('#star-sign').val().trim();

    dataObject.HeightFt = $('#height-ft').val().trim();
    dataObject.HeightInch = $('#height-inch').val().trim();
    dataObject.Weight = $('#weight').val().trim();
    dataObject.BloodGroup = $('#blood-group').val().trim();
    dataObject.BodyType = $('#body-type').val().trim();
    dataObject.Complexion = $('#complexion').val().trim();
    dataObject.Optics = $('#optics').val().trim();
    dataObject.Diet = $('#diet').val().trim();
    dataObject.Smoke = $('#smoke').val().trim();
    dataObject.Drink = $('#drink').val().trim();
    dataObject.Deformity = $('#deformity').val().trim();

    var url = BASEURL + '/api/MatrimonialApi/SavePersonalInfo';

    $.ajax({
      url: url,
      type: 'POST',
      dataType: 'json',
      contentType: 'application/json',
      data: JSON.stringify(dataObject),
      statusCode: {
        400: function () {
          $('#message').html('<strong> Error! </strong>There are certain invalid or empty fields. Please fill in all the required information correctly and try again.');
          $('#message').removeClass('hide');
        },
        201: function (responseData) {
          $(location).attr('href', BASEURL + '/Matrimonial/AddPersonalInfo?code=' + responseData);
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
  else {
    $('#message').html('<strong> Error! </strong>There are certain invalid or empty fields. Please fill in all the required information correctly and try again.');
    $('#message').removeClass('hide');
  }
}