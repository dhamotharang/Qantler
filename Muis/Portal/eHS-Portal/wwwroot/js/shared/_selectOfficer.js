app.select = app.select || {};

(function () {
  'use strict';

  var options;
  var cache = {};

  var datatable;

  var controller;

  this.init = function (o) {
    options = o;
    invalidate();

    if (!cache[o.key]) {
      fetchData();
    }

    $('.select-officer .modal-title').html(o.title);
  };

  this.onSelect = function (identity) {
  }

  function invalidate() {
    datatable.ajax.reload();
  }

  function fetchData() {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Initializing. Please wait...');

    var params = '';

    if (options.requestTypes) {
      options.requestTypes.forEach((e, i) => {
        if (params.length > 0) {
          params += '&';
        }

        params += 'requestTypes[' + i + ']=' + encodeURI(e);
      });
    }

    if (options.permissions) {
      options.permissions.forEach((e, i) => {
        if (params.length > 0) {
          params += '&';
        }

        params += 'permissions[' + i + ']=' + encodeURI(e);
      });
    }

    fetch('/api/identity/list?' + params, {
      method: 'GET',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then(res => res.json())
      .then(res => {
        app.dismissProgress();

        cache[options.key] = res;

        invalidate();
      }).catch(err => {
        app.dismissProgress();

        // TODO Handle show error
      });
  }

  $(function () {
    datatable = $('.select-officer .table').DataTable({
      ajax: function (data, callback, settings) {
        var dataset = [];

        if (options && cache[options.key]) {
          dataset = cache[options.key];
        }

        // Filter base on options
        if (options && options.skip) {
          dataset = dataset.filter(e => !options.skip.includes(e.id));
        }

        callback({ data: dataset })
      },
      scrollY: '60vh',
      scrollX: true,
      scrollCollapse: true,
      deferRender: true,
      scroller: {
        displayBuffer: 100
      },
      "dom": "lfrti",
      "aLengthMenu": [
        [5, 10, 15, -1],
        [5, 10, 15, "All"]
      ],
      "iDisplayLength": 5,
      "bLengthChange": false,
      responsive: true,
      autoWidth: false,
      columns: [{
        title: 'Name',
        data: 'id',
        render: function (data, type, row) {
          var action = options.actionText ? options.actionText : 'Select';

          return '<div class="d-flex officer"><span>' + app.utils.titleCase(row.name) + '</span>' +
            '<button class="btn btn-outline-primary select" data-id="' + row.id + '" data-name="' + row.name + '">' + action + '</button>' +
            '</div>';
        }
      }],
      columnDefs: [],
      fixedColumns: true
    });

    $('.select-officer .table').on('click', '.officer .btn.select', function () {
      var id = $(this).data('id');
      var identity = cache[options.key].filter(e => e.id == id)[0];
      app.select.officer.onSelect(identity);

      $('.select-officer .close').click();
    });
  });

}).apply(app.select.officer = app.select.officer || {});