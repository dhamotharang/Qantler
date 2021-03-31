(function (self) {

  $('.select-multiple').select2();

  $('.select-single').select2({
    placeholder: 'Select',
    allowClear: true
  });

  self.toParams = function () {
    var name = $('#filterName').val();
    var code = $('#filterCode').val();
    var groupCode = $('#filterGroupCode').val();
    var certificateNo = $('#filterCertificate').val();
    var premise = $('#filterPremise').val();
    var status = $('#filterStatusSelect').select2('data');


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

    if (name.trim().length > 0) {
      params = appendParam(params, 'name', name.trim());
    }

    if (code.trim().length > 0) {
      params = appendParam(params, 'code', code.trim());
    }

    if (groupCode.trim().length > 0) {
      params = appendParam(params, 'groupCode', groupCode.trim());
    }

    if (certificateNo.trim().length > 0) {
      params = appendParam(params, 'certificateNo', certificateNo.trim());
    }

    if (premise.trim().length > 0) {
      params = appendParam(params, 'premise', premise.trim());
    }

    return params;
  };

  function hide() {
    $('#filterCloseButton').click();
  }

  function clear() {
    $('#filterName').val('');
    $('#filterCode').val('');
    $('#filterGroupCode').val('');
    $('#filterCertificate').val('');
    $('#filterPremise').val('');
    $('#filterStatusSelect').val(null).trigger('change');
  }

  self.reset = function () {
    clear();

    $('#filterStatusSelect').val(app.page.model.defaultStatuses);
    $('#filterStatusSelect').trigger('change');

  }


  $(function () {

    $('#filterApplyButton').click(function () {
      if (app.page.fetch()) {
        hide();
      }
    });

    $("#filterClearButton").click(function () {
      clear();
    });

    $('#filterResetButton').click(function () {
      self.reset();
    });


  });

})(app.page.filter = app.page.filter || {});

$(document).ready(function () {
  app.page.filter.reset();
});