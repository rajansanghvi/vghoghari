var isValid = false;
var isBannerImagePresent = false;

$(document).ready(function () {

  var code = getQueryStringValue('code');

  $('#short-description').summernote({
    disableResizeEditor: true,
    height: 150
  });

  $('#description').summernote({
    disableResizeEditor: true,
    height: 250
  });

  if (code != '') {
    fetchProjectDetails(code, function (response) {
      if (response !== undefined && response !== null) {
        fillProjectDetails(response);
      }
      else {
        $(location).attr('href', BASEURL + '/Admin/Project/Manage');
      }
    });
  }

  $(document).on('change', ':file', function () {
    let input = $(this),
        numFiles = input.get(0).files ? input.get(0).files.length : 1,
        label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
    // console.log(input.get(0).files.length)
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
    let input = $(this).parents('.input-group').find(':text'),
        log = numFiles > 1 ? numFiles + ' files selected' : label;

    if (input.length) {
      input.val(log);
    } else {
      if (log) alert(log);
    }
  });

  $('#title').focusout(function () {
    titleValidation();
  });

  $('#short-description').on('summernote.blur', function () {
    shortDescriptionValidation();
  });

  $('#contact-person').focusout(function () {
    contactPersonValidation();
  });

  $('#contact-number').focusout(function () {
    contactNumberValidation();
  });

  $('#contact-email').focusout(function () {
    contactEmailValidation();
  });

  $('#reset').on('click', function () {
    resetData();
  });

  $('#submit').on('click', function () {
    saveProject(code);
  });
});

function fetchProjectDetails(code, callback) {
  var url = BASEURL + '/Admin/api/ProjectApi/GetProjectDetails?code=' + code;

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

function fillProjectDetails(data) {
  
  $('#title').val(data.Title);
  $('#short-description').summernote('code', data.ShortDescription);
  $('#description').summernote('code', data.Description);

  $('#contact-person').val(data.ContactPerson);
  $('#contact-number').val(data.ContactNumber);
  $('#contact-email').val(data.ContactEmail);

  if (data.BannerImage !== undefined && data.BannerImage !== null && data.BannerImage !== '' && data.BannerImage.length > 0) {
    isBannerImagePresent = true;
    $('#banner-image').attr("src", BASEURL + "/AppData/projects/" + data.BannerImage);
  }
}

function titleValidation() {
  $('#title-error').empty();

  let errMsg = validateProperName($('#title').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#title-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function shortDescriptionValidation() {
  $('#short-description-error').empty();

  if ($('#short-description').summernote('isEmpty')) {
    isValid = false;
    $('#short-description-error').html('This is a required field.');
  }
  else {
    isValid = true;
  }
}

function contactPersonValidation() {
  $('#contact-person-error').empty();

  let errMsg = validateOptionalFullName($('#contact-person').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#contact-person-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function contactNumberValidation() {
  $('#contact-number-error').empty();

  let errMsg = validateOptionalContactNo($('#contact-number').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#contact-number-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function contactEmailValidation() {
  $('#contact-email-error').empty();

  let errMsg = validateOptionalEmailId($('#contact-email').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#contact-email-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function resetData() {
  
  $('#title').val('');
  $('#short-description').summernote('reset');
  $('#description').summernote('reset');
  
  $('#contact-person').val('');
  $('#contact-number').val('');
  $('#contact-email').val('');

  $('#inp-banner-image').val('');
  $('#image-upload-info').removeClass('text-danger');
  $('#file-name').val('');

  $('#title-error').empty();
  $('#short-description-error').empty();
  $('#description-error').empty();

  $('#contact-person-error').empty();
  $('#contact-number-error').empty();
  $('#contact-email-error').empty();

  $('#message').empty();
  $('#message').addClass('hide');
}

function readFileData(file, success, error) {
  let reader = new FileReader();

  reader.onload = function () {
    // console.log(reader.result);
    success(reader.result);
  };
  reader.onerror = function (error) {
    // console.log(error);
    error(error);
  };

  reader.readAsDataURL(file);
}

function saveProject(code) {
  
  titleValidation();
  shortDescriptionValidation();
  contactPersonValidation();
  contactNumberValidation();
  contactEmailValidation();

  if (!isBannerImagePresent) {
    if ($('#inp-banner-image').get(0).files.length === 0) {
      isValid = false;
      $('#image-upload-info').addClass('text-danger');
    }
    else {
      let label = $('#inp-banner-image').val().replace(/\\/g, '/').replace(/.*\//, '');
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
    if ($('#inp-banner-image').get(0).files.length !== 0) {
      let label = $('#inp-banner-image').val().replace(/\\/g, '/').replace(/.*\//, '');
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
    let dataObject = new Object();

    dataObject.Code = code;
    
    dataObject.Title = $('#title').val().trim();
    dataObject.ShortDescription = $('#short-description').summernote('code');
    dataObject.Description = $('#description').summernote('code');
    
    dataObject.ContactPerson = $('#contact-person').val().trim();
    dataObject.ContactNumber = $('#contact-number').val().trim();
    dataObject.ContactEmail = $('#contact-email').val().trim();

    if ($('#inp-banner-image').get(0).files.length === 0) {
      dataObject.BannerImageData = '';
      save(dataObject);
      return;
    }

    readFileData($('#inp-banner-image').get(0).files[0],
      function (responseData) {
        dataObject.BannnerImageData = responseData;
        save(dataObject);
      },
      function (error) {
        $('#message').html('<strong> Error! </strong>Some Error while processing the banner image. Please refresh and try again.');
        $('#message').removeClass('hide');
      });
  }
  else {
    $('#message').html('<strong> Error! </strong>There are certain invalid or empty fields. Please fill in all the required information correctly and try again.');
    $('#message').removeClass('hide');
  }
}

function save(object) {
  let url = BASEURL + '/Admin/api/ProjectApi/SaveProject';

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
        $(location).attr('href', BASEURL + '/Admin/Project/Manage');
      },
      404: function () {
        $(location).attr('href', BASEURL + '/Admin/Project/Manage');
      }
    }
  });
}