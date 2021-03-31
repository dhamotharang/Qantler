(function (self) {
  'use strict'

  var model;
  var notes;
  var attachments = [];
  var controller;
  var isSave = false;
  var amount;
  var bankName;
  var finalAmount;

  Inputmask.extendAliases({
    'decimal': {
      integerDigits: 5,
      digits: 2,
      allowPlus: false,
      allowMinus: false,
      digitsOptional: false,
      numericInput: true,
      autoGroup: true,
      groupSeparator: ",",
      radixPoint: ".",
      placeholder: "0",
    }
  });

  $(".payment #amount").inputmask('decimal');

  self.init = function (_model) {
    model = _model
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

    var body = {};
    body.RefNo = model.id;
    body.RefID = model.id;
    body.PayerID = model.offender.id;
    body.PayerName = model.offender.name;
    body.BankAccountName = bankName;
    body.Amount = amount.replaceAll(",", "");
    body.Notes = notes;
    body.Attachments = attachments;

    $('.payment .close').click();
    app.showProgress('Processing. Please wait...');

    fetch('/api/case/' + model.id + '/payment/composition?', {
      method: 'POST',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(body)
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
    if (app.utils.isNullOrEmpty(bankName)) {
      $('.payment #bankName').closest('.form-group').addClass('has-danger');
    }

    if (parseFloat(amount) == 0 || amount == "") {
      $('.payment #amount').closest('.form-group').addClass('has-danger');
    }

    if (app.utils.isNullOrEmpty(notes)) {
      $('.payment #note').closest('.form-group').addClass('has-danger');
    }

    if (attachments.length == 0) {
      $('.payment .upload-btn').closest('.form-group').addClass('has-danger');
    }
    return $('.payment .has-danger').length == 0;
  };

  function clearError() {
    $('.payment .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  };

  function reset() {
    isSave = false;
    attachments = [];
    notes = "";
    amount = "";
    bankName = "";
    $('.payment #note').val('');
    $('.payment .attachments').empty();
    $('.payment #amount').val('');
    $('.payment #bankName').val('');
    $('.payment #submitBtn').attr('disabled', true);

    var finalActions = model.sanctionInfos.filter(x => x.type == 1);
    var maxFinalActionID = Math.max.apply(null, finalActions.map(x => x.id));
    var sanctionInfo = finalActions.find(x => x.id == maxFinalActionID);
    finalAmount = sanctionInfo.value;

    clearError();
  };

  function onFileUploaded(a) {
    attachments.push(a);

    var attachmentTemplate = $('.payment .templates .attachment');

    $('.payment .attachments').append(attachmentTemplate
      .prop('outerHTML')
      .replaceAll('{fileName}', a.fileName)
      .replaceAll('{fileSize}', app.utils.formatFileSize(a.size)));

    if (isSave) {
      validate();
    }
  };

  $('.payment .fileuploader').uploadFile({
    url: "/api/file",
    allowedTypes: "pdf,doc,docx,xls,xlsx,csv,png,bmp,jpg,gif,jpeg,txt",
    maxFileSize: 5000000,
    onSelect: function (files, data) {

      var file = files[0];
      if (file.size > 5000000) {
        Swal.fire({
          icon: 'error',
          title: 'Oops...',
          html: file.name + ' is not allowed!<br>Allowed max size: <strong>5MB</strong>.'
        });

        return false;
      }

      var allowedExtensions = /(\.jpg|\.jpeg|\.png|\.gif|\.pdf|\.doc|\.docx|\.xls|\.xlsx|\.csv|\.txt)$/i;
      if (!allowedExtensions.exec(file.name)) {
        Swal.fire({
          icon: 'error',
          title: 'Oops...',
          html: file.name + ' is not allowed!<br>Allowed extensions: <strong>pdf,doc,xls,csv,png,jpg,jpeg,txt</strong>'
        });
      }

      return true;
    },
    onSubmit: function (e) {
      app.showProgress('Uploading! Please wait...');
    },
    onSuccess: function (files, data) {
      onFileUploaded(data[0]);
      app.dismissProgress();
    },
    onError: function (e) {
      showError('Something went wrong!');
    },
    onCancel: function (e) {
    }
  });

  $(".payment #note").on('change input paste', function () {
    notes = $(this).val().trim();
    if (isSave) {
      validate();
    }
  });

  $('.payment .upload-btn').click(function () {
    $('.payment .ajax-file-upload input[type="file"]').click();
  });

  $('.payment #bankName').on('change input paste', function () {
    bankName = $(this).val().trim();
    if (isSave) {
      validate();
    }
  });

  $('.payment #amount').on('change input paste', function () {
    amount = $(this).val();
    if (amount.replaceAll(",", "") == finalAmount) {
      $('.payment #submitBtn').attr('disabled', false);
    }
    else {
      $('.payment #submitBtn').attr('disabled', true);
    }

    if (isSave) {
      validate();
    }
  });

  $('.payment #submitBtn').click(function () {
    submit();
  });

  $('#add-payment').click(function () {
    reset();
  });

})(app.page.payment = app.page.payment || {});