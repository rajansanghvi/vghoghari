var isValid = false;

$(document).ready(function () {

  var code = getQueryStringValue('code');

  fetchCountries(function (countryList) {
    $.each(countryList, function (index, value) {
      $('#country').append($('<option>', {
        value: value,
        text: value
      }));
    });

    $('#country').selectpicker('refresh');
  });

  $('#country').on('hidden.bs.select', function (e) {
    countryValidation();

    if ($('#country').val() !== '') {
      loadStates($('#country').val(), '', '');
    }
  });

  $('#state').on('hidden.bs.select', function (e) {
    if ($('#state').val() !== '') {
      loadCities($('#state').val(), '');
    }
  });
  
  fetchFamilyBiodataInfo(code, function (response) {
    if (response !== undefined && response !== null) {
      fillFamilyInfo(response);
    }
    else {
      $(location).attr('href', BASEURL + '/Matrimonial/Manage');
    }
  });

  $('#father-name').focusout(function () {
    fatherNameValidation();
  });

  $('#father-mobile-no').focusout(function () {
    fatherMobileValidation();
  });

  $('#grandfather-name').focusout(function () {
    grandFatherNameValidation();
  });

  $('#mother-name').focusout(function () {
    motherNameValidation();
  });

  $('#mother-mobile-no').focusout(function () {
    motherMobileValidation();
  });

  $('#grandmother-name').focusout(function () {
    grandMotherNameValidation();
  });

  $('#no-of-bros').focusout(function () {
    noOfBrosValidation();
  });

  $('#no-of-sis').focusout(function () {
    noOfSisValidation();
  });

  $('#family-landline-no').focusout(function () {
    familyLandlineNoValidation();
  });

  $('#family-type').on('hidden.bs.select', function (e) {
    familyTypeValidation();
  });

  $('#family-address').focusout(function () {
    familyAddressValidation();
  });

  $('#family-address-type').on('hidden.bs.select', function (e) {
    familyAddressTypeValidation();
  });

  $('#father-occupation').on('hidden.bs.select', function (e) {
    fatherOccupationValidation();
  });

  $('#father-professional-sector').on('hidden.bs.select', function (e) {
    fatherProfessionalSectorValidation();
  });

  $('#father-organization-name').focusout(function () {
    fatherOrganizationNameValidation();
  });

  $('#father-designation').focusout(function () {
    fatherDesignationValidation();
  });

  $('#father-organization-address').focusout(function () {
    fatherOrganizationAddressValidation();
  });

  $('#uncle-name').focusout(function () {
    uncleNameValidation();
  });

  $('#maternal-grandfather-name').focusout(function () {
    maternalGrandFatherNameValidation();
  });

  $('#maternal-grandmother-name').focusout(function () {
    maternalGrandMotherNameValidation();
  });

  $('#maternal-native').focusout(function () {
    maternalGrandMotherNameValidation();
  });

  $('#contact-no').focusout(function () {
    contactNoValidation();
  });

  $('#mosal-address').focusout(function () {
    mosalAddressValidation();
  });

  $('#reset').on('click', function () {
    resetData();
  });

  $('#submit').on('click', function () {
    saveFamilyInfo(code);
  });
});

function fetchCountries(callback) {
  var url = BASEURL + '/api/AppApi/GetCountries';

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

function loadStates(countryName, stateName, cityName) {
  fetchState(countryName, function (stateList) {
    $('#state').empty();
    $('#state').append($('<option>', {
      value: '',
      text: 'Select One'
    }));
    $.each(stateList, function (index, value) {
      $('#state').append($('<option>', {
        value: value,
        text: value
      }));
    });
    $('#state').selectpicker('refresh');
    $('#state').val(stateName);
    $('#state').selectpicker('refresh');

    if (stateName !== '') {
      loadCities(stateName, cityName);
    }
  });
}

function fetchState(countryName, callback) {
  var url = BASEURL + '/api/AppApi/GetStates?countryName=' + countryName;

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

function loadCities(stateName, cityName) {
  fetchCity(stateName, function (cityList) {
    $('#city').empty();
    $('#city').append($('<option>', {
      value: '',
      text: 'Select One'
    }));
    $.each(cityList, function (index, value) {
      $('#city').append($('<option>', {
        value: value,
        text: value
      }));
    });
    $('#city').selectpicker('refresh');
    $('#city').val(cityName);
    $('#city').selectpicker('refresh');
  });
}

function fetchCity(stateName, callback) {
  var url = BASEURL + '/api/AppApi/GetCities?stateName=' + stateName;

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

function fetchFamilyBiodataInfo(code, callback) {
  var url = BASEURL + '/api/MatrimonialApi/GetFamilyInfo?code=' + code;

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
  degreesAchievedValidation();

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