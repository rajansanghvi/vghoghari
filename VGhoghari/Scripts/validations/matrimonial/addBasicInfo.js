var isValid = false;

$(document).ready(function () {

  var code = getQueryStringValue('code');

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

  $('#birthtime').datetimepicker({
    format: 'HH:mm',
    useCurrent: false,
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

  if (code != '') {
    fetchBasicBiodataInfo(code, function (response) {
      if (response !== undefined && response !== null) {
        fillBasicInfo(response);
      }
      else {
        $(location).attr('href', BASEURL + '/Matrimonial/Manage');
      }
    });
  }

  $('#gender').on('hidden.bs.select', function (e) {
    genderValidation();
  });

  $('#fullname').focusout(function () {
    fullnameValidation();
  });

  $('#dob').focusout(function () {
    dobValidation();
  });

  $('#birthtime').focusout(function () {
    optionalBirthTimeValidation();
  });

  $('#marital-status').on('hidden.bs.select', function (e) {
    maritalStatusValidation();
  });

  $('#native').focusout(function () {
    nativeValidation();
  });

  $('#birth-place').focusout(function () {
    optionalBirthPlaceValidation();
  });

  $('#about-me').focusout(function () {
    optionalAboutMeValidation();
  });

  $('#mobile-no').focusout(function () {
    mobileNoValidation();
  });

  $('#landline-no').focusout(function () {
    optionalLandlineValidation();
  });

  $('#email-id').focusout(function () {
    optionalEmailValidation();
  });

  $('#facebook-url').focusout(function () {
    optionalFacebookUrlValidation();
  });

  $('#address').focusout(function () {
    addressValidation();
  });

  $('#address-type').on('hidden.bs.select', function (e) {
    addressTypeValidation();
  });

  $('#pincode').focusout(function () {
    optionalPincodeValidation();
  });

  $('#reset').on('click', function () {
    resetData();
  });

  $('#submit').on('click', function () {
    saveBasicInfo(code);
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

function fetchBasicBiodataInfo(code, callback) {
  var url = BASEURL + '/api/MatrimonialApi/GetBasicInfo?code=' + code;

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

function fillBasicInfo(data) {

  $('#gender').val(data.BasicInfo.Gender);
  $('#gender').selectpicker('refresh');
  $('#fullname').val(data.BasicInfo.Fullname);
  $('#dob').data("DateTimePicker").date(moment(data.BasicInfo.Dob, 'YYYY-MM-DD'));

  if (data.BasicInfo.StringBirthTime === undefined || data.BasicInfo.StringBirthTime === null || data.BasicInfo.StringBirthTime === '') {
    $('#birthtime').val('');
  }
  else {
    $('#birthtime').data("DateTimePicker").date(moment(data.BasicInfo.StringBirthTime, 'HH:mm'));
  }

  $('#age').val(data.BasicInfo.Age);
  $('#marital-status').val(data.BasicInfo.MaritalStatus);
  $('#marital-status').selectpicker('refresh');
  $('#native').val(data.BasicInfo.Native);
  $('#birth-place').val(data.BasicInfo.BirthPlace);
  $('#about-me').val(data.BasicInfo.AboutMe);

  $('#mobile-no').val(data.ContactInfo.MobileNumber);
  $('#landline-no').val(data.ContactInfo.LandlineNumber);
  $('#email-id').val(data.ContactInfo.EmailId);
  $('#facebook-url').val(data.ContactInfo.FacebookUrl);
  $('#address').val(data.ContactInfo.Address);
  $('#address-type').val(data.ContactInfo.AddressType);
  $('#address-type').selectpicker('refresh');
  $('#country').val(data.ContactInfo.Country);
  $('#country').selectpicker('refresh');
  loadStates($('#country').val(), data.ContactInfo.State, data.ContactInfo.City);
  $('#pincode').val(data.ContactInfo.Pincode);
}

function genderValidation() {
  $('#gender-error').empty();

  var errMsg = validateDropdown($('#gender').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#gender-error').html(errMsg);
  }
  else {
    isValid = true;
  }
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

function dobValidation() {
  $('#dob-error').empty();
  $('#age').val('');
  $('#age-error').empty();

  var errMsg = validateDob($('#dob').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#dob-error').html(errMsg);
  }
  else {
    var response = validateAge($('#dob').val().trim());
    if (isNaN(response)) {
      isValid = false;
      $('#age-error').html(response);
    }
    else {
      isValid = true;
      $('#age').val(response);
    }
  }
}

function optionalBirthTimeValidation() {
  $('#birthtime-error').empty();

  var errMsg = validateOptionalTime($('#birthtime').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#birthtime-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function maritalStatusValidation() {
  $('#marital-status-error').empty();

  var errMsg = validateDropdown($('#marital-status').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#marital-status-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function nativeValidation() {
  $('#native-error').empty();

  var errMsg = validateGeography($('#native').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#native-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function optionalBirthPlaceValidation() {
  $('#birth-place-error').empty();

  var errMsg = validateOptionalGeography($('#birth-place').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#birth-place-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function optionalAboutMeValidation() {
  $('#about-me-error').empty();

  var errMsg = validateOptionalFreeText($('#about-me').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#about-me-error').html(errMsg);
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

function addressValidation() {
  $('#address-error').empty();

  var errMsg = validateAddress($('#address').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#address-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function addressTypeValidation() {
  $('#address-type-error').empty();

  var errMsg = validateDropdown($('#address-type').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#address-type-error').html(errMsg);
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

function resetData() {
  $('#gender').selectpicker('val', '0');
  $('#fullname').val('');
  $('#dob').val('');
  $('#birthtime').val('');
  $('#age').val('');
  $('#marital-status').selectpicker('val', '0');
  $('#native').val('');
  $('#birth-place').val('');
  $('#about-me').val('');
  $('#mobile-no').val('');
  $('#landline-no').val('');
  $('#email-id').val('');
  $('#facebook-url').val('');
  $('#address').val('');
  $('#address-type').selectpicker('val', '0');
  $('#country').selectpicker('val', '');
  $('#state').selectpicker('val', '');
  $('#city').selectpicker('val', '');
  $('#pincode').val('');

  $('#gender-error').empty();
  $('#fullname-error').empty();
  $('#dob-error').empty();
  $('#birthtime-error').empty();
  $('#age-error').empty();
  $('#marital-status-error').empty();
  $('#native-error').empty();
  $('#birth-place-error').empty();
  $('#about-me-error').empty();
  $('#mobile-no-error').empty();
  $('#landline-no-error').empty();
  $('#email-id-error').empty();
  $('#facebook-url-error').empty();
  $('#address-error').empty();
  $('#address-type-error').empty();
  $('#country-error').empty();
  $('#state-error').empty();
  $('#city-error').empty();
  $('#pincode-error').empty();
  $('#message').empty();
  $('#message').addClass('hide');
}

function saveBasicInfo(code) {
  genderValidation();
  fullnameValidation();
  dobValidation();

  optionalBirthTimeValidation();

  maritalStatusValidation();
  nativeValidation();

  optionalBirthPlaceValidation();
  optionalAboutMeValidation();

  mobileNoValidation();

  optionalLandlineValidation();
  optionalEmailValidation();
  optionalFacebookUrlValidation();

  addressValidation();
  addressTypeValidation();
  countryValidation();

  optionalPincodeValidation();

  if (isValid) {
    var dataObject = new Object();

    dataObject.Code = code;

    dataObject.Gender = $('#gender').val().trim();
    dataObject.Fullname = $('#fullname').val().trim();
    dataObject.Dob = $('#dob').val().trim();
    dataObject.BirthTime = $('#birthtime').val().trim();
    dataObject.Age = $('#age').val().trim();
    dataObject.MaritalStatus = $('#marital-status').val().trim();
    dataObject.Native = $('#native').val().trim();
    dataObject.BirthPlace = $('#birth-place').val().trim();
    dataObject.AboutMe = $('#about-me').val().trim();

    dataObject.MobileNumber = $('#mobile-no').val().trim();
    dataObject.LandlineNumber = $('#landline-no').val().trim();
    dataObject.EmailId = $('#email-id').val().trim();
    dataObject.FacebookUrl = $('#facebook-url').val().trim();
    dataObject.Address = $('#address').val().trim();
    dataObject.AddressType = $('#address-type').val().trim();
    dataObject.Country = $('#country').val().trim();
    dataObject.State = $('#state').val().trim();
    dataObject.City = $('#city').val().trim();
    dataObject.Pincode = $('#pincode').val().trim();

    var url = BASEURL + '/api/MatrimonialApi/SaveBasicInfo';

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

