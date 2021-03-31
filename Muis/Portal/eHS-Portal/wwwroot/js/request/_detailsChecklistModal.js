(function () {

  var properties = [];
  var data = null;
  var controller;

  this.show = function (p) {
    properties = p;
    setup();
  }

  function clear() {
    var tabContainer = $('.checklist-modal .modal-header .nav');
    var contentContainer = $('.checklist-modal .modal-body .tab-content');

    tabContainer.empty();
    contentContainer.empty();
  }

  function setup() {
    clear();

    if (!data) {
      fetchData();
      return;
    }

    var tabContainer = $('.checklist-modal .modal-header .nav');
    var contentContainer = $('.checklist-modal .modal-body .tab-content');

    data.forEach((e, i) => {
      initTab(tabContainer, e, i);
      initContent(contentContainer, e, i);
    });
  }

  function initTab(container, d, i) {
    var elem = '<li class="nav-item">' +
      '<a class="nav-link ' + (i === 0 ? 'active' : '') + '" data-toggle="pill"' + '" data-scheme="' + d.scheme + '"' +
      ' href= "#scheme' + d.scheme + '" role="tab">' + d.schemeText + '</a>' +
      '</li>';

    container.append(elem);
  }

  function initContent(container, d, i) {
    var elem = '<div class="tab-pane fade ' + (i === 0 ? 'show active' : '') + '" id="scheme' + d.scheme + '" data-scheme="' + d.scheme + '"' + '>' +
      '<div class="row">' +
      '<div class="col-lg-12 left">' +
      '<ul class="lineitem-list">';

    d.categories.forEach(c => {
      elem += '<li class="list header">' +
        '<h5 class="font-weight-bold"> ' + c.index + ' ' + c.text + '</h5>' +
        '</li>';

      var items = c.items.sort((a, b) => a.index - b.index);

      items.forEach(i => {
        elem += '<li class="list item" data-category="' + c.index + '" data-item="' + i.index + '" data-scheme="' + d.scheme + '">' +
          '<div class="compliance">' +
          '<p class="font-weight-bold">' + c.index + '.' + i.index + '</p>' +
          '<div class="compliance-body ml-2">' +
          i.text +
          '</div>';

        if (!app.utils.isNullOrEmpty(i.notes)) {
          elem += '<div class="checklist-tooltip"><span class="mdi mdi-information-outline"></span>' +
            '<div class="tooltip-text">' +
            i.notes +
            '</div></div>';
        }

        elem += '</div> ' +
          '</li>';
      });
    });

    elem += '</ul></div>' +
      '<div class="col-12 version"><span>Version ' + d.version + '</span></div>' +
      '</div></div>';

    container.append(elem);
  }

  function fetchData() {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    var param = '';
    for (var i = 0; i < properties.length; i++) {
      if (i !== 0) param += '&'

      param += 'ids=' + properties[i];
    }

    fetch('/api/checklist?' + param, {
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
        data = r;
        setup();
      })
      .catch(app.http.catch);
  }

}).apply(app.page.checklist = app.page.checklist || {});