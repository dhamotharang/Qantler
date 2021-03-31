(function () {
  var table;
  this.dataset = [];
  this.map;

  this.loadMap = function () {
    map = new Map($('#clusterMapModal .map'));
    $('#clusterMapModal .geo').addClass('readonly');

    var nodeArray = [].concat.apply([], app.page.dataset.map(x => x.nodes));
    var nodes = nodeArray.map(function (e) {
      return parseInt(e, 10).toString();
    });

    map.selectAllNodesColor(nodes, app.page.dataset);
  }
  
  this.fetch = function () {

    fetch('api/cluster/index?', {
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
        this.loadMap();
      })
      .catch(app.http.catch);
  };

  this.invalidate = function () {
    table.ajax.reload();
  }

  $(function () {

    table = $('#Clisting').DataTable({
      ajax: function (data, callback, settings) {
        callback({ data: app.page.dataset })
      },
      scrollY: '53vh',
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
        data: "district",
        render: function (data, type, row) {
          return data;
        }
      }, {
          data: "nodes",
          render: function (data, type, row) {
            var result = '';
            data.forEach(a => {
              result += a + ',';
            });
            if (result != '') {
              result = result.slice(0, -1);
            }
            return result;
          }
        }, {
        data: "locations",
        render: function (data, type, row) {
          return data;
        }
      }, {
        data: "id",
        render: function (data, type, row) {
          return '<button class="btn btn-outline-primary btn-view" data-id="' + data + '">View</button>';
        }
      }],
      columnDefs: [{
        targets: [3],
        width: 80
      }]
    });

    $('#Clisting').on('click', '.btn-view', function () {
      window.location.href = "/cluster/details/" + $(this).data('id');
    });

    $('.newClusterBtn').click(function () {
      window.location.href = "/cluster/createCluster";
    });

    $('#fixed-column_wrapper').resize(function () {
      fixedColumnTable.draw();
    });

  });

}).apply(app.page);

$(document).ready(function () {
  app.page.fetch();
});