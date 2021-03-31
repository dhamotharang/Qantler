$.fn.dataTable.ext.search.push(
  function (settings, data, dataIndex) {
    if (settings.nTable.id === 'ingredients-listing') {
      var category = $('#riskCategorySelect').select2('data');
      var unDeclared = $('#ingredientUndeclared').prop('checked');

      if (!category || category.length == 0) {
        return settings.aoData[dataIndex]._aData.undeclared === unDeclared;
      }

      return settings.aoData[dataIndex]._aData.riskCategory === parseInt(category[0].id) &&
        settings.aoData[dataIndex]._aData.undeclared === unDeclared;
    }

    return true;
  }
);

(function () {
  'use strict';

  var controller;
  var data = [];
  var table;
  var controller;

  this.isEditable;

  this.dirtyObjects = [];

  this.init = function (d, editable) {
    data = d;
    this.isEditable = editable;

    setup();
    invalidate();
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

  this.save = function (onSuccess, onFail) {

    if (!this.isDirty()) {
      onSuccess();
      return;
    }

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    fetch('/api/request/ingredients/review', {
      method: 'PUT',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(this.dirtyObjects)
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(r => {

        app.page.main.ingredients.clearState();

        if (onSuccess) {
          onSuccess();
        }
      })
      .catch(error => {

        if (onFail) {
          onFail(error);
        }
      });

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

          var style = '';
          var statustext = '';
          var styleSpan = '';

          switch (row.changeType) {
            case 1:
              style = 'text-success';
              statustext = 'New';
              styleSpan = '';
              break;
            case 2:
              style = 'text-warning';
              statustext = 'Updated';
              styleSpan = '';
              break;
            case 3:
              style = 'text-danger';
              statustext = 'Removed';
              styleSpan = 'text-line-through';
              break;
          }

          var elem = '<div class="ingredient item-row">' +
            '<div class="info">' +
            '<div>' +
            '<span class="badge badge-pill ' + app.utils.riskColor(row.riskCategory) + '"></span>' +
            '<span class="">' + app.utils.titleCase(row.text) + '</span> ' +
            '<sup class="text-danger">' + (row.status == 0 ? row.ingredientStatusText : ' ') + '</sup>' +
            '</div>';

          if (row.approved != null) {
            elem += '<span class="' + (row.approved ? 'text-success' : 'text-danger') + '">' + (row.approved ? 'Verified' : 'Unverified') + '</span>';
          };

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

          var showRemarks = !app.utils.isNullOrEmpty(row.remarks)
            || row.addRemarks;

          if (app.page.main.menu.isEditable
            && !showRemarks) {
            elem += '<span class="add-remarks" data-row="' + meta.row + '">+ Add remarks</span>';
          }

          elem += '<div class="remarks ' + (showRemarks ? '' : 'd-none') + '" data-row="' + meta.row + '">';

          var remarks = app.utils.isNullOrEmpty(row.remarks) ? '' : row.remarks;

          if (app.page.main.menu.isEditable) {
            elem += '<input type="text" maxlength="2000" placeholder="Enter remarks here" data-row="' + meta.row + '" value="' + remarks + '">';
          } else {
            elem += '<p>' + remarks + '<p>';
          }

          elem += '</div></div>';

          return elem;
        }
      }]
    });

    $('#ingredients-listing').each(function () {
      var datatable = $(this);

      var filter = '<select id="riskCategorySelect" class="select-multiple select2-hidden-accessible" style="width:160px;text-align:left;" aria-hidden="true">' +
        '<option value="400">High Risk</option>' +
        '<option value="300">Medium-High Risk</option>' +
        '<option value="200">Medium-Low Risk</option>' +
        '<option value="100">Low Risk</option>' +
        '<option value="500">Non-Halal</option>' +
        '<option value="999">Uncategorized</option>' +
        '</select>' +
        '<label class="Undeclared-label"> <input id="ingredientUndeclared" type="checkbox"> Undeclared </label>';

      datatable.closest('.dataTables_wrapper').find(".dataTables_filter").append(filter);
    });

    $('.ingredients .select-multiple').select2({
      placeholder: "Select a category",
      allowClear: true
    });

    $('#ingredientUndeclared').iCheck({
      checkboxClass: 'icheckbox_flat-blue',
      radioClass: 'iradio_flat'
    });

    $('#riskCategorySelect').on('select2:select select2:unselecting', function (e) {
      table.draw();
    });

    $('#ingredientUndeclared').on('ifChanged', function (e) {
      table.draw();
    });
  }

  $(function () {

    $(document).on('click', '.ingredient.item-row .add-remarks', function () {
      var index = $(this).data('row');
      app.page.main.ingredients.getData()[index].addRemarks = true;

      $('.ingredient.item-row .remarks[data-row="' + index + '"]').removeClass('d-none');
      $(this).addClass('d-none');
    });

    $(document).on('change input keyup paste', '.ingredient.item-row .remarks input', function () {
      var index = $(this).data('row');

      var data = app.page.main.ingredients.getData()[index];
      if (!data.firstUpdate) {
        data.firstUpdate = true;
        data.oldRemarks = data.remarks;
      }
      data.remarks = $(this).val().trim();

      var isDirty = (app.utils.isNullOrEmpty(data.oldRemarks) ? '' : data.oldRemarks) != data.remarks;

      var dirtyObjects = app.page.main.ingredients.dirtyObjects.filter(e => {
        return e.id != data.id;
      });

      if (isDirty) {
        dirtyObjects.push(data);
      }

      app.page.main.ingredients.dirtyObjects = dirtyObjects;

      app.page.onDirtyStateChanged();
    });
  });
}).apply(app.page.main.ingredients = app.page.main.ingredients || {});

