(function (self) {
  'use strict'

  var requestID;

  var cache;
  var isDirty;

  var newData = [];

  var controller;

  self.init = function (id) {
    requestID = id;

    if (tryFetch()) {
      render();
    }
  }

  self.add = function (d) {
    if (!cache) {
      return;
    }

    cache.push(d);
    isDirty = true;
    render();
  }

  function tryFetch() {
    if (cache) {
      return true;
    }

    if (controller) {
      controller.abort();
    }

    controller = new AbortController();

    app.showProgress('Loading. Please wait...');

    fetch('/api/request/' + requestID + '/notes', {
      method: 'GET',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(res => {
        isDirty = true;
        app.dismissProgress();

        cache = res;

        render();

      }).catch(app.http.catch);
  }

  function render() {
    if (!isDirty) {
      return;
    }

    var container = $('.notes .horizontal-timeline');
    container.empty();

    var canAddNotes = $('#dropdown-menu-notes').length
      && !$('#dropdown-menu-notes').hasClass('d-none');

    if (canAddNotes) {
      $('.notes .placeholder .add').removeClass('d-none');
    } else {
      $('.notes .placeholder .add').addClass('d-none');
    }

    if (!cache.length) {
      $('.notes .placeholder').removeClass('d-none');
      return;
    }

    $('.notes .placeholder').addClass('d-none');

    var map = {};
    var groupSet = [];

    cache.forEach(e => {
      var date = moment.utc(e.createdOn);
      var key = date.local().format('YYYYMMDD');

      var holder = map[key];
      if (!holder) {
        holder = {
          date: date,
          data: []
        }

        map[key] = holder;
        groupSet.push(holder);
      }

      holder.data.push(e);
    });

    groupSet = groupSet.sort((a, b) => moment(b.date).diff(moment(a.date)));

    var timeFrameTemplate = $('.notes .templates .time-frame');
    var eventTemplate = $('.notes .templates .event');
    var attachmentsTemplate = $('.notes .templates .attachments');
    var attachmentTemplate = $('.notes .templates .attachment');

    groupSet.forEach(e => {

      var timeFrame = timeFrameTemplate.clone(true);

      var dataSet = e.data.sort((a, b) => moment(b.createdOn).diff(moment(a.createdOn)));

      dataSet.forEach(d => {

        var event = eventTemplate.clone(true);
        var attachments;

        if (d.attachments) {

          attachments = attachmentsTemplate.clone(true);

          d.attachments.forEach(a => {

            attachments.append(attachmentTemplate
              .prop('outerHTML')
              .replaceAll('{fileName}', a.fileName)
              .replaceAll('{fileSize}', app.utils.formatFileSize(a.size))
              .replaceAll('{fileID}', a.fileID));
          });
        }

        timeFrame.append(eventTemplate.prop('outerHTML')
          .replaceAll('{user}', app.utils.userContext(e.createdBy, d.officer.name))
          .replaceAll('{text}', d.text)
          .replaceAll('{attachments}', attachments ? attachments.prop('outerHTML') : '')
          .replaceAll('{time}', app.utils.formatTime(d.createdOn, true)));
      });

      container.append(timeFrame.prop('outerHTML')
        .replaceAll('{text}', app.utils.dayAgo(e.date, true)));
    });
  }

  $(function () {
    $('.notes .placeholder button').click(function () {
      app.page.notes.new.init(requestID);
    });
  });

}) (app.page.notes = app.page.notes || {});