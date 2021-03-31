(function (self) {
  'use strict'

  self.init = function (d) {
    if (!$('.response-attachments').length) {
      return;
    }

    render(d);
  }

  function render(data) {

    var container = $('.response-attachments .attachments');

    var template = $('.checklist-template .attachment');

    data.lineItems.forEach(li => {

      if (li.replies) {
        li.replies.forEach(r => {

          if (r.attachments) {

            r.attachments.forEach(a => {

              container.append(template
                .prop('outerHTML')
                .replaceAll('{fileName}', a.fileName)
                .replaceAll('{fileSize}', app.utils.formatFileSize(a.size))
                .replaceAll('{fileID}', a.fileID));

            });
          }
        });
      }
    });
  }

})(app.page.attachments = app.page.attachments || {});