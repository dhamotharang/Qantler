(function () {
  var table;
  var controller;

  function doAcknowledge() {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Acknowledging. Please wait...');

    var ids = [];
    ids.push(app.page.id);

    fetch('/api/mufti/acknowledge', {
      method: 'POST',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(ids)
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(res => {
        app.page.model.status = 200;
        app.dismissProgress();

        updateViewState();
      })
      .catch(app.http.catch);
  }

  function updateViewState() {
    var acknowledgeBtn = $('#acknowledgeBtn');

    if (app.page.model.status == 100
      && app.hasPermission(11)) {
      acknowledgeBtn.removeClass('d-none');
    } else {
      acknowledgeBtn.addClass('d-none');
    }
  }

  $(function () {
    table = $('#listing').DataTable({
      ajax: function (data, callback, settings) {
        callback({ data: app.page.model.certificates })
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
          return app.common.formatScheme(row.schemeText, row.subSchemeText);
        }
      }, {
        data: "requestTypeText"
      }, {
        data: "customerName"
      }, {
        data: "premise",
        render: function (data, type, row) {
          return '<p class="premise">' + app.utils.formatPremise(data) + '</p>';
        }
      }, {
        data: "id",
        render: function (data, type, row) {
          return row.remarks ? row.remarks : '';
        }
      }],
      columnDefs: [],
      fixedColumns: true
    });

    updateViewState();

    $('#acknowledgeBtn').click(function () {
      doAcknowledge();
    });
  });
}).apply(app.page);