var isValid = false;
var isBannerImagePresent = false;

$(document).ready(function () {

  var code = getQueryStringValue('code');

  fetchEventTags(function (tagList) {
    $.each(tagList, function (index, value) {
      $('#event-tags').append($('<option>', {
        value: value,
        text: value
      }));
    });

    $('#event-tags').selectpicker('refresh');
  });

  $('#short-description').summernote({
    disableResizeEditor: true,
    height: 150
  });

  $('#description').summernote({
    disableResizeEditor: true,
    height: 250
  });

  $('#start-time').datetimepicker({
    format: 'YYYY-MM-DD HH:mm',
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

  $('#end-time').datetimepicker({
    useCurrent: false,
    format: 'YYYY-MM-DD HH:mm',
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

  $("#start-time").on("dp.change", function (e) {
    $('#end-time').data("DateTimePicker").minDate(e.date);
  });

  $("#end-time").on("dp.change", function (e) {
    $('#start-time').data("DateTimePicker").maxDate(e.date);
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
    fetchEventDetails(code, function (response) {
      if (response !== undefined && response !== null) {
        fillEventDetails(response);
      }
      else {
        $(location).attr('href', BASEURL + '/Admin/Event/Manage');
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

  $('#event-tags').on('hidden.bs.select', function (e) {
    eventTagsValidation();
  });

  $('#title').focusout(function () {
    titleValidation();
  });

  $('#short-description').on('summernote.blur', function () {
    shortDescriptionValidation();
  });

  $('#start-time').focusout(function () {
    startTimeValidation();
  });

  $('#end-time').focusout(function () {
    endTimeValidation();
  });

  $('#cost-per-person').focusout(function () {
    costPerPersonValidation();
  });

  $('#total-capacity').focusout(function () {
    totalCapacityValidation();
  });

  $('#venue').focusout(function () {
    venueValidation();
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
    saveEvent(code);
  });
});

function fetchEventTags(callback) {
  let url = BASEURL + '/Admin/api/EventApi/GetEventCategories';

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

function fetchCountries(callback) {
  let url = BASEURL + '/api/AppApi/GetCountries';

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
  let url = BASEURL + '/api/AppApi/GetStates?countryName=' + countryName;

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
  let url = BASEURL + '/api/AppApi/GetCities?stateName=' + stateName;

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

function fetchEventDetails(code, callback) {
  var url = BASEURL + '/Admin/api/EventApi/GetEventDetails?code=' + code;

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

function fillEventDetails(data) {
  $('#event-tags').selectpicker('refresh');
  $('#event-tags').selectpicker('val', data.TagList);

  $('#title').val(data.Title);
  $('#short-description').summernote('code', data.ShortDescription);
  $('#description').summernote('code', data.Description);

  $('#start-time').data("DateTimePicker").date(moment(data.FormattedStartDate, 'YYYY-MM-DD HH:mm'));

  if (data.FormattedEndDate === undefined || data.FormattedEndDate === null || data.FormattedEndDate === '') {
    $('#end-time').val('');
  }
  else {
    $('#end-time').data("DateTimePicker").date(moment(data.FormattedEndDate, 'YYYY-MM-DD HH:mm'));
  }

  $('#cost-per-person').val(data.CostPerPerson);
  $('#total-capacity').val(data.TotalCapacity);
  $('#venue').val(data.Venue);
  $('#country').val(data.Country);
  $('#country').selectpicker('refresh');
  loadStates($('#country').val(), data.State, data.City);
  $('#contact-person').val(data.ContactPerson);
  $('#contact-number').val(data.ContactNumber);
  $('#contact-email').val(data.ContactEmail);

  if (data.BannerImage !== undefined && data.BannerImage !== null && data.BannerImage !== '' && data.BannerImage.length > 0) {
    isBannerImagePresent = true;
    $('#banner-image').attr("src", BASEURL + "/AppData/events/" + data.BannerImage);
  }
}

function eventTagsValidation() {
  $('#event-tags-error').empty();

  let errMsg = validateMultipleDropdown($('#event-tags').val());
  if (errMsg !== '') {
    isValid = false;
    $('#event-tags-error').html(errMsg);
  }
  else {
    isValid = true;
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

function startTimeValidation() {
  let content = $('#start-time').val().trim();
  $('#start-time-error').empty();

  if (content === undefined || content === null || content === '' || content.length === 0) {
    isValid = false;
    $('#start-time-error').html('This is a required field.');
  }
  else {
    if (moment(content, 'YYYY-MM-DD HH:mm').isValid()) {
      let startDate = moment(content, 'YYYY-MM-DD HH:mm');
      let endDateContent = $('#end-time').val().trim();
      if (moment(endDateContent, 'YYYY-MM-DD HH:mm').isValid()) {
        let endDate = moment(endDateContent, 'YYYY-MM-DD HH:mm');
        if (startDate.isAfter(endDate)) {
          //console.log(startDate);
          //console.log(endDate);
          isValid = false;
          $('#start-time-error').html('Start time can not be later than the end time.');
        }
        else {
          isValid = true;
        }
      }
      else {
        isValid = true;
      }
    }
    else {
      isValid = false;
      $('#start-time-error').html('Please enter a valid date and time.');
    }
  }
}

function endTimeValidation() {
  let content = $('#end-time').val().trim();
  $('#end-time-error').empty();

  if (content !== undefined && content !== null && content !== '' && content.length > 0) {
    if (moment(content, 'YYYY-MM-DD HH:mm').isValid()) {
      let endDate = moment(content, 'YYYY-MM-DD HH:mm');
      let startDateContent = $('#start-time').val().trim();

      if (startDateContent === undefined || startDateContent === null || startDateContent === '' || startDateContent.length === 0) {
        isValid = false;
        $('#start-time-error').html('This is a required field. Please enter the start time first.');
      }
      else {
        if (moment(startDateContent, 'YYYY-MM-DD HH:mm').isValid()) {
          var startDate = moment(startDateContent, 'YYYY-MM-DD HH:mm');
          //console.log(startDate);
          //console.log(endDate);
          if (endDate.isBefore(startDate)) {
            isValid = false;
            $('#end-time-error').html('End time can not be earlier than the end time.');
          }
          else {
            isValid = true;
          }
        }
        else {
          isValid = false;
          $('#start-time-error').html('Please enter a valid date and time.');
        }
      }
    }
    else {
      isValid = false;
      $('#end-time-error').html('Please enter a valid date and time.');
    }
  }
  else {
    isValid = true;
  }
}

function costPerPersonValidation() {

  $('#cost-per-person-error').empty();

  let errMsg = validateNumberField($('#cost-per-person').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#cost-per-person-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function totalCapacityValidation() {

  $('#total-capacity-error').empty();

  let errMsg = validateNumberField($('#total-capacity').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#total-capacity-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function venueValidation() {

  $('#venue-error').empty();

  let errMsg = validateOptionalAddress($('#venue').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#venue-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function countryValidation() {
  $('#country-error').empty();

  let errMsg = validateDropdown($('#country').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#country-error').html(errMsg);
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
  $('#event-tags').selectpicker('val', '');
  $('#title').val('');
  $('#short-description').summernote('reset');
  $('#description').summernote('reset');
  $('#start-time').val('');
  $('#end-time').val('');
  $('#cost-per-person').val('');
  $('#total-capacity').val('');
  $('#venue').val('');
  $('#country').selectpicker('val', '');
  $('#state').selectpicker('val', '');
  $('#city').selectpicker('val', '');
  $('#contact-person').val('');
  $('#contact-number').val('');
  $('#contact-email').val('');

  $('#inp-banner-image').val('');
  $('#image-upload-info').removeClass('text-danger');
  $('#file-name').val('');

  $('#event-tags-error').empty();
  $('#title-error').empty();
  $('#short-description-error').empty();
  $('#description-error').empty();
  $('#start-time-error').empty();
  $('#end-time-error').empty();
  $('#cost-per-person-error').empty();
  $('#total-capacity-error').empty();
  $('#venue-error').empty();
  $('#country-error').empty();
  $('#state-error').empty();
  $('#city-error').empty();
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

function saveEvent(code) {
  eventTagsValidation();
  titleValidation();
  shortDescriptionValidation();
  startTimeValidation();
  endTimeValidation();
  costPerPersonValidation();
  totalCapacityValidation();
  venueValidation();
  countryValidation();
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
    dataObject.Tags = $('#event-tags').val();
    dataObject.Title = $('#title').val().trim();
    dataObject.ShortDescription = $('#short-description').summernote('code');
    dataObject.Description = $('#description').summernote('code');
    dataObject.StartTime = $('#start-time').val().trim();
    dataObject.EndTime = $('#end-time').val().trim();
    dataObject.CostPerPerson = $('#cost-per-person').val().trim();
    dataObject.TotalCapacity = $('#total-capacity').val().trim();
    dataObject.Venue = $('#venue').val().trim();
    dataObject.Country = $('#country').val().trim();
    dataObject.State = $('#state').val().trim();
    dataObject.City = $('#city').val().trim();
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
  let url = BASEURL + '/Admin/api/EventApi/SaveEvent';

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
        $(location).attr('href', BASEURL + '/Admin/Event/Manage');
      },
      404: function () {
        $(location).attr('href', BASEURL + '/Admin/Event/Manage');
      }
    }
  });
}