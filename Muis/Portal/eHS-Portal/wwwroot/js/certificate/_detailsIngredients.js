(function () {
  'use strict';

  var controller;
  var id;
  var data = [];

  var table;

  this.dirtyObjects = [];

  this.init = function (d) {
    id = d;
    fetchIngredient();
  }

  function fetchIngredient() {
    if (controller) {
      return;
    }
    controller = new AbortController();

    fetch('/api/certificate/' + id + '/ingredient', {
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
        data = res;

        invalidate();

        $('.row.ingredients .grid-margin.stretch-card').addClass('d-none');

      }).catch(err => {
        controller = null;
        app.http.catch(err);
      });
  }

  this.isDirty = function () {
    return this.dirtyObjects.length > 0;
  }

  this.clearState = function () {
    this.dirtyObjects.forEach((e, i) => {
      e.oldRemarks = null;
      e.firstUpdate = false;
    });

    this.dirtyObjects = [];
  }

  this.getData = function () {
    return data;
  };

  function invalidate() {
    table.ajax.reload();
  };

  function setup() {
    table = $('#ingredients-listing').DataTable({
      scrollY: '60vh',
      scrollCollapse: false,
      deferRender: true,
      scroller: {
        displayBuffer: 100,
        rowHeight: 60
      },
      "dom": "lfrti",
      "aLengthMenu": [
        [5, 10, 15, -1],
        [5, 10, 15, "All"]
      ],
      "iDisplayLength": 5,
      "bLengthChange": false,
      ajax: function (data, callback, settings) {
        callback({ data: app.page.main.ingredients.getData() })
      },
      columns: [{
        data: "text",
        width: "100px",
        render: function (data, type, row, meta) {


          var elem = '<div class="ingredient item-row">' +
            '<div class="info">' +
            '<div>' +
            '<span class="">' + app.utils.titleCase(row.text) + '</span> ' +
            '</div>';



          elem += '</div>';

          if (!app.utils.isNullOrEmpty(row.brandName) && !app.utils.isNullOrEmpty(row.supplierName)) {
            elem += '<div class="ingredient-sub-info"><span class="sub-info-supplierName">' +
              (!app.utils.isNullOrEmpty(row.brandName) && !app.utils.isNullOrEmpty(row.supplierName) ?
                (app.utils.titleCase(row.supplierName).concat(', ')) :
                (app.utils.isNullOrEmpty(row.supplierName) ? '' : app.utils.titleCase(row.supplierName))) + '</span>' +
              '<b>' + (app.utils.isNullOrEmpty(row.brandName) ? '' : app.utils.titleCase(row.brandName)) + '</b></div>';
          }

          if (!app.utils.isNullOrEmpty(row.certifyingBodyName)) {
            elem += '<div class="ingredient-sub-info"><span class="sub-info-certifyingBody">' + app.utils.titleCase(row.certifyingBodyName) + '</span>' +
              '<sup class="text-danger">' + (row.certifyingBodyStatus == 0 ? row.certifyingBodyStatusText : ' ') + '</sup></div>';
          }

          elem += '</div></div>';

          return elem;
        }
      }]
    });
  }
  $(function () {
    setup();
  });
}).apply(app.page.main.ingredients = app.page.main.ingredients || {});

