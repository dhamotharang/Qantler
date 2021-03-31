(function () {
  'use strict';

  $('.select-multiple').select2();

  $('.select-single').select2({
    placeholder: 'Select',
    allowClear: true
  });

  this.toParams = function () {
    var id = $('#filterID').val();
    var premise = $('#filterPremise').val();
    var customerCode = $('#filterCustomerCode').val();
    var customerId = $('#filterCustomerId').val();
    var customer = $('#filterCustomer').val();
    var status = $('#filterStatusSelect').select2('data');
    var types = $('#filterTypeSelect').select2('data');
    var user = $('#filterUserSelect').select2('data');
    var rfa = $('#filterRFAStatusSelect').select2('val');
    var escalate = $('#filterEscalateStatusSelect').select2('val');
    var statusMinor = $('#filterStatusMinorSelect').select2('data');

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

    if (!app.utils.isNullOrEmpty(rfa)) {
      params = appendParam(params, 'rfaStatus', rfa);
    }

    if (id.trim().length > 0) {
      params = appendParam(params, 'id', id.trim());
    }

    if (premise.trim().length > 0) {
      params = appendParam(params, 'premise', premise.trim());
    }

    if (customerCode.trim().length > 0) {
      params = appendParam(params, 'customerCode', customerCode.trim());
    }

    if (customerId.trim().length > 0) {
      params = appendParam(params, 'customerID', customerId.trim());
    }

    if (customer.trim().length > 0) {
      params = appendParam(params, 'customer', customer.trim());
    }

    if (!app.utils.isNullOrEmpty(escalate)) {
      params = appendParam(params, 'escalateStatus', escalate);
    }

    var from = $('#datepicker-from').datepicker('getDate');
    if (from) {
      params = appendParam(params, 'from', (from ? moment(from).startOf('day').utc().format() : ''));
    }

    var to = $('#datepicker-to').datepicker('getDate');
    if (to) {
      params = appendParam(params, 'to', (to ? moment(to).startOf('day').utc().format() : ''));
    }

    if (statusMinor.length > 0) {
      statusMinor.forEach((e, i) => {
        params = appendParam(params, 'statusMinor[' + i + ']', e.id);
      });
    }

    return params;
  };

  this.hide = function () {
    $('#filterCloseButton').click();
  }

  this.clear = function () {
    $('#filterID').val('');
    $('#filterPremise').val('');
    $('#filterCustomerCode').val('');
    $('#filterCustomerId').val('');
    $('#filterCustomer').val('');
    $('#filterStatusSelect').val(null).trigger('change');
    $('#filterTypeSelect').val(null).trigger('change');
    $('#filterRFAStatusSelect').val(null).trigger('change');
    $('#filterUserSelect').val(null).trigger('change');
    $('#filterEscalateStatusSelect').val(null).trigger('change');
    $('#datepicker-from').datepicker('update', null);
    $('#datepicker-to').datepicker('update', null);
    $('#filterStatusMinorSelect').val(null).trigger('change');
    $("#divfilterStatusMinor").hide();
  }

  this.reset = function () {
    app.page.filter.clear();

    $('#filterStatusSelect').val(app.page.model.defaultStatuses);
    $('#filterStatusSelect').trigger('change');

    $('#filterUserSelect').val(app.page.model.assignedTo);
    $('#filterUserSelect').trigger('change');

    $('#filterCustomerId').val(app.page.model.customerId);

    $('#filterStatusMinorSelect').val(null).trigger('change');
    $("#divfilterStatusMinor").hide();
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

    $('#filterStatusMinorSelect').val(null).trigger('change');
    $("#divfilterStatusMinor").hide();
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
      if (app.page.fetch()) {
        app.page.filter.hide();
      }
    });

    $("#filterClearButton").click(function () {
      app.page.filter.clear();
    });

    $('#filterResetButton').click(function () {
      app.page.filter.reset();
    });

    $("#filterStatusSelect").on('change', function () {
      var status = $('#filterStatusSelect').select2('data');
      if (status.length > 0) {
        var sMinor;
        status.forEach((e, i) => {
          sMinor = sMinor + ',' + e.id;
        });
        if (sMinor.search(100) > -1) {         
          $("#divfilterStatusMinor").show();
        }
        else {
          $('#filterStatusMinorSelect').val(null).trigger('change');
          $("#divfilterStatusMinor").hide();
        }
      }
      else {
        $('#filterStatusMinorSelect').val(null).trigger('change');
        $("#divfilterStatusMinor").hide();
      }
    });

    init();
  });

}).apply(app.page.filter = app.page.filter || {});


$(document).ready(function () {
  app.page.filter.reset();
});