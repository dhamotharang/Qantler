(function () {

  var data;

  var controller;

  this.init = function (d) {
    data = d;
    invalidate();
    loadMenu();
  }

  function loadMenu() {
    $.each(data.lineItems, function (index, value) {
      $('#main-tab-details').parent().after($('#addmenu').html()
        .replaceAll("{dataId}", value.scheme)
        .replaceAll("{scheme}", app.common.schemeInitial(value.schemeText)));
    });

  }

  function invalidate() {
    invalidateRequestDetails();
    invalidateCustomerDetails();
    invalidateLineItems();
    invalidateHalalTeam();
    invalidatePremises();
    invalidateAttachments();

    app.page.workflow.init(data);

    var allowEdit = data.status == 200
      && (data.assignedTo == app.identity.id
        || app.hasPermission(7));

    if (data.menus
      && app.page.main.menu) {

      app.page.main.menu.init(data.menus, allowEdit);

      app.utils.show($('#main-tab-menu'));
    }

    if (data.ingredients
      && data.ingredients.length > 0
      && app.page.main.ingredients) {

      app.utils.show($('#main-tab-ingredients'));

      app.page.main.ingredients.init(data.ingredients, allowEdit);
    }

    if (data.type == 0
      || (data.type == 1 && ((data.status >= 500 && data.status <= 900) || data.status == 1300))
      || data.type == 3
      || data.type == 4) {
      app.utils.show($('#main-tab-finance'));
    }
  };

  function invalidateRequestDetails() {
    var container = $('#request-details-container');

    container.find('.card-title').html('Request Details' + (data.expedite ? '&nbsp;<span class="badge badge-outline-danger">Express</span>' : ''));

    updateLabel(container, 'id', data.id);
    updateLabel(container, 'refid', data.refID);
    updateLabel(container, 'status', data.statusText);
    updateLabel(container, 'type', data.typeText);

    if (data.characteristics && data.characteristics.length > 0) {
      $('#request-details-container .info.characteristics').removeClass('d-none');

      var charLeft = $('#request-details-container .characteristics-left');
      var charRight = $('#request-details-container .characteristics-right');

      data.characteristics.forEach((e, i) => {
        if (e.type != 16) {
          addCharacteristics(i % 2 === 0 ? charLeft : charRight, e);
        }
      });
    }

    updateLabel(container, 'source', 'Licence One');
    updateLabel(container, 'submittedon', app.utils.formatDate(data.submittedOn));

    if (data.dueOn) {
      updateLabel(container, 'dueon', app.utils.formatDate(data.dueOn));
    }

    updateLabel(container, 'assignedto', data.assignedToName);

    updateLabel(container, 'noOfCopies', getCharValue(data.characteristics, 4));
  }

  function invalidateLineItems() {
    if (!data.lineItems || data.lineItems.length === 0) {
      return;
    }
    $('#request-details-container .info.lineitems').removeClass('d-none');

    var container = $('#request-details-container .info.lineitems .container');

    var elem = '';

    data.lineItems.forEach((e, i) => {
      var name = e.schemeText;
      if (e.subSchemeText) {
        name += ' (' + e.subSchemeText + ')';
      }

      elem += '<div class="row pt-2">' +
        '<div class="col-6">' + name + '</div>';

      if (e.characteristics && e.characteristics.length > 0) {
        elem += '<div class="col-6">';

        e.characteristics.forEach(ec => {
          elem += '<p class="mb-1">' +
            '<strong>' + ec.typeText + '&nbsp;:&nbsp;</strong>' +
            '<span>' + formatCharacteristicsValue(ec) + '</span>' +
            '</p>';
        });

        elem += '</div>';
      }

      if (i < data.lineItems.length - 1) {
        elem += '<div class="col-12">' +
          '<div class="border-bottom mt-2" ></div>' +
          '</div>';
      }

      elem += '</div>';
    });

    container.append(elem);
  }

  function formatCharacteristicsValue(char) {
    var val = char.value;
    switch (char.type) {
      case 0:
        val = val + (val == 1 ? ' year' : ' years');
        break;
    }
    return val;
  }

  function addCharacteristics(container, char) {

    container.append('<p class="mb-1 duration"><strong>' + app.utils.titleCase(char.typeText) + '&nbsp;:&nbsp;</strong><span id="duration">' + formatCharacteristicsValue(char) + '</span></p>')
  }

  function invalidateCustomerDetails() {

    var customerCode = data.customer.code;
    var groupCode = data.customer.groupCode;

    var codeText = customerCode ? ('<b>' + customerCode.value + '</b>, ' + customerCode.text) : 'No code assigned';
    var groupCodeText = groupCode ? ('<b>' + groupCode.value + '</b>, ' + groupCode.text) : 'No group code assigned';

    var parent = 'No parent assigned';
    if (data.customer.parent
      && data.customer.parent.name) {
      parent = app.utils.titleCase(data.customer.parent.name);
    }
    var office = 'No officer assigned';
    if (data.customer.officer
      && data.customer.officer.name) {
      office = app.utils.titleCase(data.customer.officer.name);
    }

    $('.customer-info #code').html(codeText);
    $('.customer-info #code-group').html(groupCodeText);
    $('.customer-info #parent-code').html(parent);
    $('.customer-info #officer-code').html(office);

    if (data.status < 500) {
      $('#codeBtn').removeClass('d-none');
      $('#groupCodeBtn').removeClass('d-none');
      $('#parentBtn').removeClass('d-none');
      $('#officerBtn').removeClass('d-none');
    }

    $('.customer-info #name').html(app.utils.titleCase(data.customer.name));
    $('.customer-info #altid').html(data.customer.altID);
    $('.customer-info #licence-no').html(data.customer.altID);
    $('.customer-info #licence-issuedon').html(app.utils.formatDate(data.customer.licenceIssuedOn));
    $('.customer-info #licence-expireson').html(app.utils.formatDate(data.customer.licenceExpiresOn));

    

    if (data.customer.certificates) {
      updateCertificateInfo($('.customer-info'), data.customer)
    }

    if (data.agent) {
      updatePersonInfo($('.filer-info'), data.agent);

      if (data.requestor) {
        updatePersonInfo($('.applicant-info'), data.requestor);
      }

    } else {
      updatePersonInfo($('.filer-info'), data.requestor);
      app.utils.hide($('.applicant-root'));
    }
  }

  function updateCertificateInfo(container, data) {
    var certificateInfos = container.find('.customer-certificates');
    certificateInfos.empty();

    if (data.certificates && data.certificates.length > 0) {

      data.certificates.forEach(ce => {
        var elem = '<p class="mb-1"><strong>License # : </strong>' + ce.number + '</p>' +
          '<p class="mb-1"><strong>Date : </strong>' + app.utils.formatDate(ce.issuedOn) + '</p>' +
          '<p class="mb-1"><strong>Expiry : </strong>' + app.utils.formatDate(ce.expiresOn) + '</p>';
        certificateInfos.append(elem);
      });
    }
  }

  function updatePersonInfo(container, data) {
    container.find('#name').html(app.utils.titleCase((data.salutation ?? '') + ' ' + data.name));
    container.find('#idtype').html(data.idTypeText);
    container.find('#altid').html(data.altID);

    if (data.nationality) {
      app.utils.show(container.find(".nationality"));
      container.find('#nationality').html(app.utils.titleCase(data.nationality));
    }

    if (data.gender) {
      app.utils.show(container.find(".gender"));
      container.find('#gender').html(data.gender);
    }

    if (data.dob) {
      app.utils.show(container.find(".dob"));
      container.find('#dob').html(app.utils.formatDate(data.dob));
    }

    var contactInfos = container.find('.customer-other');
    contactInfos.empty();

    if (data.contactInfos && data.contactInfos.length > 0) {
      contactInfos.append('<p class="mb-1"><strong>Contact Details </strong></p>');

      data.contactInfos.forEach(ci => {
        contactInfos.append('<p class="mb-1"><span class="badge ' + (ci.isPrimary ? 'badge-primary' : 'badge-outline-dark') + '">' + ci.typeText + '</span>&nbsp;&nbsp;' + ci.value + '</p>');
      });
    }
  }

  function invalidateHalalTeam() {
    if (!data.teams || data.teams.length === 0) {
      return;
    }

    $('.halal-team').removeClass('d-none');

    var container = $('.halal-team .row');
    container.empty();

    data.teams.forEach(e => {
      container.append($('#addHalalTeam').html().replaceAll("{name}", app.utils.initials(e.name))
        .replaceAll("{u_name}", app.utils.titleCase(e.name))
        .replaceAll("{designation}", app.utils.titleCase(e.designation))
        .replaceAll("{isCertified}", app.utils.certified(e.isCertified))
        .replaceAll("{joinedOn}", app.utils.formatDate(e.joinedOn))
        .replaceAll("{altID}", e.altID)
        .replaceAll("{color}", app.utils.seriesColor()));
    });
  }

  function invalidatePremises() {
    if (!data.premises || data.premises.length === 0) {
      return;
    }

    $('.premises').removeClass('d-none');

    var container = $('.premises .container');
    container.empty();

    var premises = data.premises.sort((a, b) => {
      if (a.isPrimary) return -1;
      if (b.isPrimary) return 1;
      return a.type - b.type;
    });

    premises.forEach(e => {
      var elem = '<div class="list-item d-flex align-items-start premise">' +
        '<div class="badge ' + (e.isPrimary ? 'badge-primary' : 'badge-outline-dark') + '"> ' + e.typeText + ' </div>' +
        '<p class="ml-2">' + app.utils.formatPremise(e, true) + '</p>' +
        '</div >';

      container.append(elem);
    });
  }

  function invalidateAttachments() {
    if (!data.attachments || data.attachments.length === 0) {
      return;
    }

    $('.supporting-documents').removeClass('d-none');

    var container = $('.supporting-documents .container');
    container.empty();

    data.attachments.forEach(e => {
      var elem = '<div class="list-item">' +
        '<div class="d-flex align-items-center justify-content-center img-sm badge-inverse-primary">' +
        '<i class="mdi ' + app.utils.fileExtensionIcon(e.extension) + ' icon-sm"></i>' +
        '</div>' +
        '<div class="wrapper pl-3 w-100">' +
        '<h6 class="mb-0">' + e.fileName + '</h6>' +
        '<div class="d-flex">' +
        '<p class="text-muted mb-0">' + app.utils.formatFileSize(e.size) + '</p>' +
        '<a href="/api/file/' + e.fileID + '?filename=' + e.fileName + '" download class="ml-auto">' +
        '<i data-id="' + e.fileID + '" data-filename="' + e.fileName + '" class="mdi mdi-arrow-down-bold text-muted ml-auto download"></i>' +
        '</a>' +
        '</div>' +
        '</div>' +
        '</div>';

      container.append(elem);
    });
  }

  function updateLabel(container, name, value) {
    if (value) {
      app.utils.show(container.find('.' + name));
      container.find('#' + name).html(value);
    }
  }

  function getCharValue(chars, type) {
    var obj = chars.find(e => e.type === type);
    return obj ? obj.value : null;
  }

  function initSelectCode(type) {
    var skip = [];

    if (type == 0 && data.customer.code) {
      skip.push(data.customer.code.id);
    } else if (type == 1 && data.customer.groupCode) {
      skip.push(data.customer.groupCode.id);
    }

    app.select.code.init({
      type: type,
      skip: skip,
      defaults: {
        text: app.utils.titleCase(data.customer.name)
      }
    });

    app.select.code.create.onCodeCreated = function (c) {
      app.select.code.reset();
      app.select.code.dismiss();
      app.select.code.create.dismiss();
      setCustomerCode(c);
    }

    app.select.code.onSelect = setCustomerCode;
  }

  function setCustomerCode(c) {

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Updating. Please wait');

    fetch('/api/customer/' + data.customer.id + '/code', {
      method: 'PUT',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      body: JSON.stringify(c),
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(res => {
        app.dismissProgress();

        if (c.type == 0) {
          data.customer.code = c;
        } else {
          data.customer.groupCode = c;
        }
        invalidateCustomerDetails();

      }).catch(app.http.catch);
  }

  function setParent(c) {

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Updating. Please wait');

    fetch('/api/customer/' + data.customer.id + '/parent/' + c.id, {
      method: 'PUT',
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

        data.customer.parent = c;
        invalidateCustomerDetails();

      }).catch(app.http.catch);
  }

  $("#main-tab-menu").on("click", function () {

    $('#dropdown-menu-menu').removeClass('d-none');
    $('#dropdown-menu-ingredient').addClass('d-none');
  });

  $("#main-tab-ingredients").on("click", function () {

    $('#dropdown-menu-ingredient').removeClass('d-none');
    $('#dropdown-menu-menu').addClass('d-none');
  });

  $(function () {
    $('.customer-other button[id="codeBtn"]').click(function () {
      initSelectCode(0);
    });

    $('.customer-other button[id="groupCodeBtn"]').click(function () {
      initSelectCode(1);
    });

    $('.customer-other button[id="officerBtn"]').click(function () {
      app.page.officer.init(data.customer, function (o) {
        var office = 'No officer assigned';
        if (o
          && o.name) {
          office = app.utils.titleCase(o.name);
        }
        $('.customer-info #officer-code').html(office);
      });
    });

    $('.customer-other button[id="parentBtn"]').click(function () {
      var skip = [];
      if (data.customer.parent) {
        skip.push(data.customer.parent.id);
      }

      skip.push(data.customerID);

      app.select.customer.init({
        skip: skip,
        title: 'Select Parent'
      });

      app.select.customer.onSelect = function (c) {
        setParent(c);
      };
    });
  });

}).apply(app.page.main = app.page.main || {});