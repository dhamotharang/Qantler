(function () {
  'use strict';

  var controller;
  var table;
  var dataset = [];
  this.cache = [];

  var groupingName = ["Today", "Last Week", "Two Weeks Ago", "Three Weeks Ago", "Last Month", "Older", "Four Weeks Ago"]

  this.fetch = function () {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    fetch('api/case/index?' + app.page.filter.toParams(), {
      method: 'GET',
      cache: 'no-cache',
      credentials: 'include',
      headers: {
        'Accept': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(data => {
        app.page.cache = data;
        app.page.reloadTable();
      })
      .catch(app.http.catch);
  };

  this.reloadTable = function () {
    table.ajax.reload();
  };

  this.insertDataSet = function (key, value) {
    var map = {};
    if (!dataset.find(x => x.date == key)) {
      map = {
        date: key,
        data: []
      };
      map.data.push(value);
      dataset.push(map);
    }
    else {
      dataset.find(x => x.date == key).data.push(value);
    }
  };

  this.groupData = function () {
    var data = [];
    dataset = [];

    app.page.cache = app.page.cache.sort((a, b) => moment(b.createdOn).diff(moment(a.createdOn)));

    $.each(app.page.cache, function (key, value) {
      var tempWeek = moment.utc(value.createdOn).local().week();
      var todayWeek = moment.utc().local().week();

      var tempMonth = moment.utc(value.createdOn).local().month();
      var todayMonth = moment.utc().local().month();

      var tempYear = moment.utc(value.createdOn).local().year();
      var todayYear = moment.utc().local().year();

      if (todayYear == tempYear && tempMonth == todayMonth && tempWeek == todayWeek) {
        var reqDate = parseInt(moment.utc(value.createdOn).local().format("DD"));
        var currentDate = parseInt(moment.utc().local().format("DD"))
        var dayDiff = currentDate - reqDate;

        if (dayDiff != 0) {
          value.thisWeek = true;
          app.page.insertDataSet((moment.utc(value.createdOn).local().format('dddd')), value)
        }
        else {
          value.thisWeek = true;
          app.page.insertDataSet(groupingName[0], value)
        }

      }
      else if ((todayYear == tempYear && tempMonth == todayMonth) ||
        ((todayYear - tempYear == 1) && (todayMonth == 0 && tempMonth == 11))) {

        if (todayWeek - tempWeek == 1) {
          value.thisWeek = false;
          app.page.insertDataSet(groupingName[1], value)
        }
        else if (todayWeek - tempWeek == 2) {
          value.thisWeek = false;
          app.page.insertDataSet(groupingName[2], value)
        }
        else if (todayWeek - tempWeek == 3) {
          value.thisWeek = false;
          app.page.insertDataSet(groupingName[3], value)
        }
        else if (todayWeek - tempWeek == 4 && tempMonth == todayMonth) {
          value.thisWeek = false;
          app.page.insertDataSet(groupingName[6], value)
        }
        else {
          value.thisWeek = false;
          app.page.insertDataSet(groupingName[4], value)
        }

      }
      else if (todayMonth - tempMonth == 1 && todayYear == tempYear) {
        value.thisWeek = false;
        app.page.insertDataSet(groupingName[4], value)
      }
      else {
        value.thisWeek = false;
        app.page.insertDataSet(groupingName[5], value)
      }

    });

    $.each(dataset, function (key, value) {
      value.data = value.data.sort((a, b) => moment(b.createdOn).diff(moment(a.createdOn)));

      $.each(value.data, function (key1, value1) {
        if (key1 == 0) {
          value1.isFirst = true;
          value1.groupName = value.date
        }
        else {
          value1.isFirst = false;
          value1.groupName = value.date
        }

        data.push(value1);
      });
    });

    return data;
  };

  this.renderDataTableRow = function (d) {
    var template = '';
    template = template + $('.item').html();

    if (d.isFirst) {
      template = template.replaceAll("{group-name}", d.groupName)
    }
    else {
      template = template.replaceAll("{group-name-show}", "d-none");
    }

    template = template.replaceAll("{ID}", d.id).replaceAll("{offenceType}", d.title)
      .replaceAll("{status}", d.statusText).replaceAll("{status-color}", app.common.caseStatusColor(d.status));

    if (d.thisWeek == true) {
      template = template.replaceAll("{time}", app.utils.formatTime(d.createdOn, true));
    }
    else {
      template = template.replaceAll("{time}", moment.utc(d.createdOn).local().format('DD MMM YYYY'));
    }

    if (d.typeText != null && d.typeText != "") {
      template = template.replaceAll("{type}", d.typeText)
    }
    else {
      template = template.replaceAll("{type}", "")
    }

    if (d.sanctionText != null && d.sanctionText != "") {
      template = template.replaceAll("{sanctionValue}", d.sanctionText);
    }
    else {
      template = template.replaceAll("{sanction-show}", "d-none");
    }

    if (d.assignedToID != null && d.assignedToID != "") {
      template = template.replaceAll("{assignedToValue}", d.assignedTo.name)
        .replaceAll("{assignedToInitial}", app.utils.initials(d.assignedTo.name)).
        replaceAll("{assignedTo-random-color}", app.utils.randomColor());
    }
    else {
      template = template.replaceAll("{assigned-to-show}", "d-none");
    }

    if (d.managedByID != null && d.managedByID != "" && d.managedByID != app.identity.id) {
      template = template.replaceAll("{managedByValue}", d.managedBy.name)
        .replaceAll("{managedByInitial}", app.utils.initials(d.managedBy.name)).
        replaceAll("{managedBy-random-color}", app.utils.randomColor());
    }
    else {
      template = template.replaceAll("{managed-by-show}", "d-none");
    }

    if (d.reportedBy != null && d.reportedBy != "") {
      template = template.replaceAll("{reportedByInitial}", app.utils.initials(d.reportedBy.name)).
        replaceAll("{reportedBy-random-color}", app.utils.randomColor()).
        replaceAll("{reportedByValue}", d.reportedBy.name);
    }
    else {
      template = template.replaceAll("{reported-by-show}", "d-none");
    }

    if (d.offenderID != null && d.offenderID != "") {
      template = template.replaceAll("{offenderValue}", d.offender.name)
        .replaceAll("{offenderInitial}", app.utils.initials(d.offender.name)).
        replaceAll("{offender-random-color}", app.utils.randomColor());
    }
    else {
      template = template.replaceAll("{offender-show}", "d-none");
    }

    if (d.sourceText != null && d.sourceText != "") {
      template = template.replaceAll("{sourceValue}", d.sourceText);
    }
    else {
      template = template.replaceAll("{source-show}", "d-none");
    }

    return template;
  };

  this.toDetails = function (id) {
    var url = window.location.origin + "/case/details/" + id;
    window.location.href = url;
  };

  $(function () {
    table = $('#JCase').DataTable({
      ajax: function (data, callback, settings) {
        callback({ data: app.page.groupData() })
      },
      scrollY: '80vh',
      scrollX: false,
      scrollCollapse: true,
      deferRender: true,
      bSort: false,
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
        render: function (data, type, row) {
          return app.page.renderDataTableRow(row);
        }
      }],
      fixedColumns: true
    });

    $('#JCase').on('search.dt', function () {
      var value = $('.dataTables_filter input').val();
      if (value != "") {
        $('.tickets-date-group').hide()
      }
      else {
        $('.tickets-date-group').show()
      }
    });

    $('#fixed-column_wrapper').resize(function () {
      fixedColumnTable.draw();
    });
  });

}).apply(app.page);

$(document).ready(function () {
  app.page.fetch();
});