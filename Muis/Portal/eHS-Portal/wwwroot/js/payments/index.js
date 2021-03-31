(function (self) {
  var table;

  var dataset;
  var controller;

  self.fetch = function () {
    if (controller) {
      controller.abort();
    }

    var params = self.filter.toParams();
    if (!params) {
      Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: 'Please provide alteast one value from one of the fields.'
      })
      return;
    }

    controller = new AbortController();

    fetch('/api/payments/index/list?' + params, {
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
        self.invalidate();
      })
      .catch(app.http.catch);

    return true;
  }

  self.invalidate = function () {
    table.ajax.reload();
  }

  $(function () {
    dataset = self.model.dataset || [];

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
          return '<a href="/payments/details/' + data + '">' + data + '</button>';
        }
      }, {
        data: "paidOn",
        render: function (data, type, row) {
          return app.utils.formatDate(data, true);
        }
      }, {
        data: "name",
        render: function (data, type, row) {
          return '<p class="customer">' + data + '</p>';
        }
      }, {
        data: "totalAmount",
        render: function (data, type, row) {
          return data;
        }
      }, {
        data: "modeText",
        render: function (data, type, row) {
          return data;
        }
      }, {
        data: "methodText",
        render: function (data, type, row) {
          return data;
        }
      }, {
        data: "statusText",
        render: function (data, type, row) {
          return '<label class="badge badge-inverse-' + self.statusColor(row.status) + ' status" data-id="' + row.id + '">' + data + '</label>';
        }
      }, {
        data: "id",
        render: function (data, type, row) {
          return row.transactionNo ? row.transactionNo : '';
        }
      }, {
        data: "id",
        render: function (data, type, row) {
          return row.receiptNo ? row.receiptNo : '';
        }
      }, {
        data: "id",
        render: function (data, type, row) {
          return '<a href="/payments/details/' + data + '" class="btn btn-outline-primary btn-view">View</button>';
        }
      },],
      fixedColumns: true
    });
  });

})(app.page);