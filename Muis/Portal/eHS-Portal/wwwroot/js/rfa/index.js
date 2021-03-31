(function (self) {
  'use strict'

  var table;
  var dataset = self.model.data;

  var controller;

  self.fetch = function () {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    var params = app.page.filter.toParams();
    if (!params) {
      Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: 'Please provide alteast one value from one of the fields.'
      })
      return;
    }

    fetch('api/rfa/list?' + params, {
      method: 'GET',
      cache: 'no-cache',
      credentials: 'include',
      headers: {
        'Accept': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(data => {
        dataset = data;
        invalidate();
      })
      .catch(app.http.catch);

    return true;
  }

  function invalidate() {
    table.ajax.reload();
  }

  $(function () {
    table = $('#listing').DataTable({
      ajax: function (data, callback, settings) {
        callback({ data: dataset || [] })
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
      searching: false,
      responsive: true,
      autoWidth: false,
      columns: [{
        data: "id"
      }, {
        data: "request.customerName",
        render: function (data, type, row) {
          return app.utils.titleCase(data);
        }
      }, {
        data: "createdOn",
        render: function (data, type, row) {
          return app.utils.formatDate(data, true);
        }
      }, {
        data: "dueOn",
        render: function (data, type, row) {
          return app.utils.formatDate(data, true);
        }
      }, {
        data: "statusText",
        render: function (data, type, row) {
          return '<label class="badge badge-inverse-' + app.common.rfaStatusColor(row.status) + '">' + data + '</label>';
        }
      }, {
        data: "raisedByName",
        render: function (data, type, row) {
          return '<div class="d-flex align-items-center">' +
            '<span class="img-xs rounded-circle bg-' + app.utils.randomColor() + ' text-white text-avatar">' + app.utils.initials(data) + '</span>' +
            '<div class="wrapper pl-2"> ' +
            '<p class="mb-0 text-gray">' + data + '</p>' +
            '</div > ' +
            '</div>';
        }
      }, {
        data: "id",
        render: function (data, type, row) {
          return '<a class="btn btn-outline-primary btn-view" href="/rfa/details/' + data + '">View</a>';
        }
      }],
      fixedColumns: true
    });

    $('#fixed-column_wrapper').resize(function () {
      fixedColumnTable.draw();
    });

  });

})(app.page);