(function (self) {
  'use strict'

  var model;
  var controller;

  var option = {
    title: 'Drafting Sanction Letter',
    action: [
      { id: 0, text: 'Save as Draft' },
      { id: 1, text: 'Send' }]
  };

  var optionView = {
    title: 'Show Sanction Letter'
  };

  self.init = function (_model) {
    model = _model;
  };

  self.onActionCallback = function (id, data, option) {
    if (id == 1
      && app.page.letterCompose.validate()) {
      return;
    }

    var method;
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

    fetch('/api/case/' + model.id + '/letter/sanction', {
      method: method,
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

  function setTitleBasedOnLetterType(letterType) {
    if (letterType == 402) {
      option.title = 'Drafting Warning Letter';
      optionView.title = 'Drafting Warning Letter';
    }
    else if (letterType == 403) {
      option.title = 'Drafting Compound Letter';
      optionView.title = 'Drafting Compound Letter';
    }
    else if (letterType == 404) {
      option.title = 'Drafting Suspension Letter';
      optionView.title = 'Drafting Suspension Letter';
    }
    else if (letterType == 405) {
      option.title = 'Drafting Immediate Suspension Letter';
      optionView.title = 'Drafting Immediate Suspension Letter';
    }
    else if (letterType == 406) {
      option.title = 'Drafting Revocation Letter';
      optionView.title = 'Drafting Revocation Letter';
    }
  };

  function setTitleBasedOnSanctionType(sanctionType) {
    if (sanctionType == 0) {
      option.title = 'Drafting Warning Letter';
      optionView.title = 'Drafting Warning Letter';
      return 402;
    }
    else if (sanctionType == 1) {
      option.title = 'Drafting Compound Letter';
      optionView.title = 'Drafting Compound Letter';
      return 403;
    }
    else if (sanctionType == 2) {
      option.title = 'Drafting Suspension Letter';
      optionView.title = 'Drafting Suspension Letter';
      return 404;
    }
    else if (sanctionType == 3) {
      option.title = 'Drafting Immediate Suspension Letter';
      optionView.title = 'Drafting Immediate Suspension Letter';
      return 405;
    }
    else if (sanctionType == 4) {
      option.title = 'Drafting Revocation Letter';
      optionView.title = 'Drafting Revocation Letter';
      return 406;
    }
  };

  self.onLetterTapped = function (letter) {
    if (!letter.type) {
      letter.type = setTitleBasedOnSanctionType(model.sanction)
    }
    else {
      setTitleBasedOnLetterType(letter.type);
    }

    app.page.letterCompose.callback = self.onActionCallback;
    app.page.letterCompose.init(letter, option);
    $('#ComposeLetterModel').modal({ show: true });
  }

})(app.page.sanctionLetter = app.page.sanctionLetter || {});