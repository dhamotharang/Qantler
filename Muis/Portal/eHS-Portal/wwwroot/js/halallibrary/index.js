(function () {
  'use strict';

  var controller;
  var nextRows = 50;
  var currentData = [];

  this.newItem = function () {
    app.page.item.init(null);
  };

  this.editItem = function (id) {
   app.page.item.init(currentData[id]);
  };

  this.viewItem = function (id) {
   app.page.establishment.init(currentData[id]);
  };

  this.fetch = function () {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    $('#listing').DataTable({
      serverSide: true,
      ordering: false,
      searching: false,
      ajax: function (data, callback, settings) {
        fetch('/api/halallibrary?offsetRows=' + data.start + '&nextRows=' + nextRows + "&" + app.page.filter.toParams(), {
          method: "GET",
          cache: 'no-cache',
          credentials: 'include',
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
          }
        })
          .then(app.http.errorInterceptor)
          .then(response => response.json())
          .then(r => {

            if (r) {
              app.dismissProgress();

              var out = [];
              currentData = r.data;
              for (var i = 0; i < r.data.length; i++) {
                out.push([
                  r.data[i].name ?
                    '<p class="name">' + r.data[i].name + '</p>' :
                    '',

                  r.data[i].brand ?
                    '<p class="brand">' + r.data[i].brand + '</p>' :
                    '',

                  r.data[i].supplier ?
                    '<p class="supplierName">' + r.data[i].supplier.name + '</p>' :
                    ' ',

                  r.data[i].certifyingBody ?
                    '<p class="certifyingBody">' + r.data[i].certifyingBody.name + '</p>' :
                    ' ',

                  r.data[i].riskCategoryText ?
                    '<span class="badge badge-pill ' + app.utils.riskColor(r.data[i].riskCategory) + '"></span>' + r.data[i].riskCategoryText :
                    ' ',

                  '<label class="badge badge-inverse-' + app.utils.halalLibraryStatusColor(r.data[i].status) + '">' + r.data[i].statusText + '</label>',

                  r.data[i].verifiedBy ?
                    '<div class="d-flex align-items-center">' +
                    '<span class="img-xs rounded-circle bg-' + app.utils.randomColor() + ' text-white text-avatar">' + app.utils.initials(r.data[i].verifiedBy.name) + '</span>' +
                    '<div class="wrapper pl-2"> ' +
                    '<p class="mb-0 text-gray">' + r.data[i].verifiedBy.name + '</p>' +
                    '</div > ' +
                    '</div>' :
                    ' ',
                  (app.hasPermission(41) ? '<button onClick="app.page.editItem(' + i + ')" data-toggle="modal" data-target="#HalallibraryDetailModal" class="btn btn-outline-primary btn-edit">Modify</button>' : '') +
                  '<button onClick="app.page.viewItem(' + i + ')" data-toggle="modal" data-target="#viewEstablishmentModal" class= "btn btn-outline-primary btn-establishment"> View Establishments</button > ']);
              }

              callback({
                draw: data.draw,
                data: out,
                recordsTotal: r.totalData,
                recordsFiltered: r.totalData
              });

            }

          }).catch(app.http.catch);
      },
      scrollY: 400,
      scrollX: true,
      sDom: "frtiS",
      bLengthChange: false,
      pageLength: nextRows,
      autoWidth: false,
      fixedColumns: true,
      scroller: {
        loadingIndicator: true,
        displayBuffer: nextRows
      },
    });
  };

  $(function () {
    app.page.item.reloadCallback = function () {
      $('#listing').DataTable().clear().destroy();
      app.page.fetch();
    }

    app.page.filter.reloadCallback = function () {
      $('#listing').DataTable().clear().destroy();
      app.page.fetch();
    }
  });

}).apply(app.page);

$(document).ready(function () {
  app.page.fetch();
});