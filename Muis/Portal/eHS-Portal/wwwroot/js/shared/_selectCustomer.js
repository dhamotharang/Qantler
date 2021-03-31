app.select = app.select || {};

(function (self) {
  'use strict';

  var options = {};
  var cache;

  var datatable;

  var controller;

  self.init = function (o) {
    options = o;

    $('.select-customer .modal-title').html(o.title);

    invalidate();

    if (!cache) {
      fetchData();
    }
  }

  self.reset = function () {
    cache = null;
  }

  self.dismiss = function () {
    $('.select-customer .close').click();
  }

  self.onSelect = function (customer) {
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

    fetch('/api/customer/list', {
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

        cache = res;

        invalidate();
      }).catch(err => {
        app.dismissProgress();

        // TODO Handle show error
      });
  }

  $(function () {
    datatable = $('.select-customer .table').DataTable({
      ajax: function (data, callback, settings) {
        var dataset = cache || [];

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
          return '<div class="d-flex entity">' +
            '<span class="flex">' + app.utils.titleCase(row.name) + '</span>' +
            '<button class="btn btn-outline-primary select" data-id="' + row.id + '">Select</button>' +
            '</div>';
        }
      }],
      columnDefs: [],
      fixedColumns: true
    });

    $('.select-customer .table').on('click', '.entity .btn.select', function () {
      var id = $(this).data('id');
      var customer = cache.filter(e => e.id == id)[0];
      app.select.customer.onSelect(customer);

      $('.select-customer .close').click();
    });

    $('.select-customer #createBtn').click(function () {
      self.create.init();

      self.create.createCallback = function (c) {
        cache = null;

        self.dismiss();
        self.onSelect(c);
      }

      setTimeout(function () {
        $('.modal-backdrop').last().css('z-index', '1070');
      }, 100);
    });

    if (self.create) {
      $('.select-customer #createBtn').removeClass('d-none');
    }
  });

})(app.select.customer = app.select.customer || {});