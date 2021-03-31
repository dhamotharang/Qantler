(function (self) {
  'use strict';

  var model;

  var didInit;
  var controller;

  var data = {};

  var currentSanction;

  self.init = function (m) {
    model = m;

    currentSanction = app.page.helper.getLatestSanction(model);
  }

  function setupView() {
    if (didInit) {
      return;
    }

    var data = [];
    switch (model.sanction) {
      case 1:
        data = [{
          id: 0,
          text: 'Warning'
        }, {
          id: 1,
          text: 'Compound Sum'
        }, {
          id: 7,
          text: 'Reinstate'
        }, {
          id: -1,
          text: 'Reject Appeal'
        }];

        break;
      case 2:
      case 4:
        data = [{
          id: 0,
          text: 'Warning'
        }, {
          id: 2,
          text: 'Suspension'
        }, {
          id: 7,
          text: 'Reinstate'
        }, {
          id: -1,
          text: 'Reject Appeal'
        }];

        break;
    }

    $('.appeal-decision .select-single').select2({
      data: data,
      placeholder: 'Select'
    });

    $('.appeal-decision #months').inputmask({
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

    $(".appeal-decision #amount").inputmask('decimal');

    didInit = true;
  }

  function reset() {
    data = {
      sanctionInfo: {},
      attachments: []
    };

    clearError();

    $('.appeal-decision #sanction-select').val(null).trigger('change');
    $('.appeal-decision #note').val('');
    $('.appeal-decision #amount').val('');
    $('.appeal-decision #months').val('');
    $('.appeal-decision .attachments').empty();

    $('.appeal-decision #suspension-months').hide();
    $('.appeal-decision #composition-amount').hide();
  }

  function onFileUploaded(a) {
    data.attachments.push(a);

    var attachmentTemplate = $('.appeal-decision .templates .attachment');

    $('.appeal-decision .attachments').append(attachmentTemplate
      .prop('outerHTML')
      .replaceAll('{fileName}', a.fileName)
      .replaceAll('{fileSize}', app.utils.formatFileSize(a.size)));
  };

  function validate() {
    clearError();
    var sanction = data.sanctionInfo.sanction;

    if (isNaN(sanction)) {
      $('.appeal-decision #sanction-select').closest('.form-group').addClass('has-danger');
    }

    if (sanction == 1
      && (app.utils.isNullOrEmpty(data.sanctionInfo.value)
        || parseFloat(data.sanctionInfo.value) == 0)) {
      $('.appeal-decision #amount').closest('.form-group').addClass('has-danger');
    }

    if (sanction == 2
      && app.utils.isNullOrEmpty(data.sanctionInfo.value)) {
      $('.appeal-decision #months').closest('.form-group').addClass('has-danger');
    }

    if (app.utils.isNullOrEmpty(data.notes)) {
      $('.appeal-decision #note').closest('.form-group').addClass('has-danger');
    }

    if (data.attachments.length == 0) {
      $('.appeal-decision .upload-btn').closest('.form-group').addClass('has-danger');
    }
    return $('.appeal-decision .has-danger').length == 0;
  }

  function clearError() {
    $('.appeal-decision .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  };

  function submit() {
    if (!validate()) {
      return;
    }

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Processing. Please wait...');

    fetch('/api/case/' + model.id + '/appeal/decision', {
      method: 'POST',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(data)
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(r => {
        location.reload();
      })
      .catch(app.http.catch);
  }

  $(function () {

    $('#show-appealDecision').click(function () {
      setupView();
      reset();
    });

    var uploadOptions = app.file.defaultOptions();
    uploadOptions.onSuccess = function (e) {
      onFileUploaded(e);
      app.dismissProgress();
    };

    $('.appeal-decision .fileuploader').uploadFile(app.file.fileUploadConfig(uploadOptions));

    $('.appeal-decision .upload-btn').click(function () {
      $('.appeal-decision .ajax-file-upload input[type="file"]').click();
    });

    $('.appeal-decision #amount').on('change', function () {
      if (data.sanctionInfo.sanction == 1) {
        data.sanctionInfo.value = $(this).val();
      }
    });

    $('.appeal-decision #months').on('change', function () {
      if (data.sanctionInfo.sanction == 2) {
        data.sanctionInfo.value = $(this).val();
      }
    });

    $(".appeal-decision textarea").on('change input paste', function () {
      data.notes = $(this).val().trim();
    });

    $('.appeal-decision #sanction-select').on('change', function () {

      var val = parseInt($(this).select2('val'));
      data.sanctionInfo.sanction = val;
      data.sanctionInfo.value = '';

      $('.appeal-decision #suspension-months').hide();
      $('.appeal-decision #composition-amount').hide();

      if (val == 1) {
        $('.appeal-decision #composition-amount').show();
      } else if (val == 2) {
        $('.appeal-decision #suspension-months').show();
      }
    });

    $('.appeal-decision #submitBtn').click(function () {
      submit();
    });
  });

})(app.page.appeal.decision = app.page.appeal.decision || {});