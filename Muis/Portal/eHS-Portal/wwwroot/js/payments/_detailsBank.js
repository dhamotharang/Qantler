(function () {
  'use strict'

  var controller;

  this.render = function (data) {
    if (!data) {
      $('.bank-card').addClass('d-none');
      return;
    }

    renderInfo(data);
  }

  function renderInfo(data) {    
    $('.bankinfo .info-accountNo .detail').html(data.accountNo);
    $('.bankinfo .info-accountName .detail').html(data.accountName);
    $('.bankinfo .info-bankName .detail').html(data.branchCode + ' ' + data.bankName);

    if (data.accountNo == null) {
      $('.bankinfo .info-accountNo').addClass('d-none');
    }

    if (data.accountName == null) {
      $('.bankinfo .info-accountName').addClass('d-none');
    }

    if (data.branchCode == null && data.bankName == null) {
      $('.bankinfo .info-bankName').addClass('d-none');
    }

    if (data.ddaStatus != 200 ) {
      $('.bankinfo .info-status').addClass('d-none');
    }
  }

  function dispose() {
    if (controller) {
      controller.abort();
    }
  }

}).apply(app.page.bank = app.page.bank || {});