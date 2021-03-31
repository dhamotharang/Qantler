(function (self) {
  var controller;

  var data;

  var bankMasterList;

  var model = {};

  self.init = function (d) {
    data = d;

    if (!bankMasterList) {
      fetchMaster(function () {
        self.init(data);
      });

      return;
    }

    reset();

    var title = (data.method == 3
      ? 'Bank Transfer - Offline Payment Verification'
      : (data.method == 4 ? 'DDA - Offline Payment Verification' : ''))

    $('.modal.offlinepayment #modalTitle').html(title);

    $('.modal.offlinepayment #accountNo').val(data.bank ? data.bank.accountNo : '').trigger('change');

    $('.modal.offlinepayment #accountName').val(data.bank ? data.bank.accountName : data.altID).trigger('change');

    $('.modal.offlinepayment #branchCode').val(data.bank ? data.bank.branchCode : '').trigger('change');

    $('.modal.offlinepayment #bankName').val(data.bank ? data.bank.bankName : '').trigger('change');
  }

  function fetchMaster(callback) {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Initializing. Please wait...');

    fetch('/api/master/600', {
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

        bankMasterList = res;

        $('#bankName').select2({
          placeholder: 'Select',
          data: res.map(e => {
            return { id: e.value, text: e.value }
          })
        });

        callback();

      }).catch(app.http.catch);
  }

  function reset() {
    $('.modal.offlinepayment .submit').prop('disabled', true);

    model.accountNo = '';
    model.accountName = '';
    model.branchCode = '';
    model.bankName = '';

    $('.modal.offlinepayment #accountNo').val('');
    $('.modal.offlinepayment #accountName').val('');
    $('.modal.offlinepayment #branchCode').val('');
    $('.modal.offlinepayment #totalAmount').val('');
    $('.modal.offlinepayment #bankName').val('').trigger('change');

    clearError();
  }

  $('.modal.offlinepayment #accountNo').on('change input paste', function () {
    model.accountNo = $(this).val().trim();
  });

  $('.modal.offlinepayment #accountName').on('change input paste', function () {
    model.accountName = $(this).val().trim();
  });

  $('.modal.offlinepayment #branchCode').on('change input paste', function () {
    model.branchCode = $(this).val().trim();
  });

  $('.modal.offlinepayment #bankName').on('change input paste', function () {
    model.bankName = $(this).select2('data')[0]?.text;
  });

  $('.modal.offlinepayment #totalAmount').on('change input paste', function () {

    if ($(this).val() != data.totalAmount) {
      $('.modal.offlinepayment .submit').prop('disabled', true);
      return;
    }
    $('.modal.offlinepayment .submit').prop('disabled', false);
  });

  function submit() {
    if (!validate()) {
      return;
    }

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Processing. Please wait...');

    fetch('/api/payments/' + app.page.id + '/action?status=300', {
      method: 'POST',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(model)
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(res => {
        app.dismissProgress();
        location.reload(true);
      }).catch(app.http.catch);
  }

  function validate() {
    clearError();

    if (app.utils.isNullOrEmpty(model.accountNo)) {
      $('.modal.offlinepayment #accountNo').closest('.form-group').addClass('has-danger');
    }

    if (app.utils.isNullOrEmpty(model.accountName)) {
      $('.modal.offlinepayment #accountName').closest('.form-group').addClass('has-danger');
    }

    if (app.utils.isNullOrEmpty(model.bankName)) {
      $('.modal.offlinepayment #bankName').closest('.form-group').addClass('has-danger');
    }

    return $('.modal.offlinepayment .has-danger').length == 0;
  }

  function clearError() {
    $('.modal.offlinepayment .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  }

  $(function () {
    $('.modal.offlinepayment .submit').click(function () {
      submit();
    });
  });

})(app.page.offline = app.page.offline || {});