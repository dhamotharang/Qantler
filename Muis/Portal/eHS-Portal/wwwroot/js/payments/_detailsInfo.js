(function () {
  'use strict'

  var controller;

  this.render = function (data) {
    if (!data) {
      return;
    }

    renderInfo(data);
  }

  function renderInfo(data) {
    if (!data) {
      return;
    }

    $(".basicinfo .info-id .detail").html(data.id);
    $(".basicinfo .info-paidOn .detail").html(app.utils.formatDate(data.paidOn, true));
    $(".basicinfo .info-transcationNo .detail").html(data.transactionNo);
    $(".basicinfo .info-mode .detail").html(data.modeText);
    $(".basicinfo .info-name .detail").html(data.name);
    $(".basicinfo .info-method .detail").html(data.methodText);
    $(".basicinfo .info-status .detail").html(data.statusText);
    $(".basicinfo .info-processedBy .detail").html(data.processedByName);
    $(".basicinfo .info-processedOn .detail").html(app.utils.formatDateTime(data.processedOn, true));

    if (data.id == null) {
      $(".basicinfo .info-id").addClass('d-none');
    }

    if (data.paidOn == null) {
      $(".basicinfo .info-id").addClass('d-none');
    }

    if (data.transactionNo == null) {
      $(".basicinfo .info-transcationNo").addClass('d-none');
    }

    if (data.modeText == null) {
      $(".basicinfo .info-mode").addClass('d-none');
    }

    if (data.name == null) {
      $(".basicinfo .info-name").addClass('d-none');
    }
    
    if (data.methodText == null) {
      $(".basicinfo .info-method").addClass('d-none');
    }

    if (data.statusText == null) {
      $(".basicinfo .info-status").addClass('d-none');
    }

    if (data.processedByName == null) {
      $(".basicinfo .info-processedBy").addClass('d-none');
    }
    
    if (data.processedOn == null) {
      $(".basicinfo .info-processedOn").addClass('d-none');
    }
  }

  function dispose() {
    if (controller) {
      controller.abort();
    }
  }

}).apply(app.page.info = app.page.info || {});