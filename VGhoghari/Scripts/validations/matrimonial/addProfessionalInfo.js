var isValid = false;

$(document).ready(function () {

  var code = getQueryStringValue('code');
  
  fetchProfessionalBiodataInfo(code, function (response) {
    if (response !== undefined && response !== null) {
      fillProfessionalInfo(response);
    }
    else {
      $(location).attr('href', BASEURL + '/Matrimonial/Manage');
    }
  });

  $('#education').on('hidden.bs.select', function (e) {
    educationValidation();
  });

  //$('#degrees-achieved').on('hidden.bs.select', function (e) {
  //  degreesAchievedValidation();
  //});

  $('#university').focusout(function () {
    universityValidation();
  });

  $('#addl-info').focusout(function () {
    addlInfoValidation();
  });

  $('#occupation').on('hidden.bs.select', function (e) {
    occupationValidation();
  });

  $('#professional-sector').on('hidden.bs.select', function (e) {
    professionalSectorValidation();
  });

  $('#organization-name').focusout(function () {
    organizationNameValidation();
  });

  $('#designation').focusout(function () {
    designationValidation();
  });

  $('#organization-address').focusout(function () {
    organizationAddressValidation();
  });

  $('#reset').on('click', function () {
    resetData();
  });

  $('#submit').on('click', function () {
    saveProfessionalInfo(code);
  });
});

function fetchProfessionalBiodataInfo(code, callback) {
  var url = BASEURL + '/api/MatrimonialApi/GetProfessionalInfo?code=' + code;

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

function fillProfessionalInfo(data) {
  $('#education').val(data.EducationInfo.HighestEducation);
  $('#education').selectpicker('refresh');

  $('#university').val(data.EducationInfo.UniversityAttended);

  $('#addl-info').val(data.EducationInfo.AddlInfo);

  $('#occupation').val(data.OccupationInfo.Occupation);
  $('#occupation').selectpicker('refresh');

  $('#professional-sector').val(data.OccupationInfo.ProfessionSector);
  $('#professional-sector').selectpicker('refresh');

  $('#organization-name').val(data.OccupationInfo.OrganizationName);

  $('#designation').val(data.OccupationInfo.Designation);

  $('#organization-address').val(data.OccupationInfo.OrganizationAddress);

  $('#degrees-achieved').selectpicker('refresh');
  $('#degrees-achieved').selectpicker('val', data.EducationInfo.DegreesList);
}

function educationValidation() {
  $('#education-error').empty();

  var errMsg = validateDropdown($('#education').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#education-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function degreesAchievedValidation() {
  $('#degrees-achieved-error').empty();

  var errMsg = validateMultipleDropdown($('#degrees-achieved').val());
  if (errMsg !== '') {
    isValid = false;
    $('#degrees-achieved-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function universityValidation() {
  $('#university-error').empty();

  var errMsg = validateOptionalProperName($('#university').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#university-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function addlInfoValidation() {
  $('#addl-info-error').empty();

  var errMsg = validateOptionalFreeText($('#addl-info').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#addl-info-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function occupationValidation() {
  $('#occupation-error').empty();

  var errMsg = validateDropdown($('#occupation').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#occupation-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function professionalSectorValidation() {
  $('#professional-sector-error').empty();

  var errMsg = validateDropdown($('#professional-sector').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#professional-sector-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function organizationNameValidation() {
  $('#organization-name-error').empty();

  var errMsg = validateOptionalProperName($('#organization-name').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#organization-name-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function designationValidation() {
  $('#designation-error').empty();

  var errMsg = validateOptionalProperName($('#designation').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#designation-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function organizationAddressValidation() {
  $('#organization-address-error').empty();

  var errMsg = validateOptionalAddress($('#organization-address').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#organization-address-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function resetData() {
  $('#education').selectpicker('val', '0');
  $('#degrees-achieved').selectpicker('val', '');

  $('#university').val('');
  $('#addl-info').val('');

  $('#occupation').selectpicker('val', '0');
  $('#professional-sector').selectpicker('val', '');

  $('#organization-name').val('');
  $('#designation').val('');
  $('#organization-address').val('');

  $('#education-error').empty();
  $('#degrees-achieved-error').empty();
  $('#university-error').empty();
  $('#addl-info-error').empty();

  $('#occupation-error').empty();
  $('#professional-sector-error').empty();
  $('#organization-name-error').empty();
  $('#designation-error').empty();
  $('#organization-address-error').empty();
  
  $('#message').empty();
  $('#message').addClass('hide');
}

function saveProfessionalInfo(code) {
  educationValidation();
  //degreesAchievedValidation();

  universityValidation();
  addlInfoValidation();
  occupationValidation();
  professionalSectorValidation();
  organizationNameValidation();
  designationValidation();
  organizationAddressValidation();

  if (isValid) {
    var dataObject = new Object();

    dataObject.Code = code;

    dataObject.Education = $('#education').val().trim();
    dataObject.DegreesAchieved = $('#degrees-achieved').val();
    dataObject.UniversityAttended = $('#university').val().trim();
    dataObject.AddlInfo = $('#addl-info').val().trim();

    dataObject.Occupation = $('#occupation').val().trim();
    dataObject.ProfessionalSector = $('#professional-sector').val().trim();
    dataObject.OrganizationName = $('#organization-name').val().trim();
    dataObject.Designation = $('#designation').val().trim();
    dataObject.OrganizationAddress = $('#organization-address').val().trim();

    var url = BASEURL + '/api/MatrimonialApi/SaveProfessionalInfo';

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
        200: function (responseData) {
          $(location).attr('href', BASEURL + '/Matrimonial/AddFamilyInfo?code=' + responseData);
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