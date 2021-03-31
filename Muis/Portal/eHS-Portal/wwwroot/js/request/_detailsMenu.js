$.fn.dataTable.ext.search.push(
  function (settings, data, dataIndex) {
    if (settings.nTable.id === 'menu-listing') {
      var schemeID = $('ul#request-menu >li a.active').attr('data-id');
      var undeclared = $('#Undeclared').prop('checked');
      return (settings.aoData[dataIndex]._aData.scheme === Number(schemeID) &&
        settings.aoData[dataIndex]._aData.undeclared == undeclared);
    }

    return true;
  }
);

(function () {
  'use strict';

  var controller;
  var data = [];
  var table;
  var editor;
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
    return data || [];
  };

  function invalidate() {
    table.ajax.reload();
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

    fetch('/api/request/menus/review', {
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

        app.page.main.menu.clearState();

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

  function setup() {
    table = $('#menu-listing').DataTable({
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
        callback({ data: app.page.main.menu.getData() })
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

          var elem = '<div class="menu item-row">' +
            '<div class="info">' +
            '<div>' +
            '<span class="' + styleSpan + '">' + app.utils.titleCase(row.text) + '</span> ' +
            '<sup class="' + style + '">' + statustext + '</sup>' +
            '</div>';

          if (row.approved != null) {
            elem += '<span class="' + (row.approved ? 'text-success' : 'text-danger') + '">' + (row.approved ? 'Verified' : 'Unverified') + '</span>';
          };

          elem += '</div>';

          if (!app.utils.isNullOrEmpty(row.subText)) {
            elem += '<span class="sub-info ' + styleSpan + '">' + app.utils.titleCase(row.subText) + '</span>';
          }

          var showRemarks = !app.utils.isNullOrEmpty(row.remarks)
            || row.addRemarks;

          if (app.page.main.menu.isEditable && !showRemarks) {
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

    $('#menu-listing').each(function () {
      var filter = '<label class="Undeclared-label"><input id="Undeclared" type = "checkbox"> Undeclared </label>';

      $(this).closest('.dataTables_wrapper').find(".dataTables_filter").append(filter);
    });

    $('#Undeclared').iCheck({
      checkboxClass: 'icheckbox_flat-blue',
      radioClass: 'iradio_flat'
    });

    $('#Undeclared').on('ifChanged', function (e) {
      table.draw();
    });
  }

  $(function () {

    $(document).on('click', 'ul#request-menu >li a', function () {
      if ($(this).attr('data-type') == 'menu' && app.page.model.menus.length != 0) {
        table.draw();
      }

    });

    $(document).on('change', '#Undeclared', function () {
      if ($('ul#request-menu >li a.active').attr('data-type') == 'menu' && app.page.model.menus.length != 0) {
        table.draw();
      }

    });

    $(document).on('click', '.menu.item-row .add-remarks', function () {
      var index = $(this).data('row');
      app.page.main.menu.getData()[index].addRemarks = true;

      $('.menu.item-row .remarks[data-row="' + index + '"]').removeClass('d-none');
      $(this).addClass('d-none');
    });

    $(document).on('change input keyup paste', '.menu.item-row .remarks input', function () {
      var index = $(this).data('row');

      var data = app.page.main.menu.getData()[index];
      if (!data.firstUpdate) {
        data.firstUpdate = true;
        data.oldRemarks = data.remarks;
      }
      data.remarks = $(this).val().trim();

      var isDirty = (app.utils.isNullOrEmpty(data.oldRemarks) ? '' : data.oldRemarks) != data.remarks;

      var dirtyObjects = app.page.main.menu.dirtyObjects.filter(e => {
        return e.id != data.id;
      });

      if (isDirty) {
        dirtyObjects.push(data);
      }

      app.page.main.menu.dirtyObjects = dirtyObjects;

      app.page.onDirtyStateChanged();
    });
  });

}).apply(app.page.main.menu = app.page.main.menu || {});
