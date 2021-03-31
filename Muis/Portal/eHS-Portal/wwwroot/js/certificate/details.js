(function (page) {
  'use strict';

  var data;

  page.init = function () {
    data = page.model;
    page.invalidate();
  }

  page.getData = function () {
    return data;
  }

  page.invalidate = function () {

    if (page.main) {
      page.main.init(data);
    }
  };
})(app.page);

$(document).ready(function () {
  app.page.init();

  $(".nav-tabs a").click(function () {
    var type = $(this).data('type');
    if (type == 'menu'
      || type == 'ingredients') {

      setTimeout(function () {
        $($.fn.dataTable.tables(true)).css('width', '100%');
        $($.fn.dataTable.tables(true)).DataTable().columns.adjust().draw();
      }, 200);

    }
  });
});

