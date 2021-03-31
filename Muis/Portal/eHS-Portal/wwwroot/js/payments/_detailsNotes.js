(function () {
  'use strict'

  this.render = function (data) {

    var container = $('.notes .horizontal-timeline');
    container.empty();

    if (!app.page.model.notes) {
      $('.notes .placeholder').removeClass('d-none');
      return;
    }

    $('.notes .placeholder').addClass('d-none');

    var map = {};
    var groupSet = [];

    data.forEach(e => {
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
      app.page.notes.new.init(app.page.id);
    });
  });

}).apply(app.page.notes = app.page.notes || {});