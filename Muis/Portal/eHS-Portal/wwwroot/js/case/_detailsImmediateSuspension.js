(function (self) {
  'use strict'

  var model;
  var notes;
  var attachments = [];
  var controller;
  var month;
  var isSave = false;

  $('.case-immediate-suspension #months').inputmask({
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
    body.Months = month;
    body.Notes = notes;
    body.Attachments = attachments;

    $('.case-immediate-suspension .close').click();
    app.showProgress('Processing. Please wait...');

    fetch('/api/case/' + model.id + '/immediate-suspension?', {
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
      $('.case-immediate-suspension #note').closest('.form-group').addClass('has-danger');
    }

    if (app.utils.isNullOrEmpty(month)) {
      $('.case-immediate-suspension #suspension-months').closest('.form-group').addClass('has-danger');
    }

    if (attachments.length == 0) {
      $('.case-immediate-suspension .upload-btn').closest('.form-group').addClass('has-danger');
    }
    return $('.case-immediate-suspension .has-danger').length == 0;
  };

  function clearError() {
    $('.case-immediate-suspension .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  };

  function reset() {
    isSave = false;
    attachments = [];
    notes = "";
    month = "";
    $('.case-immediate-suspension #note').val('');
    $('.case-immediate-suspension #months').val('');
    $('.case-immediate-suspension .attachments').empty();

    clearError();
  };

  function onFileUploaded(a) {
    attachments.push(a);

    var attachmentTemplate = $('.case-immediate-suspension .templates .attachment');

    $('.case-immediate-suspension .attachments').append(attachmentTemplate
      .prop('outerHTML')
      .replaceAll('{fileName}', a.fileName)
      .replaceAll('{fileSize}', app.utils.formatFileSize(a.size)));

    if (isSave) {
      validate();
    }
  };

  $('.case-immediate-suspension .fileuploader').uploadFile({
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

  $(".case-immediate-suspension textarea").on('change input paste', function () {
    notes = $(this).val().trim();
    if (isSave) {
      validate();
    }
  });

  $('.case-immediate-suspension #months').on('change input paste', function () {
    month = $(this).val();
    if (isSave) {
      validate();
    }
  });

  $('.case-immediate-suspension .upload-btn').click(function () {
    $('.case-immediate-suspension .ajax-file-upload input[type="file"]').click();
  });

  $('.case-immediate-suspension #submitBtn').click(function () {
    submit();
  });

  $('#show-caseImmediatesuspension').click(function () {
    reset();
  });

})(app.page.immediateSuspension = app.page.immediateSuspension || {});