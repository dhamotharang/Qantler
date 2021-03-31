app.select = app.select || {};
app.select.customer = app.select.customer || {};

(function (self) {
  'use strict';

  var model = {};

  var controller;
  var isSave = false;

  self.init = function() {
    model = {
      idType: null
    };

    setup();
  }

  self.dismiss = function () {
    $('.create-customer .close').click();
  }

  self.createCallback = function (c) {
  }

  function setup() {
    $('.create-customer #name').val(null);

    $('.create-customer #altid').val(null);

    clearError();
    isSave = false;
  }

  function validate() {
    clearError();

    if (app.utils.isNullOrEmpty(model.name)) {
      $('.create-customer #name').closest('.form-group').addClass('has-danger');
    }

    return $('.create-customer .has-danger').length == 0;
  }

  function clearError() {
    $('.create-customer .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  }

  function submit() {
    isSave = true;

    if (!validate()) {
      return;
    }

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Creating. Please wait...');

    fetch('/api/customer', {
      method: 'POST',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      body: JSON.stringify(model),
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(res => {
        app.dismissProgress();

        self.dismiss();
        self.createCallback(res);

      }).catch(app.http.catch);
  }

  $(function () {

    $('.create-customer #name').on('change input paste', function () {
      model.name = $(this).val().trim();
      if (isSave) {
        validate();
      }
    });

    $('.create-customer #altid').on('change input paste', function () {
      model.altID = $(this).val().trim();
      if (model.altID != '') {
        model.idType = 0;
      } else {
        model.idType = null;
      }
    });

    $('.create-customer #createBtn').click(function () {
      submit();
    });
  });

})(app.select.customer.create = app.select.customer.create || {});