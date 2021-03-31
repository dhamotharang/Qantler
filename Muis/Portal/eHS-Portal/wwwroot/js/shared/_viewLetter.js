(function () {
  'use strict';

  var data;
  var option;

  this.init = function (_data, _option) {
    option = _option;
    data = _data
    $('.view-letter .modal-title').html(option.title);
    reset();
    setup();
  };

  function reset() {
    $('.view-letter #letter-body').html('');
  };

  function setup() {
    if (!data) {
      return;
    }
    $('.view-letter #letter-body').html(app.utils.emptyIfNullOrEmpty(data.body));
  };

}).apply(app.page.letterReadonly = app.page.letterReadonly || {});