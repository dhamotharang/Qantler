var type;
var table;
var data;
var dataset = [];
$('#masterTypeSelect').select2();

(function () {

  this.initItem = function (id) {
    $(".modal-backdrop").show();
    $("#MasterDetailModal").show();
    $("#MasterDetailModal").addClass("show")
    var index = data.findIndex(x => x.id == id);
    app.page.Item.init(data[index],type);
  };

  this.deleteItemMsg = function deleteItemMsg(id) {
    var index = data.findIndex(x => x.id == id);

    swal.fire({
      title: 'Are you sure?',
      text: "You want to delete the reason",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#2196f3',
      confirmButtonText: 'Ok ',
      cancelButtonColor: '#ff6258',
      cancelButtonText: 'Cancel ',
      reverseButtons: true
    }).then((result) => {
      if (result.isConfirmed) {
        app.page.deleteItem(index);
      }
    });
  };

  this.change = function change(id) {
    if (app.page.model.type != id) {
      var url = window.location.origin + "/master/" + id;
      window.location.href = url;
    }
    else {
      app.page.load(id);
    }
  }

  this.load = function load(id) {
    type = id;

    app.showProgress('Processing. Please wait...');

    fetch('/api/master/' + id, {
      method: 'get',
      cache: 'no-cache',
      credentials: 'include',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(r => {
        app.dismissProgress();

        data = r;
        dataset = r;

        app.page.invalidate();
      }).catch(app.http.catch);
  };

  this.deleteItem = function deleteItem(index) {
    app.showProgress('Processing. Please wait...');

    fetch('/api/master/' + data[index].id + '/' + type, {
      method: 'delete',
      cache: 'no-cache',
      credentials: 'include',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(response => response.json())
      .then(r => {
        if (r) {
          app.dismissProgress();
          location.reload(true);
        }
      })
      .catch(app.http.catch);
  };

  this.invalidate = function invalidate() {
    table.ajax.reload();
  };

  $(function () {
    table = $('#listing').DataTable({
      ajax: function (data, callback, settings) {
        callback({ data: dataset })
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
        data: "value",
        render: function (data, type, row) {
          return data;
        }
      }, {
        data: "id",
        render: function (data, type, row) {
          return `<button data-toggle="modal"onclick="app.page.initItem('` + data + `')" class="btn btn-outline-primary btn-edit">Edit</button>
                        <button data-toggle="modal"  onclick="app.page.deleteItemMsg('`+ data + `')" class="btn btn-outline-primary btn-delete">Delete</button>`;
        }
      }],
      columnDefs: [{
        targets: [1],
        width: 100
      }],
      fixedColumns: true
    });

    $('#fixed-column_wrapper').resize(function () {
      fixedColumnTable.draw();
    });
  });
}).apply(app.page);

$(document).ready(function () {
  $('#masterTypeSelect').val(app.page.model.type);
  $('#masterTypeSelect').trigger('change');
});