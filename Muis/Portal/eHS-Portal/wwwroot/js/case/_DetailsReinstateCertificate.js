(function (self) {
  'use strict'

  var model;
  var attachments = {};
  var controller;
  var isSave = false;
  var name;

  var canvas;
  var signaturePad;

  self.init = function (_model) {
    model = _model

    if (model.offender != null) {
      $('.reinstateCertificate #offender-Name').html(model.offender.name);
    }
    if (model.certificates.length > 0) {
      $('.reinstateCertificate #certificate-suspension-date')
        .html(app.utils.formatShortDateWithComma(model.certificates[0].suspendedUntil));
    }
  };

  function submit() {
    isSave = true;

    if (!validate()) {
      return;
    }

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    $('.reinstateCertificate .close').click();
    app.showProgress('Processing. Please wait...');

    var file = signaturePad.toDataURL();
    var fileSplit = file.split(";");

    var realData = fileSplit[1].split(",")[1];
    var fileBlob = base64toBlob(realData);

    var formData = new FormData();
    formData.append("", fileBlob, "signature.png");

    fetch("/api/file", {
      method: 'POST',
      body: formData
    }).then(res => res.json())
      .then(result => {
        attachments = result[0];
        collectCertificate();
      })
      .catch(err => {
        app.http.catch
      });
  };

  function collectCertificate() {
    fetch('/api/case/' + model.id + '/certificate/reinstate?senderName=' + name, {
      method: 'POST',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(attachments)
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(r => {
        if (r) {
          location.reload();
        }
      })
      .catch(err => {
        app.http.catch
      });
  };

  function validate() {
    clearError();
    if (app.utils.isNullOrEmpty(name)) {
      $('.reinstateCertificate #name').closest('.form-group').addClass('has-danger');
    }

    if (signaturePad.isEmpty()) {
      $('.reinstateCertificate .signature-pad--body').closest('.form-group').addClass('has-danger');
    }

    return $('.reinstateCertificate .has-danger').length == 0;
  };

  function clearError() {
    $('.reinstateCertificate .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  };

  function reset() {
    isSave = false;
    attachments = {};
    name = "";
    $('.reinstateCertificate #name').val('');

    clearError();
  };

  function base64toBlob(b64Data, sliceSize) {
    sliceSize = sliceSize || 512;
    var byteCharacters = atob(b64Data);
    var byteArrays = [];
    for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
      var slice = byteCharacters.slice(offset, offset + sliceSize);
      var byteNumbers = new Array(slice.length);
      for (var i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }

      var byteArray = new Uint8Array(byteNumbers);
      byteArrays.push(byteArray);
    }

    var blob = new Blob(byteArrays, { type: "image/png" });
    return blob;
  }

  $(".reinstateCertificate #name").on('change input paste', function () {
    name = $(this).val().trim();
    $('.reinstateCertificate #_name').html(name);
    if (isSave) {
      validate();
    }
  });

  $('.reinstateCertificate #submitBtn').click(function () {
    submit();
  });

  $('#show-caseReinstateCertificate').click(function () {
    $(window).resize(function () {
      canvas.width = $('.reinstateCertificate .signature-pad--body').width();
      canvas.height = 240;
    });

    reset();
  });

  $('.reinstateCertificate #clearBtn').click(function () {
    signaturePad.clear();
    if (isSave) {
      validate();
    }
  });

  $("#ReinstateCertificateModal").on('shown.bs.modal', function () {
    canvas = document.querySelector(".reinstateCertificate canvas");
    canvas.width = $('.reinstateCertificate .signature-pad--body').width();
    canvas.height = 240;
    signaturePad = new SignaturePad(canvas, {
      onEnd: function () {
        if (isSave) {
          validate();
        }
      }
    });
    signaturePad.clear();
  });

})(app.page.reinstatecertificate = app.page.reinstatecertificate || {});