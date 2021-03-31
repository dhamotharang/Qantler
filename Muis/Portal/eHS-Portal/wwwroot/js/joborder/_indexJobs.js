(function (self) {
  var table;
  var dataset = [];

  var controller;

  self.fetch = function () {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    dataset = [];
    invalidate();

    var params = app.page.filter.toParams();
    if (!params) {
      Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: 'Please provide alteast one value from one of the fields.'
      })
      return;
    }

    fetch('api/joborder/index?' + params, {
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
  };

  function invalidate() {
    table.ajax.reload();
  }

  $(function () {
    table = $('#Jlisting').DataTable({
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
      searching: false,
      responsive: true,
      autoWidth: false,
      columns: [{
        data: "id",
        render: function (data, type, row) {
          return '<a href="/jobOrder/details/' + data + '">' + data + '</button>';
        }
      }, {
        data: "typeText",
        render: function (data, type, row) {
          return data;
        }
      }, {
        data: "lineItems",
        render: function (data, type, row) {
          var result = '';

          if (data.length > 0) {
            data.forEach(i => {

              if (result.length > 0) {
                result += ', ';
              }

              result += app.common.formatScheme(i.schemeText, i.subSchemeText);

            });
          }

          return '<p class="scheme">' + result + '</p>';
        }
      }, {
        data: "customer",
        render: function (data, type, row) {
          if (data) {
            return '<p class="customer">' + app.utils.titleCase(data.name) + '</p>';
          }
          return '';
        }
      }, {
        data: "premises",
        width: "80px",
        render: function (data, type, row) {
          if (data.length > 0 && data[0]) {
            return '<p class="premise">' + app.utils.formatPremise(data[0]) + '</p>';
          }
          return '';
        }
      }, {
        data: "scheduledOn",
        render: function (data, type, row) {
          return app.utils.formatDate(data, true);
        }
      }, {
        data: "statusText",
        render: function (data, type, row) {
          return '<label class="badge badge-inverse-' + app.utils.requestStatusColor(row.status) + '">' + data + '</label>';
        }
      }, {
        data: "officer",
        render: function (data, type, row) {
          if (app.utils.isNullOrEmpty(data)) {
            return '';
          }

          return '<div class="d-flex align-items-center">' +
            '<span class="img-xs rounded-circle bg-' + app.utils.randomColor() + ' text-white text-avatar">' + app.utils.initials(data.name) + '</span>' +
            '<div class="wrapper pl-2"> ' +
            '<p class="mb-0 text-gray">' + data.name + '</p>' +
            '</div > ' +
            '</div>';
        }
      }, {
        data: "id",
        render: function (data, type, row) {
          return '<a class="btn btn-outline-primary btn-view" href="/jobOrder/details/' + data + '">View</button>';
        }
      }],
      columnDefs: [{
        targets: [3],
        width: 80
      }, {
        targets: [5],
        width: 80
      }],
      fixedColumns: true
    });

    $('#fixed-column_wrapper').resize(function () {
      fixedColumnTable.draw();
    });

  });

})(app.page.jobs = app.page.jobs || {});