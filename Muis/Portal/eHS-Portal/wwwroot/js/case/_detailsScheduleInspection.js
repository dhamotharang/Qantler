(function () {
  var calendar;

  var data;
  var minDate;
  var option;

  var controller;

  var scheduledOn, scheduledOnTo;

  var premisesData = [];

  $('.select-single').select2({
    placeholder: 'Select',
    allowClear: true
  });

  this.init = function (d, _option) {
    data = d;
    option = _option;

    $('.schedule .modal-title').html(option.title);

    $('#timepicker-start').datetimepicker({
      format: 'LT'
    });

    $('#timepicker-end').datetimepicker({
      format: 'LT'
    });

    minDate = moment().local().startOf('day');

    setupCalendar();
    calendar.setCurrentDate(minDate);

    setPremises();

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

  function setPremises() {
    if (premisesData.length == 0) {
      if (data.premises.length > 1) {
        premisesData.push({ id: 0, text: "Please Select" })
      }
      data.premises.forEach(function (item) {
        var premiseTemplate = "<div>" + app.utils.formatPremise(item) + "</div>";
        premisesData.push({ id: item.id, text: $(premiseTemplate).text() })
      });
      $('.schedule .select-single').select2({
        data: premisesData,
        placeholder: 'Select'
      });
    }
    else {
      $('#premisesSelect').select2("val", 0);
    }
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

    var body = {};
    body.ScheduledOn = moment.utc(scheduledOn).format();
    body.ScheduledOnTo = moment.utc(scheduledOnTo).format();
    body.PremiseID = $('#premisesSelect').select2('data')[0].id
    body.Type = option.type;
    body.Notes = $('.schedule .notes').val();
    body.PremisesText = app.utils.formatPremise(app.page.model.premises
      .find(x => x.id == body.PremiseID));

    app.showProgress('Processing. Please wait...');

    fetch('/api/case/' + data.id + '/ScheduleInspection?', {
      method: 'POST',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(body)
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

    if ($('#premisesSelect').select2("val") == 0) {
      $('#premisesSelect').closest('.form-group').addClass('has-danger');
      $('label.error-premises').html('Premise is required');
    }

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

    $('.show-Inspection').click(function () {
      reset();
    });

    $("#ScheduleInspectionModal").on('shown.bs.modal', function () {
      calendar.render()
    });
  });
}).apply(app.page.schedule = app.page.schedule || {});