(function () {

  var data;

  this.init = function (d) {
    data = d;
    invalidate();
  }

  function invalidate() {
    invalidateCerticateDetails();
    invalidateHalalTeam();
    invalidatePremises();

    app.utils.show($('#main-tab-menu'));

    app.page.main.history.init(data.id);

    app.utils.show($('#main-tab-ingredients'));
  };

  function invalidateCerticateDetails() {
    var container = $('#certificate-details-container');

    updateLabel(container, 'number', data.number);
    updateLabel(container, 'status', data.statusText);
    updateLabel(container, 'scheme', data.schemeText);
    updateLabel(container, 'subscheme', data.subSchemeText);
    updateLabel(container, 'serialNo', data.serialNo);

    if (data.suspendedUntil) {
      updateLabel(container, 'suspendeduntil', app.utils.formatDate(data.suspendedUntil));
    }

    if (data.issuedOn) {
      updateLabel(container, 'issuedon', app.utils.formatDate(data.issuedOn));
    }

    if (data.expiresOn) {
      updateLabel(container, 'expireson', app.utils.formatDate(data.expiresOn));
    }

    updateLabelHide('.number', data.number);
    updateLabelHide('.status', data.statusText);
    updateLabelHide('.scheme', data.schemeText);
    updateLabelHide('.subscheme', data.subSchemeText);
    updateLabelHide('.serialNo', data.serialNo);
    updateLabelHide('.suspendeduntil', data.suspendedUntil);
    updateLabelHide('.issuedon', data.issuedOn);
    updateLabelHide('.expireson', data.expiresOn);
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
    if (!data.premise) {
      return;
    }

    $('.premises').removeClass('d-none');

    var container = $('.premises .container');
    container.empty();

    var elem = '<div class="list-item d-flex align-items-start premise">' +
      '<div class="badge ' + (data.premise.isPrimary ? 'badge-primary' : 'badge-outline-dark') + '"> ' + data.premise.typeText + ' </div>' +
      '<p class="ml-2">' + app.utils.formatPremise(data.premise) + '</p>' +
      '</div >';

    container.append(elem);
  }

  function updateLabel(container, name, value) {
    if (value) {
      app.utils.show(container.find('.' + name));
      container.find('#' + name).html(value);
    }
  }

  function updateLabelHide(name, value) {
    if (!value) {
      $(name).addClass('d-none');
    }
  }

  $("#main-tab-menu").on("click", function () {
    app.page.main.menu.init(data.id);
  });

  $("#main-tab-ingredients").on("click", function () {
    app.page.main.ingredients.init(data.id);
  });

}).apply(app.page.main = app.page.main || {});