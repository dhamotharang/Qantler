(function () {
  var isBusy = false;

  var controller;

  var dataset = [];

  function invalidate() {
    if (!dataset || dataset.length === 0) {
      $('.widget .widget-placeholder').removeClass('d-none');
      return;
    }

    $('.widget .widget-placeholder').addClass('d-none');

    var container = $('.comments .timeline');
    container.empty();

    dataset = dataset.sort((a, b) => moment.utc(a.createdOn).diff(moment.utc(b.createdOn)));

    dataset.forEach(e => {
      container.append(createCommentElement(e));
    });

    container.scrollTop(container[0].scrollHeight);
  }

  function sync() {
    isBusy = true;
    updateViewState();

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    fetch('/api/mufti/' + app.page.id + '/comments', {
      method: 'GET',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(res => {
        isBusy = false;
        updateViewState();

        dataset = res;
        invalidate();
      })
      .catch(err => {
        isBusy = false;
        updateViewState();

        app.http.catch(err);
      });
  }

  function createCommentElement(comment) {
    var user = app.utils.userContext(comment.userID, comment.userName);
    var time = app.utils.timeAgo(comment.createdOn);

    var result = '<li class="timeline-item">' +
      '<p class="timeline-content"><code>' + user + '</code></p>';

    result += '<p class="timeline-notes text-muted">' + comment.text.replace(/(?:\r\n|\r|\n)/g, '<br>') + '</p>';

    result += '<p class="event-time">' + time + '</p>' + '</li>';

    return result;
  }

  function updateViewState() {
    var input = $('.compose input');
    var btn = $('.compose button');
    if (isBusy) {
      input.addClass('busy');
      btn.addClass('busy');

      input.attr('readonly', true);
    } else {
      input.removeClass('busy');
      btn.removeClass('busy');

      input.removeAttr('readonly');
    }
  }

  function reset() {
    $('.compose input').val('');
  }

  function submit() {
    if (isBusy) {
      return;
    }

    var input = $('.compose input').val();
    if (app.utils.isNullOrEmpty(input.trim())) {
      return;
    }

    isBusy = true;
    updateViewState();

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    fetch('/api/mufti/' + app.page.id + '/comment?text=' + encodeURI(input), {
      method: 'POST',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(res => {
        isBusy = false;
        updateViewState();

        dataset.push(res);
        invalidate();

        reset();
      })
      .catch(err => {
        isBusy = false;
        updateViewState();

        app.http.catch(err);
      });
  }

  function setupPermissions() {
    if (app.hasPermission(12)) {
      $('.compose').removeClass('d-none');
    }
  }

  $(function () {
    $('.btn-send').click(function () {
      submit();
    });

    $(".compose input").on('keyup', function (e) {
      if (e.key === 'Enter' || e.keyCode === 13) {
        submit();
      }
    });

    dataset = app.page.model.comments || [];

    invalidate();

    app.signalR.subscribe('notification', function (m) {
      if (m.module == 'Mufti'
        && m.refID == app.page.id) {
        sync();
      }
    });

    setupPermissions();
  })
}).apply(app.page.comments = app.page.comments || {});