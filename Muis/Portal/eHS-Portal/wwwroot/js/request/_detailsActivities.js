(function () {

  var data = [];

  this.init = function (d) {
    data = d;
    invalidate();
  }

  function invalidate() {
    if (!data || data.length === 0) {
      $('.widget.activities .widget-placeholder').removeClass('d-none');
      return;
    }

    var container = $('.activities-container');
    container.empty();

    data = data.sort((a, b) => moment.utc(b.createdOn).diff(moment.utc(a.createdOn)));

    data.forEach(e => {
      var elem = app.common.createLogElement(app.utils.userContext(e.userID, e.userName),
        formatLog(e),
        e.notes,
        app.utils.timeAgo(e.createdOn));

      container.append(elem);
    });
  }

  function formatLog(e) {
    var result = e.action;
    if (e.params) {
      var i;
      for (i = 0; i < e.params.length; i++) {

        var val = e.params[i];

        var tryDateTime = moment.utc(val);
        if (tryDateTime.isValid()) {
          val = app.utils.formatDate(tryDateTime, true);
        }

        result = result.replace("{" + i + "}", '<b>' + val + '</b>');
      }
    }

    result += app.common.createHashtag(e.type, e.refID);

    return result;
  }

  $(function () {
    $('.activities-container').on('click', '.hashtag', function (e) {
      var id = $(this).data('id');
      app.page.rfa.show(id);
    });
  });

}).apply(app.page.activities = app.page.activities || {});