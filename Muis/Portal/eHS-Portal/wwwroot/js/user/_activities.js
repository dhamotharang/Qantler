(function (self) {
  'use strict'

  var data;

  function invalidate() {
    if (!data || data.length === 0) {
      $('.widget.activities').addClass('d-none');
      return;
    }
    $('.widget.activities').removeClass('d-none');

    var container = $('.widget.activities .timeline');
    container.empty();

    data.forEach(e => {
      var elem = app.common.createLogElement(app.utils.userContext(e.userID, e.userName),
        e.action,
        e.notes,
        app.utils.timeAgo(e.createdOn));

      container.append(elem);
    });

  }

  $(function () {

    data = app.page.model.data.logs;
    if (data) {
      data = data.sort((a, b) => moment.utc(b.createdOn).diff(moment.utc(a.createdOn)));

      invalidate();
    }

  });

})(app.page.activities = app.page.activities || {});