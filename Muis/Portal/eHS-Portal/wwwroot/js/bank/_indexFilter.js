(function (self) {
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
    var params = '';

    params = appendParam(params, 'accountNo', $('#filterAccountNo').val());
    params = appendParam(params, 'accountName', $('#filterAccountName').val());
    params = appendParam(params, 'branchCode', $('#filterBranchCode').val());
    params = appendParam(params, 'bankName', $('#filterBankName').select2('val'));
    params = appendParam(params, 'status', $('#filterDDAStatus').select2('val'));  

    return params;
  };

  function hide() {
    $('#filterCloseButton').click();
  }

  function clear() {
    $('#filterAccountNo').val('');
    $('#filterAccountName').val('');
    $('#filterBranchCode').val('');
    $('#filterBankName').val('').trigger('change');
    $('#filterDDAStatus').val('').trigger('change');
  }

  self.reset = function () {
    clear();     
  }

  self.init = function(d) {
    $('#filterBankName').select2({
      placeholder: "Select",
      data: d.map(e => {
        return { id: e.value, text: e.value }
      })
    });
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
