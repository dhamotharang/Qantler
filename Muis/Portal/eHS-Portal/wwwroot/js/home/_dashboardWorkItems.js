(function (self) {
  var controller;

  var dataset = [];

  var table;

  self.init = function () {
    fetchData();
  }

  function invalidate() {
    table.ajax.reload();
  }

  function showLoader() {
    $('.work-items').each(function () {
      $(this).addClass('busy');
    });
  }

  function hideLoader() {
    $('.work-items').each(function () {
      $(this).removeClass('busy');
    });
  }

  function fetchData() {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    showLoader();

    dataset = [];

    var option = {};
    option.id = app.identity.id;

    option.statuses = [];

    if (app.hasPermission(1)) {
      Array.prototype.push.apply(option.statuses, [200, 300]);
    }

    if (app.hasPermission(22)) {
      option.statuses.push(250);
    }

    //RequestReview = 1 or RequestReviewApproval = 22
    app.page.workitem.builder.buildRequest(option.statuses.length > 0, option, controller)
      .then(res => {
        Array.prototype.push.apply(dataset, res);

        option = {};
        option.statuses = [];

        if (app.hasPermission(2)) {
          option.statuses.push(400);
        }

        if (app.hasPermission(3)) {
          option.statuses.push(600);
        }

        if (app.hasPermission(4)) {
          option.statuses.push(800);
        }

        //RequestApprove = 2 or RequestInvoice = 3 or RequestIssuance = 4
        return app.page.workitem.builder.buildRequest(option.statuses.length > 0, option, controller);
      })
      .then(res => {
        Array.prototype.push.apply(dataset, res);

        option = {};
        option.statuses = [];

        if (app.hasPermission(21)) {
          Array.prototype.push.apply(option.statuses, [100, 200]);
        }
        else if (app.hasPermission(1)) {
          option.id = app.identity.id;

          option.statuses.push(200);
        }

        //RFASupport = 21 or RequestReview = 1
        return app.page.workitem.builder.buildRFA(option.statuses.length > 0, option, controller);
      })
      .then(res => {
        Array.prototype.push.apply(dataset, res);

        option = {};
        option.status = 200;

        //PaymentReadWrite = 14
        return app.page.workitem.builder.buildPayment(app.hasPermission(14), option, controller);
      })
      .then(res => {
        Array.prototype.push.apply(dataset, res);

        invalidate();

        hideLoader();
      });
  }

  $(function () {
    table = $('#listing').DataTable({
      ajax: function (data, callback, settings) {
        callback({ data: dataset })
      },
      scrollY: '40vh',
      scrollX: false,
      scrollCollapse: true,
      deferRender: true,
      scroller: {
        displayBuffer: 100
      },
      'dom': 'lfrti',
      'aLengthMenu': [
        [5, 10, 15, -1],
        [5, 10, 15, 'All']
      ],
      'iDisplayLength': 4,
      'bLengthChange': false,
      searching: false,
      responsive: true,
      order: [3, 'asc'],
      columns: [{
        data: 'RefID',
        render: function (data, type, row) {
          return '<a href="/' + row.Type + '/details/' + row.Id + '">' + data + '</button>';
        }
      }, {
        data: 'Item',
        render: function (data, type, row) {
          return data;
        }
      }, {
        data: 'StatusText',
        render: function (data, type, row) {
          var color = '';
          switch (row.Type) {
            case 'Request':
              color = app.utils.requestStatusColor(row.Status);
              break;
            case 'Payments':
              color = app.utils.paymentStatusColor(row.Status);
              break;
            case 'RFA':
              color = app.utils.rfaStatusColor(row.Status);
              break;
          }

          return '<label class="badge badge-inverse-' + color + '">' + data + '</label>';
        }
      }, {
        data: 'LastAction',
        render: function (data, type, row) {
          return data;
        }
      }],
      columnDefs: [{
        targets: [0, 2, 3],
        width: '15%'
      }, {
        targets: [1],
        width: '55%'
      }],
      fixedColumns: true
    });

    $('#fixed-column_wrapper').resize(function () {
      fixedColumnTable.draw();
    });
  });

})(app.page.workitem = app.page.workitem || {});