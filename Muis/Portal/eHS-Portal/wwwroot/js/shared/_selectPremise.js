app.select = app.select || {};

(function (self) {
  'use strict';

  var controller;
  var cache;
  var customerID;
  var options = {};

  var datatable;

  self.init = function (o) {
    $('.select-premise .modal-title').html(o.title);

    options = o;

    invalidate();

    if (customerID != options.customerID || !cache) {
      customerID = options.customerID;
      fetchData();
    }
  };

  self.reset = function () {
    cache = null;
  };

  self.dismiss = function () {
    $('.select-premise .close').click();
  };

  self.onSelect = function (premise) {
  };

  function invalidate() {
    datatable.search('').draw();
    datatable.ajax.reload();
  };

  function fetchData() {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Initializing. Please wait...');

    fetch('/api/premise/list?customerID=' + customerID, {
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
      });
  };

  $(function () {
    datatable = $('.select-premise .table').DataTable({
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
        displayBuffer: 500
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
          var data = app.utils.formatPremise(row);
          if (row.customer != null
            && row.customer.name != null) {
            data = '<span>' + row.customer.name + ',</span> ' + data;
          }

          return '<div class="row container">' +
            '<div class="row col-11">' + app.utils.titleCase(data) + '</div>' +
            '<div class="col-1"><button class="btn btn-outline-primary select" data-id="' + row.id + '">Select</button></div>' +
            '</div>';
        }
      }],
      columnDefs: [],
      fixedColumns: true
    });

    $('.select-premise .table').on('click', '.btn.select', function () {
      var id = $(this).data('id');
      var premise = cache.filter(e => e.id == id)[0];
      app.select.premise.onSelect(premise);

      $('.select-premise .close').click();
    });

    $('.select-premise #createBtn').click(function () {
      self.create.init(customerID);

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
      $('.select-premise #createBtn').removeClass('d-none');
    }

  });

})(app.select.premise = app.select.premise || {});