(function () {
  'use strict';

  var table;
  var selectedData = [];

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

    fetch('api/delivery/index?' + params, {
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

  function allAreEqual(array) {
    if (!array.length) return true;
    return array.reduce(function (a, b) { return (a === b) ? a : (!b); }) === array[0];
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




    var deliverBtn = $('#deliverButton');
    var clearSelectionBtn = $('#clearSelectionButton');

    if (allAreEqual(statuses)) {
      if (statuses[0] == 100) {
        app.utils.show(deliverBtn);
        app.utils.show(clearSelectionBtn);
      }
      else {
        hideAllActionButtons();
      }
    }
    else {
      hideAllActionButtons();
    }

    app.utils.show(clearSelectionBtn);
  };


  function findCommonElements(arr1, arr2) {
    return arr1.some(item => arr2.includes(item))
  }

  function hideAllActionButtons() {
    app.utils.hide($('#deliverButton'));
    app.utils.hide($('#clearSelectionButton'));
  }

  function executeDelivery(certIDs) {
    fetch('/api/delivery/deliver', {
      method: 'POST',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(certIDs)
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(r => {
        location.reload(true);
        Swal.fire(
          'Delivered!',
          'Selected certificates were delivered.',
          'success'
        )
      })
      .catch(error => {
        isBusy = false;
        app.http.catch(error);
      });
  }

  $(function () {
    table = $('#Dlisting').DataTable({
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
        data: "refID",
        render: function (data, type, row) {
          return '<a href="/request/details/' + row.requestID + '">' + data + '</button>';
        }
      }, {
        data: "customerCode",
        render: function (data, type, row) {
          var result = '';
          if (!app.utils.isNullOrEmpty(data.value)) {
            result += '<b>' + data.value + '</b>, ';
          }

          result += row.customerName;

          return '<p class="customer">' + result + '</p>';
        }
      }, {
        data: "number",
        render: function (data, type, row) {
          return data;
        }
      }, {
        data: "serialNo",
        render: function (data, type, row) {
          return data;
        }
      }, {
        data: "issuedOn",
        render: function (data, type, row) {
          return app.utils.formatDate(data, true);
        }
      }, {
        data: "statusText",
        render: function (data, type, row) {
          return '<label class="badge badge-inverse-' + app.utils.certDeliveryStatusColor(row.status) + '">' + data + '</label>';
        }
      }, {
        data: "premise",
        render: function (data, type, row) {
          return '<p class="premise">' + app.utils.formatPremise(data) + '</p>';
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

    $('#clearSelectionButton').click(function () {
      app.page.clearSelection();
    });

    $('#deliverButton').click(function () {
      // validate premise 
      var holder = [];
      var selectedPremIDs = selectedData.map(e => e.premiseID).filter(e => {
        if (holder.indexOf(e) != -1) {
          return false;
        }
        holder.push(e);
        return true;
      });


      var holder = [];
      var selectedCertIDs = selectedData.map(e => e.id).filter(e => {
        if (holder.indexOf(e) != -1) {
          return false;
        }
        holder.push(e);
        return true;
      });


      var notSelPremiseIDs = [];
      for (let i = 0; i < app.page.dataset.length; i++) {
        if (jQuery.inArray(app.page.dataset[i].id, selectedCertIDs) != -1) {

        } else {
          notSelPremiseIDs.push(app.page.dataset[i].premiseID);
        }
      }

      var result = findCommonElements(notSelPremiseIDs, selectedPremIDs);

      if (result) {
        Swal.fire({
          title: 'Same Premises were not selected, Do you want to proceed?',
          icon: 'warning',
          showCancelButton: true,
          confirmButtonText: 'Proceed'
        }).then((result) => {
          if (result.isConfirmed) {
            executeDelivery(selectedCertIDs);
          }
        });
      }
      else {
        executeDelivery(selectedCertIDs);
      }

    });

  });


}).apply(app.page);

$(document).ready(function () {
  app.page.fetch();
});