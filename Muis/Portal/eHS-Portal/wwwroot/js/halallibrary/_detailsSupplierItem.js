(function () {
  'use strict';

  var supplierIsSave = false;
  var supplierName = "";

  this.init = function () {
    supplierIsSave = false;
    $('#supplierName').val('').change();
  };

  this.save = function () {
    var saveModel =
    {
      "Name": supplierName
    };

    supplierIsSave = true;

    if (this.isValid()) {

      var controller;
      if (controller) {
        controller.abort();
      }

      controller = new AbortController();

      app.showProgress('Please wait...');
      fetch('/api/supplier', {
        method: "POST",
        cache: 'no-cache',
        credentials: 'include',
        signal: controller.signal,
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(saveModel)
      }).then(app.http.errorInterceptor)
        .then(res => res.json())
        .then(r => {
          app.dismissProgress();
          if (r > 0) {
            if (this.addSupplierCallback) {
              this.addSupplierCallback(r, supplierName);
            };

            this.close();
          }

        }).catch(app.http.catch);
    }
  }

  this.close = function () {
    $("#SupplierDetailModal").removeClass("in");
    $(".modal-backdrop").remove();
    $("#SupplierDetailModal").hide();
  };

  this.isValid = function () {
    if (app.utils.isNullOrEmpty(supplierName)) {
      $('#supplierName').next().html('Name is required');
      $('#supplierName').next().css('display', 'block');
      $('#supplierName').addClass('border-danger');
      return false
    }
    else {
      $('#supplierName').next().css('display', 'none');
      $('#supplierName').removeClass('border-danger');
      return true;
    }
  }

  $('#supplierName').keyup(function (e) {
    if (supplierIsSave) {
      supplierName = $(this).val().trim();
      app.page.supplier.isValid();
    }
  });

  $('#supplierName').on('change input paste', function () {
    supplierName = $(this).val().trim();
  });

  this.addSupplierCallback = function (id, text) { };

}).apply(app.page.supplier = app.page.supplier || {});

$(document).ready(function () {
});
