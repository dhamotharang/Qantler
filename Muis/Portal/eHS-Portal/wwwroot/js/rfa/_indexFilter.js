(function (self) {

  self.toParams = function () {
    var id = $('#filterID').val();
    var customer = $('#filterCustomer').val();
    var status = $('#filterStatusSelect').select2('data');
    var user = $('#filterUserSelect').select2('val');

    function appendParam(p, key, val) {
      if (p.length > 0) {
        p += '&';
      }
      p += key + '=' + val;
      return p;
    }

    var params = '';
    if (status.length > 0) {
      status.forEach((e, i) => {
        params = appendParam(params, 'status[' + i + ']', e.id);
      });
    }

    if (!app.utils.isNullOrEmpty(user)) {
      params = appendParam(params, 'raisedBy', user);
    }

    if (id.trim().length > 0) {
      params = appendParam(params, 'id', id.trim());
    }


    if (customer.trim().length > 0) {
      params = appendParam(params, 'customer', customer.trim());
    }

    var from = $('#datepicker-raisedOn').datepicker('getDate');
    if (from) {
      params = appendParam(params, 'createdOn', (from ? moment(from).startOf('day').utc().format() : ''));
    }

    var to = $('#datepicker-dueOn').datepicker('getDate');
    if (to) {
      params = appendParam(params, 'dueOn', (to ? moment(to).startOf('day').utc().format() : ''));
    }

    return params;
  };

  function hide() {
    $('#filterCloseButton').click();
  }

  function clear() {
    $('#filterID').val('');
    $('#filterCustomer').val('');
    $('#filterStatusSelect').val(null).trigger('change');
    $('#filterUserSelect').val(null).trigger('change');
    $('#datepicker-raisedOn').datepicker('update', null);
    $('#datepicker-dueOn').datepicker('update', null);
  }

  function reset() {
    clear();

    var defaultstatus = [100, 200];

    var defaultUser;
    if (app.hasPermission(1)
      || app.hasPermission(7)) {
      defaultUser = app.identity.id;
    }

    $('#filterStatusSelect').val(defaultstatus);
    $('#filterStatusSelect').trigger('change');

    $('#filterUserSelect').val(defaultUser);
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
      allowClear: true,
      data: users
    });
  }

  $(function () {
    $('.select-multiple').select2();

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

    $('#filterID').inputmask({
      regex: '[0-9]*'
    });

    $('#filterApplyButton').click(function () {
      if (app.page.fetch()) {
        hide();
      }
    });

    $("#filterClearButton").click(function () {
      clear();
    });

    $('#filterResetButton').click(function () {
      reset();
    });

    init();
    reset();
  });

})(app.page.filter = app.page.filter || {});