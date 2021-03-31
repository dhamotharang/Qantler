(function () {

  this.show = function () {
    $('#exportPDFModal').modal('show');
  }

  this.hide = function () {
    $('#exportPDFModal').modal('hide');
  }

}).apply(app.page.export = app.page.export || {});