(function () {

  var controller;

  var id;
  var data = [];

  this.init = function (d) {
    id = d;
    fetchRelated(id);
  }

  function fetchRelated(id) {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    fetch('/api/request/' + id + '/related', {
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
        data = res;

        invalidate();

        $('.widget.related .grid-margin.stretch-card').addClass('d-none');

      }).catch(app.http.catch);
  }

  function invalidate() {
    if (!data
      || data.length === 0) {

      $('.widget.related .widget-placeholder').removeClass('d-none');
      $('.widget.related .related-container').addClass('d-none');

      return;
    }

    var template = $('.related-request-template .tickets-card');
    var itemTemplate = $('.related-request-template > .info');
    var container = $('.related-container');

    container.empty();
    data.forEach((e, index) => {

      var schemes = '';

      if (e.lineItems) {
        e.lineItems.forEach(s => {
          schemes += itemTemplate.clone().prop('outerHTML')
            .replaceAll('{icon}', 'mdi-hand-pointing-right')
            .replaceAll('{text}', s.schemeText)
        });
      }

      var premiseText = '';
      if (e.premises
        && e.premises.length > 0) {
        premiseText = app.utils.formatPremise(e.premises[0]);
      }

      container.append(template.clone()
        .prop('outerHTML')
        .replaceAll('{first}', index === 0 ? 'first' : '')
        .replaceAll('{status}', e.statusText)
        .replaceAll('{id}', e.id)
        .replaceAll('{type}', e.typeText)
        .replaceAll('{schemes}', schemes)
        .replaceAll('{premisetext}', premiseText)
        .replaceAll('{assignto}', e.assignedToName)
        .replaceAll('{submittedon}', app.utils.formatDateTime(e.submittedOn))
        .replaceAll('{express}', e.pxpedite ? '' : 'd-none')
        .replaceAll('{assigndone}', !e.assignedToName ? 'd-none' : '')
        .replaceAll('{refid}', e.refID));
    });
  }
}).apply(app.page.relatedrequests = app.page.relatedrequests || {});