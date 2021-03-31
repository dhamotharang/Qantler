(function () {

  this.format = function (val, digits = 2) {
    if (!val && val != 0) {
      return '';
    }

    return Number.parseFloat(val).toFixed(2);
  }

  this.financial = function (val, digits = 2) {
    if (!val && val != 0) {
      return '';
    }

    var num = Number.parseFloat(val).toFixed(2);
    var result = num.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    if (num < 0) {
      result = '(' + result.replace('-', '') + ')';
    }
    return result;
  }

  this.precise = function (val, digits) {
    if (!val && val != 0) {
      return '';
    }

    var num = Number.parseFloat(val);
    return digits ? num.toFixed(digits).toPrecision(digits) : num.toPrecision();
  }

}).apply(app.number = app.number || {});