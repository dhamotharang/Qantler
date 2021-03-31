(function () {
  var table;  

  function updateViewState() {
    var acknowledgeBtn = $('#viewAllPendingButton');

    var data = app.page.model
    var haspending = data.filter(e => e.status == 100).length > 0

    if (haspending
      && app.hasPermission(11)) {
      acknowledgeBtn.removeClass('d-none');
    } else {
      acknowledgeBtn.addClass('d-none');
    }
  }

  $(function () {
    table = $('#listing').DataTable({
      ajax: function (data, callback, settings) {
        callback({ data: app.page.model })
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
        data: "code",
      }, {
        data: "description"
      }, {
        data: "statusText",
        render: function (data, type, row) {
          var color = 'dark';
          if (row.status >= 200) {
            data = 'Acknowledged';
            color = 'success';
          }
          return '<label class="badge badge-inverse-' + color + '">' + data + '</label>';
        }
      }, {
        data: "createdOn",
        render: function (data, type, row) {
          return app.utils.formatDate(data, true);
        }
      }, {
        data: "acknowledgedOn",
        render: function (data, type, row) {
          return app.utils.formatDateTime(data, true);
        }
      }, {
        data: "id",
        render: function (data, type, row) {
          return '<a class="btn btn-outline-primary btn-view" href="/mufti/details/' + data + '">View</a>';
        }
      }],
      columnDefs: [{
        targets: [1],
        width: 180
      }, {
        targets: [3, 4],
        width: 80
      }],
      fixedColumns: true
    });

    updateViewState();

    $('#viewAllPendingButton').click(function () {
      window.location.href = '/mufti/Details';
    });

  });
}).apply(app.page);