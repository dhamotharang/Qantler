(function () {
  'use strict';

  var editor;
  var data;
  var option;

  this.init = function (_data, _option) {
    option = _option;
    data = _data;
    reset();
    setup();
  };

  function reset() {
    $('.compose-letter .action').remove();
    editor.setContent('');
    $('.compose-letter .tox-tinymce').removeClass('has-error');
    $('.compose-letter .body .error').hide();
  };

  function setup() {
    $.each(option.action, function (index, value) {
      var element = '';
      element = $('.action-button').html();
      element = element.replace("{id}", value.id).replace("{name}", value.text);
      $('.compose-letter .modal-footer').append(element);
    });

    $('.compose-letter .modal-title').html(option.title);

    editor.setContent(app.utils.emptyIfNullOrEmpty(data.body));
  };

  this.validate = function () {
    var body = editor.getContent();
    if (body == null
      || body == "") {
      $('.compose-letter .tox-tinymce').addClass('has-error');
      $('.compose-letter .body .error').show();
    }
    else {
      $('.compose-letter .tox-tinymce').removeClass('has-error');
      $('.compose-letter .body .error').hide();
    }
    return $('.compose-letter .has-error').length > 0;
  };

  function onActionTapped(element) {
    var id = $(element).attr('data-id');
    data.body = editor.getContent();

    app.page.letterCompose.callback(id, data, option);
  };

  this.callback = function (id, data, option) {
  };

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
  };

  $(function () {
    if ($('.compose-letter #tinyMce')) {
      tinymce.init({
        selector: '.compose-letter #tinyMce',
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
    }

    $('.compose-letter').on('click', '.action', function () {
      onActionTapped(this);
    });
  });

}).apply(app.page.letterCompose = app.page.letterCompose || {});