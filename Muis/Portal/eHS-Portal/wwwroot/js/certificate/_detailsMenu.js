(function () {
  'use strict';

  var controller;

  var id;
  var data = [];

  var table;

  this.dirtyObjects = [];

  this.init = function (d) {
    id = d;
    fetchMenu();
  }

  function fetchMenu() {
    if (controller) {
      return;
    }
    controller = new AbortController();

    fetch('/api/certificate/' + id + '/menu', {
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

        $('.row.menus .grid-margin.stretch-card').addClass('d-none');

      }).catch(err => {
        controller = null;
        app.http.catch(err);
      });
  }

  this.isDirty = function () {
    return this.dirtyObjects.length > 0;
  }

  this.clearState = function () {
    this.dirtyObjects.forEach((e, i) => {
      e.oldRemarks = null;
      e.firstUpdate = false;
    });

    this.dirtyObjects = [];
  }

  this.getData = function () {
    return data || [];
  };

  function invalidate() {
    table.ajax.reload();
  };

  function setup() {
    table = $('#menu-listing').DataTable({
      scrollY: '60vh',
      scrollCollapse: false,
      deferRender: true,
      scroller: {
        displayBuffer: 100,
        rowHeight: 60
      },
      "dom": "lfrti",
      "aLengthMenu": [
        [5, 10, 15, -1],
        [5, 10, 15, "All"]
      ],
      "iDisplayLength": 5,
      "bLengthChange": false,
      ajax: function (data, callback, settings) {
        callback({ data: app.page.main.menu.getData() })
      },
      columns: [{
        data: "text",
        width: "100px",
        render: function (data, type, row, meta) {
          var style = '';
          var statustext = '';
          var styleSpan = '';

          switch (row.changeType) {
            case 1:
              style = 'text-success';
              statustext = 'New';
              styleSpan = '';
              break;
            case 2:
              style = 'text-warning';
              statustext = 'Updated';
              styleSpan = '';
              break;
            case 3:
              style = 'text-danger';
              statustext = 'Removed';
              styleSpan = 'text-line-through';
              break;
          }

          var elem = '<div class="menu item-row">' +
            '<div class="info">' +
            '<div>' +
            '<span class="' + styleSpan + '">' + app.utils.titleCase(row.text) + '</span> ' +
            '<sup class="' + style + '">' + statustext + '</sup>' +
            '</div>';

          elem += '</div>';

          if (!app.utils.isNullOrEmpty(row.subText)) {
            elem += '<span class="sub-info ' + styleSpan + '">' + app.utils.titleCase(row.subText) + '</span>';
          }

          elem += '</div></div>';

          return elem;
        }
      }]
    });
  }

  $(function () {
    setup();
  });
}).apply(app.page.main.menu = app.page.main.menu || {});
