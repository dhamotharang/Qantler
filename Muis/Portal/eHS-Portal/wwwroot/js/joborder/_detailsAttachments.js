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