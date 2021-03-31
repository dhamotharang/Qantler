(function () {
  'use strict';

  this.toParams = function () {
    function appendParam(p, key, val) {
      if (!val) {
        return p;
      }

      if (p.length > 0) {
        p += '&';
      }
      p += key + '=' + val;
      return p;
    }
    var status = $('#filterStatus').select2('val');

    var params = '';
    params = appendParam(params, 'id', $('#filterID').val());
    params = appendParam(params, 'name', $('#filterName').val());
    params = appendParam(params, 'transactionNo', $('#filterTransactionNo').val());

    if (status.trim().length > 0) {
      params = appendParam(params, 'status', status);
    }

    params = appendParam(params, 'method', $('#filterMethod').select2('val'));
    params = appendParam(params, 'mode', $('#filterMode').select2('val'));

    var from = $('#datepicker-from').datepicker('getDate');
    if (from) {
      params = appendParam(params, 'from', (from ? moment.utc(from).format() : ''));
    }

    var to = $('#datepicker-to').datepicker('getDate');
    if (to) {
      params = appendParam(params, 'to', (to ? moment.utc(to).format() : ''));
    }

    return params;
  };

  function hide() {
    $('#filterCloseButton').click();
  }

  function clear() {
    $('#filterID').val('');
    $('#filterName').val('');
    $('#filterTransactionNo').val('');
    $('#filterStatus').val(null).trigger('change');
    $('#filterMethod').val(null).trigger('change');
    $('#filterMode').val(null).trigger('change');

    $('#datepicker-from').datepicker('update', '');
  }

  function reset() {
    clear();

    $('#filterStatus').val(app.page.model.defaultStatus).trigger('change');
  }

  $(function () {
    $('.select-single').select2({
      placeholder: 'Select',
      allowClear: true
    });

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
        hide();
      }
    });

    $("#filterClearButton").click(function () {
      clear();
    });

    $('#filterResetButton').click(function () {
      reset();
    });

    reset();
  });

}).apply(app.page.filter = app.page.filter || {});