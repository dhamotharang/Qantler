(function () {

  var data;
  var didInit;

  this.setData = function (d) {
    data = d;
  }

  this.init = function () {
    initView();
  }

  function initView() {
    if (didInit) {
      return;
    }
    didInit = true;

    var container = $('#attachmentsModal .lineitem-list');

    var elem = '';

    if (data.attachments.length > 0) {
      elem += '<li class="list header"><h5 class="font-weight-bold">Supporting Documents</h5></li>' +
        '<li class="list item">' +
        '<div class="attachments"><ul>';

      data.attachments.forEach(a => {
        elem += createAttachmentElement(a);
      });

      elem += '</ul></div></li>';
    }

    if (data.rfAs.length > 0) {
      var rfas = data.rfAs.sort((a, b) => b.id - a.id);

      rfas.forEach(rfa => {
        var rfaHasAttachments;

        var rfaAttachments = '<li class="list header"><h5 class="font-weight-bold">RFA #' + rfa.id + '</h5></li>' +
          '<li class="list item">' +
          '<div class="attachments"><ul>';

        if (rfa.attachments) {

          rfa.attachments.forEach(a => {
            rfaHasAttachments = true;
            rfaAttachments += createAttachmentElement(a);
          });
        }

        if (rfa.lineItems) {
          rfa.lineItems.forEach(li => {

            if (li.attachments) {
              li.attachments.forEach(a => {
                rfaHasAttachments = true;
                rfaAttachments += createAttachmentElement(a);
              });
            }
          });
        }

        rfaAttachments += '</ul></div>';

        // Attachments from customer
        if (rfa.lineItems) {
          var hasResponseAttachments;
          var responseAttachments = '<li class="list item"><div class="reply">' +
            '<div class="attachments"><ul>';

          rfa.lineItems.forEach(li => {

            if (li.replies) {
              li.replies.forEach(reply => {
                if (reply.attachments) {
                  reply.attachments.forEach(a => {
                    hasResponseAttachments = true;
                    responseAttachments += createAttachmentElement(a);
                  });
                }
              });
            }
          });

          responseAttachments += '</ul></div></div></li>'

          if (hasResponseAttachments) {
            rfaHasAttachments = true;
            rfaAttachments += responseAttachments;
          }
        }

        rfaAttachments += '</li>';

        if (rfaHasAttachments) {
          elem += rfaAttachments;
        }
      });
    }
    container.append(elem);
  }

  function createAttachmentElement(file) {
    return '<li data-fileid="' + file.fileID + '" data-filename="' + file.fileName + '" data-size="' + file.size + '" data-extension="' + file.extension + '">' +
      '<div class="thumb">' +
      '<i class="mdi ' + app.utils.fileExtensionIcon(file.extension) + '"></i>' +
      '</div>' +
      '<div class="details">' +
      '<p class="file-name">' + file.fileName + '</p>' +
      '<div class="buttons">' +
        '<p class="file-size">' + app.utils.formatFileSize(file.size) + '</p>' +
        '<a href="/api/file/' + file.fileID + '?filename=' + file.fileName + '" class="download" download>Download</a>' +
      '</div>' +
      '</div>' +
      '</li>';
  }

}).apply(app.page.attachments = app.page.attachments || {});