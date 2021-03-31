(function (self) {
  'use strict'

  var model;
  var controller;

  var option = {
    title: 'Draft Show Cause Letter',
    action: [
      { id: 0, text: 'Save as Draft' },
      { id: 1, text: 'Send' }]
  };

  self.init = function (_model) {
    model = _model;
  };

  self.onActionCallback = function (id, data, option) {
    if (id == 1
      && app.page.letterCompose.validate()) {
      return;
    }

    data.status = id == 1 ? 200 : 100;

    if (controller) {
      controller.abort();
    }

    controller = new AbortController();

    $('.compose-letter .close').click();
    app.showProgress('Processing. Please wait...');

    fetch('/api/case/' + model.id + '/showcause?', {
      method: 'POST',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(data)
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

})(app.page.showCause = app.page.showCause || {});