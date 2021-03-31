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

    var params = '';
    params = appendParam(params, 'id', $('#filterID').val());
    params = appendParam(params, 'invoiceNo', $('#filterInvoiceNo').val());
    params = appendParam(params, 'customerName', $('#filterName').val());
    params = appendParam(params, 'refID', $('#filterReferenceID').val());
    params = appendParam(params, 'status', $('#filterStatus').select2('val'));  
    params = appendParam(params, 'type', $('#filterType').select2('val'));  

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
    $('#filterInvoiceNo').val('');
    $('#filterName').val('');
    $('#filterReferenceID').val('');
    $('#filterStatus').val(null).trigger('change');
    $('#filterType').val(null).trigger('change');

    $('#datepicker-from').datepicker('update', '');
    $('#datepicker-to').datepicker('update', '');
  }

  function reset() {
    clear();

    $('#filterStatus').val(200).trigger('change');
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
      if (app.page.invoice.fetch()) {
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