app.select = app.select || {};

(function (self) {
  'use strict';

  var options = {};
  var cache = {};

  var datatable;

  var controller;

  self.init = function (o) {
    options = o;

    invalidate();

    if (!cache[o.type]) {
      fetchData();
    }

    var title = o.type == 0 ? 'Code' : 'Group Code';
    $('.select-code .modal-title').html(title);
  };

  self.reset = function () {
    cache = {};
  }

  self.dismiss = function () {
    $('.select-code .close').click();
  }

  self.onSelect = function (code) {
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

    fetch('/api/code/list?type=' + options.type, {
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

        cache[options.type] = res;

        invalidate();
      }).catch(err => {
        app.dismissProgress();

        // TODO Handle show error
      });
  }

  $(function () {
    datatable = $('.select-code .table').DataTable({
      ajax: function (data, callback, settings) {
        var dataset = [];

        if (options
          && cache[options.type]) {
          dataset = cache[options.type];
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
        title: 'Code',
        data: 'id',
        render: function (data, type, row) {
          return '<div class="d-flex code">' +
            '<span class="font-weight-bold">' + row.value + '</span>&nbsp;&nbsp;&nbsp;' +
            '<span class="flex">' + app.utils.titleCase(row.text) + '</span>' +
            '<button class="btn btn-outline-primary select" data-id="' + row.id + '">Select</button>' +
            '</div>';
        }
      }],
      columnDefs: [],
      fixedColumns: true
    });

    $('.select-code .table').on('click', '.code .btn.select', function () {
      var id = $(this).data('id');
      var code = cache[options.type].filter(e => e.id == id)[0];
      app.select.code.onSelect(code);

      $('.select-code .close').click();
    });

    $('.select-code #createBtn').click(function () {
      self.create.init(options, cache[options.type]);

      setTimeout(function () {
        $('.modal-backdrop').last().css('z-index', '1070');
      }, 100);
    });

    if (self.create) {
      $('.select-code #createBtn').removeClass('d-none');
    }
  });

})(app.select.code = app.select.code || {});