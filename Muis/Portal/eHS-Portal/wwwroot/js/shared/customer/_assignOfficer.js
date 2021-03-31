(function () {
  'use strict'
  var data;

  var controller;

  var callback;

  this.init = function (d, cb) {
    data = d;
    callback = cb;
    initOfficerInCharge();
  }

  function initOfficerInCharge() {
    var skip = [];

    if (data.officer) {
      skip.push(data.officer.id);
    }

    app.select.officer.init({
      key: 'officers',
      title: 'Assign Officer',
      actionText: 'Assign',
      permissions: [],
      skip: skip
    });

    app.select.officer.onSelect = onOfficerSelected;
  }

  function onOfficerSelected(o) {

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Updating. Please wait');

    fetch('/api/customer/' + data.id + '/officer/' + o.id, {
      method: 'PUT',
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
        app.dismissProgress();

        data.officer = o;

        callback(o);

      }).catch(app.http.catch);
  }
}).apply(app.page.officer = app.page.officer || {});