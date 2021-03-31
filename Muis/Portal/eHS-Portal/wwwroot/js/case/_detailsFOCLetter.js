(function (self) {
  'use strict'

  var model;
  var controller;

  var option = {
    title: 'Drafting Fact of Case',
    action: [
      { id: 0, text: 'Save as Draft' },
      { id: 1, text: 'Send' }]
  };

  self.init = function (_model) {
    model = _model;

    if (model.sanction == 7) {
      option.title = 'Update Case File';
    }
  };

  self.onActionCallback = function (id, data, option) {
    if (id == 1
      && app.page.letterCompose.validate()) {
      return;
    }

    var method = "";
    var body = {
      letter: data
    };

    if (id == 1) {
      data.status = 200;
      method = "POST";
    } else {
      data.status = 100;
      method = "PUT";
    }

    if (controller) {
      controller.abort();
    }

    controller = new AbortController();

    $('.compose-letter .close').click();
    app.showProgress('Processing. Please wait...');

    fetch('/api/case/' + model.id + '/foc', {
      method: method,
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
  }

  self.onLetterTapped = function (letter) {
    app.page.letterCompose.callback = self.onActionCallback;
    app.page.letterCompose.init(letter, option);
    $('#ComposeLetterModel').modal({ show: true });
  }

})(app.page.focLetter = app.page.focLetter || {});