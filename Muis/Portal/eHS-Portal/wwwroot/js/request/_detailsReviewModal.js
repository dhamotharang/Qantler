(function () {
  'use strict';

  var controller;

  var data;

  var email;
  var emailTemplate;
  var didComposeEmail;

  this.init = function (d) {
    data = d;
  }

  this.show = function () {
    didComposeEmail = false;
    setupView();
  }

  this.submit = function () {
    if (!validate()) {
      return;
    }

    var step = data.status == 300 ? 1 : 0;

    var lineItems = [];

    $('.review-modal fieldset').each(function () {
      var scheme = $(this).data('scheme');
      var subScheme = $(this).data('subscheme');

      var approved = $(this).find('select').select2('val') == 1;
      var remarks = $(this).find('textarea').val();

      lineItems.push({
        scheme: scheme,
        subScheme: subScheme,
        remarks: remarks,
        approved: approved
      })
    });

    var models = [];

    var refID;
    if (step == 1 && data.jobID) {
      refID = data.jobID;
    }

    models.push({
      step: step,
      refID: refID,
      lineItems: lineItems,
      requestID: app.page.id,
      email: email
    });

    var hasReject = models[0].lineItems.filter(e => !e.approved);
    if (hasReject.length > 0 && !email) {
      swal.fire({
        text: 'Are you sure you want to proceed without drafing the rejection email?',
        allowOutsideClick: false,
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: 'Yes'
      }).then(res => {
        if (res.isConfirmed) {
          submitToServer(models);
        }
      });
      return;
    }

    submitToServer(models);
  }

  function submitToServer(payload) {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Submitting. Please wait...');

    fetch('/api/request/recommend', {
      method: 'POST',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(payload)
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(r => {
        location.reload(true);
      })
      .catch(app.http.catch);
  }

  function validate() {
    clearError();

    var hasReject = false;
    $('.review-modal fieldset').each(function () {

      var select = $(this).find('select');
      var textarea = $(this).find('textarea');

      var selectVal = select.select2('val');
      if (app.utils.isNullOrEmpty(selectVal)) {
        select.closest('.form-group').addClass('has-danger');
      }

      var remarks = textarea.val();
      if (selectVal == 0
        && !app.utils.isNullOrEmpty(selectVal)
        && app.utils.isNullOrEmpty(remarks)) {
        textarea.closest('.form-group').addClass('has-danger');
      }

      if (!hasReject && selectVal == 0 && !app.utils.isNullOrEmpty(selectVal)) {
        hasReject = true;
      }
    });

    if (hasReject
      && data.status == 400
      && (!email || !didComposeEmail)) {
      $('.review-modal .rejection-email').addClass('has-danger');
    }

    return $('.review-modal .has-danger').length == 0;
  }

  function clearError() {
    $('.review-modal .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  }

  function setupView() {
    var tabTemplate = $('.reviewmodel-template .review-recommendation');

    var container = $('.review-modal form');
    container.empty();

    data.lineItems.forEach(e => {
      container.append(tabTemplate.clone()
        .prop('outerHTML')
        .replaceAll('{scheme}', e.scheme)
        .replaceAll('{subscheme}', e.subScheme)
        .replaceAll('{text}', app.common.formatScheme(e.schemeText, e.subSchemeText)));
    });


    $('.review-modal .select-single').select2({
      placeholder: 'Select',
      allowClear: true
    });

    $('.review-modal .rejection-email').removeClass('d-flex');
  }

  function initRejectEmail() {
    if (!emailTemplate) {
      fetchEmailTemplate();
      return;
    }

    if (!data.jobOrder
      && data.jobID) {
      fetchJobOrder();
      return;
    }

    var tmpEmail = email;

    if (!tmpEmail) {
      tmpEmail = app.page.helper.prepareDraftRejectionEmail(emailTemplate, data);
    }

    var options = {
      fields: 'body'
    }

    app.page.email.init(tmpEmail, options);
    app.page.email.callback = function (e) {
      email = e;
      didComposeEmail = true;
      $('.review-modal .rejection-email').removeClass('has-danger');
    };
  }

  function fetchEmailTemplate() {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Initializing. Please wait...');

    fetch('/api/master/email/100', {
      method: 'GET',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(res => {
        app.dismissProgress();

        emailTemplate = res;

        initRejectEmail();
      }).catch(app.http.catch);
  }

  function fetchJobOrder() {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Initializing. Please wait...');

    fetch('/api/jobOrder/' + data.jobID, {
      method: 'GET',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(res => {
        app.dismissProgress();

        data.jobOrder = res;

        initRejectEmail();
      }).catch(app.http.catch);
  }

  function invalidateRejectionEmail() {
    var showComposeEmail = false;
    $('.review-modal fieldset').each(function () {
      var val = $(this).find('select').select2('val');
      if (val) {
        var approved = val == 1;
        if (!showComposeEmail && !approved) {
          showComposeEmail = true;
        }
      }
    });

    var e = $('.review-modal .rejection-email');
    if (showComposeEmail) {
      e.addClass('d-flex');
    } else {
      e.removeClass('d-flex');
    }
  }

  $(function () {
    $('.review-modal .submit').click(function () {
      app.page.review.submit();
    });

    $(document).on('change', '.review-modal select', function () {
      var val = $(this).select2('val');

      if (!app.utils.isNullOrEmpty(val)) {
        $(this).closest('.form-group').removeClass('has-danger');
      }

      invalidateRejectionEmail();
    });

    $(document).on('change input paste', '.review-modal textarea', function () {
      var val = $(this).val();

      if (!app.utils.isNullOrEmpty(val)) {
        $(this).closest('.form-group').removeClass('has-danger');
      }
    });

    $('.review-modal .rejection-email button').click(function () {
      app.page.email.init(null);

      initRejectEmail();
    });
  });

}).apply(app.page.review = app.page.review || {});