(function () {
  var table;

  var controller;

  function print(batchID) {
    loadFile(batchID, function(data) {
      app.print.printFromURL(getDownloadUrl(data));
    });
  }

  function loadFile(batchID, callback) {
    var data = app.page.model.filter(e => e.id == batchID)[0];
    if (data.fileID) {
      callback(data);
    } else {
      generatePdf(data, batchID, function (d) {
        callback(d);
      });
    }
  }

  function getDownloadUrl(data) {
    return '/api/file/' + data.fileID + '?fileName=' + data.code + '.pdf';
  }

  function generatePdf(data, batchID, callback) {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Generating. Please wait...');

    fetch('/api/certificate/batch/preview/' + batchID + '/generate/pdf', {
      method: 'GET',
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
        app.dismissProgress();

        data.fileID = res.id;

        if (data.status == 200) {
          data.status = 300;
          data.statusText = 'Downloaded';
          updateRowState(data);
        }

        callback(data);
      }).catch(app.http.catch);
  }

  function updateStatus(batchID, status) {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Processing. Please wait...');

    fetch('/api/certificate/batch/' + batchID + '/status?status=' + status, {
      method: 'PUT',
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
        app.dismissProgress();

        var data = app.page.model.filter(e => e.id == batchID)[0];
        data.status = res.status;
        data.statusText = res.statusText;

        updateRowState(data);

      }).catch(app.http.catch);
  }

  function updateRowState(d) {
    
    var status = $('.status[data-id="' + d.id + '"]');

    var oldColor = statusColor(d.status - 100);
    var color = statusColor(d.status);

    status.html(d.statusText);
    status.removeClass('badge-inverse-' + oldColor);
    status.addClass('badge-inverse-' + color);
  }

  function statusColor(status) {
    switch (status) {
      case 200: // acknowledged
        return 'dark';
      case 300: // inspection
        return 'primary'
      case 400: // pending approval
        return 'warning';
      case 500: // approved
        return 'success';
    }
  };

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
          var color = statusColor(row.status);
          return '<label class="badge badge-inverse-' + color + ' status" data-id="' + row.id + '">' + data + '</label>';
        }
      }, {
        data: "createdOn",
        render: function (data, type, row) {
          return app.utils.formatDate(data, true);
        }
      }, {
        data: "lastAction",
        render: function (data, type, row) {
          return app.utils.formatDateTime(data, true);
        }
      }, {
        data: "id",
        render: function (data, type, row) {
          var courierVissbility = row.status == 300 ? '' : 'd-none';
          var deliveredVisibility = row.status == 400 ? '' : 'd-none';

          var result = '<button class="btn btn-outline-primary btn-preview" data-toggle="modal" data-target="#previewModal" data-id="' + data + '">Preview</button>' +
            '<button class="btn btn-outline-primary btn-print ml-1" data-id="' + data + '">Print</button>';

          return result;
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

    $('#listing').on('click', '.btn-preview', function () {
      app.page.preview.preview($(this).data('id'));
    });

    $('#listing').on('click', '.btn-print', function () {
      print($(this).data('id'));
    });

    $('#listing').on('click', '.btn-sent-courier', function () {
      updateStatus($(this).data('id'), 400);
    });

    $('#listing').on('click', '.btn-delivered', function () {
      updateStatus($(this).data('id'), 500);
    });
  });
}).apply(app.page);