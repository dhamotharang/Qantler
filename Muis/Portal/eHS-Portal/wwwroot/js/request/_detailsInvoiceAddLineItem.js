(function (self) {

  var selectedCode;

  var gst;

  var data;

  self.callback = function (e) { }

  self.init = function (pGst) {
    gst = pGst;
    selectedCode = null;
    data = null;

    reset();
    invalidate();
  }

  function invalidate() {
    var unitPriceText = '';
    var unitPrice = 0;
    var qty = 0;

    var price = getLatestPrice(selectedCode);
    if (price) {
      unitPrice = price.amount;
      unitPriceText = app.number.financial(unitPrice) + '/unit';
    }

    var qtyVal = $('.invoice-add-lineitem #quantity').val();
    if (qtyVal) {
      qty = Number.parseFloat(Number.parseFloat(qtyVal).toFixed(2));
    }

    var isReverse = $('.invoice-add-lineitem #reverse').is(':checked');
    var reverse = isReverse ? -1 : 1;

    var amount = qty * unitPrice * reverse;
    var gstAmount = amount * gst;
    var totalAmount = amount + gstAmount;

    data = {
      unitPrice: unitPrice,
      qty: qty,
      codeID: (selectedCode ? selectedCode.id : null),
      code: (selectedCode ? selectedCode.code : ''),
      descr: (selectedCode ? selectedCode.text : ''),
      amount: amount,
      gstAmount: gstAmount,
      totalAmount: totalAmount,
      gst: gst,
      isEditable: true
    }

    $('.invoice-add-lineitem .price').html(unitPriceText);
    $('.invoice-add-lineitem .total .amount').html(app.number.financial(totalAmount));
  }

  function reset() {
    selectedCode = null;

    $('.invoice-add-lineitem #code').val('');
    $('.invoice-add-lineitem #quantity').val('');
    $('.icheck-flat input').iCheck('uncheck');

    clearError();
  }

  function validate() {
    clearError();

    if (selectedCode == null) {
      $('.invoice-add-lineitem #code').closest('.form-group').addClass('has-danger');
    }

    var qty = $('.invoice-add-lineitem #quantity');

    if (app.utils.isNullOrEmpty(qty.val().trim())) {
      qty.closest('.form-group').addClass('has-danger');
    }

    return $('.invoice-add-lineitem .has-danger').length == 0;
  }

  function clearError() {
    $('.invoice-add-lineitem .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  }

  function getLatestPrice(code) {
    var result;

    if (code != null
      && code.priceHistory
      && code.priceHistory.length > 0) {
      var latestPrice = code.priceHistory
        .filter(e => moment.utc(e.effectFrom).diff(moment.utc()) <= 0)
        .sort((a, b) => moment.utc(b.effectFrom).diff(moment.utc(a.effectFrom)));

      if (latestPrice.length > 0) {
        return latestPrice[0];
      }
    }
  }

  $(function () {

    $(':input').inputmask();

    $('.icheck-flat input').iCheck({
      checkboxClass: 'icheckbox_flat-blue',
      radioClass: 'iradio_flat',
      increaseArea: '20%'
    });

    $('.invoice-add-lineitem .search').click(function () {
      app.page.transactioncode.init();
    });

    $('.invoice-add-lineitem #quantity').on('change input paste', function () {
      invalidate();
    });

    $('.invoice-add-lineitem #reverse').on('ifChanged', function () {
      invalidate();
    });

    $('.invoice-add-lineitem .submit').click(function () {
      if (!validate()) {
        return;
      }

      self.callback(data);

      $('.invoice-add-lineitem .dismiss').click();
    });

    app.page.transactioncode.onSelect = function (e) {
      selectedCode = e;
      $('.invoice-add-lineitem #code').val(app.utils.titleCase(e.text));
      invalidate();
    }
  });

})(app.page.finance.invoice = app.page.finance.invoice || {});