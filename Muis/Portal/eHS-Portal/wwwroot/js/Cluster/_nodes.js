(function () {
  var IsSave = false;
  var map = new Map($('.map'));
  var name = $('#district').val();
  var generalLocation = $('#locations').val();

  this.loadMap = function () {

    if (app.page.model != null) {

      if (app.page.model.data != null) {
        var nodes = app.page.model.data.nodes.map(function (e) {
          return parseInt(e, 10).toString();
        });

        map.selectAll(nodes);
      }

      if (app.page.model.oClusters != null) {
        var nodeArray = [].concat.apply([], app.page.model.oClusters.map(x => x.nodes));
        var nodes = nodeArray.map(function (e) {
          return parseInt(e, 10).toString();
        });

        map.selectAllNodesColor(nodes, app.page.model.oClusters);
      }
    }
  }

  function validate() {
    var IsValid = true;
    var element = $('#district');

    if (app.utils.isNullOrEmpty(name)) {
      $(element).next().css('display', 'block');
      $(element).addClass('border-danger');
      IsValid = false;
    }
    else {
      $(element).next().css('display', 'none');
      $(element).prev().removeClass('border-danger');
    }
    return IsValid;
  }

  $('.saveClusterBtn').click(function () {
    IsSave = true;
    var IsValidate = validate();
    var currentNodes = [];

    (map.getSelectedIDs()).each(function (index, element) {
      currentNodes.push(("0" + element).slice(-2));
    });

    if (IsValidate) {
      var controller;
      if (controller) {
        controller.abort();
      }

      controller = new AbortController();
      var saveModel =
      {
        "ID": 0,
        "District": name,
        "Locations": generalLocation,
        "Nodes": currentNodes
      }

      app.showProgress('Processing. Please wait...');

      fetch('/api/cluster/create', {
        method: "POST",
        cache: 'no-cache',
        credentials: 'include',
        signal: controller.signal,
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(saveModel)
      }).then(app.http.errorInterceptor)
        .then(res => res.json())
        .then(r => {
          if (r > 0) {
            app.dismissProgress();
            window.location.href = "/cluster/details/" + r;
          }
        }).catch(app.http.catch);
    }
  });

  $('.saveClusterDetailsBtn').click(function () {
    IsSave = true;
    var IsValidate = validate();
    var currentNodes = [];

    (map.getSelectedIDs()).each(function (index, element) {
      currentNodes.push(("0" + element).slice(-2));
    });

    if (IsValidate) {
      var controller;
      if (controller) {
        controller.abort();
      }

      controller = new AbortController();
      var saveModel =
      {
        "ID": app.page.model.data.id,
        "District": name,
        "Locations": generalLocation,
        "Nodes": currentNodes
      }

      app.showProgress('Processing. Please wait...');

      fetch('/api/cluster', {
        method: 'PUT',
        cache: 'no-cache',
        credentials: 'include',
        signal: controller.signal,
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(saveModel)
      }).then(app.http.errorInterceptor)
        .then(res => res.json())
        .then(r => {
          if (r) {
            app.dismissProgress();
            location.reload(true);
          }
        }).catch(app.http.catch);
    }
  });

  $('.deleteClusterBtn').click(function () {
    var controller;
    if (controller) {
      controller.abort();
    }

    controller = new AbortController();

    app.showProgress('Processing. Please wait...');

    fetch('/api/cluster/' + app.page.model.data.id, {
      method: 'DELETE',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(r => {
        app.dismissProgress();
        window.location.href = "/cluster";
      }).catch(app.http.catch);
  });

  $('#district').on('change input paste keyup', function () {
    name = $(this).val().trim();
    if (IsSave) {
      if (app.utils.isNullOrEmpty(this.value)) {
        $(this).next().css('display', 'block');
        $(this).addClass('border-danger');
      }
      else {
        $(this).next().css('display', 'none');
        $(this).removeClass('border-danger');
      }
    }
  });

  $('#locations').on('change input paste', function () {
    generalLocation = $(this).val().trim();
  });

  $(function () {
    app.page.nodes.loadMap();
  });

}).apply(app.page.nodes = app.page.nodes || {});