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
    // countryValidation();

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

  //$('#grandfather-name').focusout(function () {
  //  grandFatherNameValidation();
  //});

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

  //$('#family-type').on('hidden.bs.select', function (e) {
  //  familyTypeValidation();
  //});

  $('#family-address').focusout(function () {
    familyAddressValidation();
  });

  $('#family-address-type').on('hidden.bs.select', function (e) {
    familyAddressTypeValidation();
  });

  $('#uncle-name').focusout(function () {
    uncleNameValidation();
  });

  //$('#maternal-grandfather-name').focusout(function () {
  //  maternalGrandFatherNameValidation();
  //});

  $('#maternal-grandmother-name').focusout(function () {
    maternalGrandMotherNameValidation();
  });

  $('#maternal-native').focusout(function () {
    maternalNativeValidation();
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

function fillFamilyInfo(data) {
  $('#father-name').val(data.FamilyInfo.FatherName);
  $('#father-mobile-no').val(data.FamilyInfo.FatherMobileNumber);
  $('#grandfather-name').val(data.FamilyInfo.GrandFatherName);

  $('#mother-name').val(data.FamilyInfo.MotherName);
  $('#mother-mobile-no').val(data.FamilyInfo.MotherMobileNumber);
  $('#grandmother-name').val(data.FamilyInfo.GrandMotherName);

  $('#no-of-bros').val(data.FamilyInfo.NoOfBrothers);
  $('#no-of-sis').val(data.FamilyInfo.NoOfSisters);
  
  $('#family-landline-no').val(data.FamilyInfo.LandlineNumber);
  $('#family-type').val(data.FamilyInfo.FamilyType);
  $('#family-type').selectpicker('refresh');

  $('#family-address').val(data.FamilyInfo.Address);

  $('#country').val(data.FamilyInfo.Country);
  $('#country').selectpicker('refresh');
  loadStates($('#country').val(), data.FamilyInfo.State, data.FamilyInfo.City);

  $('#family-address-type').val(data.FamilyInfo.ResidenceStatus);
  $('#family-address-type').selectpicker('refresh');

  $('#uncle-name').val(data.MosalInfo.UncleName);
  $('#maternal-grandfather-name').val(data.MosalInfo.GrandFatherName);
  $('#maternal-grandmother-name').val(data.MosalInfo.GrandMotherName);

  $('#maternal-native').val(data.MosalInfo.Native);
  $('#contact-no').val(data.MosalInfo.ContactNumber);
  $('#mosal-address').val(data.MosalInfo.Address);
}

function fatherNameValidation() {
  $('#father-name-error').empty();

  var errMsg = validateFullName($('#father-name').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#father-name-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function fatherMobileValidation() {
  $('#father-mobile-no-error').empty();

  var errMsg = validateOptionalMobileNo($('#father-mobile-no').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#father-mobile-no-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function grandFatherNameValidation() {
  $('#grandfather-name-error').empty();

  var errMsg = validateFullName($('#grandfather-name').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#grandfather-name-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function motherNameValidation() {
  $('#mother-name-error').empty();

  var errMsg = validateFullName($('#mother-name').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#mother-name-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function motherMobileValidation() {
  $('#mother-mobile-no-error').empty();

  var errMsg = validateOptionalMobileNo($('#mother-mobile-no').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#mother-mobile-no-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function grandMotherNameValidation() {
  $('#grandmother-name-error').empty();

  var errMsg = validateOptionalFullName($('#grandmother-name').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#grandmother-name-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function noOfBrosValidation() {
  $('#no-of-bros-error').empty();

  var errMsg = validateNumberField($('#no-of-bros').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#no-of-bros-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function noOfSisValidation() {
  $('#no-of-sis-error').empty();

  var errMsg = validateNumberField($('#no-of-sis').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#no-of-sis-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function familyLandlineNoValidation() {
  $('#family-landline-no-error').empty();

  var errMsg = validateOptionalLandline($('#family-landline-no').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#family-landline-no-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function familyTypeValidation() {
  $('#family-type-error').empty();

  var errMsg = validateDropdown($('#family-type').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#family-type-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function familyAddressValidation() {
  $('#family-address-error').empty();

  var errMsg = validateAddress($('#family-address').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#family-address-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function countryValidation() {
  $('#country-error').empty();

  var errMsg = validateDropdown($('#country').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#country-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function familyAddressTypeValidation() {
  $('#family-address-type-error').empty();

  var errMsg = validateDropdown($('#family-address-type').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#family-address-type-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function uncleNameValidation() {
  $('#uncle-name-error').empty();

  var errMsg = validateOptionalFullName($('#uncle-name').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#uncle-name-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function maternalGrandFatherNameValidation() {
  $('#maternal-grandfather-name-error').empty();

  var errMsg = validateFullName($('#maternal-grandfather-name').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#maternal-grandfather-name-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function maternalGrandMotherNameValidation() {
  $('#maternal-grandmother-name-error').empty();

  var errMsg = validateOptionalFullName($('#maternal-grandmother-name').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#maternal-grandmother-name-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function maternalNativeValidation() {
  $('#maternal-native-error').empty();

  var errMsg = validateGeography($('#maternal-native').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#maternal-native-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function contactNoValidation() {
  $('#contact-no-error').empty();

  var errMsg = validateOptionalContactNo($('#contact-no').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#contact-no-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function mosalAddressValidation() {
  $('#mosal-address-error').empty();

  var errMsg = validateOptionalAddress($('#mosal-address').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#mosal-address-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function resetData() {
  $('#father-name').val('');
  $('#father-mobile-no').val('');
  $('#grandfather-name').val('');

  $('#mother-name').val('');
  $('#mother-mobile-no').val('');
  $('#grandmother-name').val('');

  $('#no-of-bros').val(0);
  $('#no-of-sis').val(0);

  $('#family-landline-no').val('');
  $('#family-type').selectpicker('val', '0');

  $('#family-address').val('');

  $('#country').selectpicker('val', '');
  $('#state').selectpicker('val', '');
  $('#city').selectpicker('val', '');

  $('#family-address-type').selectpicker('val', '0');

  $('#uncle-name').val('');
  $('#maternal-grandfather-name').val('');
  $('#maternal-grandmother-name').val('');

  $('#maternal-native').val('');
  $('#contact-no').val('');
  $('#mosal-address').val('');

  $('#father-name-error').empty();
  $('#father-mobile-no-error').empty();
  $('#grandfather-name-error').empty();

  $('#mother-name-error').empty();
  $('#mother-mobile-no-error').empty();
  $('#grandmother-name-error').empty();

  $('#no-of-bros-error').empty();
  $('#no-of-sis-error').empty();

  $('#family-landline-no-error').empty();
  $('#family-type-error').empty();

  $('#family-address-error').empty();

  $('#country-error').empty();
  $('#state-error').empty();
  $('#city-error').empty();

  $('#family-address-type-error').empty();

  $('#uncle-name-error').empty();
  $('#maternal-grandfather-name-error').empty();
  $('#maternal-grandmother-name-error').empty();

  $('#maternal-native-error').empty();
  $('#contact-no-error').empty();
  $('#mosal-address-error').empty();
  
  $('#message').empty();
  $('#message').addClass('hide');
}

function saveFamilyInfo(code) {
  fatherNameValidation();
  fatherMobileValidation();
  //grandFatherNameValidation();

  motherNameValidation();
  motherMobileValidation();
  grandMotherNameValidation();

  noOfBrosValidation();
  noOfSisValidation();
  familyLandlineNoValidation();
  //familyTypeValidation();

  familyAddressValidation();
  familyAddressTypeValidation();
  //countryValidation();

  uncleNameValidation();
  //maternalGrandFatherNameValidation();
  maternalGrandMotherNameValidation();

  maternalNativeValidation();
  contactNoValidation();
  mosalAddressValidation();

  if (!isFilled($('#father-mobile-no').val().trim()) && !isFilled($('#mother-mobile-no').val().trim()) && !isFilled($('#family-landline-no').val().trim())) {
    isValid = false;
    $('#message').html('<strong> Error! </strong>Please provide with atleast one of the following three numbers: Father\'s Mobile No, Mother\'s Mobile No or Landline No.');
    $('#message').removeClass('hide');
    return;
  }

  if (isValid) {
    var dataObject = new Object();

    dataObject.Code = code;

    dataObject.FatherName = $('#father-name').val().trim();
    dataObject.FatherMobileNumber = $('#father-mobile-no').val();
    dataObject.GrandFatherName = $('#grandfather-name').val().trim();
    dataObject.MotherName = $('#mother-name').val().trim();
    dataObject.MotherMobileNumber = $('#mother-mobile-no').val();
    dataObject.GrandMotherName = $('#grandmother-name').val().trim();
    dataObject.NoOfBrothers = $('#no-of-bros').val().trim();
    dataObject.NoOfSisters = $('#no-of-sis').val().trim();
    dataObject.LandlineNumber = $('#family-landline-no').val().trim();
    dataObject.FamilyType = $('#family-type').val().trim();
    dataObject.Address = $('#family-address').val().trim();
    dataObject.Country = $('#country').val().trim();
    dataObject.State = $('#state').val().trim();
    dataObject.City = $('#city').val().trim();
    dataObject.ResidenceStatus = $('#family-address-type').val().trim();

    dataObject.UncleName = $('#uncle-name').val().trim();
    dataObject.MaternalGrandFatherName = $('#maternal-grandfather-name').val().trim();
    dataObject.MaternalGrandMotherName = $('#maternal-grandmother-name').val().trim();
    dataObject.Native = $('#maternal-native').val().trim();
    dataObject.ContactNumber = $('#contact-no').val().trim();
    dataObject.MosalAddress = $('#mosal-address').val().trim();

    var url = BASEURL + '/api/MatrimonialApi/SaveFamilyInfo';

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
          $(location).attr('href', BASEURL + '/Matrimonial/AddFamilyOccupationInfo?code=' + responseData);
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