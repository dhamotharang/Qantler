(function () {
  var calendar;

  var data;
  var minDate;

  var controller;

  var scheduledOn, scheduledOnTo;

  this.init = function (d) {
    data = d;

    $('#timepicker-start').datetimepicker({
      format: 'LT'
    });

    $('#timepicker-end').datetimepicker({
      format: 'LT'
    });

    minDate = moment().local().startOf('day');

    setupCalendar();
    calendar.setCurrentDate(minDate);

    setTimeout(function () {
      calendar.render();
    }, 200);

    reset();
  }

  function reset() {
    $(".schedule textarea").val("");
    $(".schedule input[type='text']").val("");
    clearError();
  }

  function setupCalendar() {
    calendar = new Calendar($('.schedule #calendar'));

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

    calendar.init();
  }

  function submit() {
    if (!validate()) {
      return;
    }

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    var param = 'scheduledOn=' + encodeURI(moment.utc(scheduledOn).format());
    param += '&scheduledOnTo= ' + encodeURI(moment.utc(scheduledOnTo).format());
    param += '&notes=' + $('.schedule .notes').val();

    app.showProgress('Processing. Please wait...');
    
    fetch('/api/request/' + data.id + '/schedule?' + param, {
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

  function validate() {
    clearError();

    scheduledOn = setDate($('.schedule input.timepicker-start').val());
    scheduledOnTo = setDate($('.schedule input.timepicker-end').val());

    if (app.utils.isNullOrEmpty(scheduledOn)) {
      $('.schedule input.timepicker-start[type="text"]').closest('.form-group').addClass('has-danger');
      $('.schedule label.error-timepicker-start').html('Start time is required');
    }

    if (app.utils.isNullOrEmpty(scheduledOnTo)) {
      $('.schedule input.timepicker-end[type="text"]').closest('.form-group').addClass('has-danger');
      $('.schedule label.error-timepicker-end').html('End time is required');
    }

    if (!app.utils.isNullOrEmpty(scheduledOn)
      && !app.utils.isNullOrEmpty(scheduledOnTo)
      && (scheduledOnTo < scheduledOn)) {
      $('.schedule input.timepicker-start[type="text"]').closest('.form-group').addClass('has-danger');
      $('.schedule label.error-timepicker-start').html('Start time should be smaller than end time');

      $('.schedule input.timepicker-end[type="text"]').closest('.form-group').addClass('has-danger');
      $('.schedule label.error-timepicker-end').html('End time should be bigger than start time');
    }

    return $('.schedule .has-danger').length == 0;
  }

  function clearError() {
    $('.schedule .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  }

  function setDate(cuVal) {
    if (!app.utils.isNullOrEmpty(calendar.currentDate)
      && !app.utils.isNullOrEmpty(cuVal)) {
      return moment.utc(app.utils.addTimeToDate(calendar.currentDate, cuVal)).format();
    }
    return null;
  }

  $(function () {
    $('.schedule .schedule-button').click(function () {
      submit();
    });
  });
}).apply(app.page.schedule = app.page.schedule || {});