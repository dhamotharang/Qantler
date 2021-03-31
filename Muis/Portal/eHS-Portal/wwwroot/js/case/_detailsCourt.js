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

    $('.file-case-to-court .close').click();
    app.showProgress('Processing. Please wait...');

    fetch('/api/case/' + model.id + '/court?', {
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
      $('.file-case-to-court #note').closest('.form-group').addClass('has-danger');
    }

    return $('.file-case-to-court .has-danger').length == 0;
  };

  function clearError() {
    $('.file-case-to-court .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  };

  function reset() {
    isSave = false;
    attachments = [];
    notes = "";
    $('.file-case-to-court #note').val('');
    $('.file-case-to-court .attachments').empty();

    clearError();
  };

  function onFileUploaded(a) {
    attachments.push(a);

    var attachmentTemplate = $('.file-case-to-court .templates .attachment');

    $('.file-case-to-court .attachments').append(attachmentTemplate
      .prop('outerHTML')
      .replaceAll('{fileName}', a.fileName)
      .replaceAll('{fileSize}', app.utils.formatFileSize(a.size)));

  };

  $('.file-case-to-court .fileuploader').uploadFile({
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

  $(".file-case-to-court textarea").on('change input paste', function () {
    notes = $(this).val().trim();
    if (isSave) {
      validate();
    }
  });

  $('.file-case-to-court .upload-btn').click(function () {
    $('.file-case-to-court .ajax-file-upload input[type="file"]').click();
  });

  $('.file-case-to-court #submitBtn').click(function () {
    submit();
  });

  $('#show-fileCaseToCourt').click(function () {
    reset();
  });

})(app.page.court = app.page.court || {});