var app = app || {};
app.page = {};

(function () {
  'use strict'

  this.showProgress = function (msg) {
    swal.fire({
      title: msg,
      timerProgressBar: true,
      allowOutsideClick: false,
      onBeforeOpen: () => {
        Swal.showLoading()
      }
    });
  }

  this.dismissProgress = function () {
    setTimeout(function () {
      swal.close();
    }, 700);
  }

  this.onHashtagTapped = function (type, refID) {
    return false;
  };

  $(function () {
    $(document).on('click', '.hashtag', function () {
      var tag = $(this).data('tag');
      var id = $(this).data('id');
      if (!tag || !id) {
        return;
      }

      if (app.onHashtagTapped(tag, id)) {
        return;
      }

      switch (tag) {
        case 'request':
          window.location.href = '/request/details/' + id;
          break;
        case 'rfa':
          // TODO Handle rfa hashtag tap. Load to retrieve request id
          break;
        case 'jo':
          window.location.href = '/jobOrder/details/' + id;
          break;
      }
    });
  });

  this.hasPermission = function (p) {
    return app.permission.hasPermission(app.identity.permissions, p);
  }

}).apply(app);

(function ($) {
  'use strict';

  if ($('#notification-toggler').length) {
    $('#notification-toggler').click(function () {
      $("#notification-sidebar").toggleClass("open");
    });

    $('#notification-close').click(function () {
      $("#notification-sidebar").removeClass("open");
    });
  }

})(jQuery);