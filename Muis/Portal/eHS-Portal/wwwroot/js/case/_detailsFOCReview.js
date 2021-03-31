(function (self) {
  'use strict'

  var model;
  var controller;

  var option = {
    title: 'Review Fact of Case',
    action: [
      { id: 0, text: 'Revert' },
      { id: 1, text: 'Save as Draft' },
      { id: 2, text: 'Approve' }]
  };

  self.init = function (_model) {
    model = _model;
  };

  self.onActionCallback = function (id, data, option) {

    if (id == 0) {
      $('.compose-letter .close').click();

      app.page.focRevert.init(model, data);
      $('#focRevertModal').modal({ show: true });
      return;
    }

    if (id == 2
      && app.page.letterCompose.validate()) {
      return;
    }

    var method = "";
    var body = {
      Letter: data
    };

    if (id == 2) {
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

    fetch('/api/case/' + model.id + '/review-foc', {
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

})(app.page.focReview = app.page.focReview || {});