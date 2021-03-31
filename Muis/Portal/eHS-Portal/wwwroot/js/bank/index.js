(function (self) {
  var table;

  var dataset;

  var controller;

  self.fetch = function () {
    if (controller) {
      controller.abort();
    }

    var params = self.filter.toParams();

    if (!params) {
      Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: 'Please provide alteast one value from one of the fields.'
      })
      return;
    }

    controller = new AbortController();

    fetch('/api/bank/index/list?' + params, {
      method: 'GET',
      cache: 'no-cache',
      credentials: 'include',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(data => {
        dataset = data;
        self.invalidate();
      })
      .catch(app.http.catch);

    return true;
  }

  self.invalidate = function () {
    table.ajax.reload();
  }  

  $(function () {
    dataset = self.model.dataset || [];

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
      searching: true,
      responsive: true,
      autoWidth: false,
      columns: [{
        data: "accountNo"
      }, {
        data: "accountName"
      }, {
        data: "branchCode"
      }, {
        data: "bankName"
      }, {
        data: "ddaStatus",
        render: function (data, type, row) {
          return (row.ddaStatus == 200 ? '<span class="badge badge-outline-success ml-2 info-status" style="">DDA Approved</span>' : '');
        }
      }],
      fixedColumns: true
    });

    self.filter.init(self.model.banks);    
  });

})(app.page);