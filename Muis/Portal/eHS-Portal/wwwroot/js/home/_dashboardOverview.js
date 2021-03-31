(function (self) {
  const KEY_NEW = 0;
  const KEY_RENEWAL = 1;
  const KEY_AMEND = 2;

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
        setData(KEY_RENEWAL, label, e.renewal);
        setData(KEY_AMEND, label, e.amend);
      });
    }
    console.log(dataHolder);
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
    $('.statistics-overview').each(function () {
      $(this).addClass('busy');
    });
  }

  function hideLoader() {
    $('.statistics-overview').each(function () {
      $(this).removeClass('busy');
    });
  }

  function invalidateChart() {
    chart.update();
  }

  function refreshInfo() {
    invalidateInfo(KEY_NEW);
    invalidateInfo(KEY_RENEWAL);
    invalidateInfo(KEY_AMEND);

    var currTotal = getValue(KEY_NEW, options.currentIndex)
      + getValue(KEY_RENEWAL, options.currentIndex)
      + getValue(KEY_AMEND, options.currentIndex);

    var oldTotal = getValue(KEY_NEW, options.currentIndex - 1)
      + getValue(KEY_RENEWAL, options.currentIndex - 1)
      + getValue(KEY_AMEND, options.currentIndex - 1);

    var diff = currTotal - oldTotal;

    var growth = oldTotal == 0 ? diff * 100 : parseInt((diff / oldTotal) * 100);

    $('.statistics-overview .header h2').html((diff > 0 ? '+' : '-') + Math.abs(growth) + '%');

    $('.statistics-overview .header h2').removeClass('text-success');
    $('.statistics-overview .header h2').removeClass('text-danger');

    $('.statistics-overview .header h2').addClass(diff > 0 ? 'text-success' : 'text-danger');

    $('.statistics-overview .header .badge').html(labels[options.currentIndex]);
  }

  function invalidateInfo(key) {
    var container = $('.statistics-overview .info:eq(' + key + ')');

    var val = getValue(key, options.currentIndex);
    var oldVal = getValue(key, options.currentIndex - 1);

    container.find('h5').html(val);

    var other1 = getValue((key + 1) % 3, options.currentIndex);
    var other2 = getValue((key + 2) % 3, options.currentIndex);

    var total = val + other1 + other2;
    var percent = total == 0 ? 0 : parseInt((val / total) * 100);

    container.find('.badge').html(percent + '%');
  }

  function refreshChart() {

    var i;
    for (i = 0; i < options.noOfPoints; i++) {

      var newData = getValue(KEY_NEW, i);
      var renewal = getValue(KEY_RENEWAL, i);
      var amend = getValue(KEY_AMEND, i);

      chart.data.datasets[KEY_NEW].data[i] = newData;
      chart.data.datasets[KEY_RENEWAL].data[i] = renewal;
      chart.data.datasets[KEY_AMEND].data[i] = amend;
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

  function setupChart() {
    var canvas = $('.statistics-overview canvas').get(0).getContext("2d");

    chart = new Chart(canvas, {
      type: 'bar',
      data: {
        labels: labels,
        datasets: [{
          label: 'New',
          data: [0],
          backgroundColor: function (ctx) {
            return ctx.dataIndex == options.currentIndex ? darkColor : primaryColor;
          },
          backgroundColor: function (ctx) {
            return ctx.dataIndex == options.currentIndex ? darkColor : primaryColor;
          },
          borderWidth: 0
        }, {
          label: 'Renewal',
          data: [0],
            backgroundColor: function (ctx) {
              return ctx.dataIndex == options.currentIndex ? darkColor : warningColor;
            },
            backgroundColor: function (ctx) {
              return ctx.dataIndex == options.currentIndex ? darkColor : warningColor;
            },
          borderWidth: 0
        }, {
          label: 'Amend',
          data: [0],
          backgroundColor: function (ctx) {
            return ctx.dataIndex == options.currentIndex ? darkColor : dangerColor;
          },
          backgroundColor: function (ctx) {
            return ctx.dataIndex == options.currentIndex ? darkColor : dangerColor;
          },
          borderWidth: 0
        }],
      },
      options: {
        responsive: true,
        maintainAspectRatio: true,
        layout: {
          padding: {
            left: 0,
            right: 0,
            top: 0,
            bottom: 0
          }
        },
        scales: {
          yAxes: [{
            display: false,
            ticks: {
              min: 0
            },
            gridLines: {
              display: false
            },
            stacked: true
          }],
          xAxes: [{
            stacked: true,
            ticks: {
              fontColor: "#354168"
            },
            gridLines: {
              color: "rgba(0, 0, 0, 0)",
              display: false
            },
            barPercentage: 0.4
          }]
        },
        legend: {
          display: false
        },
        elements: {
          point: {
            radius: 0
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
        }
      }
    });
  }

  function fetchData() {
    if (controller) {
      controller.abot();
    }
    controller = new AbortController();

    showLoader();

    fetch('/api/statistics/overview?from=' + options.from.format('YYYY-MM-DD'), {
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

})(app.page.overview = app.page.overview || {});