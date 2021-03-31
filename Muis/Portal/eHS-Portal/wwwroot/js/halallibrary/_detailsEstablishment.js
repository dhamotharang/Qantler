(function () {
  'use strict';

  var datatable;

  var controller;

  var dataitem;

  var cache = {};

  this.init = function (data) {
    dataitem = [];
    dataitem = data;
    invalidate();

    if (!cache[dataitem.id]) {
      fetchData();
    }
    setTimeout(function () {
      $($.fn.dataTable.tables(true)).css('width', '100%');
      $($.fn.dataTable.tables(true)).DataTable().columns.adjust().draw();
    }, 200);
  };

  this.close = function () {
    $("#viewEstablishmentModal").removeClass("in");
    $(".modal-backdrop").remove();
    $("#viewEstablishmentModal").hide();
  };

  function invalidate() {
    datatable.ajax.reload();
  }

  function fetchData() {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    var params;
    if (dataitem.name) {
      params = 'name=' + dataitem.name;
    }
    if (dataitem.brand) {
      params += '&brand=' + dataitem.brand;
    }
    if (dataitem.supplier &&
      dataitem.supplier.name) {
      params += '&supplier=' + dataitem.supplier.name;
    }
    if (dataitem.certifyingBody &&
      dataitem.certifyingBody.name) {
      params += '&certifyingBody=' + dataitem.certifyingBody.name;
    }


    app.showProgress('Initializing. Please wait...');

    fetch('/api/certificate/with-ingredient?' + params, {
      method: 'GET',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then(res => res.json())
      .then(res => {
        app.dismissProgress();

        cache[dataitem.id] = res;

        invalidate();
      }).catch(err => {
        app.dismissProgress();
      });
  }

  $(function () {
    datatable = $('.view-establishment .table').DataTable({
      ajax: function (data, callback, settings) {
        if (dataitem && cache[dataitem.id]) {
          callback({ data: cache[dataitem.id] })
        }
      }
      ,
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
      responsive: true,
      autoWidth: false,
      columns: [{
        title: 'Name',
        data: "customer",
        render: function (data, type, row) {
          return '<a href="/customer/details/' + row.customerID + '">' + data.name + '</button>';
        }
      }, {
        title: 'Customer Code',
        data: "customer",
        render: function (data, type, row) {
          if (data.code
            && data.code.id > 0) {
            return data.code.value
          }
          return '';
        }
      }, {
        title: 'Group Code',
        data: "customer",
        render: function (data, type, row) {
          if (data.groupCode
            && data.groupCode.id > 0) {
            return data.groupCode.value
          }
          return '';
        }
      }, {
        title: 'Certificate #',
        data: "number",
        render: function (data, type, row) {
          return '<a href="/certificate/details/' + row.id + '">' + data + '</button>';
        }
      }, {
        title: 'Issued On',
        data: "issuedOn",
        render: function (data, type, row) {
          return app.utils.formatDate(data, true);
        }
      }, {
        title: 'Premise',
        data: "premise",
        render: function (data, type, row) {
          return '<p class="premise">' + app.utils.formatPremise(data) + '</p>';
        }
      }],
      columnDefs: [],
      fixedColumns: true
    });
  });

}).apply(app.page.establishment = app.page.establishment || {});
