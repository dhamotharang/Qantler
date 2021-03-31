(function () {
  'use strict';

  var controller;

  var data;
  var emailCache = {};

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

    var lineItems = [];

    $('.approval-modal fieldset').each(function () {
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
    if (data.jobID) {
      refID = data.jobID;
    }

    models.push({
      step: 3,
      refID: refID,
      lineItems: lineItems,
      requestID: app.page.id,
      email: email
    });

    var hasReject = models[0].lineItems.filter(e => !e.approved);
    if (hasReject.length > 0 && !email) {
      $('.approval-modal .rejection-email label span').text('Rejection email is required.');
      $('.approval-modal .rejection-email').addClass('has-danger');
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

    fetch('/api/request/approve', {
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

    var aoRecommendation = getAORecommendation();

    var hasReject = false;
    $('.approval-modal fieldset').each(function () {

      var select = $(this).find('select');
      var textarea = $(this).find('textarea');

      var scheme = $(this).data('scheme');
      var subScheme = $(this).data('subscheme');

      var selectVal = select.select2('val');
      if (app.utils.isNullOrEmpty(selectVal)) {
        select.closest('.form-group').addClass('has-danger');
      }
      else {
        var li = aoRecommendation.lineItems
          .filter(e => e.scheme == scheme && e.subScheme == subScheme)[0];

        var remarks = textarea.val();
        if (li.approved != selectVal
          && app.utils.isNullOrEmpty(remarks)) {
          textarea.closest('.form-group').addClass('has-danger');
        }
      }

      if (!hasReject && selectVal == 0 && !app.utils.isNullOrEmpty(selectVal)) {
        hasReject = true;
      }
    });
    
  if (hasReject
    && aoRecommendation.emailID
    && data.status == 400
    && (!email || !didComposeEmail)) {
    $('.approval-modal .rejection-email label span').text('Please review rejection email.');
    $('.approval-modal .rejection-email').addClass('has-danger');
  }

  return $('.approval-modal .has-danger').length == 0;
}

  function clearError() {
  $('.approval-modal .has-danger').each(function () {
    $(this).removeClass('has-danger');
  });
}

function setupView() {
  var tabTemplate = $('.approvalmodel-template .approval-recommendation');

  var container = $('.approval-modal form');
  container.empty();

  data.lineItems.forEach(e => {
    container.append(tabTemplate.clone()
      .prop('outerHTML')
      .replaceAll('{scheme}', e.scheme)
      .replaceAll('{subscheme}', e.subScheme)
      .replaceAll('{text}', app.common.formatScheme(e.schemeText, e.subSchemeText)));
  });

  $('.approval-modal .select-single').select2({
    placeholder: 'Select',
    allowClear: true
  });

  $('.approval-modal .rejection-email').removeClass('d-flex');
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
    var emailID = getAORecommendation().emailID;

    var drafts = [];
    if (emailID) {

      if (!emailCache[emailID]) {
        fetchEmail(emailID);
        return;
      }

      drafts.push(emailCache[emailID]);
    }

    tmpEmail = app.page.helper.prepareRejectionEmail(emailTemplate, [data], drafts);
  }

  var options = {
    fields: 'to,cc,bcc,subject,body'
  }

  app.page.email.init(tmpEmail, options);
  app.page.email.callback = function (e) {
    email = e;
    didComposeEmail = true;
    $('.approval-modal .rejection-email').removeClass('has-danger');
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

function fetchEmail(id) {
  if (controller) {
    controller.abort();
  }
  controller = new AbortController();

  app.showProgress('Initializing. Please wait...');

  fetch('/api/email/request/' + id, {
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

      emailCache[id] = res;
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
  $('.approval-modal fieldset').each(function () {
    var val = $(this).find('select').select2('val');
    if (val) {
      var approved = val == 1;
      if (!showComposeEmail && !approved) {
        showComposeEmail = true;
      }
    }
  });

  var e = $('.approval-modal .rejection-email');
  if (showComposeEmail) {
    var emailID = getAORecommendation().emailID;

    if (emailID) {
      $('.approval-modal .rejection-email button span').text('Review Rejection Email');
    }

    e.addClass('d-flex');
  } else {
    e.removeClass('d-flex');
  }
}

function getAORecommendation() {
  var reviews = data.reviews.filter(e => e.step <= 2)
    .sort((a, b) => moment.utc(b.createdOn).diff(moment.utc(a.createdOn)));
  return reviews[0];
}

$(function () {
  $('.approval-modal .submit').click(function () {
    app.page.approval.submit();
  });

  $(document).on('change', '.approval-modal select', function () {
    var val = $(this).select2('val');

    if (!app.utils.isNullOrEmpty(val)) {
      $(this).closest('.form-group').removeClass('has-danger');
      $('.approval-modal .rejection-email').removeClass('has-danger');
    }

    invalidateRejectionEmail();
  });

  $(document).on('change input paste', '.approval-modal textarea', function () {
    var val = $(this).val();

    if (!app.utils.isNullOrEmpty(val)) {
      $(this).closest('.form-group').removeClass('has-danger');
      $('.approval-modal .rejection-email').removeClass('has-danger');
    }
  });

  $('.approval-modal .rejection-email button').click(function () {
    app.page.email.init(null);

    initRejectEmail();
  });
});

}).apply(app.page.approval = app.page.approval || {});