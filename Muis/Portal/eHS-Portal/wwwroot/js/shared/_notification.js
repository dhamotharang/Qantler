(function () {
  'use strict';

  var controller;
  var dataset = [];
  var isLoading = false;

  this.sync = function () {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    fetch('/api/notification/list', {
      method: 'GET',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json'
      }
    })
      .then(response => response.json())
      .then(data => {
        if (data.length > 0) {
          data = data.sort((a, b) => moment.utc(b.createdOn).diff(moment.utc(a.createdOn)));
        }
        dataset = data;
        this.invalidate();
      })
      .catch(error => {
        // TODO handle resync
      });
  };

  this.invalidate = function () {
    var container = $('.notification-list');
    if (container.length) {
      container.empty();

      if (dataset.length == 0) {
        return;
      }

      var hasUnread = false;

      dataset.forEach(e => {
        // todo check notification read state
        var level, icon = 'bell';
        switch (e.level) {
          case 0:
            level = 'info';
            break;
          case 1:
            level = 'warning';
            icon = 'alert';
            break;
          case 2:
            level = 'danger';
            icon = 'alert-decagram';
            break;
          default:
            level = 'dark';
            break;
        }

        var elem = '<li data-state="' + e.state + '" data-id="' + e.id + '" data-refid="' + e.refID + '" data-module="' + e.module + '" class="list ' + (e.state == 0 ? 'active' : '') + '">'

        elem = elem +
          '<div class="profile">' +
          '<span class="img-xs rounded-circle text-white text-avatar bg-' + level + '"><i class="mdi mdi-' + icon + '"></i></span>' +
          '<span class="indicator"></span>' +
          '</div>' +
          '<div class="info">' +
          '<p>' + e.title + '</p>' +
          '<p>' + e.body + '</p>' +
          '<p class="text-muted">' +
          '<i class="mdi mdi-clock"></i> ' + app.utils.timeAgo(e.createdOn) + '.' +
          '</p>' +
          '</div>' +
          '</li>';

        if (!hasUnread && e.state == 0) {
          hasUnread = true;
        }

        container.append(elem);
      });

      if (hasUnread) {
        $('.sidebar-toggler.notification').addClass('active');
      }
    }
  };

  function checkReadState() {
    var toggler = $('.sidebar-toggler.notification');
    if (!toggler.hasClass('active')) {
      return;
    }

    var hasUnread = $('.notification-list li[class="active"]').length > 0;
    if (!hasUnread) {
      toggler.removeClass('active');
    }
  }

  function resetToastPosition() {
    $('.jq-toast-wrap').removeClass('bottom-left bottom-right top-left top-right mid-center');
    $(".jq-toast-wrap").css({
      "top": "",
      "left": "",
      "bottom": "",
      "right": ""
    });
  };

  function updateState(id, state, success, fail) {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    isLoading = true;

    fetch('/api/notification/' + id + '/state?state=' + state, {
      method: 'PUT',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json'
      }
    })
      .then(response => response.json())
      .then(data => {
        isLoading = false;
        success();
      })
      .catch(error => {
        isLoading = false;
        fail();
      });
  }

  function handleTap(id, module, refID) {
    $('.notification-list li[data-id="' + id + '"]').removeClass('active');

    if (app.utils.isNullOrEmpty(module)) {
      return;
    }

    switch (module.toLowerCase()) {
      case 'request':
        location.href = '/request/details/' + refID;
        break;
      case 'mufti':
        location.href = '/mufti/details/' + refID;
        break;
      case 'certificatebatch':
        location.href = '/batch/certificate';
        break;
      case 'joborder':
        location.href = '/jobOrder/details/' + refID;
        break;
    }
  }

  $(function () {
    app.signalR.subscribe('notification', function (m) {
      var icon, bg;
      switch (m.level) {
        case 0:
          icon = 'info';
          bg = '#46c35f';
          break;
        case 1:
          icon = 'warning';
          bg = '#57c7d4';
          break;
        case 2:
          icon = 'error';
          bg = '#f2a654';
          break;
      }

      resetToastPosition();

      if (!$('.settings-panel.notification').hasClass('open')) {
        $.toast({
          heading: 'Info',
          text: m.message,
          showHideTransition: 'slide',
          icon: icon,
          loaderBg: bg,
          position: 'top-right'
        });
      }

      app.notification.sync();
    });

    $('.notification-list').on('click', 'li', function (e) {
      if (isLoading) {
        return;
      }

      var id = $(this).data('id');
      var state = $(this).data('state');
      var refID = $(this).data('refid');
      var module = $(this).data('module');

      if (state == 0) {
        updateState(id,
          1,
          function () {
            handleTap(id, module, refID);
          },
          function () {
          });
        return;
      }

      handleTap(id, module, refID);
    });

    app.notification.sync();
  });

}).apply(app.notification = app.notification || {});