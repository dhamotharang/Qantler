(function (self) {

  var controller;
  var id;
  var dataset = [];

  self.init = function (d) {
    id = d;

    fetchData();

    $('a.payment-index').attr('href', '/payments?customerID=' + id);
  }

  function showLoader() {
    $('.recent-payment').each(function () {
      $(this).addClass('busy');
    });
  }

  function hideLoader() {
    $('.recent-payment').each(function () {
      $(this).removeClass('busy');
    });
  }

  function prepareData(d) {
    dataset = d;

    if (d.length == 0) {
      $("a.payment-index").addClass('d-none');
    }

    table = $('#paymentlisting').DataTable({
      ajax: function (data, callback, settings) {
        callback({ data: dataset })
      },
      searching: false,
      ordering: true,
      responsive: true,
      paging: false,
      info: false,
      autoWidth: false,
      columns: [{
        data: "paidOn",
        render: function (data, type, row) {
          return app.utils.formatDateTime(data);
        }
      }, {
        data: "amount"
      }, {
        data: "modeText"
      }, {
        data: "methodText"
      }, {
        data: "statusText"
      }, {
        data: "id",
        render: function (data, type, row) {
          return '<a class="btn btn-outline-primary btn-view" href="/payments/details/' + row.id + '">View</a>';
        }
      }],
      columnDefs: [],
      fixedColumns: false
    });
  }

  function fetchData() {
    if (controller) {
      controller.abot();
    }
    controller = new AbortController();

    showLoader();

    fetch('/api/customer/' + id + '/payment/recent', {
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(res => {
        prepareData(res);
        hideLoader();
      })
      .catch(err => {
        hideLoader();
      });
  }
})(app.page.recentpayment = app.page.recentpayment || {});