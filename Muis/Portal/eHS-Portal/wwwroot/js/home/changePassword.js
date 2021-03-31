var app = {};

(function (self) {

  var data = {};

  function validate() {
    clearError();

    if (!data.newSecret || data.newSecret.length == 0) {
      $('.new-secret').addClass('has-danger');
      $('.new-secret .error').html('New password is required.');
    }
    else if (data.newSecret.length < 6) {
      $('.new-secret').addClass('has-danger');
      $('.new-secret .error').html('New password should be atleast 6 character long.');
    }
    else if (data.newSecret != data.confirmSecret)
    {
      $('.confirm-secret').addClass('has-danger');
    }

    return $('.has-danger').length == 0;
  }

  function clearError() {
    $('.has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  }

  $(function () {
    $('#form').on('submit', function (e) {
      if (!validate()) {
        e.preventDefault();
        return;
      }
    });

    $("#newSecret").on('change input paste', function () {
      data.newSecret = $(this).val().trim();
    });

    $("#confirmSecret").on('change input paste', function () {
      data.confirmSecret = $(this).val().trim();
    });
  });

})(app.page = app.page || {});
