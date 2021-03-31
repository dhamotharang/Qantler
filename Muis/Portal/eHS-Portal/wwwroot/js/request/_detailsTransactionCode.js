(function () {
  'use strict';

  var cache;

  var datatable;

  var controller;

  this.init = function () {
    invalidate();

    if (!cache) {
      fetchData();
    }
  };

  this.onSelect = function (entity) {
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

    fetch('/api/transactioncode/list', {
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

        cache = res;

        invalidate();
      }).catch(app.http.catch);
  }

  $(function () {
    datatable = $('.select-transation-code .table').DataTable({
      ajax: function (data, callback, settings) {
        callback({ data: cache || [] })
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
        title: 'Code',
        data: 'code'
      }, {
        title: 'Text',
        data: 'text',
        render: function (data, type, row) {
          return '<div class="code-text"><p>' + app.utils.titleCase(data) + '</p></div>';
        }
      }, {
        title: 'Actions',
        data: 'id',
        render: function (data, type, row) {
          return '<button class="btn btn-outline-primary select" data-id="' + row.id + '" data-name="' + row.text + '">Select</button>';
        }
      }],
      columnDefs: [],
      fixedColumns: true
    });

    $('.select-transation-code .table').on('click', '.btn.select', function () {
      var id = $(this).data('id');
      var entity = cache.filter(e => e.id == id)[0];
      app.page.transactioncode.onSelect(entity);

      $('.select-transation-code .close').click();
    });
  });

}).apply(app.page.transactioncode = app.page.transactioncode || {});