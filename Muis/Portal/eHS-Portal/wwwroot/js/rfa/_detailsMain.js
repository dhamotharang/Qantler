(function (self) {

  self.init = function (d) {

    $('.main .createdOn span').html(app.utils.formatDate(d.createdOn));

    $('.main .dueOn span').html(app.utils.formatDate(d.dueOn));
  }

})(app.page.main = app.page.main || {});