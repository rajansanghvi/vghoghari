var isValid = false;

$(document).ready(function () {

  var code = getQueryStringValue('code');
  
  fetchSibblingBiodataInfo(code, function (response) {
    if (response !== undefined && response !== null) {
      $.each(response, function (index, value) {
        if (value.Name !== '') {
          $('#sibbling-data > tbody:last-child')
            .append('<tr>'
            + '<td>'
            + value.Name
            + '</td>'
            + '<td>'
            + value.GenderString
            + '</td>'
            + '<td>'
            + value.Family
            + '</td>'
            + '<td>'
            + value.Native
            + '</td>'
            + '<td>'
            + '<a id="' + value.Code + '" href="#" onclick="deleteSibbling(this)">Delete</a>'
            + '</td>'
            + '</tr>'
            );
        }
      });
    }
    else {
      $(location).attr('href', BASEURL + '/Matrimonial/Manage');
    }
  });

  $('#sibbling-name').focusout(function () {
    sibblingNameValidation();
  });

  $('#sibbling-gender').on('hidden.bs.select', function (e) {
    sibblingGenderValidation();
  });

  $('#sibbling-family').focusout(function () {
    sibblingFamilyValidation();
  });

  $('#sibbling-native').focusout(function () {
    sibblingNativeValidation();
  });

  $('#reset').on('click', function () {
    resetData();
  });

  $('#submit').on('click', function () {
    saveSibblingInfo(code);
  });

  $('#next').on('click', function () {
    $(location).attr('href', BASEURL + '/Matrimonial/AddAdditionalInfo?code=' + code);
  });
});

function fetchSibblingBiodataInfo(code, callback) {
  var url = BASEURL + '/api/MatrimonialApi/GetSibblingInfo?code=' + code;

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

function deleteSibbling(element) {
  var biodataCode = getQueryStringValue('code');
  var sibblingCode = $(element).attr('id');

  var url = BASEURL + '/api/MatrimonialApi/DeleteSibblingInfo';
  var dataObject = new Object();
  dataObject.BiodataCode = biodataCode;
  dataObject.SibblingCode = sibblingCode;
  var that = this;
  $.ajax({
    url: url,
    type: 'POST',
    dataType: 'json',
    contentType: 'application/json',
    data: JSON.stringify(dataObject),
    statusCode: {
      400: function () {
        $(location).attr('href', BASEURL + '/Matrimonial/Manage');
      },
      403: function () {
        $(location).attr('href', BASEURL + '/Matrimonial/Manage');
      },
      200: function (response) {
        $('#' + response).closest('tr').remove();
      },
      401: function () {
        $(location).attr('href', BASEURL + '/User/Logout');
      }
    }
  });
}

function sibblingNameValidation() {
  $('#sibbling-name-error').empty();

  var errMsg = validateFullName($('#sibbling-name').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#sibbling-name-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function sibblingGenderValidation() {
  $('#sibbling-gender-error').empty();

  var errMsg = validateDropdown($('#sibbling-gender').val());
  if (errMsg !== '') {
    isValid = false;
    $('#sibbling-gender-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function sibblingFamilyValidation() {
  $('#sibbling-family-error').empty();

  var errMsg = validateOptionalFullName($('#sibbling-family').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#sibbling-family-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function sibblingNativeValidation() {
  $('#sibbling-native-error').empty();

  var errMsg = validateOptionalGeography($('#sibbling-native').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#sibbling-native-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function resetData() {
  $('#sibbling-gender').selectpicker('val', '0');

  $('#sibbling-name').val('');
  $('#sibbling-family').val('');
  $('#sibbling-native').val('');

  $('#sibbling-name-error').empty();
  $('#sibbling-family-error').empty();
  $('#sibbling-native-error').empty();
  $('#sibbling-gender-error').empty();
  
  $('#message').empty();
  $('#message').addClass('hide');
}

function saveSibblingInfo(code) {
  sibblingNameValidation();
  sibblingGenderValidation();

  sibblingFamilyValidation();
  sibblingNativeValidation();

  if (isValid) {
    var dataObject = new Object();

    dataObject.Code = code;

    dataObject.Name = $('#sibbling-name').val().trim();
    dataObject.Gender = $('#sibbling-gender').val();
    dataObject.Family = $('#sibbling-family').val().trim();
    dataObject.Native = $('#sibbling-native').val().trim();

    var url = BASEURL + '/api/MatrimonialApi/SaveSibblingInfo';

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
          $('#sibbling-data > tbody:last-child')
            .append('<tr>'
            + '<td>'
            + responseData.Name
            + '</td>'
            + '<td>'
            + responseData.GenderString
            + '</td>'
            + '<td>'
            + responseData.Family
            + '</td>'
            + '<td>'
            + responseData.Native
            + '</td>'
            + '<td>'
            + '<a id="' + responseData.Code + '" href="#" onclick="deleteSibbling(this)">Delete</a>'
            + '</td>'
            + '</tr>'
            );
          resetData();
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