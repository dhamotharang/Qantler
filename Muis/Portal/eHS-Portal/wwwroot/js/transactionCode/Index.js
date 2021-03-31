(function () {
  var table;
  this.dataset = [];

  this.fetch = function () {

    fetch('api/transactioncode/index', {
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
  };

  this.invalidate = function () {
    table.ajax.reload();
  }

  $(function () {
    table = $('#Tlisting').DataTable({
      ajax: function (data, callback, settings) {
        callback({ data: app.page.dataset })
      },
      scrollY: '53vh',
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
        data: "code",
        render: function (data, type, row) {
          return data ;
        }
      }, {
          data: "glEntry",
          render: function (data, type, row) {
            return data ;
          }
        },
        {
          data: "text",
          render: function (data, type, row) {
            return data;
          }
        },
        {
          data: "priceHistory",
          render: function (data, type, row) {
            var result = '';
            if (data != undefined) {

              data.sort(function (a, b) {

                if (a.effectiveFrom < b.effectiveFrom) {
                  return 1;
                }

                if (a.effectiveFrom > b.effectiveFrom) {
                  return -1;
                }

                return 0;
              });

              var today = new Date();
              today.setHours(0, 0, 0, 0);

              data.forEach(a => {
                var tempDate = new Date(a.effectiveFrom);
                tempDate.setHours(0, 0, 0, 0);

                if (tempDate >= today) {
                  result = Number(a.amount).toLocaleString();
                }

              });

            }
            return result;
          }
        }, {
          data: "modifiedOn",
          render: function (data, type, row) {
            return app.utils.formatDate(data, true);
          }
        }, {
        data: "id",
          render: function (data, type, row) {
            return '<button class="btn btn-outline-primary btn-view" data-id="' + data + '">View</button>';
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

    $('#Tlisting').on('click', '.btn-view', function () {
      window.location.href = "/transactioncode/details/" + $(this).data('id');
    });

    $('#fixed-column_wrapper').resize(function () {
      fixedColumnTable.draw();
    });

  });

  function formatLog(e) {
    var result = e.action;

    if (e.params) {
      var i;
      for (i = 0; i < e.params.length; i++) {

        var val = e.params[i];

        if (!isNaN(Date.parse(val)))
          val = moment(new Date(val)).format('DD MMM YYYY : h:mmA');

        result = result.replace("{" + i + "}", '<b>' + val + '</b>');
      }
    }

    result += app.common.createHashtag(e.type, e.refID);

    return result;
  }


}).apply(app.page);

$(document).ready(function () {
  app.page.fetch();
});