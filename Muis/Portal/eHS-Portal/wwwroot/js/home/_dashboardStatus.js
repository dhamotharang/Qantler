(function (self) {
  const KEY_NEW = 0;
  const KEY_CLOSED = 1;
  const KEY_REJECTED = 2;

  var controller;

  var labels = [];
  var dataHolder = [];

  var data;

  var chart;

  var hasInit;

  var options;

  self.init = function (o) {
    options = o;

    var i;
    for (i = options.noOfPoints - 1; i >= 0; i--) {
      var d = moment().add(-i, 'M');

      labels.push(d.format('MMM \'YY'));
    }

    setupChart();

    fetchData();
  }

  self.invalidate = function () {
    if (hasInit) {
      refreshInfo();
      invalidateChart();
    }
  }

  function prepareData(d) {
    data = d;

    if (d) {
      d.forEach((e, i) => {
        var date = moment(e.monthYear);

        var label = date.format('MMM \'YY');

        setData(KEY_NEW, label, e.new);
        setData(KEY_CLOSED, label, e.closed);
        setData(KEY_REJECTED, label, e.rejected);
      });
    }
  }

  function setData(key, label, e) {
    var index = labels.indexOf(label);

    var dataset = dataHolder[key];
    if (!dataset) {
      dataset = [];
      dataHolder[key] = dataset;
    }

    dataset[index] = e;
  }

  function showLoader() {
    $('.statistics-status').each(function () {
      $(this).addClass('busy');
    });
  }

  function hideLoader() {
    $('.statistics-status').each(function () {
      $(this).removeClass('busy');
    });
  }

  function invalidateChart() {
    var newData = getValue(KEY_NEW, options.currentIndex);
    var renewal = getValue(KEY_CLOSED, options.currentIndex);
    var amend = getValue(KEY_REJECTED, options.currentIndex);

    chart.data.datasets[0].data[KEY_NEW] = newData;
    chart.data.datasets[0].data[KEY_CLOSED] = renewal;
    chart.data.datasets[0].data[KEY_REJECTED] = amend;

    chart.update();
  }

  function refreshInfo() {
    var newData = getValue(KEY_NEW, options.currentIndex);
    var closed = getValue(KEY_CLOSED, options.currentIndex);
    var rejected = getValue(KEY_REJECTED, options.currentIndex);

    var total = newData + closed + rejected;

    invalidateInfo(KEY_NEW, parseInt((newData / total) * 100));
    invalidateInfo(KEY_CLOSED, parseInt((closed / total) * 100));
    invalidateInfo(KEY_REJECTED, parseInt((rejected / total) * 100));

    $('.statistics-status .header h4').html(total);
    $('.statistics-status .header small').html('Units');
  }

  function invalidateInfo(key, val) {
    var container = $('.statistics-status .info:eq(' + key + ')');

    container.find('p:last-child').html(val + '%');
  }

  function getValue(key, i) {
    var dataset = dataHolder[key];
    if (!dataset) {
      dataset = [];
    }

    var e = dataset[i];
    if (!e) {
      e = 0;
    }
    return e;
  }

  function setupChart() {
    var canvas = $('.statistics-status canvas').get(0).getContext("2d");

    chart = new Chart(canvas, {
      type: 'doughnut',
      data: {
        datasets: [{
          data: [],
          backgroundColor: [
            primaryColor,
            successColor,
            dangerColor
          ],
          borderColor: [
            primaryColor,
            successColor,
            dangerColor
          ],
        }],
        labels: [
          'New',
          'Closed',
          'Rejected'
        ]
      },
      options: {
        cutoutPercentage: 75,
        animationEasing: "easeOutBounce",
        animateRotate: true,
        animateScale: false,
        responsive: true,
        maintainAspectRatio: true,
        showScale: true,
        legend: {
          display: false
        },
        layout: {
          padding: {
            left: 0,
            right: 0,
            top: 0,
            bottom: 0
          }
        },
        events: []
      }
    });
  }

  function fetchData() {
    if (controller) {
      controller.abot();
    }
    controller = new AbortController();

    showLoader();

    fetch('/api/statistics/status?from=' + options.from.format('YYYY-MM-DD'), {
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
        hasInit = true;
        prepareData(res);
        refreshInfo();
        invalidateChart();
        hideLoader();
      })
      .catch(err => {
        hideLoader();
      });
  }

})(app.page.status = app.page.status || {});