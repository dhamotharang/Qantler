(function (self) {
  $('.select-single').select2({
    placeholder: 'Select',
    allowClear: true
  });

  $('.select-multiple').select2({
    placeholder: 'Select',
    allowClear: true
  });

  self.toParams = function () {
    var id = $('#filterID').val();
    var offenceType = $('#filterOffenceType').select2('data');
    var source = $('#filterSourceSelect').select2('data');
    var status = $('#filterStatusSelect').select2('data');
    var user = $('#filterAssignedTo').select2('data');
    var from = $('#datepicker-from').datepicker('getDate');
    var to = $('#datepicker-to').datepicker('getDate');
    var managedBy = $('#filterManagedBy').select2('data');

    function appendParam(p, key, val) {
      if (p.length > 0) {
        p += '&';
      }
      p += key + '=' + val;
      return p;
    }

    var params = '';

    if (id.trim().length > 0) {
      params = appendParam(params, 'id', id.trim());
    }

    if (offenceType.length > 0) {
      offenceType.forEach((e, i) => {
        params = appendParam(params, 'offenceType', e.id);
      });
    }

    if (source.length > 0) {
      source.forEach((e, i) => {
        params = appendParam(params, 'source', e.id);
      });
    }

    if (status.length > 0) {
      status.forEach((e, i) => {
        params = appendParam(params, 'status[' + i + ']', e.id);
      });
    }

    if (user.length > 0) {
      user.forEach((e, i) => {
        params = appendParam(params, 'assignedTo[' + i + ']', e.id);
      });
    }

    if (managedBy.length > 0) {
      managedBy.forEach((e, i) => {
        params = appendParam(params, 'managedBy[' + i + ']', e.id);
      });
    }

    if (from) {
      params = appendParam(params, 'from', (from ? moment(from).startOf('day').utc().format() : ''));
    }

    if (to) {
      params = appendParam(params, 'to', (to ? moment(to).startOf('day').utc().format() : ''));
    }

    return params;
  };

  function clear() {
    $('#filterID').val('');
    $('#filterOffenceType').val(null).trigger('change');
    $('#filterSourceSelect').val(null).trigger('change');
    $('#filterStatusSelect').val(null).trigger('change');
    $('#filterAssignedTo').val(null).trigger('change');
    $('#filterManagedBy').val(null).trigger('change');
    $('#datepicker-from').datepicker('update', null);
    $('#datepicker-to').datepicker('update', null);
  }

  function init() {
    var users = [];
    var offenceType = [];

    if (app.page.model.users) {
      users = app.page.model.users.sort((a, b) => a.name.localeCompare(b.name)).map(e => {
        return { id: e.id, text: e.name };
      });
    }

    if (users) {
      users = users.sort((a, b) => a.text.localeCompare(b.text)).map(e => {
        return { id: e.id, text: e.text };
      });
    }

    if (app.page.model.offenceType) {
      offenceType = app.page.model.offenceType.sort((a, b) =>
        a.value.localeCompare(b.value)).map(e => {
          return { id: e.id, text: e.value };
        });
    }

    if (offenceType) {
      offenceType = offenceType.sort((a, b) => a.text.localeCompare(b.text)).map(e => {
        return { id: e.id, text: e.text };
      });
    }

    $('#filterOffenceType').select2({
      data: offenceType
    });

    $('#filterAssignedTo').select2({
      data: users
    });

    $('#filterManagedBy').select2({
      data: users
    });
    clear();

    var userCaseReadWrite = app.identity.permissions.charAt(24) == "1" ? true : false;
    if (userCaseReadWrite) {
      $('#filterManagedBy').val([app.identity.id]).trigger('change');
    }
  };

  $(function () {
    $('.datepicker').datepicker({
      enableOnReadonly: true,
      todayHighlight: true,
      container: '#filterModal'
    });

    $('.datepicker').on('dp.show', function () {
      var datepicker = $('body').find('.bootstrap-datetimepicker-widget:last');
      if (datepicker.hasClass('bottom')) {
        var top = $(this).offset().top + $(this).outerHeight();
        var left = $(this).offset().left;
        datepicker.css({
          'top': top + 'px',
          'bottom': 'auto',
          'left': left + 'px'
        });
      } else if (datepicker.hasClass('top')) {
        var top = $(this).offset().top - datepicker.outerHeight();
        var left = $(this).offset().left;
        datepicker.css({
          'top': top + 'px',
          'bottom': 'auto',
          'left': left + 'px'
        });
      }
    });

    $('#filterApplyButton').click(function () {
      app.page.fetch();
      $('#filterCloseButton').click();
    });

    $("#filterClearButton").click(function () {
      clear();
    });

    init();
  });

})(app.page.filter = app.page.filter || {});
