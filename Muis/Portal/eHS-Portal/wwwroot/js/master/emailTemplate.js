(function (self) {

  var editor;
  var controller;
  var data = jQuery.extend({}, app.page.model);

  function save() {
    if (!validate()) {
      return;
    }

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Updating. Please wait...');

    data.body = editor.getContent();

    app.page.model = data;

    fetch('/api/master/email/' + app.page.model.type, {
      method: 'PUT',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      body: JSON.stringify(data),
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(res => {
        app.dismissProgress();
        location.reload(true);
      })
      .catch(app.http.catch);
  }

  function validate() {
    clearError();

    if (app.utils.isNullOrEmpty($('#from').val())) {
      $('#from').closest('.form-group').addClass('has-danger');
      $('.from .error').html('From is required');
    } else if (!validateEmail($('#from').val())) {
      $('#from').closest('.form-group').addClass('has-danger');
      $('.from .error').html('Invalid email address');
    }

    if (!validateEmail($('#cc').val())) {
      $('#cc').closest('.form-group').addClass('has-danger');
      $('.cc .error').html('Invalid email address');
    }

    if (!validateEmail($('#bcc').val())) {
      $('#bcc').closest('.form-group').addClass('has-danger');
      $('.bcc .error').html('Invalid email address');
    }

    return $('.has-danger').length == 0;
  }

  function cleanEmail(e) {
    if (!e) {
      return e;
    }
    return e.split(";").filter(e => e.trim().length > 0).map(e => e.trim()).join('; ');
  }

  function validateEmail(e) {
    if (!e) {
      return true;
    }

    var emails = e.split(";");
    var invalid = emails.filter(e => {
      if (!e.trim() || e.startsWith("{")) return false;

      return !app.utils.isValidEmail(e.trim());
    });

    return invalid.length == 0;
  }

  function clearError() {
    $('.has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
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

  function isDirty() {
    if (data.from == (app.page.model.from ? app.page.model.from : '')
      && (data.cc ? data.cc : '') == (app.page.model.cc ? app.page.model.cc : '')
      && (data.bcc ? data.bcc : '') == (app.page.model.bcc ? app.page.model.bcc : '')
      && (data.title ? data.title : '') == (app.page.model.title ? app.page.model.title : '')
      && tinymce.get("tinyMce").getContent().replace(/>\s+</g, '><') == (app.page.model.body.replace(/>\s+</g, '><'))) {
      return false;
    }
    else {
      return true
    }
  }

  $(function () {
    tinymce.init({
      selector: '#tinyMce',
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
      images_upload_handler: uploadHandler,
      init_instance_callback: function (inst) {
        editor.setContent(self.model.body);
      }
    });

    editor = tinymce.get(0);

    $("#from").on('change input paste', function () {      
      data.from = $(this).val().trim();
    });

    $("#cc").on('change input paste', function () {
      data.cc = $(this).val().trim();
    });

    $("#bcc").on('change input paste', function () {
      data.bcc = $(this).val().trim();
    });

    $("#subject").on('change input paste', function () {
      data.title = $(this).val().trim();
    });

    $('#save').click(function () {
      save();
    });

    $('#emailTypeSelect').on('change', function () {
      var TemplateType = $('#emailTypeSelect').val();

      if (app.page.model.type != TemplateType) {
        var url = window.location.origin + "/master/email/" + TemplateType;
        window.location.href = url;
      }
    });

    $('#emailTypeSelect').select2();
    $('#emailTypeSelect').val(app.page.model.type);
    $('#emailTypeSelect').trigger('change');
  });

  window.onbeforeunload = e => {
    if (isDirty()) {
      $('#emailTypeSelect').val(app.page.model.type);
      $('#emailTypeSelect').trigger('change');

      return 'You have unsaved changes that will be lost. Are you sure you want to leave this page?';
    }
    else {
      return null;
    }
  };

})(app.page);

