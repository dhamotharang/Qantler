(function () {
  'use strict';

  var certifyingName = "";
  var certifyingStatus = "";
  var certifyingIsSave = false;

  $('#certifyingBodyStatus').select2();

  this.init = function () {
    $('#certifyingBodyName').val('').change();
    $('#certifyingBodyStatus').val(0).trigger('change');
    certifyingIsSave = false;
  };

  this.save = function () {
    var saveModel =
    {
      "Name": certifyingName,
      "Status": certifyingStatus,
    };

    certifyingIsSave = true;

    if (this.isValid()) {

      var controller;
      if (controller) {
        controller.abort();
      }

      controller = new AbortController();

      app.showProgress('Please wait...');
      fetch('/api/certifyingbody', {
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
            if (this.addCertifyingBodyCallback) {
              this.addCertifyingBodyCallback(r, certifyingName);
            }

            this.close();
          }

        }).catch(app.http.catch);
    }
  }

  this.close = function () {
    $("#CertifyingBodyModal").removeClass("in");
    $(".modal-backdrop").remove();
    $("#CertifyingBodyModal").hide();
  };

  this.isValid = function () {
    if (app.utils.isNullOrEmpty(certifyingName)) {
      $('#certifyingBodyName').next().html('Name is required');
      $('#certifyingBodyName').next().css('display', 'block');
      $('#certifyingBodyName').addClass('border-danger');
      return false;
    }
    else {
      $('#certifyingBodyName').next().css('display', 'none');
      $('#certifyingBodyName').removeClass('border-danger');
      return true;
    }
  }

  $('#certifyingBodyName').keyup(function (e) {
    if (certifyingIsSave) {
      certifyingName = $(this).val().trim();
      app.page.certifyingbody.isValid();
    }
  });

  $('#certifyingBodyName').on('change input paste', function () {
    certifyingName = $(this).val().trim();
  });

  $('#certifyingBodyStatus').on('change input paste', function () {
    certifyingStatus = $(this).val().trim();
  });

  this.addCertifyingBodyCallback = function (id, text) { };

}).apply(app.page.certifyingbody = app.page.certifyingbody || {});

$(document).ready(function () {
});
