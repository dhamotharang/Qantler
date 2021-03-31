(function () {

  var willApprove;
  var data;
  var reviews;

  var controller;

  var isBusy;

  var email;
  var emailCache = {};
  var emailTemplate;
  var didComposeEmail;

  this.init = function (d, isApprove) {
    data = d;
    didComposeEmail = true;
    willApprove = isApprove;
    reviews = null;
    email = null;
    clearError();

    var title = isApprove ? 'Approve All' : 'Reject All';
    $('#approveModal .modal-title').html(title);
    $('#approveModal #actionButton').html(title);

    if (isApprove) {
      $('#approveModal #actionButton').addClass('btn-outline-primary');
      $('#approveModal #actionButton').removeClass('btn-outline-danger');
    } else {
      $('#approveModal #actionButton').removeClass('btn-outline-primary');
      $('#approveModal #actionButton').addClass('btn-outline-danger');
    }

    $('#approveModal .rejection-email').removeClass('d-flex');

    if (!isApprove) {
      $('#approveModal .rejection-email').addClass('d-flex');
    }

    fetchReviews();
  }

  function fetchReviews() {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Loading. Please wait...');

    var params = '';

    data.forEach((e, i) => {
      if (params.length > 0) {
        params += '&';
      }
      params += 'requestIDs[' + i + ']=' + encodeURI(e.id);
    });

    fetch('/api/request/reviews?' + params, {
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
      .then(r => {
        reviews = r;

        setupView();

        app.dismissProgress();
      })
      .catch(app.http.catch);
  }

  function setupView() {
    var container = $('#approveModal .wrapper');
    container.empty();

    data = data.sort((a, b) => a.id - b.id);

    data.forEach((e, i) => {
      var hasPremise = e.premises && e.premises.length > 0;

      var html = (i > 0 ? '<div class="divider my-4"></div>' : '') +
        '<div class="info">' +
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

      var review = reviews.filter(r => r.requestID == e.id && r.step <= 1)
        .sort((a, b) => moment.utc(b.createdOn).diff(moment.utc(a.createdOn)))[0];

      if (review
        && review.lineItems.length > 0) {
        var lineItems = review.lineItems.sort(li => li.scheme);

        html += '<ul class="bullet-line-list with-number ml-4">';

        lineItems.forEach((li, i) => {

          var action = li.approved ? 'Recommend for Approval' : 'Recommend for Rejection';
          var actionColor = li.approved ? 'text-success' : 'text-danger';

          var badge = review.step == 0 ? 'mdi-badge-account' : 'mdi-account-search';

          html += '<li>' +
            '<span class="processes_icon">' +
            '<i class="mdi ' + badge + '"></i>' +
            '</span>' +
            '<div class="section mb-3">' +
            '<h7 class="mr-3">' + app.common.formatScheme(li.schemeText, li.subSchemeText) + '</h7>' +
            '<span class="' + actionColor + ' font-weight-bold">' + action + '</span>' +
            '</div>';

          if (!app.utils.isNullOrEmpty(li.remarks)) {
            html += '<div class="p-3 bg-inverse-info mb-3">' +
              '<p class="m-0">' + app.utils.escapeHtml(li.remarks) + '</p>' +
              '</div>';
          }

          /* Log information */
          if (i == lineItems.length - 1) {

            html += '<div class="section">' +
              '<p>';

            if (review.grade) {
              html += '<span class="btn btn-outline-dark py-1 px-2 mr-2">' + app.common.gradeLabel(review.grade) + '</span>';
            }

            html += '<span class="text-primary">' + review.reviewerName + '</span>,&nbsp;' +
              app.utils.formatDateTime(review.createdOn) +
              '</p>';

            /* Recommendation made on stage 1 (Site Inspection) */
            if (review.step == 1
              && !app.utils.isNullOrEmpty(review.refID)) {
              html += '<a class="text-right" href="/jobOrder/details/' + review.refID + '">View Findings</a>';
            }

            html += '</div>'
          }

          html += '</li>';
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

    if (!validate()) {
      return;
    }

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    var textarea = $('#approveModal textarea');
    var remarks = textarea.val().trim();

    isBusy = true;

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
          approved: willApprove
        });
      });

      models.push({
        step: 2,
        lineItems: lineItems,
        requestID: e.id,
        email: email
      });

    });

    app.showProgress('Processing. Please wait...');

    fetch('/api/request/approve', {
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
      .catch(err => {
        isBusy = false;
        app.http.catch(err);
      });
  }

  function initRejectEmail() {
    if (!emailTemplate) {
      fetchEmailTemplate();
      return;
    }

    var holder = {};

    var reviewsWithEmail = reviews.filter(e => e.emailID && e.step <= 1)
      .sort((a, b) => moment.utc(b.createdOn).diff(moment.utc(a.createdOn)))
      .filter(e => {
        if (!holder[e.requestID]) {
          holder[e.requestID] = e;
          return true;
        }
        return false;
      }).sort((a, b) => a.id - b.id);

    var tmpEmail = email;

    if (!tmpEmail
      && reviewsWithEmail.length > 0) {

      var unloadedEmails = reviewsWithEmail.filter(e => !emailCache[e.emailID]);
      if (unloadedEmails.length > 0) {
        fetchEmail(unloadedEmails[0].emailID);
        return;
      }

      tmpEmail = app.page.helper.prepareRejectionEmail(emailTemplate, data, Object.values(emailCache));
    }

    if (!tmpEmail) {
      tmpEmail = app.page.helper.prepareRejectionEmail(emailTemplate, data, []);
    }

    app.page.email.init(tmpEmail);
    app.page.email.callback = function (e) {
      email = e;
      didComposeEmail = true;
      $('#approveModal .rejection-email').removeClass('has-danger');
    };
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

  function validate() {
    if (willApprove) {
      return true;
    }

    var textarea = $('#approveModal textarea');
    var remarks = textarea.val().trim();

    if (app.utils.isNullOrEmpty(remarks)) {
      textarea.closest('.form-group').addClass('has-danger');
    }

    if (!email || !didComposeEmail) {
      $('#approveModal .rejection-email').addClass('has-danger');
    }

    return $('#approveModal .has-danger').length == 0;
  }

  function clearError() {
    $('#approveModal .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  }

  $(function () {
    $('#approveModal').on('change input paste', 'textarea', function () {
      var val = $(this).val();

      if (!app.utils.isNullOrEmpty(val)) {
        $(this).closest('.form-group').removeClass('has-danger');
      }
    });

    $('#approveModal #actionButton').click(function () {
      submit();
    });

    $('#approveModal .rejection-email button').click(function () {
      app.page.email.init(emailTemplate);

      initRejectEmail();
    });
  });

}).apply(app.page.approve = app.page.approve || {});