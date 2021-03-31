(function () {
  var calendar;

  var data;
  var minDate;

  var controller;

  this.init = function (d) {
    data = d;

    minDate = moment.utc(data.dueOn).local().add(1, 'days').startOf('day');

    setupCalendar();
    calendar.setCurrentDate(minDate);

    setTimeout(function () {
      calendar.render();
    }, 200);
  }

  function setupCalendar() {
    calendar = new Calendar($('#calendar'));

    calendar.selectAllow = function (info) {
      var diff = info.end.diff(info.start, 'days');
      if (diff > 1) {
        calendar.setCurrentDate(calendar.currentDate);
        return false;
      }

      if (info.start < minDate) {
        calendar.setCurrentDate(calendar.currentDate);
        return false;
      }

      calendar.setCurrentDate(info.start);

      return true;
    }

    calendar.eventSources = function () {
      return [{
        events: function (start, end, timezone, callback) {
          var events = [];

          var dueOn = moment.utc(data.dueOn).local();

          if (dueOn >= start && dueOn <= end) {
            events.push({
              title: 'Current Due Date',
              start: dueOn,
              allDAy: true,
            })
          }

          callback(events);
        },
        color: 'red',
        textColor: 'white'
      }];
    }

    calendar.init();
  }

  function submit() {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    var param = 'toDate=' + encodeURI(moment.utc(calendar.currentDate).format()) + '&notes=' + $('#ExtendRFAModal .notes').val();

    fetch('/api/rfa/' + data.id + '/extend?' + param , {
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
    $('#ExtendRFAModal .extend-button').click(function () {
      submit();
    });
  });

}).apply(app.page.rfa.extend = app.page.rfa.extend || {});