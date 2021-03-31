(function (self) {
  'use strict'

  var model;
  var sanction;
  var notes;
  var attachments = [];
  var controller;
  var isSave = false;
  var amount;
  var month;
  var finalDecisionData = [];

  self.init = function (_model) {
    model = _model;

    switch (model.sanction) {
      case 0:
        finalDecisionData = [{
          id: 0,
          text: 'Warning'
        }, {
          id: 5,
          text: 'Dismiss Case'
        }];

        break;
      case 1:
        finalDecisionData = [{
          id: 0,
          text: 'Warning'
        }, {
          id: 1,
          text: 'Composition Sum'
        }, {
          id: 5,
          text: 'Dismiss Case'
        }];

        break;
      case 2:
        finalDecisionData = [{
          id: 0,
          text: 'Warning'
        }, {
          id: 2,
          text: 'Suspension'
        }, {
          id: 5,
          text: 'Dismiss Case'
        }];

        break;
      case 3:
        finalDecisionData = [{
          id: 0,
          text: 'Warning'
        }, {
          id: 3,
          text: 'Immediate Suspension'
        }, {
          id: 5,
          text: 'Dismiss Case'
        }];

        break;
      case 4:
        finalDecisionData = [{
          id: 0,
          text: 'Warning'
        }, {
          id: 2,
          text: 'Suspension'
        }, {
          id: 4,
          text: 'Revoke'
        }, {
          id: 5,
          text: 'Dismiss Case'
        }];

        break;
      case 7:
        finalDecisionData = [{
          id: 7,
          text: 'Reinstate'
        }];
    }

    $('.foc-decision .select-single').select2({
      data: finalDecisionData,
      placeholder: 'Select'
    });

  };

  $('.foc-decision #months').inputmask({
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

  $(".foc-decision #amount").inputmask('decimal');


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
    body.Sanction.Type = 1;
    body.Sanction.Sanction = sanction;
    body.Note = notes;
    body.Attachment = attachments;
    if (sanction == "1") {
      body.Sanction.Value = amount.replaceAll(",", "");
    }
    else if (sanction == "2") {
      body.Sanction.Value = month;
    }

    $('.foc-decision .close').click();
    app.showProgress('Processing. Please wait...');

    fetch('/api/case/' + model.id + '/foc-decision?', {
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
      $('.foc-decision #finalDecision-select').closest('.form-group').addClass('has-danger');
    }

    if (sanction == "1"
      && (app.utils.isNullOrEmpty(amount)
        || parseFloat(amount) == 0)) {
      $('.foc-decision #amount').closest('.form-group').addClass('has-danger');
    }

    if (sanction == "2"
      && app.utils.isNullOrEmpty(month)) {
      $('.foc-decision #months').closest('.form-group').addClass('has-danger');
    }

    if (app.utils.isNullOrEmpty(notes)) {
      $('.foc-decision #note').closest('.form-group').addClass('has-danger');
    }

    if (attachments.length == 0) {
      $('.foc-decision .upload-btn').closest('.form-group').addClass('has-danger');
    }
    return $('.foc-decision .has-danger').length == 0;
  };

  function clearError() {
    $('.foc-decision .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  };

  function reset() {
    isSave = false;
    attachments = [];
    notes = "";
    amount = "";
    month = "";
    $('#finalDecision-select').select2('val', 0);
    $('.foc-decision #note').val('');
    $('.foc-decision .attachments').empty();
    $('.foc-decision #amount').val('');
    $('.foc-decision #months').val('');

    var recommendedActions = model.sanctionInfos.filter(x => x.type == 0);
    var maxRecommendedActionID = Math.max.apply(null, recommendedActions.map(x => x.id));
    var sanctionInfo = recommendedActions.find(x => x.id == maxRecommendedActionID);
    sanction = sanctionInfo.sanction.toString();
    $('#finalDecision-select').select2('val', sanction);

    if (sanction == "1") {
      $('.foc-decision #composition-amount').show();
      $('.foc-decision #suspension-months').hide();

      $('.foc-decision #amount').val(sanctionInfo.value);
      amount = sanctionInfo.value
    } else if (sanction == "2") {
      $('.foc-decision #suspension-months').show();
      $('.foc-decision #composition-amount').hide();

      $('.foc-decision #months').val(sanctionInfo.value);
      month = sanctionInfo.value
    } else {
      $('.foc-decision #suspension-months').hide();
      $('.foc-decision #composition-amount').hide();
    }

    if (finalDecisionData.length == 1) {
      $(".foc-decision .select-single").select2("val", finalDecisionData[0].id.toString());
    }

    clearError();
  };

  function onFileUploaded(a) {
    attachments.push(a);

    var attachmentTemplate = $('.foc-decision .templates .attachment');

    $('.foc-decision .attachments').append(attachmentTemplate
      .prop('outerHTML')
      .replaceAll('{fileName}', a.fileName)
      .replaceAll('{fileSize}', app.utils.formatFileSize(a.size)));

    if (isSave) {
      validate();
    }
  };

  $('.foc-decision .fileuploader').uploadFile({
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

  $(".foc-decision textarea").on('change input paste', function () {
    notes = $(this).val().trim();
    if (isSave) {
      validate();
    }
  });

  $('.foc-decision .upload-btn').click(function () {
    $('.foc-decision .ajax-file-upload input[type="file"]').click();
  });

  $('#finalDecision-select').on('change', function () {
    sanction = $(this).select2('val');
    if (sanction == "1") {
      $('.foc-decision #composition-amount').show();
      $('.foc-decision #suspension-months').hide();
    } else if (sanction == "2") {
      $('.foc-decision #suspension-months').show();
      $('.foc-decision #composition-amount').hide();
    } else {
      $('.foc-decision #suspension-months').hide();
      $('.foc-decision #composition-amount').hide();
    }

    if (isSave) {
      validate();
    }
  });

  $('.foc-decision #amount').on('change', function () {
    amount = $(this).val();
    if (isSave) {
      validate();
    }
  });

  $('.foc-decision #months').on('change', function () {
    month = $(this).val();
    if (isSave) {
      validate();
    }
  });

  $('.foc-decision #submitBtn').click(function () {
    submit();
  });

  $('#show-focDecision').click(function () {
    reset();
  });

})(app.page.focDecision = app.page.focDecision || {});