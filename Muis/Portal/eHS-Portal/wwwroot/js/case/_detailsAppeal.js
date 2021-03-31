(function (self) {
  'use strict'

  var model;
  var notes;
  var attachments = [];
  var controller;
  var isSave = false;

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
    body.Notes = notes;
    body.Attachments = attachments;

    $('.case-appeal .close').click();
    app.showProgress('Processing. Please wait...');

    fetch('/api/case/' + model.id + '/appeal?', {
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
    if (app.utils.isNullOrEmpty(notes)) {
      $('.case-appeal #note').closest('.form-group').addClass('has-danger');
    }

    if (attachments.length == 0) {
      $('.case-appeal .upload-btn').closest('.form-group').addClass('has-danger');
    }
    return $('.case-appeal .has-danger').length == 0;
  };

  function clearError() {
    $('.case-appeal .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  };

  function reset() {
    isSave = false;
    attachments = [];
    notes = "";
    $('.case-appeal #note').val('');
    $('.case-appeal .attachments').empty();

    clearError();
  };

  function onFileUploaded(a) {
    attachments.push(a);

    var attachmentTemplate = $('.case-appeal .templates .attachment');

    $('.case-appeal .attachments').append(attachmentTemplate
      .prop('outerHTML')
      .replaceAll('{fileName}', a.fileName)
      .replaceAll('{fileSize}', app.utils.formatFileSize(a.size)));

    if (isSave) {
      validate();
    }
  };

  $('.case-appeal .fileuploader').uploadFile({
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

  $(".case-appeal textarea").on('change input paste', function () {
    notes = $(this).val().trim();
    if (isSave) {
      validate();
    }
  });

  $('.case-appeal .upload-btn').click(function () {
    $('.case-appeal .ajax-file-upload input[type="file"]').click();
  });

  $('.case-appeal #submitBtn').click(function () {
    submit();
  });

  $('#show-caseAppeal').click(function () {
    reset();
  });

})(app.page.appeal = app.page.appeal || {});