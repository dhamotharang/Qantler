(function (self) {
  'use strict'

  var model;
  var notes = "";
  var controller;
  var isSave = false
  var letter;

  function validate() {
    clearError();

    if (app.utils.isNullOrEmpty(notes)) {
      $('.foc-revert #notes').closest('.form-group').addClass('has-danger');
    }

    return $('.foc-revert .has-danger').length == 0;
  }

  function clearError() {
    $('.foc-revert .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  }

  self.init = function (_model, _letter) {
    model = _model;
    isSave = false;
    notes = "";
    letter = _letter;
    $(".foc-revert #notes").val('');
  };

  self.submit = function () {
    isSave = true;

    if (!validate()) {
      return;
    }

    var body = {
      Letter: letter,
      Notes: notes
    };

    if (controller) {
      controller.abort();
    }

    controller = new AbortController();

    $('.foc-revert .close').click();
    app.showProgress('Processing. Please wait...');

    fetch('/api/case/' + model.id + '/review-foc', {
      method: 'DELETE',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(body)
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(r => {
        location.reload(true);
      })
      .catch(error => {
        app.http.catch(error);
      });
  };

  $(".foc-revert #notes").on('change input paste', function () {
    notes = $(this).val().trim();
    if (isSave) {
      validate();
    }
  });

  $(function () {
    $('.foc-revert #submitBtn').click(function () {
      self.submit();
    });
  });

})(app.page.focRevert = app.page.focRevert || {});