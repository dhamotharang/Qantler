(function (page) {
  'use strict';

  var controller;
  var data = {};

  function validate() {
    clearError();

    if (!data.key
      || data.key.length == 0) {

      $('.key').addClass('has-danger');

    }

    return $('.has-danger').length == 0;
  }

  function clearError() {
    $('.has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  }

  function resetPassword() {

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Processing request. Please wait...');

    fetch('/api/home/forgot-password?email=' + data.key, {
      method: 'POST',
      cache: 'no-cache',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(res => {

        Swal.fire(
          'Forgot Password!',
          'Password has been reset. Please check your email.',
          'success'
        ).then(res => {
          window.location = '/home/index?key=' + data.key;
        });

      }).catch(app.http.catch);
  }

  $(function () {
    $('#form').on('submit', function (e) {
      e.preventDefault();

      if (!validate()) {
        return;
      }

      resetPassword();
    });

    $("#key").on('change input paste', function () {
      data.key = $(this).val().trim();
    });
  });

})(app.page = app.page || {});
