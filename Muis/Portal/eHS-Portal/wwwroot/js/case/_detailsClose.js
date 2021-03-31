﻿(function (self) {
  'use strict'

  var model;

  var caseID;

  var controller;

  var isSave = false;

  self.init = function (id) {
    caseID = id;

    reset();
  }

  self.callback = function (d) {
  }

  function submit() {
    isSave = true;

    if (!validate()) {
      return;
    }

    model.type = 0;
    model.action = 'Added notes'

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Processing. Please wait...');

    fetch('/api/case/' + caseID + '/close', {
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

        $('.case-close .close').click();
      })
      .catch(app.http.catch);
  }

  function validate() {
    clearError();

    if (app.utils.isNullOrEmpty(model.notes)) {
      $('.case-close textarea').closest('.form-group').addClass('has-danger');
    }

    return $('.case-close .has-danger').length == 0;
  }

  function clearError() {
    $('.case-close .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  }

  function reset() {
    isSave = false;
    model = { attachments: [] };
    $('.case-close textarea').val('');
    $('.case-close .attachments').empty();
    clearError();
  }

  function onFileUploaded(a) {
    model.attachments.push(a);

    var attachmentTemplate = $('.create-notes .templates .attachment');

    $('.case-close .attachments').append(attachmentTemplate
      .prop('outerHTML')
      .replaceAll('{fileName}', a.fileName)
      .replaceAll('{fileSize}', app.utils.formatFileSize(a.size)));
  }

  $(function () {
    $('.case-close .fileuploader').uploadFile({
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

    $(".case-close textarea").on('change input paste', function () {
      model.notes = $(this).val().trim();
      if (isSave) {
        validate();
      }
    });

    $('.case-close .upload-btn').click(function () {
      var self = $(this);

      $('.case-close .ajax-file-upload input[type="file"]').click();
    });

    $('.case-close #submitBtn').click(function () {
      submit();
    });

    $('#show-caseClose').click(function () {
      reset();
    });
  });

})(app.page.caseClose = app.page.caseClose || {});