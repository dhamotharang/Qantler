(function (self) {
  'use strict';

  var chartOptions;

  function indexChangeHandler(i) {
    chartOptions.currentIndex = i;

    self.overview.invalidate();
    self.performance.invalidate();
    self.status.invalidate();
  }

  $(function () {
    var to = moment().startOf('month');
    var from = to.add('M', -12);

    chartOptions = {
      noOfPoints: 12,
      currentIndex: 11,
      from: from
    }

    self.overview.init(chartOptions);
    self.performance.init(chartOptions);
    self.status.init(chartOptions);
    self.workitem.init();

    self.overview.indexChangeHandler = indexChangeHandler;
    self.performance.indexChangeHandler = indexChangeHandler;
    self.status.indexChangeHandler = indexChangeHandler;
  });
})(app.page);