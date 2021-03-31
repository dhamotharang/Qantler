(function (self) {
  const KEY_ASSIGNED = 0;
  const KEY_PROCESSED = 1;
  const KEY_NEW = 2;
  const KEY_CLOSED = 3;

  var controller;

  var labels = [];
  var dataHolder = [];

  var data;

  var charts = {};

  var hasInit;

  self.init = function (o) {
    options = o;

    var i;
    for (i = options.noOfPoints - 1; i >= 0; i--) {
      var d = moment().add(-i, 'M');

      labels.push(d.format('MMM YYYY'));
    }

    setupCharts();

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

        var label = date.format('MMM YYYY');

        setData(KEY_ASSIGNED, label, e.assigned);
        setData(KEY_PROCESSED, label, e.processed);
        setData(KEY_NEW, label, e.new);
        setData(KEY_CLOSED, label, e.closed);
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
    $('.statistics-performance').each(function () {
      $(this).addClass('busy');
    });
  }

  function hideLoader() {
    $('.statistics-performance').each(function () {
      $(this).removeClass('busy');
    });
  }

  function invalidateChart() {
    charts[KEY_ASSIGNED].update();
    charts[KEY_PROCESSED].update();
    charts[KEY_NEW].update();
    charts[KEY_CLOSED].update();
  }

  function refreshInfo() {
    updateInfo(KEY_ASSIGNED);
    updateInfo(KEY_PROCESSED);
    updateInfo(KEY_NEW);
    updateInfo(KEY_CLOSED);
  }

  function refreshChart() {
    updateChart(KEY_ASSIGNED);
    updateChart(KEY_PROCESSED);
    updateChart(KEY_NEW);
    updateChart(KEY_CLOSED);
  }

  function updateInfo(key) {
    var container = $('.statistics-performance:eq(' + key + ')');

    var val = getValue(key, options.currentIndex);
    var oldVal = getValue(key, options.currentIndex - 1);

    container.find('h4').html(val);

    var diff = val - oldVal;

    container.find('h6').html(diff > 0 ? '+' + diff : diff);
    container.find('h6').removeClass('text-danger');
    container.find('h6').removeClass('text-success');

    var growth = oldVal == 0 ? diff * 100 : parseInt((diff / oldVal) * 100);

    container.find('h6').addClass(diff > 0 ? 'text-success' : 'text-danger');

    var suffix = diff == 0 ? '% growth'
      : (diff > 0 ? '% higher growth' : '% lower growth');
    container.find('small').html(Math.abs(growth) + suffix);
  }

  function updateChart(key) {
    var chart = charts[key];

    var i;
    for (i = 0; i < options.noOfPoints; i++) {
      var e = getValue(key, i);
      chart.data.datasets[0].data[i] = e;
    }

    chart.update();
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

  function setupCharts() {
    if ($('.statistics-performance .assigned canvas').length) {
      var canvas = $('.statistics-performance .assigned canvas').get(0).getContext("2d");
      charts[KEY_ASSIGNED] = setupChart(canvas, warningColor);
    }

    if ($('.statistics-performance .processed canvas').length) {
      var canvas = $('.statistics-performance .processed canvas').get(0).getContext("2d");
      charts[KEY_PROCESSED] = setupChart(canvas, infoColor);
    }

    if ($('.statistics-performance .new canvas').length) {
      var canvas = $('.statistics-performance .new canvas').get(0).getContext("2d");
      charts[KEY_NEW] = setupChart(canvas, primaryColor);
    }

    if ($('.statistics-performance .closed canvas').length) {
      var canvas = $('.statistics-performance .closed canvas').get(0).getContext("2d");
      charts[KEY_CLOSED] = setupChart(canvas, dangerColor);
    }
  }

  function setupChart(canvas, color) {
    var lineChartStyleOption = {
      responsive: true,
      scales: {
        yAxes: [{
          ticks: {
            suggestedMin: 0,
            beginAtZero: true,
          },
          display: false
        }],
        xAxes: [{
          display: false
        }]
      },
      legend: {
        display: false
      },
      elements: {
        point: {
          radius: 0
        },
        line: {
          tension: 0
        }
      },
      layout: {
        padding: {
          top: 4
        }
      },
      onHover: function (evt, item) {
        var index = this.getElementsAtXAxis(evt)[0]._index;

        if (self.indexChangeHandler) {
          self.indexChangeHandler(index);
        }

      },
      tooltips: {
        enabled: false
      },
      scaleOverride: true,
      stepsize: 1,
      scaleStartValue: 0
    };

    var gradientStrokeFill = canvas.createLinearGradient(1, 2, 1, 400);
    gradientStrokeFill.addColorStop(0, '#fff');
    gradientStrokeFill.addColorStop(1, color);

    var lineChart = new Chart(canvas, {
      type: 'line',
      data: {
        labels: labels,
        datasets: [{
          label: 'Value',
          data: [],
          borderColor: color,
          backgroundColor: gradientStrokeFill,
          borderWidth: 2,
          pointStyle: 'circle',
          pointRadius: function (ctx) {
            return ctx.dataIndex == options.currentIndex ? 3 : 0;
          },
          pointBackgroundColor: 'white',
          fill: true
        }]
      },
      options: lineChartStyleOption
    });

    return lineChart;
  }

  function fetchData() {
    if (controller) {
      controller.abot();
    }
    controller = new AbortController();

    showLoader();

    var query = 'from=' + options.from.format('YYYY-MM-DD');

    if (!app.hasPermission(7)
      && !app.hasPermission(30)) {
      var keys = [];

      if (app.hasPermission(1)) {
        keys.push(app.identity.id);
      }

      if (app.hasPermission(2)) {
        keys.push('AO');
      }

      if (app.hasPermission(3)
        || app.hasPermission(4)) {
        keys.push('IO');
      }

      if (app.hasPermission(14)) {
        keys.push('Finance');
      }

      if (keys.length > 0) {
        keys.forEach((e, i) => {
          query += '&keys[' + i + ']=' + e;
        });
      }
    }

    fetch('/api/statistics/performance?' + query, {
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
        refreshChart();
        hideLoader();
      })
      .catch(err => {
        hideLoader();
      });
  }

})(app.page.performance = app.page.performance || {});