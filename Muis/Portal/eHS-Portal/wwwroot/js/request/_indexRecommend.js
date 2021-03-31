(function () {

  var data;
  var isBusy;
  var controller;

  this.init = function (d) {
    data = d;
    setupView();
  }

  function setupView() {
    var container = $('#recommendModal .wrapper');
    container.empty();

    data = data.sort((a, b) => a.id - b.id);

    data.forEach((e, i) => {
      var hasPremise = e.premises && e.premises.length > 0;

      var html = (i > 0 ? '<div class="divider my-5"></div>' : '') +
        '<div class="info mb-4">' +
        '<div class="id mr-2"><p class="font-weight-bold">' + e.id + '</p></div>' +
        '<div class="font-weight-bold mr-5"><p>' + e.typeText + '</p></div>' +
        '<div class="customer mr-5"><p><strong>' + e.customerCode + '</strong>, ' + app.utils.titleCase(e.customerName) + '</p></div>';


      if (hasPremise) {
        html += '<div class="premise mr-3"><p>' + app.utils.formatPremise(e.premises[0]) + '</p></div>';
      }

      html += '<div class="action">' +
        '<a class="btn btn-inverse-primary btn-rounded" href="/request/details/' + e.id + '">View Details</a>' +
        '</div>' +
        '</div>';

      if (hasPremise) {
        html += '<div class="premise-long mb-4"><p>' + app.utils.formatPremise(e.premises[0]) + '</p></div>';
      }

      if (e.lineItems.length > 0) {
        var lineItems = e.lineItems.sort(li => li.scheme);

        html += '<ul class="bullet-line-list with-number ml-4">';

        lineItems.forEach(li => {
          html += '<li>' +
            '<span class="processes_icon">' +
            '<i class="mdi mdi-badge-account"></i>' +
            '</span>' +
            '<h7 class="mb-3">' + app.common.formatScheme(li.schemeText, li.subSchemeText) + '</h7>' +
            '</li>';
        });

        html += '</ul>';
      }

      container.append(html);
    });
  }

  function submit() {
    if (isBusy) {
      return;
    }
    isBusy = true;

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    var remarks = $('#recommendModal textarea').val();

    var models = [];

    data.forEach(e => {

      var lineItems = [];

      e.lineItems = e.lineItems.sort((a, b) => a.scheme - b.scheme);

      e.lineItems.forEach((li, i) => {

        var liRemarks = remarks;
        if (i < e.lineItems.length - 1) {
          liRemarks = '<all>';
        }

        lineItems.push({
          scheme: li.scheme,
          subScheme: li.subScheme,
          remarks: liRemarks,
          approved: true
        });
      });

      models.push({
        step: 0,
        lineItems: lineItems,
        requestID: e.id
      });

    });

    app.showProgress('Submitting recommendation. Please wait...');

    fetch('/api/request/recommend', {
      method: 'POST',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(models)
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(r => {
        location.reload(true);
      })
      .catch(error => {
        isBusy = false;

        app.http.catch(error);
      });
  }

  $(function () {

    $('#recommendModal #recommendAllButton').click(function () {
      submit();
    });

  });

}).apply(app.page.recommend = app.page.recommend || {});