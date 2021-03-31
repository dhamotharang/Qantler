(function (self) {

  var controller;
  var id;
  var dataset = [];

  self.init = function (d) {
    id = d;

    fetchData();
  }

  function showLoader() {
    $('.premise-certificate').each(function () {
      $(this).addClass('busy');
    });
  }

  function hideLoader() {
    $('.premise-certificate').each(function () {
      $(this).removeClass('busy');
    });
  }

  function prepareData(d) {
    dataset = d;

    if (!dataset
      || dataset.length === 0) {

      $('.premises .widget-placeholder').removeClass('d-none');
      $('.premises .premise-container').addClass('d-none');

      return;
    }

    dataset = dataset.sort((a, b) => a.premiseID - b.premiseID);

    var container = $('.premises .container');
    var premiseTemplate = $('.certificate-container-template .certificate-container');
    var itemTemplate = $('.certificate-container-template .certificate-item');

    container.empty();

    var lastPremise = -1;


    var itemContainer;

    dataset.forEach((e, i) => {

      if (lastPremise != e.premiseID) {

        var elem = $(premiseTemplate.clone()
          .prop('outerHTML')
          .replaceAll('{premisetext}', app.utils.formatPremise(e.premise)));

        itemContainer = elem.find('.certificate-list');

        container.append(elem);

        lastPremise = e.premiseID;
      }

      itemContainer.append(itemTemplate.clone()
        .prop('outerHTML')
        .replaceAll('{id}', e.id)
        .replaceAll('{number}', e.number)
        .replaceAll('{scheme}', e.schemeText)
        .replaceAll('{status}', e.statusText)
        .replaceAll('{status-color}', statusColour(e.status))
        .replaceAll('{untilon}', app.utils.formatDateTime(e.suspendedUntil))
        .replaceAll('{display-until}', e.suspendedUntil ? '' : 'd-none')
        .replaceAll('{issuedon}', app.utils.formatDateTime(e.issuedOn))
        .replaceAll('{display-issuedon}', e.issuedOn ? '' : 'd-none')
        .replaceAll('{expireson}', app.utils.formatDateTime(e.expiresOn))
        .replaceAll('{display-expires}', e.expiresOn ? '' : 'd-none'));
    });
  }

  function statusColour(status) {
    switch (status) {
      case 100: // active
        return 'badge-outline-success';
      case 400: // expired
        return 'badge-outline-danger'
      case 500: // suspended
        return 'badge-outline-warning';
      default:
        return 'badge-outline-success';
    }
  }

  function fetchData() {
    if (controller) {
      controller.abot();
    }
    controller = new AbortController();

    showLoader();

    fetch('/api/customer/' + id + '/premise', {
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
        prepareData(res);
        hideLoader();
      })
      .catch(err => {
        hideLoader();
      });
  }
})(app.page.premise = app.page.premise || {});