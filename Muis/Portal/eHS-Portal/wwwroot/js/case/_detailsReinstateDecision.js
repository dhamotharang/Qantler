(function (self) {
  'use strict'

  var model;
  var sanction;
  var notes;
  var attachments = [];
  var controller;
  var isSave = false;
  var month;

  $('.select-single').select2({
    placeholder: 'Select'
  });

  $('.reinstate-decision #months').inputmask({
    regex: '^[1-9]|1[0-2]$',
    placeholder: ""
  });

  self.init = function (_model) {
    model = _model;
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
    body.Sanction = {};
    body.Sanction.Sanction = sanction;
    body.Sanction.CaseID = model.id;
    body.Sanction.Type = 0;
    body.Sanction.Sanction = sanction;
    body.Note = notes;
    body.Attachment = attachments;
    if (sanction == "2") {
      body.Sanction.Value = month;
    }

    $('.reinstate-decision .close').click();
    app.showProgress('Processing. Please wait...');

    fetch('/api/case/' + model.id + '/reinstate/decision?', {
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
    if (sanction == null) {
      $('.reinstate-decision #decision-select').closest('.form-group').addClass('has-danger');
    }

    if (sanction == "2"
      && app.utils.isNullOrEmpty(month)) {
      $('.reinstate-decision #months').closest('.form-group').addClass('has-danger');
    }

    if (app.utils.isNullOrEmpty(notes)) {
      $('.reinstate-decision #note').closest('.form-group').addClass('has-danger');
    }

    if (attachments.length == 0) {
      $('.reinstate-decision .upload-btn').closest('.form-group').addClass('has-danger');
    }
    return $('.reinstate-decision .has-danger').length == 0;
  };

  function clearError() {
    $('.reinstate-decision .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  };

  function reset() {
    isSave = false;
    attachments = [];
    notes = "";
    month = "";
    $('#decision-select').select2('val', 0);
    $('.reinstate-decision #note').val('');
    $('.reinstate-decision .attachments').empty();
    $('.reinstate-decision #months').val('');
    $('.reinstate-decision #suspension-months').hide();

    clearError();
  };

  function onFileUploaded(a) {
    attachments.push(a);

    var attachmentTemplate = $('.reinstate-decision .templates .attachment');

    $('.reinstate-decision .attachments').append(attachmentTemplate
      .prop('outerHTML')
      .replaceAll('{fileName}', a.fileName)
      .replaceAll('{fileSize}', app.utils.formatFileSize(a.size)));

    if (isSave) {
      validate();
    }
  };

  $('.reinstate-decision .fileuploader').uploadFile({
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

  $(".reinstate-decision textarea").on('change input paste', function () {
    notes = $(this).val().trim();
    if (isSave) {
      validate();
    }
  });

  $('.reinstate-decision .upload-btn').click(function () {
    $('.reinstate-decision .ajax-file-upload input[type="file"]').click();
  });

  $('#decision-select').on('change', function () {
    sanction = $(this).select2('val');
    if (sanction == "2") {
      $('.reinstate-decision #suspension-months').show();
    } else {
      $('.reinstate-decision #suspension-months').hide();
    }

    if (isSave) {
      validate();
    }
  });

  $('.reinstate-decision #months').on('change', function () {
    month = $(this).val();
    if (isSave) {
      validate();
    }
  });

  $('.reinstate-decision #submitBtn').click(function () {
    submit();
  });

  $('#show-reinstateDecision').click(function () {
    reset();
  });

})(app.page.reinstateDecision = app.page.reinstateDecision || {});