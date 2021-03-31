(function (self) {

  var controller;
  var id;
  var dataset = [];

  self.init = function (d) {
    id = d;

    fetchData();

    $('a.request-index').attr('href', '/request?customerID=' + id);
  }

  function showLoader() {
    $('.recent-request').each(function () {
      $(this).addClass('busy');
    });
  }

  function hideLoader() {
    $('.recent-request').each(function () {
      $(this).removeClass('busy');
    });
  }

  function prepareData(d) {
    dataset = d;

    if (d.length == 0) {
      $("a.request-index").addClass('d-none');
    }

    table = $('#requestlisting').DataTable({
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
        data: "refID"
      }, {
        data: "typeText"
      }, {
        data: "id",
        render: function (data, type, row) {
          var result = '';
          if (row.lineItems && row.lineItems.length > 0) {
            row.lineItems.forEach(e => {
              if (result.length > 0) {
                result += ', ';
              }
              result += e.schemeText;
              if (!app.utils.isNullOrEmpty(e.subSchemeText)) {
                result += ' <b>(' + e.subSchemeText + ')</b>';
              }
            });
          }

          return '<p class="scheme">' + result + '</p>';
        }
      }, {
        data: "submittedOn",
        render: function (data, type, row) {
          return app.utils.formatDateTime(data);
        }
      }, {
        data: "statusText"
      }, {
        data: "id",
        render: function (data, type, row) {
          return '<a class="btn btn-outline-primary btn-view" href="/request/details/' + row.id + '">View</a>';
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

    fetch('/api/customer/' + id + '/request/recent', {
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
})(app.page.recentrequest = app.page.recentrequest || {});