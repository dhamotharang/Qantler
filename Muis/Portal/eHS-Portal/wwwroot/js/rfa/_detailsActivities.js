(function (self) {

  self.init = function (d) {

    if (!d || d.length === 0) {
      $('.widget.activities .widget-placeholder').removeClass('d-none');
      return;
    }

    var container = $('.widget.activities .timeline');
    container.empty();

    d = d.sort((a, b) => moment.utc(b.createdOn).diff(moment.utc(a.createdOn)));

    d.forEach(e => {
      var elem = app.common.createLogElement(app.utils.userContext(e.userID, e.userName),
        e.action,
        e.notes,
        app.utils.timeAgo(e.createdOn));

      container.append(elem);
    });
  }

})(app.page.activities = app.page.activities || {});