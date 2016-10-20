var isValid = false;

$(document).ready(function () {
  $('#category-name').focusout(function () {
    categoryNameValidation();
  });

  $('#category-description').focusout(function () {
    categoryDescriptionValidation();
  });

  $('#reset').on('click', function () {
    resetData();
  });

  $('#submit').on('click', function () {
    saveCategory();
  });
});


function categoryNameValidation() {
  $('#category-name-error').empty();

  let errMsg = validateCategoryName($('#category-name').val().trim());

  if (errMsg !== '') {
    isValid = false;
    $('#category-name-error').html(errMsg);
  }
  else {
    checkCategoryPresent($('#category-name').val().trim(), function (isPresent) {
      if (isPresent) {
        isValid = false;
        $('#category-name-error').html('Category with this name already exists.');
      }
      else {
        isValid = true;
      }
    });
  }
}

function categoryDescriptionValidation() {
  $('#category-description-error').empty();

  let errMsg = validateOptionalCategoryDescription($('#category-description').val().trim());
  if (errMsg !== '') {
    isValid = false;
    $('#category-description-error').html(errMsg);
  }
  else {
    isValid = true;
  }
}

function checkCategoryPresent(categoryName, callback) {
  let url = BASEURL + '/Admin/api/EventApi/IsCategoryPresent?category=' + categoryName;

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

function resetData() {
  $('#category-name').val('');
  $('#category-description').val('');

  $('#category-name-error').empty();
  $('#category-description-error').empty();

  $('#message').empty();
  $('#message').addClass('hide');
}

function saveCategory() {
  categoryNameValidation();
  categoryDescriptionValidation();

  if (isValid) {
    let dataObject = new Object();
    dataObject.Name = $('#category-name').val().trim();
    dataObject.Description = $('#category-description').val().trim();

    let url = BASEURL + '/Admin/api/EventApi/SaveCategory';

    $.ajax({
      url: url,
      type: 'POST',
      dataType: 'json',
      contentType: 'application/json',
      data: JSON.stringify(dataObject),
      statusCode: {
        401: function () {
          $(location).attr('href', BASEURL + '/User/Logout');
        },
        400: function () {
          $('#message').html('<strong> Error! </strong>There are certain invalid or empty fields. Please fill in all the required information correctly and try again.');
          $('#message').removeClass('hide');
        },
        201: function () {
          $(location).attr('href', BASEURL + '/Admin/Event/ManageCategories');
        }
      }
    });
  }
  else {
    $('#message').html('<strong> Error! </strong>There are certain invalid or empty fields. Please fill in all the required information correctly and try again.');
    $('#message').removeClass('hide');
  }
}