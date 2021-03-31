(function () {
  'use strict';

  var table;

  this.dataset = [];

  var controller;


  this.fetch = function () {
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

    fetch('api/customer/index?' + params, {
      method: 'GET',
      cache: 'no-cache',
      credentials: 'include',
      headers: {
        'Accept': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(data => {
        app.page.dataset = data;
        app.page.invalidate();
      })
      .catch(app.http.catch);

    return true;
  };

  this.invalidate = function () {
     table.ajax.reload();
  }

  this.clearSelection = function () {
    table.rows().deselect();
    hideAllActionButtons();
  }

  $(function () {
    table = $('#customerlisting').DataTable({
      ajax: function (data, callback, settings) {
        callback({ data: app.page.dataset })
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
        data: "customer",
        render: function (data, type, row) {
          return '<a href="/customer/details/' + row.customerID + '">' + data.name + '</button>';
        }
      }, {
        data: "customer",
        render: function (data, type, row) {
          if (data.code
            && data.code.id > 0) {
            return data.code.value
          }
          return '';
        }
      }
        , {
        data: "customer",
        render: function (data, type, row) {
          if (data.groupCode
            && data.groupCode.id > 0) {
            return data.groupCode.value
          }
          return '';
        }
      }, {
          data: "number",
          render: function (data, type, row) {
            if (data) {
              return data
            }
            return '';
          }
      }, {
          data: "statusText",
          render: function (data, type, row) {
            if (row.status > 0) {
              return '<label class="badge badge-inverse-' + app.utils.certificateStatusColor(row.status) + '">' + data + '</label>';
            }
            return '';
          }
      }, {
        data: "premise",
        render: function (data, type, row) {
          return '<p class="premise">' + app.utils.formatPremise(data) + '</p>';
        }
      }, {
        data: "id",
        render: function (data, type, row) {
          return '<a class="btn btn-outline-primary btn-view" href="/customer/details/' + row.customerID + '">View</a>';
        }
      }
      ],
      columnDefs: [{
        targets: [3],
        width: 80
      }, {
        targets: [5],
        width: 80
      }],
      fixedColumns: true
    });
  });
}).apply(app.page);

$(document).ready(function () {
  app.page.fetch();
});