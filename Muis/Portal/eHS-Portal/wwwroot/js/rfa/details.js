(function (self) {
  'use strict'

  var controller;

  function closeRFA() {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Processing. Please wait...');

    fetch('/api/rfa/' + self.id + '/close', {
      method: 'POST',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(r => {
        location.reload(true);
      })
      .catch(app.http.catch);
  }

  $(function () {

    self.main.init(self.model);
    self.checklist.init(self.model);
    self.activities.init(self.model.logs);
    self.attachments.init(self.model);

    $('#action-close').click(function () {
      closeRFA();
    });
  });

})(app.page);