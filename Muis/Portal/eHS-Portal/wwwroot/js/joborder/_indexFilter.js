(function (self) {

  $('.select-multiple').select2();

  $('.select-single').select2({
    placeholder: 'Select',
    allowClear: true
  });

  function appendParam(p, key, val) {
    if (p.length > 0) {
      p += '&';
    }
    p += key + '=' + val;
    return p;
  }

  self.toParams = function () {
    var id = $('#filterID').val();
    var premise = $('#filterPremise').val();
    var customer = $('#filterCustomer').val();
    var status = $('#filterStatusSelect').select2('data');
    var types = $('#filterTypeSelect').select2('data');
    var user = $('#filterUserSelect').select2('data');
    
    var params = '';
    if (status.length > 0) {
      status.forEach((e, i) => {
        params = appendParam(params, 'status[' + i + ']', e.id);
      });
    }

    if (types.length > 0) {
      types.forEach((e, i) => {
        params = appendParam(params, 'type[' + i + ']', e.id);
      });
    }

    if (user.length > 0) {
      user.forEach((e, i) => {
        params = appendParam(params, 'assingedTo[' + i + ']', e.id);
      });
    }

    if (id.trim().length > 0) {
      params = appendParam(params, 'id', id.trim());
    }

    if (premise.trim().length > 0) {
      params = appendParam(params, 'premise', premise.trim());
    }

    if (customer.trim().length > 0) {
      params = appendParam(params, 'customer', customer.trim());
    }

    var from = $('#datepicker-from').datepicker('getDate');
    if (from) {
      params = appendParam(params, 'from', (from ? moment(from).startOf('day').utc().format() : ''));
    }

    var to = $('#datepicker-to').datepicker('getDate');
    if (to) {
      params = appendParam(params, 'to', (to ? moment(to).startOf('day').utc().format() : ''));
    }
    
    return params;
  };

  function hide() {
    $('#filterCloseButton').click();
  }

  function clear() {
    $('#filterID').val('');
    $('#filterPremise').val('');
    $('#filterCustomer').val('');
    $('#filterStatusSelect').val(null).trigger('change');
    $('#filterTypeSelect').val(null).trigger('change');
    $('#filterUserSelect').val(null).trigger('change');
    $('#datepicker-from').datepicker('update', null);
    $('#datepicker-to').datepicker('update', null);
  }

  self.reset = function () {
    clear();

    $('#filterStatusSelect').val(app.page.model.defaultStatuses);
    $('#filterStatusSelect').trigger('change');

    $('#filterTypeSelect').val(app.page.model.defaultType);
    $('#filterTypeSelect').trigger('change');

    $('#filterUserSelect').val(app.page.model.assignedTo);
    $('#filterUserSelect').trigger('change');
  }

  function init() {
    var users = [];
    if (app.page.model.users) {
      users = app.page.model.users.sort((a, b) => a.name.localeCompare(b.name)).map(e => {
        return { id: e.id, text: e.name };
      });
    }

    $('#filterUserSelect').select2({
      placeholder: "Select",
      data: users
    });
  }

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
      if (app.page.jobs.fetch()) {
        hide();
      }
    });

    $("#filterClearButton").click(function () {
      clear();
    });

    $('#filterResetButton').click(function () {
      self.reset();
    });

    init();
  });

})(app.page.filter = app.page.filter || {});

$(document).ready(function () {
  app.page.filter.reset();
});