(function (self) {
  var calendar;

  var data;
  var minDate;

  var controller;

  var masterList;

  var model = {};

  self.init = function (d) {
    data = d;

    $('#timepicker-reschedule-start').datetimepicker({
      format: 'LT'
    });

    $('#timepicker-reschedule-end').datetimepicker({
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

    if (!masterList) {
      fetchMaster();
    }

    reset();
  }

  function reset() {
    $(".reschedule textarea").val("");
    $(".reschedule input[type='text']").val("");
    clearError();
  }

  function fetchMaster() {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Initializing. Please wait...');

    fetch('/api/master/200', {
      method: 'GET',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(res => {
        app.dismissProgress();

        masterList = res;

        $('#reasonSelect').select2({
          placeholder: "Select",
          data: res.map(e => {
            return { id: e.id, text: e.value }
          })
        });

        if (res && res.length > 0) {
          model.reason = res[0];
        }

      }).catch(app.http.catch);
  }

  function setupCalendar() {
    calendar = new Calendar($('.reschedule #calendar'));

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

    fetch('/api/jobOrder/' + data.id + '/reschedule', {
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

    model.scheduledOn = setDate($('.reschedule input.timepicker-reschedule-start').val());
    model.scheduledOnTo = setDate($('.reschedule input.timepicker-reschedule-end').val());

    if (app.utils.isNullOrEmpty(model.scheduledOn)) {
      $('.reschedule input.timepicker-reschedule-start[type="text"]').closest('.form-group').addClass('has-danger');
      $('.reschedule label.error-timepicker-reschedule-start').html('Start time is required');
    }

    if (app.utils.isNullOrEmpty(model.scheduledOnTo)) {
      $('.reschedule input.timepicker-reschedule-end[type="text"]').closest('.form-group').addClass('has-danger');
      $('.reschedule label.error-timepicker-reschedule-end').html('End time is required');
    }

    if (!app.utils.isNullOrEmpty(model.scheduledOn)
      && !app.utils.isNullOrEmpty(model.scheduledOnTo)
      && (model.scheduledOnTo < model.scheduledOn)) {
      $('.reschedule input.timepicker-reschedule-start[type="text"]').closest('.form-group').addClass('has-danger');
      $('.reschedule label.error-timepicker-reschedule-start').html('Start time should be smaller than end time');

      $('.reschedule input.timepicker-reschedule-end[type="text"]').closest('.form-group').addClass('has-danger');
      $('.reschedule label.error-timepicker-reschedule-end').html('End time should be bigger than start time');
    }

    return $('.reschedule .has-danger').length == 0;
  }

  function clearError() {
    $('.reschedule .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  }

  $(function () {
    $('.reschedule .primary').click(function () {
      submit();
    });

    $('#reasonSelect').on('select2:select', function (e) {
      model.reason = {
        id: e.params.data.id,
        value: e.params.data.text
      };
    });

    $(".reschedule textarea").on('change input paste', function () {
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

})(app.page.reschedule = app.page.reschedule || {});