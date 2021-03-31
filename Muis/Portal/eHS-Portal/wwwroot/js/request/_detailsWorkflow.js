(function () {
  var data;

  var didInit;
  var emailCache = {};

  var controller;

  this.init = function (d) {
    data = d;
    initView();
  }

  function initView() {
    if (!data || !data.reviews || data.reviews.length == 0) {
      return;
    }

    $('.workflow').removeClass('d-none');

    var dataSet = data.reviews.sort((a, b) => moment.utc(b.createdOn).diff(moment.utc(a.createdOn)));

    dataSet.forEach(e => {

      var badge = '';

      switch (e.step) {
        case 0:
          badge = 'mdi-badge-account';
          break;
        case 1:
          badge = 'mdi mdi-account-search';
          break;
        case 2:
          badge = 'mdi mdi-account-star';
          break;
        case 3:
          badge = 'mdi-account-multiple-check';
          break;
      }

      var elem = '<li>' +
        '<span class="processes_icon"><i class="mdi ' + badge + '"></i></span>';

      if (e.lineItems.length > 0) {

        var lineItems = e.lineItems.sort((a, b) => a.scheme - b.scheme);

        lineItems.forEach(li => {

          var action = '';
          var actionColor = li.approved ? 'text-success' : 'text-danger';

          switch (e.step) {
            case 0:
            case 1:
            case 2:
              action = li.approved ? 'Recommend for Approval' : 'Recommend for Rejection';
              break;
            case 3:
              action = li.approved ? 'Approved' : 'Rejected';
              break;
          }

          elem += '<div class="section mb-3">' +
            '<h7 class="mr-3">' +
            app.common.formatScheme(li.schemeText, li.subSchemeText) +
            '</h7>' +
            '<span class="' + actionColor + ' font-weight-bold">' + action + '</span>' +
            '</div>';

          if (!app.utils.isNullOrEmpty(li.remarks)) {
            elem += '<div class="p-3 bg-inverse-info mb-3">' +
              '<p class="m-0">' + app.utils.escapeHtml(li.remarks) + '</p>' +
              '</div>';
          }

        });
      }

      /* Log information */
      elem += '<div class="section">' +
        '<p>';

      if (e.grade) {
        elem += '<span class="btn btn-outline-dark py-1 px-2 mr-2">' + app.common.gradeLabel(e.grade) + '</span>';
      }

      elem += '<span class="text-primary">' + e.reviewerName + '</span>,&nbsp;' +
        app.utils.formatDateTime(e.createdOn) +
        '</p>' +
        '<div class="actions">';

      if (e.emailID) {
        var text = e.step >= 3 ? 'View Rejection Email' : 'View Draft Email';
        elem += '<a class="text-right view-email" data-step="' + e.step + '" data-id="' + e.emailID + '" href="javascript:void(0);"  data-toggle="modal" data-target="#viewEmail" >' + text + '</a>';
      }

      /* Recommendation made on stage 1 (Site Inspection) */
      if (e.step == 1
        && !app.utils.isNullOrEmpty(e.refID)) {
        elem += '<a class="text-right ml-3" href="/jobOrder/details/' + e.refID + '">View Findings</a>';
      }

      elem += '</div>' +
        '</div>' +
        '</li>';

      $('.workflow .bullet-line-list').append(elem);
    });
  }

  function loadEmail(id, step) {
    var cache = emailCache[id];

    var title = step != 3 ? 'Draft Rejection Email' : 'Rejection Email';

    var options;
    if (step != 3) {
      options = {
        fields: 'body'
      }
    }

    app.page.email.readonly.init(title, options);

    if (!cache) {
      fetchEmail(id, step);
      return;
    }

    app.page.email.readonly.email(cache);
  }

  function fetchEmail(id, step) {
    if(controller) {
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
        loadEmail(id, step);

      }).catch(app.http.catch)
  }

  $(function () {
    $('.workflow').on('click', '.view-email', function () {
      var id = $(this).data('id');
      var step = $(this).data('step');
      loadEmail(id, step);
    });
  });

}).apply(app.page.workflow = app.page.workflow || {});