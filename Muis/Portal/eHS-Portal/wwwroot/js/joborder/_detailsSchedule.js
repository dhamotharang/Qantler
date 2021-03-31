(function (self) {
  var calendar;

  var data;
  var minDate;

  var controller;

  var model = {};

  self.init = function (d) {
    data = d;

    $('#timepicker-schedule-start').datetimepicker({
      format: 'LT'
    });

    $('#timepicker-schedule-end').datetimepicker({
      format: 'LT'
    });

    if (moment().startOf('day').utc().diff(moment.utc(data.scheduledOn)) == 0) {
      minDate = moment().local().add('d', 1).startOf('day');
    } else {
      minDate = moment().local().startOf('day');
    }

    setupCalendar();
    calendar.setCurrentDate(minDate);

    setTimeout(function () {
      calendar.render();
    }, 200);

    reset();
  }

  function reset() {
    $(".pischedule textarea").val("");
    $(".pischedule input[type='text']").val("");
    clearError();
  }

  function setupCalendar() {
    calendar = new Calendar($('.pischedule #calendar'));

    calendar.eventSources = function () {
      return [{
        events: [{
          title: 'Current',
          start: moment.utc(data.scheduledOn).local().startOf('day')
        }],
        color: 'red',
        textColor: 'white'
      }];
    };

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

      if (moment(info.start).add('m', -moment().local().utcOffset()).utc().diff(moment.utc(data.scheduledOn)) == 0) {
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

    app.showProgress('Processing. Please wait...');

    fetch('/api/jobOrder/' + data.id + '/schedule', {
      method: 'PUT',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      body: JSON.stringify(model),
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
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

    model.scheduledOn = setDate($('.pischedule input.timepicker-schedule-start').val());
    model.scheduledOnTo = setDate($('.pischedule input.timepicker-schedule-end').val());

    if (app.utils.isNullOrEmpty(model.scheduledOn)) {
      $('.pischedule input.timepicker-schedule-start[type="text"]').closest('.form-group').addClass('has-danger');
      $('.pischedule label.error-timepicker-schedule-start').html('Start time is required');
    }

    if (app.utils.isNullOrEmpty(model.scheduledOnTo)) {
      $('.pischedule input.timepicker-schedule-end[type="text"]').closest('.form-group').addClass('has-danger');
      $('.pischedule label.error-timepicker-schedule-end').html('End time is required');
    }

    if (!app.utils.isNullOrEmpty(model.scheduledOn)
      && !app.utils.isNullOrEmpty(model.scheduledOnTo)
      && (model.scheduledOnTo < model.scheduledOn)) {
      $('.pischedule input.timepicker-schedule-start[type="text"]').closest('.form-group').addClass('has-danger');
      $('.pischedule label.error-timepicker-schedule-start').html('Start time should be smaller than end time');

      $('.pischedule input.timepicker-schedule-end[type="text"]').closest('.form-group').addClass('has-danger');
      $('.pischedule label.error-timepicker-schedule-end').html('End time should be bigger than start time');
    }
    return $('.pischedule .has-danger').length == 0;
  }

  function clearError() {
    $('.pischedule .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  }

  $(function () {
    $('.pischedule .primary').click(function () {
      submit();
    });

    $(".pischedule textarea").on('change input paste', function () {
      model.notes = $(this).val().trim();
    });

  });

  function setDate(cuVal) {
    if (!app.utils.isNullOrEmpty(calendar.currentDate)
      && !app.utils.isNullOrEmpty(cuVal)) {
      return moment.utc(app.utils.addTimeToDate(calendar.currentDate, cuVal)).format();
    }
    return null;
  }

})(app.page.schedule = app.page.schedule || {});