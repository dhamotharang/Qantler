var app = {};

(function (self) {

  var data = { provider: 0 };

  function validate() {
    clearError();

    if (!data.key || data.key.length == 0) {
      $('.key').addClass('has-danger');
    }

    if (!data.secret || data.secret.length == 0) {
      $('.secret').addClass('has-danger');
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

    $("#key").on('change input paste', function () {
      data.key = $(this).val().trim();
    });

    $("#secret").on('change input paste', function () {
      data.secret = $(this).val().trim();
    });
  });

})(app.page = app.page || {});

$(document).ready(function () {
  if (error) {
    Swal.fire({
      icon: 'error',
      title: error
    });
  }
});