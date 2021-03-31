(function () {
  'use strict';

  this.printFromURL = function (url, showProgress = true) {
    printJS({ printable: url, showModal: showProgress })
  }

}).apply(app.print = app.print || {});