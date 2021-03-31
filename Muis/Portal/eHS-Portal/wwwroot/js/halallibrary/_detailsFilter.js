(function () {
  'use strict';

  var filterName;
  var filterBrand;
  var filterRiskCategoryID;
  var filterStatus;
  var filterSupplier;
  var filterCertifyingBody;
  var filterVerifiedBy;

  $('#filterRiskCategory').select2({ allowClear: true, placeholder: "" });
  $('#filterRiskCategory').val(null).change();

  $('#filterIsVerified').select2({ allowClear: true, placeholder: "" });
  $('#filterIsVerified').val(null).change();

  $('#filterName').val('').trigger('change');
  $('#filterBrand').val('').trigger('change');
  $('#filterSupplier').val('').trigger('change');
  $('#filterCertifyingBody').val('').trigger('change');
  $('#filterVerifiedBy').val('').trigger('change');

  this.toParams = function () {

    function appendParam(p, key, val) {
      if (p.length > 0) {
        p += '&';
      }
      p += key + '=' + val;
      return p;
    }

    var params = '';

    if (!app.utils.isNullOrEmpty(filterName)) {
      params = appendParam(params, 'Name', filterName);
    }

    if (!app.utils.isNullOrEmpty(filterBrand)) {
      params = appendParam(params, 'Brand', filterBrand);
    }

    if (!app.utils.isNullOrEmpty(filterSupplier)) {
      params = appendParam(params, 'Supplier', filterSupplier);
    }

    if (!app.utils.isNullOrEmpty(filterCertifyingBody)) {
      params = appendParam(params, 'CertifyingBody', filterCertifyingBody);
    }

    if (!app.utils.isNullOrEmpty(filterRiskCategoryID)) {
      params = appendParam(params, 'RiskCategory', filterRiskCategoryID);
    }

    if (!app.utils.isNullOrEmpty(filterVerifiedBy)) {
      params = appendParam(params, 'VerifiedBy', filterVerifiedBy);
    }

    if (!app.utils.isNullOrEmpty(filterStatus)) {
      params = appendParam(params, 'Status', filterStatus);
    }

    return params;
  };

  this.apply = function () {
    this.close();
    if (this.reloadCallback) {
      this.reloadCallback();
    };
  };

  this.clear = function () {
    $('#filterName').val('').change();
    $('#filterBrand').val('').change();
    $('#filterSupplier').val('').change();
    $('#filterCertifyingBody').val('').change();
    $('#filterVerifiedBy').val('').change();
    $('#filterRiskCategory').val(null).trigger('change');
    $('#filterIsVerified').val(null).trigger('change');
  }

  this.close = function () {
    $("#HalallibraryFilterModal").removeClass("in");
    $(".modal-backdrop").remove();
    $("#HalallibraryFilterModal").hide();
  };

  $('#filterName').on('change input paste', function () {
    filterName = $(this).val().trim();
  });

  $('#filterBrand').on('change input paste', function () {
    filterBrand = $(this).val().trim();
  });

  $('#filterSupplier').on('change input paste', function () {
    filterSupplier = $(this).val();
  });

  $('#filterCertifyingBody').on('change input paste', function () {
    filterCertifyingBody = $(this).val();
  });

  $('#filterRiskCategory').on('change input paste', function () {
    filterRiskCategoryID = $(this).val();
  });

  $('#filterIsVerified').on('change input paste', function () {
    filterStatus = $(this).val();
  });

  $('#filterVerifiedBy').on('change input paste', function () {
    filterVerifiedBy = $(this).val();
  });

  this.reloadCallback = function () { };

}).apply(app.page.filter = app.page.filter || {});

$(document).ready(function () {
});
