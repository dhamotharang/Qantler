(function () {
  var table;

  var dataset;
  var controller;

  this.fetch = function () {
    if (controller) {
      controller.abort();
    }

    var params = app.page.filter.toParams();
    if (!params) {
      Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: 'Please provide alteast one value from one of the fields.'
      })
      return;
    }

    controller = new AbortController();

    fetch('/api/bill/list?' + params, {
      method: 'GET',
      cache: 'no-cache',
      credentials: 'include',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(data => {
        dataset = data;
        app.page.invoice.invalidate();
      })
      .catch(app.http.catch);

    return true;
  }

  this.invalidate = function () {
    table.ajax.reload();
  }

  function onDataUpdated(d) {
    var data = dataset.find(e => e.id == d.id);

    var index = dataset.indexOf(data);
    dataset[index] = d;

    var statusCol = $('.badge.status[data-id="' + d.id + '"]');
    statusCol.removeClass('badge-inverse-' + app.page.statusColor(data.status));
    statusCol.addClass('badge-inverse-' + app.page.statusColor(d.status));
    statusCol.html(d.statusText);
  }

  $(function () {
    dataset = app.page.model.dataset || [];
    console.log(dataset);
    table = $('#listing').DataTable({
      ajax: function (data, callback, settings) {
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
      searching: true,
      responsive: true,
      autoWidth: false,
      columns: [{
        data: "id",
        render: function (data, type, row) {
          return '<a href="/bill/details/' + data + '">' + data + '</button>';
        }
      }, {
        data: "customerName",
        render: function (data, type, row) {
          if (data) {
            return '<p class="customer">' + app.utils.titleCase(data) + '</p>';
          }
          return '';
        }
      }, {
        data: "type",
        render: function (data, type, row) {
          if (data == 0 || data == 1) {
            return '<a href="/request/details/' + row.requestID + '">' + row.refID + '</a>';
          } else if (data == 2) {
            return '<a href="/case/details/' + row.refID + '">' + row.refID + '</a>';
          }
          return '';
        }
      }, {
        data: "invoiceNo",
        render: function (data, type, row) {
          if (data) {
            return data;
          }
          return '';
        }
      }, {
        data: "typeText"
      }, {
        data: "statusText",
        render: function (data, type, row) {
          return '<label class="badge badge-inverse-' + app.page.statusColor(row.status) + ' status" data-id="' + row.id + '">' + data + '</label>';
        }
      }, {
        data: "id",
        render: function (data, type, row) {
          return app.number.financial(row.totalAmount);
        }
      }, {
        data: "issuedOn",
        render: function (data, type, row) {
          return app.utils.formatDate(data, true);
        }
      }, {
        data: "id",
        render: function (data, type, row) {
          return '<a class="btn btn-outline-primary btn-view" href="/bill/details/' + data + '">View</button>';
        }
      }],
      fixedColumns: true
    });

    $('#fixed-column_wrapper').resize(function () {
      fixedColumnTable.draw();
    });

    $('#listing').on('click', '.btn-view', function () {
      this.view($(this).data('id'));
    });

    this.updateCallback = function (d) {
      onDataUpdated(d);
    }
  });

}).apply(app.page.invoice = app.page.invoice || {});