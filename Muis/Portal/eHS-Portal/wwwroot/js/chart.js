(function ($) {
  'use strict';

  $(function () {
    Chart.defaults.global.tooltips.enabled = false;
    Chart.defaults.global.defaultFontColor = '#354d66';
    Chart.defaults.global.defaultFontFamily = '"Poppins", sans-serif';
    Chart.defaults.global.legend.labels.fontStyle = "italic";
    Chart.defaults.global.tooltips.intersect = false;
  });
})(jQuery);