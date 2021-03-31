app.select = app.select || {};
app.select.code = app.select.code || {};

(function (self) {
  'use strict';

  var options = {};

  var code;
  var model = {};
  var list = [];
  var isSave = false;
  var controller;

  self.init = function (o, data = []) {
    list = data;

    isSave = false;

    $('#description').removeClass('border-danger');
    $('#description').parent().next('label').hide();

    var title = o.type == 0 ? 'Create Code' : 'Create Group Code';
    $('.create-code .modal-title').html(title);

    var oldType = options.type;
    options = o;

    if (!code
      || oldType != o.type) {
      code = '';
      generateCode();
    } else {
      setup();
    }
  }

  self.dismiss = function () {
    $('.create-code .close').click();
  }

  self.onCodeCreated = function (c) {
  }

  function setup() {
    $('.create-code #code').val(code);

    var descr = '';
    if (options.defaults) {
      descr = app.utils.emptyIfNullOrEmpty(options.defaults.text);
    }
    $('.create-code #description').val(descr);

    model = {
      value: code,
      text: descr,
      type: options.type
    }
  }

  function generateCode() {
    if (code) {
      return;
    }

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Generating code. Please wait...');

    fetch('/api/code/generate?type=' + options.type, {
      method: 'GET',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal
    }).then(app.http.errorInterceptor)
      .then(res => res.text())
      .then(res => {
        app.dismissProgress();

        code = res;

        setup();

      }).catch(app.http.catch);
  }

  function validate(description) {
    if (description != '') {
      var duplicate = list.filter(function (obj) {
        return (obj.text === description);
      });

      if (duplicate && duplicate.length == 0) {
        $('#description').removeClass('border-danger');
        $('#description').parent().next('label').hide();
        return true;
      }
      else {
        $('#description').addClass('border-danger');
        $('#description').parent().next('label').show();
        $('#description').parent().next('label').html("Duplicate description");
        return false;
      }
    }
    else {
      $('#description').addClass('border-danger');
      $('#description').parent().next('label').show();
      $('#description').parent().next('label').html("Description is required");
      return false;
    }

  }

  function submit() {
    isSave = true;

    var isValid = validate($('#description').val());

    if (isValid) {

      if (controller) {
        controller.abort();
      }
      controller = new AbortController();

      app.showProgress('Creating code. Please wait...');

      fetch('/api/code', {
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
          code = '';
          model = {};

          self.onCodeCreated(res);

        }).catch(app.http.catch);
    }
  }

  $(function () {

    $('#description').keyup(function (e) {
      if (isSave) {
        validate($('#description').val());
      }
    });

    $('.create-code #description').on('change input paste', function () {
      model.text = $(this).val();
    });

    $('.create-code #createBtn').click(function () {
      submit();
    });
  });

})(app.select.code.create = app.select.code.create || {});