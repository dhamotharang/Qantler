(function () {

  var data;
  var controller;

  var calendar;

  this.init = function (d) {
    data = d;

    setupCalendar();
    calendar.setCurrentDate(calendar.minDate);

    setTimeout(function () {
      calendar.render();
    }, 200);
  }

  function setupCalendar() {
    calendar = new Calendar($('.kiv #calendar'));
    calendar.setMinDate(moment().local().add(1, 'days').startOf('day'));
    calendar.init();
  }

  function kiv() {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    var param = 'remindOn=' + encodeURI(moment.utc(calendar.currentDate).format()) + '&notes=' + encodeURI($('.kiv .notes').val());

    fetch('/api/request/' + data.id + '/kiv?' + param, {
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

  this.revert = function (data) {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    fetch('/api/request/' + data.id + '/kiv', {
      method: 'DELETE',
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
    $('.kiv .kiv-button').click(function () {
      kiv();
    })
  });

}).apply(app.page.kiv = app.page.kiv || {});