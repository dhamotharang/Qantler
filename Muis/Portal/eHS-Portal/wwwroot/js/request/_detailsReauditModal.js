(function () {

  var data;
  var controller;

  this.init = function (d) {
    data = d;
  }

  this.submit = function () {
    if (!validate()) {
      return;
    }

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    var remarks = $('.reaudit textarea').val().trim();

    fetch('/api/request/' + data.id + '/reaudit?remarks=' + encodeURI(remarks), {
      method: 'POST',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(r => {
        location.reload(true);
      })
      .catch(app.http.catch);
  }

  function validate() {
    clearError();

    var textArea = $('.reaudit textarea');
    if (app.utils.isNullOrEmpty(textArea.val().trim())) {
      textArea.closest('.form-group').addClass('has-danger');
    }

    return $('.reaudit .modal-body .has-danger').length == 0;
  }

  function clearError() {
    $('.reaudit .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  }

  $(function () {

    $('.reaudit .reaudit-button').click(function () {
      app.page.reaudit.submit();
    });

    $(document).on('change input paste', '.reaudit textarea', function () {
      var val = $(this).val();

      if (!app.utils.isNullOrEmpty(val)) {
        $(this).closest('.form-group').removeClass('has-danger');
      }
    });
  });

}).apply(app.page.reaudit = app.page.reaudit || {});