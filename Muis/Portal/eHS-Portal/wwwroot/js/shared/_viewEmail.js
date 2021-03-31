(function () {
  'use strict';

  var email;

  var editorOption;

  var defaultOptions = {
    fields: 'to,cc,bcc,subject,body'
  }

  this.init = function (title, option = defaultOptions) {
    $('.view-email .modal-title').html(title);

    editorOption = option;

    reset();
  }

  this.email = function (e) {
    email = e;
    setup();
  }

  function reset() {
    $('.view-email #to').val('');
    $('.view-email #cc').val('');
    $('.view-email #bcc').val('');
    $('.view-email #subject').val('');
    $('.view-email #email-body').html('');
  }

  function setup() {
    if (!email) {
      return;
    }

    if (editorOption && editorOption.fields) {
      var showSeparator = false;

      if (editorOption.fields.indexOf('to') != -1) {
        showSeparator = true;
        $('.view-email .to').removeClass('d-none');
      } else {
        $('.view-email .to').addClass('d-none');
      }

      if (editorOption.fields.indexOf('cc') != -1) {
        showSeparator = true;
        $('.view-email .cc').removeClass('d-none');
      } else {
        $('.view-email .cc').addClass('d-none');
      }

      if (editorOption.fields.indexOf('bcc') != -1) {
        showSeparator = true;
        $('.view-email .bcc').removeClass('d-none');
      } else {
        $('.view-email .bcc').addClass('d-none');
      }

      if (editorOption.fields.indexOf('subject') != -1) {
        showSeparator = true;
        $('.view-email .subject').removeClass('d-none');
      } else {
        $('.view-email .subject').addClass('d-none');
      }

      if (editorOption.fields.indexOf('body') != -1) {
        $('.view-email .body').removeClass('d-none');
      } else {
        $('.view-email .body').addClass('d-none');
      }

      if (showSeparator) {
        $('.view-email .hr').removeClass('d-none');
      } else {
        $('.view-email .hr').addClass('d-none');
      }
    }

    $('.view-email #to').val(app.utils.emptyIfNullOrEmpty(email.to));
    $('.view-email #cc').val(app.utils.emptyIfNullOrEmpty(email.cc));
    $('.view-email #bcc').val(app.utils.emptyIfNullOrEmpty(email.bcc));
    $('.view-email #subject').val(app.utils.emptyIfNullOrEmpty(email.title));
    $('.view-email #email-body').html(app.utils.emptyIfNullOrEmpty(email.body));
  }

}).apply(app.page.email.readonly = app.page.email.readonly || {});