(function () {
  'use strict';

  var dataitem;
  var isSupplierLoaded = false;
  var isCertifyingBodyLoaded = false;
  var isDetailsSave = false;
  var detailsName;
  var detailsBrand;
  var detailsRiskCategoryID;
  var detailsStatus;
  var detailsSupplierID;
  var detailsCertifyingBodyID;

  this.supplierlist = [];
  this.certifyingBody = [];

  $('#detailRiskCategory').select2();

  $('.icheck-flat input').iCheck({
    checkboxClass: 'icheckbox_flat-blue',
    radioClass: 'iradio_flat'
  });

  this.init = function (data) {
    dataitem = data;
    isDetailsSave = false;

    if (data != null) {
      $('#HalallibraryDetailModal .modal-title').html('Modify Ingredient');
      $('#deleteButton').show();

      $('#detailName').val(data.name).change();
      $('#detailBrand').val(data.brand).change();
      $('#detailRiskCategory').val(data.riskCategory).trigger('change');

      if (data.status == 1) {
        $('#detailIsVerified').iCheck('check').change();
      }
      else {
        $('#detailIsVerified').iCheck('uncheck').change();
      }

    }
    else {
      $('#HalallibraryDetailModal .modal-title').html('Add Ingredient');
      $('#deleteButton').hide();

      $('#detailName').val('').change();
      $('#detailBrand').val('').change();
      $('#detailRiskCategory').val(999).trigger('change');
      $('#detailIsVerified').iCheck('uncheck').change();
    }

    this.loadSupplierControl(dataitem && dataitem.supplierID ? dataitem.supplierID : 0);
    this.loadCertifyingBodyControl(dataitem && dataitem.certifyingBodyID ? dataitem.certifyingBodyID : 0);
  };

  this.loadSupplierControl = function (value) {
    if (this.supplierlist.length == 0) {
      $('#detailSupplier').select2({ allowClear: true, placeholder: "" });

      app.showProgress('Please wait...');
      fetch('/api/supplier', {
        method: "GET",
        cache: 'no-cache',
        credentials: 'include',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        }
      })
        .then(app.http.errorInterceptor)
        .then(response => response.json())
        .then(r => {

          if (r) {
            for (var i = 0; i < r.length; i++) {
              this.supplierlist.push({ id: r[i].id, text: r[i].name });
            }

            $('#detailSupplier').select2('destroy').empty().select2({ data: this.supplierlist, allowClear: true, placeholder: "" });
            $('#detailSupplier').val(value).trigger('change');
            isSupplierLoaded = true;
            this.dismissProgress();
          }
        }).catch(app.http.catch);
    }
    else {
      $('#detailSupplier').select2({ allowClear: true, placeholder: "" });
      $('#detailSupplier').val(value).trigger('change');
      isSupplierLoaded = true;
      this.dismissProgress();
    }
  }

  this.loadCertifyingBodyControl = function (value) {
    if (this.certifyingBody.length == 0) {
      $('#detailCertifyingBody').select2({ allowClear: true, placeholder: "" });

      app.showProgress('Please wait...');
      fetch('/api/certifyingbody', {
        method: "GET",
        cache: 'no-cache',
        credentials: 'include',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        }
      })
        .then(app.http.errorInterceptor)
        .then(response => response.json())
        .then(r => {

          if (r) {
            for (var i = 0; i < r.length; i++) {
              this.certifyingBody.push({ id: r[i].id, text: r[i].name })
            }

            $('#detailCertifyingBody').select2('destroy').empty().select2({ data: this.certifyingBody, allowClear: true, placeholder: "" });
            $('#detailCertifyingBody').val(value).trigger('change');
            isCertifyingBodyLoaded = true;
            this.dismissProgress();
          }
        }).catch(app.http.catch);
    }
    else {
      $('#detailCertifyingBody').select2({ allowClear: true, placeholder: "" });
      $('#detailCertifyingBody').val(value).trigger('change');
      isCertifyingBodyLoaded = true;
      this.dismissProgress();
    }
  }

  this.save = function () {
    isDetailsSave = true;

    if (this.isValid()) {
      var saveModel =
      {
        "ID": dataitem ? dataitem.id : 0,
        "Name": detailsName,
        "Brand": detailsBrand,
        "RiskCategory": detailsRiskCategoryID,
        "Status": detailsStatus,
        "SupplierID": detailsSupplierID,
        "CertifyingBodyID": detailsCertifyingBodyID,
        "VerifiedByID": detailsStatus == 1 ? app.identity.id : null
      }

      var controller;
      if (controller) {
        controller.abort();
      }

      controller = new AbortController();

      app.showProgress('Please wait...');
      fetch('/api/halallibrary', {
        method: dataitem ? "PUT" : "POST",
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

          this.close();

          if (this.reloadCallback) {
            this.reloadCallback();
          };
        }).catch(app.http.catch);
    }
  }

  this.delete = function () {
    var controller;
    if (controller) {
      controller.abort();
    }

    controller = new AbortController();

    swal.fire({
      title: 'Are you sure?',
      text: "You want to delete the ingredient",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#2196f3',
      confirmButtonText: 'Ok ',
      buttons: {
        cancel: {
          text: "Cancel",
          value: null,
          visible: true,
          className: "btn btn-danger",
          closeModal: true,
        },
        confirm: {
          text: "OK",
          value: true,
          visible: true,
          className: "btn btn-primary",
          closeModal: true
        }
      }
    }).then((result) => {
      if (result.isConfirmed) {
        app.showProgress('Please wait...');
        fetch('/api/halallibrary/' + dataitem.id, {
          method: 'DELETE',
          cache: 'no-cache',
          credentials: 'include',
          signal: controller.signal,
          headers: {
            'Accept': 'application/json'
          }
        }).then(app.http.errorInterceptor)
          .then(res => res.json())
          .then(r => {

            this.close();

            if (this.reloadCallback) {
              this.reloadCallback();
            };
          }).catch(app.http.catch);
      }
    });
  };

  this.close = function () {
    $("#HalallibraryDetailModal").removeClass("in");
    $(".modal-backdrop").remove();
    $("#HalallibraryDetailModal").hide();
  };

  this.dismissProgress = function () {
    if (isSupplierLoaded && isCertifyingBodyLoaded) {
      app.dismissProgress();
    }
  };

  this.isValid = function () {
    if (app.utils.isNullOrEmpty(detailsName)) {
      $('#detailName').next().html('Name is required');
      $('#detailName').next().css('display', 'block');
      $('#detailName').addClass('border-danger');
      return false;
    }
    else {
      $('#detailName').next().css('display', 'none');
      $('#detailName').removeClass('border-danger');
      return true;
    }
  }

  $('#detailName').keyup(function (e) {
    if (isDetailsSave) {
      app.page.item.isValid();
    }
  });

  $('#detailName').on('change input paste', function () {
    detailsName = $(this).val().trim();
  });

  $('#detailBrand').on('change input paste', function () {
    detailsBrand = $(this).val().trim();
  });

  $('#detailSupplier').on('change input paste', function () {
    detailsSupplierID = $(this).val();
  });

  $('#detailCertifyingBody').on('change input paste', function () {
    detailsCertifyingBodyID = $(this).val();
  });

  $('#detailRiskCategory').on('change input paste', function () {
    detailsRiskCategoryID = $(this).val();
  });

  this.addSupplier = function addSupplier() {
    $(".modal-backdrop").show();
    $("#SupplierDetailModal").show();
    $("#SupplierDetailModal").addClass("show");
    app.page.supplier.init();
  };

  this.addCertifyingBody = function addCertifyingBody() {
    $(".modal-backdrop").show();
    $("#CertifyingBodyModal").show();
    $("#CertifyingBodyModal").addClass("show");
    app.page.certifyingbody.init();
  };

  this.reloadCallback = function () { };

  $(function () {
    app.page.supplier.addSupplierCallback = function (id, text) {
      app.page.item.supplierlist.push({ id: id, text: text });
      $('#detailSupplier').select2('destroy').empty().select2({ data: app.page.item.supplierlist, allowClear: true, placeholder: "" });
      $('#detailSupplier').val(id).trigger('change');
    };

    $('#detailIsVerified').on('ifChanged', function () {
      detailsStatus = $(this).prop('checked') == true ? 1 : 0;
    });

    app.page.certifyingbody.addCertifyingBodyCallback = function (id, text) {
      app.page.item.certifyingBody.push({ id: id, text: text });
      $('#detailCertifyingBody').select2('destroy').empty().select2({ data: app.page.item.certifyingBody, allowClear: true, placeholder: "" });
      $('#detailCertifyingBody').val(id).trigger('change');
    };
  });

}).apply(app.page.item = app.page.item || {});

$(document).ready(function () {
});
