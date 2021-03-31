(function () {
  'use strict';

  var table;
  var selectedData = [];

  var controller;

  this.dataset = [];

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

    fetch('api/request/index?' + params, {
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
    selectedData = [];
    table.ajax.reload();
  }

  this.clearSelection = function () {
    selectedData = [];
    table.rows().deselect();
    hideAllActionButtons();
  }

  function setupActionButtons() {
    if (selectedData.length == 0) {
      hideAllActionButtons();
      return;
    }

    var holder = [];
    var statuses = selectedData.map(e => e.status).filter(e => {
      if (holder.indexOf(e) != -1) {
        return false;
      }
      holder.push(e);
      return true;
    });

    holder = [];
    var customers = selectedData.map(e => e.customerID).filter(e => {
      if (holder.indexOf(e) != -1) {
        return false;
      }
      holder.push(e);
      return true;
    });

    var recommendBtn = $('#recommendButton');
    var clearSelectionBtn = $('#clearSelectionButton');
    var approveAllBtn = $('#approveAllButton');
    var rejectAllBtn = $('#rejectAllButton');

    if (statuses.length >= 2) {
      hideAllActionButtons();
      app.utils.show(clearSelectionBtn);
      return;
    }

    app.utils.show(clearSelectionBtn);

    switch (statuses[0]) {
      case 200:
        /* Can recommend all for same customer */
        if (customers.length == 1
          && (app.hasPermission(7)
            || selectedData.filter(e => e.assignedTo != app.identity.id).length == 0)) {

          app.utils.show(recommendBtn);

          app.utils.hide(approveAllBtn);
          app.utils.hide(rejectAllBtn);
        }

        break;
      case 400:

        if (app.hasPermission(2)
          || app.hasPermission(7)) {

          /* Can reject only for single customer */
          if (customers.length > 1) {
            app.utils.hide(rejectAllBtn);
          } else {
            app.utils.show(rejectAllBtn);
          }

          app.utils.show(approveAllBtn);
        }

        break;
      default:

        hideAllActionButtons();
        app.utils.show(clearSelectionBtn);

        break;
    }
  }

  function hideAllActionButtons() {
    app.utils.hide($('#recommendButton'));
    app.utils.hide($('#clearSelectionButton'));
    app.utils.hide($('#approveAllButton'));
    app.utils.hide($('#rejectAllButton'));
  }

  $(function () {
    table = $('#listing').DataTable({
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
      createdRow: function (row, data, index) {
        if (data.escalateStatus == 100) {
          $(row).addClass('escalated');
        }
      },
      columns: [{
        data: "expedite",
        render: function (data, type, row) {
          return data ? '<span class="badge badge-outline-danger">E</span>' : '';
        }
      }, {
        data: "id",
        render: function (data, type, row) {
          return '<a href="/request/details/' + data + '">' + row.refID + '</button>';
        }
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
        data: "customerName",
        render: function (data, type, row) {
          var result = '';
          if (!app.utils.isNullOrEmpty(row.customerCode)) {
            result += '<b>' + row.customerCode.value + '</b>, ';
          }

          result += row.customerName;

          return '<p class="customer">' + result + '</p>';
        }
      }, {
        data: "id",
        width: "80px",
        render: function (data, type, row) {
          if (row.premises
            && row.premises.length > 0) {
            return '<p class="premise">' + app.utils.formatPremise(row.premises[0]) + '</p>';
          }
          return '';
        }
      }, {
        data: "submittedOn",
        render: function (data, type, row) {
          return app.utils.formatDate(data, true);
        }
      }, {
        data: "lastAction",
        render: function (data, type, row) {
          return app.utils.formatDateTime(data, true);
        }
      }, {
        data: "statusText",
        render: function (data, type, row) {
          return '<label class="badge badge-inverse-' + app.utils.requestStatusColor(row.status) + '">' + data + '</label>';
        }
      }, {
        data: "id",
        render: function (data, type, row) {
          if (row.rfAs && row.rfAs.length > 0) {
            var rfa = row.rfAs[0];
            return '<label class="badge badge-inverse-' + app.utils.rfaStatusColor(rfa.status) + '">' + rfa.statusText + '</label>'
          }
          return '';
        }
      }, {
        data: "assignedToName",
        render: function (data, type, row) {
          if (app.utils.isNullOrEmpty(data)) {
            return '';
          }

          return '<div class="d-flex align-items-center">' +
            '<span class="img-xs rounded-circle bg-' + app.utils.randomColor() + ' text-white text-avatar">' + app.utils.initials(data) + '</span>' +
            '<div class="wrapper pl-2"> ' +
            '<p class="mb-0 text-gray">' + data + '</p>' +
            '</div > ' +
            '</div>';
        }
      }],
      columnDefs: [{
        targets: [3],
        width: 80
      }, {
        targets: [5],
        width: 80
      }],
      select: {
        style: 'multi'
      },
      fixedColumns: true
    });

    $('#fixed-column_wrapper').resize(function () {
      fixedColumnTable.draw();
    });

    table.on('select', function (e, dt, type, indexes) {
      var data = app.page.dataset[indexes[0]]
      selectedData.push(data);

      setupActionButtons();
    });

    table.on('deselect', function (e, dt, type, indexes) {

      if (selectedData.length > 0) {
        var data = app.page.dataset[indexes[0]];
        selectedData = selectedData.filter(e => e.id != data.id);

        setupActionButtons();
      }
    });

    table.on('user-select', function (e, dt, type, cell, originalEvent) {
      if (cell[0][0].column == 11) {
        e.preventDefault();
      }
    });

    $('#recommendButton').click(function () {
      app.page.recommend.init(selectedData);
    });

    $('#approveAllButton').click(function () {
      app.page.approve.init(selectedData, true);
    });

    $('#rejectAllButton').click(function () {
      app.page.approve.init(selectedData, false);
    });

    $('#clearSelectionButton').click(function () {
      app.page.clearSelection();
    });
  });
}).apply(app.page);

$(document).ready(function () {
  app.page.fetch();
});