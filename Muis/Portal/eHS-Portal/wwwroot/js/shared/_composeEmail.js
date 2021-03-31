(function () {
  'use strict';

  var email;
  var editor;

  var editorOption;
  var defaultOptions = {
    fields: 'to,cc,bcc,subject,body'
  }

  this.init = function (e, option = defaultOptions) {
    email = e;
    editorOption = option;
    reset();
    setup();
  }

  function reset() {
    $('.compose-email #to').val('');
    $('.compose-email #cc').val('');
    $('.compose-email #bcc').val('');
    $('.compose-email #subject').val('');
    editor.setContent('');
  }

  function setup() {
    if (!email) {
      return;
    }

    if (editorOption && editorOption.fields) {
      if (editorOption.fields.indexOf('to') != -1) {
        $('.compose-email .to').removeClass('d-none');
      } else {
        $('.compose-email .to').addClass('d-none');
      }

      if (editorOption.fields.indexOf('cc') != -1) {
        $('.compose-email .cc').removeClass('d-none');
      } else {
        $('.compose-email .cc').addClass('d-none');
      }

      if (editorOption.fields.indexOf('bcc') != -1) {
        $('.compose-email .bcc').removeClass('d-none');
      } else {
        $('.compose-email .bcc').addClass('d-none');
      }

      if (editorOption.fields.indexOf('subject') != -1) {
        $('.compose-email .subject').removeClass('d-none');
      } else {
        $('.compose-email .subject').addClass('d-none');
      }

      if (editorOption.fields.indexOf('body') != -1) {
        $('.compose-email .body').removeClass('d-none');
      } else {
        $('.compose-email .body').addClass('d-none');
      }
    }

    $('.compose-email #to').val(app.utils.emptyIfNullOrEmpty(email.to));
    $('.compose-email #cc').val(app.utils.emptyIfNullOrEmpty(email.cc));
    $('.compose-email #bcc').val(app.utils.emptyIfNullOrEmpty(email.bcc));
    $('.compose-email #subject').val(app.utils.emptyIfNullOrEmpty(email.title));
    editor.setContent(app.utils.emptyIfNullOrEmpty(email.body));
  }

  function validate(model) {
    clearError();

    if (editorOption.fields.indexOf('to') != -1
      && app.utils.isNullOrEmpty(model.to)) {
      $('.compose-email #to').closest('.form-group').addClass('has-danger');
    }

    if (editorOption.fields.indexOf('subject') != -1
      && app.utils.isNullOrEmpty(model.title)) {
      $('.compose-email #subject').closest('.form-group').addClass('has-danger');
    }

    return $('.compose-email .form-group.has-danger').length == 0;
  }

  function clearError() {
    $('.compose-email .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  }

  function onDoneTapped() {
    var model = convertToModel();

    if (!validate(model)) {
      return;
    }

    app.page.email.callback(model);

    $('.compose-email .close').click();
  }

  function convertToModel() {
    var result = {
      from: email.from,
      to: $('.compose-email #to').val(),
      cc: $('.compose-email #cc').val(),
      bcc: $('.compose-email #bcc').val(),
      title: $('.compose-email #subject').val(),
      body: editor.getContent(),
      isBodyHtml: true
    }
    return result;
  }

  this.callback = function (e) {
  }

  function uploadHandler(blobInfo, success, failure, progress) {
    const formData = new FormData();
    formData.append('file', blobInfo.blob(), blobInfo.filename());

    fetch('/api/file', {
      method: 'POST',
      cache: 'no-cache',
      credentials: 'include',
      body: formData
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(res => {
        success('/api/file/' + res[0].fileID + '?fileName=' + encodeURI(res[0].fileName));
      })
      .catch(app.http.catch);
  }

  $(function () {
    if ($('.compose-email #tinyMce')) {
      tinymce.init({
        selector: '.compose-email #tinyMce',
        height: 500,
        theme: 'silver',
        plugins: [
          'advlist autolink lists link image charmap preview hr',
          'searchreplace wordcount visualblocks visualchars code fullscreen',
          'insertdatetime nonbreaking save table contextmenu directionality',
          'emoticons paste textcolor colorpicker textpattern imagetools'
        ],
        toolbar1: 'undo redo | insert | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
        toolbar2: 'preview | forecolor backcolor emoticons',
        image_advtab: true,
        forced_root_block: 'div',
        content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:.875rem }',
        content_css: [],
        images_upload_handler: uploadHandler
      });

      editor = tinymce.get(0);

      $('.compose-email .done').click(function () {
        onDoneTapped();
      });
    }
  });

}).apply(app.page.email = app.page.email || {});