(function (self) {
  'use strict'

  var sanction;
  var notes;
  var amount;
  var month;
  var attachments = [];
  var caseID;
  var controller;
  var isSave = false;

  $('.select-single').select2({
    placeholder: 'Select'
  });

  $('.acknowledge-cause #months').inputmask({
    regex: '^[1-9]|1[0-2]$',
    placeholder: ""
  });

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

  $("#amount").inputmask('decimal');

  self.init = function (id) {
    caseID = id;

    reset();
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

    var model = {};
    model.Sanction = {};
    model.Sanction.Sanction = sanction;
    model.Sanction.CaseID = caseID;
    model.Sanction.Type = 0;
    model.Sanction.Sanction = sanction;
    model.Note = notes;
    model.Attachment = attachments;
    if (sanction == 1) {
      model.Sanction.Value = amount.replaceAll(",", "");
    }
    else if (sanction == 2) {
      model.Sanction.Value = month;
    }

    app.showProgress('Processing. Please wait...');

    fetch('/api/case/' + caseID + '/acknowledge?', {
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
      .then(r => {
        if (r) {
          location.reload();
        }

        $('.acknowledge-cause .close').click();
      })
      .catch(err => {
        app.http.catch
      });
  };

  function validate() {
    clearError();
    if (sanction == null) {
      $('.acknowledge-cause #recommendations-select').closest('.form-group').addClass('has-danger');
    }

    if (sanction == 1
      && (app.utils.isNullOrEmpty(amount)
        || parseFloat(amount) == 0)) {
      $('.acknowledge-cause #amount').closest('.form-group').addClass('has-danger');
    }

    if (sanction == 2
      && app.utils.isNullOrEmpty(month)) {
      $('.acknowledge-cause #months').closest('.form-group').addClass('has-danger');
    }

    if (attachments.length == 0) {
      $('.acknowledge-cause .upload-btn').closest('.form-group').addClass('has-danger');
    }
    return $('.acknowledge-cause .has-danger').length == 0;
  };

  function clearError() {
    $('.acknowledge-cause .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  };

  function reset() {
    isSave = false;
    attachments= [];
    $('#recommendations-select').select2('val', 0);
    $('.acknowledge-cause #note').val('');
    $('.acknowledge-cause #amount').val('');
    $('.acknowledge-cause #months').val('');
    $('.acknowledge-cause .attachments').empty();
    clearError();
  };

  function onFileUploaded(a) {
    attachments.push(a);

    var attachmentTemplate = $('.acknowledge-cause .templates .attachment');

    $('.acknowledge-cause .attachments').append(attachmentTemplate
      .prop('outerHTML')
      .replaceAll('{fileName}', a.fileName)
      .replaceAll('{fileSize}', app.utils.formatFileSize(a.size)));

    if (isSave) {
      validate();
    }
  };

  $('.acknowledge-cause .fileuploader').uploadFile({
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

  $(".acknowledge-cause textarea").on('change input paste', function () {
    notes = $(this).val().trim();
  });

  $('.acknowledge-cause .upload-btn').click(function () {
    var self = $(this);

    $('.acknowledge-cause .ajax-file-upload input[type="file"]').click();
  });

  $('#amount').on('change', function () {
    amount = $(this).val();
    if (isSave) {
      validate();
    }
  });

  $('#months').on('change', function () {
    month = $(this).val();
    if (isSave) {
      validate();
    }
  });

  $('#recommendations-select').on('change', function () {
    sanction = $(this).select2('val');
    if (sanction == 1) {
      $('#composition-amount').show();
      $('#suspension-months').hide();
    } else if (sanction == 2) {
      $('#suspension-months').show();
      $('#composition-amount').hide();
    } else {
      $('#suspension-months').hide();
      $('#composition-amount').hide();
    }

    if (isSave) {
      validate();
    }
  });

  $('.acknowledge-cause #submitBtn').click(function () {
    submit();
  });

})(app.page.AcknowledgeCause = app.page.AcknowledgeCause || {});