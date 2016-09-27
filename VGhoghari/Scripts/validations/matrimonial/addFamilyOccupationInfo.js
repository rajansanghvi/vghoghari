var isValid = false;

$(document).ready(function () {

  var code = getQueryStringValue('code');
  
  fetchFamilyOccupationDetails(code, function (response) {
    if (response !== undefined && response !== null) {
      fillFamilyOccupationDetails(response);
    }
    else {
      $(location).attr('href', BASEURL + '/Matrimonial/Manage');
    }
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

  $('#mother-occupation').on('hidden.bs.select', function (e) {
    motherOccupationValidation();
  });

  $('#mother-professional-sector').on('hidden.bs.select', function (e) {
    motherProfessionalSectorValidation();
  });

  $('#mother-organization-name').focusout(function () {
    motherOrganizationNameValidation();
  });

  $('#mother-designation').focusout(function () {
    motherDesignationValidation();
  });

  $('#mother-organization-address').focusout(function () {
    motherOrganizationAddressValidation();
  });

  $('#reset').on('click', function () {
    resetData();
  });

  $('#submit').on('click', function () {
    saveFamilyOccupationDetails(code);
  });
});

function fetchFamilyOccupationDetails(code, callback) {
  var url = BASEURL + '/api/MatrimonialApi/GetFamilyOccupationDetails?code=' + code;

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

function fillFamilyOccupationDetails(data) {
 
  $('#father-occupation').val(data.FatherOccupationInfo.Occupation);
  $('#father-occupation').selectpicker('refresh');

  $('#father-professional-sector').val(data.FatherOccupationInfo.ProfessionSector);
  $('#father-professional-sector').selectpicker('refresh');

  $('#father-organization-name').val(data.FatherOccupationInfo.OrganizationName);

  $('#father-designation').val(data.FatherOccupationInfo.Designation);

  $('#father-organization-address').val(data.FatherOccupationInfo.OrganizationAddress);

  $('#mother-occupation').val(data.MotherOccupationInfo.Occupation);
  $('#mother-occupation').selectpicker('refresh');

  $('#mother-professional-sector').val(data.MotherOccupationInfo.ProfessionSector);
  $('#mother-professional-sector').selectpicker('refresh');

  $('#mother-organization-name').val(data.MotherOccupationInfo.OrganizationName);

  $('#mother-designation').val(data.MotherOccupationInfo.Designation);

  $('#mother-organization-address').val(data.MotherOccupationInfo.OrganizationAddress);
}

function fatherOccupationValidation() {
  $('#father-occupation-error').empty();

  var errMsg = validateDropdown($('#father-occupation').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#father-occupation-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function fatherProfessionalSectorValidation() {
  $('#father-professional-sector-error').empty();

  var errMsg = validateDropdown($('#father-professional-sector').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#father-professional-sector-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function fatherOrganizationNameValidation() {
  $('#father-organization-name-error').empty();

  var errMsg = validateOptionalProperName($('#father-organization-name').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#father-organization-name-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function fatherDesignationValidation() {
  $('#father-designation-error').empty();

  var errMsg = validateOptionalProperName($('#father-designation').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#father-designation-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function fatherOrganizationAddressValidation() {
  $('#father-organization-address-error').empty();

  var errMsg = validateOptionalAddress($('#father-organization-address').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#father-organization-address-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function motherOccupationValidation() {
  $('#mother-occupation-error').empty();

  var errMsg = validateDropdown($('#mother-occupation').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#mother-occupation-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function motherProfessionalSectorValidation() {
  $('#mother-professional-sector-error').empty();

  var errMsg = validateDropdown($('#mother-professional-sector').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#mother-professional-sector-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function motherOrganizationNameValidation() {
  $('#mother-organization-name-error').empty();

  var errMsg = validateOptionalProperName($('#mother-organization-name').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#mother-organization-name-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function motherDesignationValidation() {
  $('#mother-designation-error').empty();

  var errMsg = validateOptionalProperName($('#mother-designation').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#mother-designation-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function motherOrganizationAddressValidation() {
  $('#mother-organization-address-error').empty();

  var errMsg = validateOptionalAddress($('#mother-organization-address').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#mother-organization-address-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function resetData() {
  $('#father-occupation').selectpicker('val', '0');
  $('#father-professional-sector').selectpicker('val', '');

  $('#father-organization-name').val('');
  $('#father-designation').val('');
  $('#father-organization-address').val('');

  $('#mother-occupation').selectpicker('val', '0');
  $('#mother-professional-sector').selectpicker('val', '');

  $('#mother-organization-name').val('');
  $('#mother-designation').val('');
  $('#mother-organization-address').val('');

  $('#father-occupation-error').empty();
  $('#father-professional-sector-error').empty();
  $('#father-organization-name-error').empty();
  $('#father-designation-error').empty();
  $('#father-organization-address-error').empty();

  $('#mother-occupation-error').empty();
  $('#mother-professional-sector-error').empty();
  $('#mother-organization-name-error').empty();
  $('#mother-designation-error').empty();
  $('#mother-organization-address-error').empty();
  
  $('#message').empty();
  $('#message').addClass('hide');
}

function saveFamilyOccupationDetails(code) {
  fatherOccupationValidation();
  fatherProfessionalSectorValidation();
  fatherOrganizationNameValidation();
  fatherDesignationValidation();
  fatherOrganizationAddressValidation();

  motherOccupationValidation();
  motherProfessionalSectorValidation();
  motherOrganizationNameValidation();
  motherDesignationValidation();
  motherOrganizationAddressValidation();

  if (isValid) {
    var dataObject = new Object();

    dataObject.Code = code;

    dataObject.FatherOccupation = $('#father-occupation').val().trim();
    dataObject.FatherProfessionalSector = $('#father-professional-sector').val().trim();
    dataObject.FatherOrganizationName = $('#father-organization-name').val().trim();
    dataObject.FatherDesignation = $('#father-designation').val().trim();
    dataObject.FatherOrganizationAddress = $('#father-organization-address').val().trim();

    dataObject.MotherOccupation = $('#mother-occupation').val().trim();
    dataObject.MotherProfessionalSector = $('#mother-professional-sector').val().trim();
    dataObject.MotherOrganizationName = $('#mother-organization-name').val().trim();
    dataObject.MotherDesignation = $('#mother-designation').val().trim();
    dataObject.MotherOrganizationAddress = $('#mother-organization-address').val().trim();

    var url = BASEURL + '/api/MatrimonialApi/SaveFamilyOccupationDetails';

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
          $(location).attr('href', BASEURL + '/Matrimonial/AddSibblingInfo?code=' + responseData);
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