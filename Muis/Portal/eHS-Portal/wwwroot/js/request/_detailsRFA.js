(function () {
  'use strict';

  var data = [];
  var current;
  var controller;

  this.init = function (d) {
    data = d;
    invalidate();
  };

  this.getCurrent = function () {
    return current;
  }

  function invalidate() {
    if (!data || data.length === 0) {
      $('.widget.rfa .widget-placeholder').removeClass('d-none');
      return;
    }

    var container = $(".widget.rfa .container");

    container.empty();

    data = data.sort((a, b) => moment.utc(b.createdOn).diff(moment.utc(a.createdOn)));

    data.forEach((e, index) => {
      var isDraft = e.status == 1;

      var elem = '<a data-id="' + e.id + '" data-status="' + e.status + '" class="tickets-card row ' + (index === 0 ? 'first' : '') + '" data-toggle="modal" data-target="' + (isDraft ? '#newRfaModal' : '#rfaModal') + '">' +
        '<div class="tickets-details col-md-12">' +
        '<div class="wrapper">' +
        '<div class="badge badge-' + app.utils.rfaStatusColor(e.status) + '">' + e.statusText + '</div>' +
        '<span class="text-muted">#' + e.id + '</span>' +
        '</div>' +
        '<div class="wrapper text-muted info">' +
        '<span><i class="mdi mdi-account-outline"></i>' + app.utils.userContext(e.raisedBy, e.raisedByName) + '</span>' +
        '</div>' +
        '<div class="wrapper text-muted info">' +
        '<span><i class="mdi mdi-clock-outline"></i>' + app.utils.timeAgo(e.createdOn) + '</span>' +
        '</div>';

      if (e.dueOn) {
        elem += '<div class="wrapper text-muted info">' +
          '<span><i class="mdi mdi-clock"></i>' + app.utils.formatDate(e.dueOn) + '</span>' +
          '</div>';
      }

      elem += '</div>' +
        '</a>';

      container.append(elem);
    });
  };

  this.show = function (id) {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    fetch('/api/rfa/' + id, {
      method: 'GET',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(r => {
        current = r;
        initModalView();
      })
      .catch(app.http.catch);

    return true;
  };

  function initModalView() {
    if (!current) {
      return;
    }

    $('#rfaModalLabel').html('RFA #' + current.id);

    initModalInfoView();
    initModalContent();
    invalidateActivities();

    app.utils.hide($('#rfaCloseButton'));
    app.utils.hide($('.rfaModal .modal-footer'));

    if (current.status == 100) {
      app.utils.show($('.rfaModal .modal-footer'));
    } else if (current.status == 200) {
      app.utils.show($('#rfaCloseButton'));
      app.utils.show($('.rfaModal .modal-footer'));
    }
  };

  function initModalInfoView() {
    $('.info.status').html('<span><i class="mdi mdi-information-outline"></i><span class"text-' + app.utils.rfaStatusColor(current.status) + '">' + current.statusText + '<span></span>');
    $('.info.managedBy').html('<span id=""><i class="mdi mdi-account-outline"></i>' + app.utils.userContext(current.raisedByID, current.raisedByName) + '</span>');
    $('.info.createdOn').html('<span id=""><i class="mdi mdi-clock-outline"></i>' + app.utils.timeAgo(current.createdOn) + '</span>');
    $('.info.dueOn').html('<span id=""><i class="mdi mdi-clock"></i>' + app.utils.formatDate(current.dueOn, true) + '</span>');
  }

  function initModalContent() {
    var tabContainer = $('#rfaModal .modal-header .nav');
    var contentContainer = $('#rfaModal .modal-body .tab-content');
    var responseContainer = $('.response-attachments');
    var responseItemContainer = responseContainer.find('.attachments ul');

    responseContainer.addClass('d-none');

    tabContainer.empty();
    contentContainer.empty();

    responseItemContainer.empty();

    var lineItems = current.lineItems.sort((a, b) => {
      var result = a.scheme - b.scheme;

      if (result == 0) {
        result = a.checklistCategoryID - b.checklistCategoryID;
      }

      if (  (a.checklistCategoryID != -1 && b.checklistCategoryID == -1)
         || (a.checklistCategoryID == -1 && b.checklistCategoryID != -1)) {
        return a.checklistCategoryID == -1 ? 1 : -1;
      }

      if (result == 0) {
        result = a.checklistID - b.checklistID;
      }
      return result;
    });

    var schemes = [];
    var dataSet = {};

    lineItems.forEach((e, i) => {
      if (!schemes.includes(e.scheme)) {
        schemes.push(e.scheme);
        dataSet[e.scheme] = [];
      }

      dataSet[e.scheme].push(e);
    });

    schemes.forEach((e, i) => {
      var data = dataSet[e];

      initTab(tabContainer, data[0], i);
      initContent(contentContainer, responseItemContainer, data, i);
    });

    if (responseItemContainer.children().length > 0) {
      responseContainer.removeClass('d-none');
    }
  }

  function initTab(container, d, i) {
    var elem = '<li class="nav-item">' +
      '<a class="nav-link ' + (i === 0 ? 'active' : '') + '" data-toggle="pill"' + '" data-scheme="' + d.scheme + '"' +
      ' href= "#scheme' + d.scheme + '" role="tab">' + d.schemeText + '</a>' +
      '</li>';

    container.append(elem);
  }

  function initContent(container, responseContainer, d, i) {

    var elem = '<div class="tab-pane fade ' + (i === 0 ? 'show active' : '') + '" id="scheme' + d[0].scheme + '" data-scheme="' + d[0].scheme + '"' + '>' +
      '<div class="row">' +
      '<div class="col-lg-12 left">' +
      '<ul class="lineitem-list">';

    var currCategory;
    var responseAttachments = '';

    d.forEach(e => {

      var isOthers = e.checklistCategoryID == -1;

      if (currCategory != e.checklistCategoryID) {
        var header = isOthers ? e.checklistCategoryText : (e.checklistCategoryID + ' ' + e.checklistCategoryText);

        elem += '<li class="list header">' +
          '<h5 class="font-weight-bold"> ' + header + '</h5>' +
          '</li>';

        currCategory = e.checklistCategoryID;
      }

      elem += '<li class="list item ' + (isOthers ? 'others' : '') + '">';

      if (!isOthers) {
        elem += '<div class="compliance">' +
          '<p class="font-weight-bold">' + e.checklistCategoryID + '.' + e.checklistID + '</p>' +
          '<div class="compliance-body">' +
          e.checklistText +
          '</div>' +
          '</div>';
      }

      elem += '<div class="remarks">' +
        (e.remarks ? e.remarks : '') +
        '<div class="attachments">' +
        '<ul>';

      if (e.attachments && e.attachments.length > 0) {
        e.attachments.forEach(a => {
          elem += '<li>' +
            '<div class="thumb" >' +
            '<i class="mdi ' + app.utils.fileExtensionIcon(a.extension) + '"></i>' +
            '</div> ' +
            '<div class="details">' +
            '<p class="file-name">' + a.fileName + '</p> ' +
            '<div class="buttons">' +
            '<p class="file-size">' + app.utils.formatFileSize(a.size) + '</p>' +
            '<a href="/api/file/' + a.fileID + '?fileName=' + encodeURI(a.fileName) + '" class="download" download="' + a.fileName + '">Download</a>' +
            '</div> ' +
            '</div> ' +
            '</li>';
        });
      }

      elem += '</ul></div></div>';

      if (e.replies && e.replies.length > 0) {
        e.replies.forEach(r => {
          elem += '<div class="reply">' +
            '<div class="remarks">' +
            r.text +
            '</div>';

          if (r.attachments && r.attachments.length > 0) {
            elem += '<div class="attachments"><ul>';

            r.attachments.forEach(ra => {
              responseAttachments += '<li>' +
                '<div class="thumb" >' +
                '<i class="mdi ' + app.utils.fileExtensionIcon(ra.extension) + '"></i>' +
                '</div > ' +
                '<div class="details">' +
                '<p class="file-name">' + ra.fileName + '</p> ' +
                '<div class="buttons">' +
                '<p class="file-size">' + app.utils.formatFileSize(ra.size) + '</p> ' +
                '<a href="/api/file/' + ra.fileID + '?fileName=' + encodeURI(ra.fileName) + '" class="download" download>Download</a> ' +
                '</div> ' +
                '</div> ' +
                '</li>';
            });

            elem += '</ul></div>';
          }
        });
      }

      elem += '</li>';
    });

    elem += '</ul></div></div></div>';

    container.append(elem);

    if (responseAttachments) {
      responseContainer.append(responseAttachments);
    }
  }

  function invalidateActivities() {
    if (!current.logs || current.logs.length === 0) {
      return;
    }

    $('.rfaModal .activities').removeClass('d-none');

    var container = $(".rfaModal .activities .timeline");
    container.empty();

    var logs = current.logs.sort((a, b) => moment.utc(b.createdOn).diff(moment.utc(a.createdOn)));

    logs.forEach(e => {
      var elem = app.common.createLogElement(app.utils.userContext(e.userID, e.userName),
        formatLog(e),
        e.notes,
        app.utils.timeAgo(e.createdOn));

      container.append(elem);
    });
  }

  function formatLog(e) {
    var result = e.action;
    if (e.params) {
      var i;
      for (i = 0; i < e.params.length; i++) {
        var val = e.params[i];

        var tryDateTime = moment.utc(val);
        if (tryDateTime.isValid()) {
          val = app.utils.formatDate(tryDateTime, true);
        }

        result = result.replace("{" + i + "}", '<b>' + val + '</b>');
      }
    }

    return result;
  }


  this.close = function () {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Processing. Please wait...');

    fetch('/api/rfa/' + current.id + '/close', {
      method: 'POST',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(r => {
        location.reload(true);
      })
      .catch(app.http.catch);
  }

}).apply(app.page.rfa = app.page.rfa || {});

$(document).ready(function () {
  $(".rfa .container").on('click', 'a', function () {
    var id = $(this).data('id');
    var status = $(this).data('status');
    if (status === 1) {
      var checklistIDs = app.page.getData().lineItems.map(e => e.checklistHistoryID);
      app.page.rfa.new.showFromExisting(checklistIDs, id);
    } else {
      app.page.rfa.show($(this).data('id'));
    }
  });

  $('#rfaCloseButton').click(function () {
    app.page.rfa.close();
  });
});