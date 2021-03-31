(function () {

  var controller;

  var id;
  var data = [];

  this.init = function (d) {
    id = d;
    fetchHistory();
  }

  function fetchHistory() {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    fetch('/api/certificate/' + id + '/history', {
      method: 'GET',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(res => {
        data = res;

        invalidate();

        $('.widget.history .grid-margin.stretch-card').addClass('d-none');

      }).catch(app.http.catch);
  }

  function invalidate() {
    if (!data
      || data.length === 0) {

      $('.widget.history .widget-placeholder').removeClass('d-none');
      $('.widget.history .history-container').addClass('d-none');

      return;
    }

    var template = $('.history-template .tickets-card');
    var container = $('.history-container');

    container.empty();
    data.forEach((e, index) => {

      container.append(template.clone()
        .prop('outerHTML')
        .replaceAll('{id}', e.requestID)
        .replaceAll('{refid}', e.refID)
        .replaceAll('{serialno}', e.serialNo)
        .replaceAll('{issuedon}', app.utils.formatDateTime(e.issuedOn)));
    });
  }
}).apply(app.page.main.history = app.page.main.history || {});