(function () {

  var source = null;
  var type = null;
  var title = null;
  var typeOfOffence = [];
  var ref = null;
  var offender = {};
  var background = null;
  var attachments = [];
  var reporterName = null;
  var reporterMobile = null;
  var reporterMail = null;
  var reporterAltEmail = null;
  var isSubmit = false;
  var selectedPremiseID = [];
  var validatePremise = false;

  $('#sourceSelect').select2({
    placeholder: 'Select',
    allowClear: true
  });

  $('#typeSelect').select2({
    placeholder: 'Select'
  });

  this.init = function () {
    var offenceType = [];
    if (app.page.model.offenceType) {
      offenceType = app.page.model.offenceType.sort((a, b) =>
        a.value.localeCompare(b.value)).map(e => {
          return { id: e.id, text: e.value };
        });
    }

    if (offenceType) {
      offenceType = offenceType.sort((a, b) => a.text.localeCompare(b.text)).map(e => {
        return { id: e.id, text: e.text };
      });
    }

    $('#OffenceTypeSelect').select2({
      data: offenceType
    });

    $("#sourceSelect").val('').trigger('change');
    type = $("#typeSelect").select2("val", "");
    $('#offenderName').val('');
    $('#title').val('');
    $('#background').val('');
    $('#reporterName').val('');
    $('#reporterMobile').val('');
    $('#reporterMail').val('');
    $('#reporterAltEmail').val('');
  };

  this.onFileUploaded = function (a) {
    attachments.push(a);

    var attachmentTemplate = $('.templates .attachment');

    $('.attachments').append(attachmentTemplate
      .prop('outerHTML')
      .replaceAll('{fileName}', a.fileName)
      .replaceAll('{fileSize}', app.utils.formatFileSize(a.size)));
  };

  function validate() {
    var isValid = true;
    if (app.utils.isNullOrEmpty(background) && isSubmit == true) {
      $('#background').closest('.form-group').addClass('has-danger');
      isValid = false;
    }
    else {
      $('#background').closest('.form-group').removeClass('has-danger');
    }

    if (app.utils.isNullOrEmpty(title) && isSubmit == true) {
      $('#title').closest('.form-group').addClass('has-danger');
      isValid = false;
    }
    else {
      $('#title').closest('.form-group').removeClass('has-danger');
    }

    if (typeOfOffence.length == 0 && isSubmit == true) {
      $('#OffenceTypeSelect').closest('.form-group').addClass('has-danger');
      isValid = false;
    }
    else {
      $('#OffenceTypeSelect').closest('.form-group').removeClass('has-danger');
    }

    if (app.utils.isNullOrEmpty(type) && isSubmit == true) {
      $('#typeSelect').closest('.form-group').addClass('has-danger');
      isValid = false;
    }
    else {
      $('#typeSelect').closest('.form-group').removeClass('has-danger');
    }

    if (app.utils.isNullOrEmpty(source) && isSubmit == true) {
      $('#sourceSelect').closest('.form-group').addClass('has-danger');
      isValid = false;
    }
    else {
      $('#sourceSelect').closest('.form-group').removeClass('has-danger');
    }

    if (app.utils.isNullOrEmpty(offender.ID) && isSubmit == true) {
      $('#offenderName').closest('.form-group').addClass('has-danger');
      isValid = false;
    }
    else {
      $('#offenderName').closest('.form-group').removeClass('has-danger');
    }

    if (selectedPremiseID.length == 0 && validatePremise == true) {
      $('#premiseErrorMsg').removeClass('d-none');
      isValid = false;
    }
    else {
      $('#premiseErrorMsg').addClass('d-none');
    }

    if (!app.utils.isNullOrEmpty(reporterMail) &&
      !app.utils.isValidEmail(reporterMail) && isSubmit == true) {
      $('#reporterMail').closest('.form-group').addClass('has-danger');
      $("#reportedCollapse").addClass('show');
      $('[aria-controls="reportedCollapse"]').attr('aria-expanded', 'true')
      isValid = false;
    }
    else {
      $('#reporterMail').closest('.form-group').removeClass('has-danger');
    }

    if (!app.utils.isNullOrEmpty(reporterAltEmail) &&
      !app.utils.isValidEmail(reporterAltEmail) && isSubmit == true) {
      $('#reporterAltEmail').closest('.form-group').addClass('has-danger');
      $("#reportedCollapse").addClass('show');
      $('[aria-controls="reportedCollapse"]').attr('aria-expanded', 'true')
      isValid = false;
    }
    else {
      $('#reporterAltEmail').closest('.form-group').removeClass('has-danger');
    }

    return isValid;
  };

  this.getModel = function () {
    var model = {
      Source: source,
      Type: type,
      Title: title,
      Offences: typeOfOffence,
      RefID: ref,
      Background: background,
      Offender: null,
      Attachments: attachments,
      ReportedBy: { Name: null, ContactInfos: [] },
      Premises: selectedPremiseID,
      IsDeleted: 0
    }
    if (!app.utils.isNullOrEmpty(offender)) {
      model.Offender = offender;
    }

    if (!app.utils.isNullOrEmpty(reporterName)) {
      model.ReportedBy.Name = reporterName;
    }
    else {
      model.ReportedBy.Name = "Anonymous";
    }

    if (!app.utils.isNullOrEmpty(reporterMobile)) {
      model.ReportedBy.ContactInfos.push({ Type: 4, Value: reporterMobile });
    }

    if (!app.utils.isNullOrEmpty(reporterMail)) {
      model.ReportedBy.ContactInfos.push({ Type: 5, Value: reporterMail });
    }

    if (!app.utils.isNullOrEmpty(reporterAltEmail)) {
      model.ReportedBy.ContactInfos.push({ Type: 6, Value: reporterAltEmail });
    }
    return model;
  }

  this.submit = function () {
    isSubmit = true;
    validatePremise = true;
    var valid = validate();
    if (valid) {
      var controller;
      if (controller) {
        controller.abort();
      }

      controller = new AbortController();
      var saveModel = this.getModel();

      app.showProgress('Processing. Please wait...');

      fetch('/api/case', {
        method: 'POST',
        cache: 'no-cache',
        credentials: 'include',
        signal: controller.signal,
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(saveModel)
      }).then(app.http.errorInterceptor)
        .then(res => res.json())
        .then(r => {
          if (r) {
            app.dismissProgress();
            var url = window.location.origin + "/case/details/" + r;
            window.location.href = url;
          }
        }).catch(app.http.catch);
    }
  };

  this.removePremise = function (e) {
    var id = $(e).parent().attr('id');
    var ItemID = selectedPremiseID.filter((item) => item.id == id);
    selectedPremiseID.pop(ItemID);
    $(e).parent().parent().remove();

    if (isSubmit == true) {
      validate();
    }
  };

  $(function () {
    $('.fileuploader').uploadFile({
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
        app.page.onFileUploaded(data[0]);
        app.dismissProgress();
      },
      onError: function (e) {
        showError('Something went wrong!');
      },
      onCancel: function (e) {
      }
    });

    $('.upload-btn').click(function () {
      $('.ajax-file-upload input[type="file"]').click();
    });

    $('#referenceID').on('change input paste', function () {
      ref = $(this).val().trim();
    });

    $('#background').on('change input paste', function () {
      background = $(this).val().trim();
      if (isSubmit == true) {
        validate();
      }
    });

    $('#reporterName').on('change input paste', function () {
      reporterName = $(this).val().trim();
    });

    $('#reporterMobile').on('change input paste', function () {
      reporterMobile = $(this).val().trim();
    });

    $('#title').on('change input paste', function () {
      title = $(this).val().trim();
      if (isSubmit == true) {
        validate();
      }
    });

    $('#reporterMail').on('change input paste', function () {
      reporterMail = $(this).val().trim();
      if (isSubmit == true) {
        validate();
      }
    });

    $('#reporterAltEmail').on('change input paste', function () {
      reporterAltEmail = $(this).val().trim();
      if (isSubmit == true) {
        validate();
      }
    });

    $('#OffenceTypeSelect').select2().on('change', function () {
      var selectedoptions = $(this).find('option:selected');
      typeOfOffence = [];

      if (selectedoptions != undefined) {
        $.each(selectedoptions, function () {
          var temp = {
            Type: 502,
            ID: $(this).val()
          };
          typeOfOffence.push(temp);
        });
      }

      if (isSubmit == true) {
        validate();
      }

    });

    $('#typeSelect').select2().on('change', function () {
      type = $(this).val();
      if (isSubmit == true) {
        validate();
      }
    });

    $('#sourceSelect').select2().on('change', function () {
      source = $(this).val();
      if (source == 3) {
        $('.referenceID').show();
      }
      else {
        $('.referenceID').hide();
        $('#referenceID').val('');
      }

      if (isSubmit == true) {
        validate();
      }
    });

    $('#premiseBtn').click(function () {

      app.select.premise.init({
        customerID: offender.ID,
        title: 'Select Premise',
        skip: selectedPremiseID.map(x => x.id)
      });

      app.select.premise.onSelect = function (c) {
        selectedPremiseID.push(c);
        var item = $('.template-premise').html()
          .replaceAll('{data}', app.utils.formatPremise(c))
          .replaceAll('{data-id}', c.id);
        $('.container.premises').append(item);

        if (isSubmit == true) {
          validate();
        }
      };
    });

    $('button[id="offenderSelect"]').click(function () {
      var skip = [];
      if (offender.ID != undefined) {
        skip.push(offender.ID);
      }

      app.select.customer.init({
        skip: skip,
        title: 'Existing Customers'
      });

      app.select.customer.onSelect = function (c) {
        $('#offenderName').val(c.name);
        offender.Name = c.name;
        offender.ID = c.id;

        selectedPremiseID = [];
        $('.container.premises').html('');
        $('#premise-parent').removeClass('d-none');

        validatePremise = false;

        if (isSubmit == true) {
          validate();
        }
      };
    });

  });

}).apply(app.page);

$(document).ready(function () {
  app.page.init();
});
