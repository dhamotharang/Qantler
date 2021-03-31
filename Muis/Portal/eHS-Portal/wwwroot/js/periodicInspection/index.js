(function (self) {
  var table;
  var dataset = [];

  var controller;

  function init() {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    fetch('api/joborder/index?type[0]=1&status[0]=100', {
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
  };

  function invalidate() {
    table.ajax.reload();
  }

  $(function () {
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
          return '<a href="/jobOrder/details/' + data + '">' + data + '</button>';
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
        data: "targetDate",
        render: function (data, type, row) {
          return app.utils.formatDate(data, true);
        }
      }, {
        data: "id",
        render: function (data, type, row) {
          return '<a class="btn btn-outline-primary btn-view" href="/jobOrder/details/' + data + '">View</button>';
        }
      }],
      columnDefs: [],
      fixedColumns: true
    });

    $('#fixed-column_wrapper').resize(function () {
      fixedColumnTable.draw();
    });

    init();
  });
})(app.page);