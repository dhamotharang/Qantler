(function (self) {
  'use strict'

  var table;

  var dataset = [];

  self.fetch = function () {
    fetch('api/user/index?' + self.filter.toParams(), {
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
  }

  function invalidate() {
    table.ajax.reload();
  }

  function setupPermissions() {
    if (app.hasPermission(18)) {
      $('.create').removeClass('d-none');
    }
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
      searching: true,
      responsive: true,
      autoWidth: false,
      columns: [{
        data: "name",
      }, {
        data: "designation"
      }, {
        data: "email"
      }, {
        data: "statusText",
        render: function (data, type, row) {
          return '<label class="badge badge-inverse-' + self.common.statusColor(row.status) + '">' + data + '</label>';
        }
      }, {
        data: "id",
        render: function (data, type, row) {
          return '<a class="btn btn-outline-primary btn-view" href="/user/details/' + data + '">View</a>';
        }
      }],
      fixedColumns: true
    });

    $('#createButton').click(function () {
      window.location.href = '/user/form';
    });
  });

  setupPermissions();

})(app.page);

$(document).ready(function () {
  app.page.fetch();
});