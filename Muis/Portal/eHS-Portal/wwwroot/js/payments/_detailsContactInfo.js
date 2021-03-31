(function () {
  'use strict'

  var controller;

  this.init = function (data) {
    if (!data) {
      $('#contactInfo').addClass('d-none');
      return;
    }

    render(data);
  }

  function render(data) {

    var infoTemplate = $('.payment-template .label');
    var contactTemplate = $('.payment-template .contact');

    $('#contactinfo').removeClass('d-none');
    $("#contactName").html(data.name);

    $("#contactInfos").empty();
    if (data.contactInfos) {
      data.contactInfos.forEach(e => {

        $("#contactInfos").append(contactTemplate.prop('outerHTML')
          .replaceAll('{label}', e.typeText)
          .replaceAll('{value}', '   ' + e.value)
          .replaceAll('{bg}', e.isPrimary ? 'badge-primary' : 'badge-outline-dark'));
      });
    }
  }

  function dispose() {
    if (controller) {
      controller.abort();
    }
  }

}).apply(app.page.contactinfo = app.page.contactinfo || {});