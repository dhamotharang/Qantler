(function () {
  'use strict';

  var controller;

  var id;
  var status;

  this.init = function (i, s) {
    id = i;
    status = s;

    setup();
  }

  function setup() {
    var isActive = status == 100;

    $('.escalate-modal .modal-title').html(isActive ? 'Escalate Action' : 'Escalate');

    $('.escalate-modal .remarks').prop('placeholder',
      isActive
        ? 'Enter escalation action remarks or notes. (e.g. key points or actions being discussed, for further action)'
        : 'Enter escalation remarks. (e.g. reason for escalating the request, key points to highlight)');

    var selectContainer = $('.escalate-modal .status-container');
    var select = $('.escalate-modal .status');

    selectContainer.addClass('d-none');

    if (isActive) {
      selectContainer.removeClass('d-none');
      select.val('100').trigger('change');
    }
  }

  function submit() {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    var status = '100';
    if (status == 100) {
      status = $('.escalate-modal .status').select2('val')  ;
    }

    var param = 'status=' + status +
      '&remarks=' + encodeURI($('.escalate-modal .remarks').val());

    fetch('/api/request/' + id + '/escalate?' + param, {
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
    $('.escalate-modal .submit').click(function () {
      submit();
    });


    $('.escalate-modal .status').select2();
  })

}).apply(app.page.escalate = app.page.escalate || {});