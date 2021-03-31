(function (self) {
  'use strict'

  var model;

  var jobOrderID;

  var controller;

  var isSave = false;

  self.init = function (id) {
    jobOrderID = id;

    reset();
  }

  self.callback = function (d) {
  }

  function submit() {
    isSave = true;

    if (!validate()) {
      return;
    }

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Processing. Please wait...');

    fetch('/api/joborder/' + jobOrderID + '/notes', {
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
        app.dismissProgress();

        if (self.callback) {
          self.callback(r);
        }

        $('.create-notes .close').click();
      })
      .catch(app.http.catch);
  }

  function validate() {
    clearError();

    if (app.utils.isNullOrEmpty(model.text)) {
      $('.create-notes textarea').closest('.form-group').addClass('has-danger');
    }

    return $('.create-notes .has-danger').length == 0;
  }

  function clearError() {
    $('.create-notes .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  }

  function reset() {
    model = { attachments: [] };
    isSave = false; 
    $('.create-notes textarea').val('');
    $('.create-notes .attachments').empty();
    clearError();
  }

  function onFileUploaded(a) {
    model.attachments.push(a);

    var attachmentTemplate = $('.create-notes .templates .attachment');

    $('.create-notes .attachments').append(attachmentTemplate
      .prop('outerHTML')
      .replaceAll('{fileName}', a.fileName)
      .replaceAll('{fileSize}', app.utils.formatFileSize(a.size)));
  }

  $(function () {
    $('.create-notes .fileuploader').uploadFile({
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

    $(".create-notes textarea").on('change input paste', function () {
      model.text = $(this).val().trim();
      if (isSave) {
        validate();
      }
    });

    $('.create-notes .upload-btn').click(function () {
      var self = $(this);

      $('.create-notes .ajax-file-upload input[type="file"]').click();
    });

    $('.create-notes #submitBtn').click(function () {
      submit();
    });
  });

})(app.page.notes.new = app.page.notes.new || {});