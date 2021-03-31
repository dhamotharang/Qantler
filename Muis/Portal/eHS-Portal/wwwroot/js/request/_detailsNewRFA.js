(function () {

  var properties = [];
  var data = null;
  var controller;

  var referenceRFAID;
  var referenceRFA

  var currentCategory;
  var currentItem;
  var currentScheme;


  this.show = function (p) {

    properties = p;
    referenceRFAID = null;
    referenceRFA = null;

    $('#newRfaModalLabel').html('New RFA');
    app.utils.hide($('#newRfaDiscardButton'));

    setup();
  }

  this.showFromExisting = function (p, id) {
    clear();
    properties = p;
    referenceRFAID = id;
    referenceRFA = null;

    $('#newRfaModalLabel').html('RFA Draft');
    app.utils.show($('#newRfaDiscardButton'));

    setup();
  };

  function clear() {
    var tabContainer = $('#newRfaModal .modal-header .nav');
    var contentContainer = $('#newRfaModal .modal-body .tab-content');

    tabContainer.empty();
    contentContainer.empty();
  }

  function setup() {
    clear();

    if (referenceRFAID != null && referenceRFA == null) {
      fetchRFA(function () { setup(); });
      return;
    }

    if (!data) {
      fetchData();
      return;
    }

    var tabContainer = $('#newRfaModal .modal-header .nav');
    var contentContainer = $('#newRfaModal .modal-body .tab-content');

    data.forEach((e, i) => {
      initTab(tabContainer, e, i);
      initContent(contentContainer, e, i);
    });

    initViewWithData();
  }

  function initTab(container, d, i) {
    var elem = '<li class="nav-item">' +
      '<a class="nav-link ' + (i === 0 ? 'active' : '') + '" data-toggle="pill"' + '" data-scheme="' + d.scheme + '"' +
      ' href= "#scheme' + d.schemeText.replaceAll(' ', '') + '" role="tab">' + d.schemeText + '</a>' +
      '</li>';

    container.append(elem);
  }

  function initContent(container, d, i) {
    var elem = '<div class="tab-pane fade ' + (i === 0 ? 'show active' : '') + '" id="scheme' + d.schemeText.replaceAll(' ', '') + '" data-scheme="' + d.scheme + '"' + '>' +
      '<div class="row">' +
      '<div class="col-lg-12 left">' +
      '<ul class="lineitem-list">';

    d.categories.forEach(c => {
      elem += '<li class="list header">' +
        '<h5 class="font-weight-bold"> ' + c.index + ' ' + c.text + '</h5>' +
        '</li>';

      var items = c.items.sort((a, b) => a.index - b.index);

      items.forEach(i => {
        elem += '<li class="list item" data-category="' + c.index + '" data-item="' + i.index + '" data-scheme="' + d.scheme + '">' +
          '<div class="compliance">' +
          '<p class="font-weight-bold">' + c.index + '.' + i.index + '</p>' +
          '<div class="compliance-body ml-2">' +
          i.text +
          '</div>' +
          '</div> ' +
          '<div class="error-msg text-danger">Remarks is required.</div>' +
          '<div class="remarks">' +
          '<input type="text" class="form-control remarks-input" maxlength="500"/>' +
          '<div class="attachments">' +
          '<ul></ul>' +
          '</div>' +
          '</div> ' +
          '<div class="upload">' +
          '<button type="button" data-category="' + c.index + '" data-item="' + i.index + '" data-scheme="' + d.scheme + '" class="btn btn-info btn-fw upload-btn">' +
          '<i class="mdi mdi-upload"></i>Attachment' +
          '</button>' +
          '</li>';
      });
    });

    /* Others */
    elem += '<li class="list header others">' +
      '<h5 class="font-weight-bold">OTHERS</h5>' +
      '<span class="mdi mdi-plus icon-sm plus" data-scheme="' + d.scheme + '"></span>' +
      '</li>' +
      '<li class="list item others placeholder mb-3" data-scheme="' + d.scheme + '">' +
      '<div class="remarks">' +
      '<span>Tap \'+\' to add customer clarifications.</span>' +
      '</div>' +
      '</li>';

    elem += '</ul></div></div></div>';

    container.append(elem);
  }

  function addOthers(scheme, data) {
    var container = $('#newRfaModal .modal-body .tab-pane[data-scheme="' + scheme + '"] .lineitem-list');

    app.utils.hide(container.find('.placeholder'));

    var index = container.find('.item.others').length + 1;

    var remarks = data ? data.remarks : '';

    var html = '<li class="list item others mb-3" data-category="-1" data-item="' + index + '" data-scheme="' + scheme + '">' +
      '<div class="error-msg text-danger">Remarks is required.</div>' +
      '<div class="remarks">' +
      '<input type="text" class="form-control remarks-input" maxlength="500" value="' + remarks + '"/>' +
      '<div class="attachments">' +
      '<ul></ul>' +
      '</div>' +
      '</div> ' +
      '<div class="upload">' +
      '<button type="button" data-category="-1" data-item="' + index + '" data-scheme="' + scheme + '" class="btn btn-info btn-fw upload-btn">' +
      '<i class="mdi mdi - upload"></i>Attachment' +
      '</button> ' +
      '</li>';

    container.append(html);
  }

  function initViewWithData() {
    if (!referenceRFA) {
      return;
    }

    referenceRFA.lineItems.forEach(i => {
      if (i.checklistCategoryID == -1) {
        return;
      }

      var remarksInput = $('#newRfaModal .modal-body .item[data-category="' + i.checklistCategoryID + '"][data-scheme="' + i.scheme + '"][data-item="' + i.checklistID + '"] .remarks-input');
      remarksInput.val(i.remarks);

      if (i.attachments.length > 0) {
        i.attachments.forEach(a => {
          appendAttachment(i.checklistCategoryID, i.checklistID, i.scheme, a);
        });
      }
    });

    var others = referenceRFA.lineItems.filter(e => e.checklistCategoryID == -1);
    if (others.length > 0) {
      others = others.sort((a, b) => a.checklistID - b.checklistID);

      others.forEach(e => {
        addOthers(e.scheme, e);

        if (e.attachments.length > 0) {
          e.attachments.forEach(a => {
            appendAttachment(e.checklistCategoryID, e.checklistID, e.scheme, a);
          });
        }
      });
    }
  }

  function fetchRFA(callback) {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    fetch('/api/rfa/' + referenceRFAID, {
      method: 'GET',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(r => {
        referenceRFA = r;

        if (callback) {
          callback();
        }
      })
      .catch(app.http.catch);
  }

  function fetchData() {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    var param = '';
    for (var i = 0; i < properties.length; i++) {
      if (i !== 0) param += '&'

      param += 'ids=' + properties[i];
    }

    fetch('/api/checklist?' + param, {
      method: 'GET',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(r => {
        data = r.sort((a, b) => a.scheme - b.scheme);
        setup();
      })
      .catch(app.http.catch);
  }

  function submit(asDraft) {
    var validated = validate(asDraft);
    if (!validated) {
      return;
    }

    var data = convertFormToData(asDraft);
    data.status = asDraft ? 1 : 100;
    data.id = referenceRFA ? referenceRFA.id : 0;

    if (data.lineItems.length == 0) {
      showError('Form is empty.<br>Fill in atleast one remarks (with optional attachments) for you to submit.');
      return;
    }

    showProgress('Submitting. Please wait...');

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    fetch('/api/rfa', {
      method: 'POST',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(data)
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(r => {
        Swal.fire({
          icon: 'success',
          title: asDraft ? 'RFA has been stored as draft.' : 'RFA has been submitted.'
        }).then(r => {
          location.reload(true);
        });
      })
      .catch(app.http.catch);
  }

  function discard() {
    Swal.fire({
      title: 'Are you sure?',
      text: "You want to discard this?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3f51b5',
      cancelButtonColor: '#ff4081',
      confirmButtonText: 'Yes',
      cancelButtonText: 'Cancel',
      reverseButtons: true
    }).then(r => {
      if (r.isConfirmed) {
        showProgress('Discarding. Please wait...');

        fetch('/api/rfa/' + referenceRFA.id, {
          method: 'DELETE',
          cache: 'no-cache',
          credentials: 'include',
          signal: controller.signal,
          headers: {
            'Accept': 'application/json'
          }
        }).then(app.http.errorInterceptor)
          .then(res => res.json())
          .then(r => {
            Swal.fire({
              icon: 'success',
              title: 'RFA has been discarded.'
            }).then(r => {
              location.reload(true);
            });
          })
          .catch(app.http.catch);
      }
    });
  }

  function convertFormToData(asDraft) {
    var data = {
      requestID: app.page.id,
      lineItems: []
    };

    var index = 0;
    $('#newRfaModal .modal-body .item').each(function () {
      var self = $(this);
      if (self.hasClass('placeholder')) {
        return;
      }

      var schemeID = self.data('scheme');
      var categoryID = self.data('category');
      var itemID = self.data('item');

      var scheme = getSchemeByID(schemeID);
      var category, item;

      if (categoryID == -1) {
        category = {
          index: -1,
          text: 'OTHERS'
        };

        item = {
          index: self.data('item'),
          text: ''
        }
      } else {
        category = getCategoryByID(scheme, categoryID);
        item = getItemByID(scheme, categoryID, itemID);
      }

      var remarks = self.find('.remarks-input').val().trim();
      var attachments = self.find('.remarks .attachments ul li');

      if (remarks.length > 0 || (asDraft && attachments.length > 0)) {
        var items = {
          index: index,
          remarks: remarks,
          scheme: schemeID,
          checklistCategoryID: category.index,
          checklistCategoryText: category.text,
          checklistID: item.index,
          checklistText: item.text,
          attachments: []
        };

        if (attachments.length > 0) {
          attachments.each(function () {
            var self = $(this);

            var file = {
              fileID: self.data('fileid'),
              fileName: self.data('filename'),
              size: self.data('size'),
              extension: self.data('extension')
            };

            items.attachments.push(file);
          });
        }

        data.lineItems.push(items);
        index++;
      }
    });

    return data;
  }

  function getSchemeByID(id) {
    return data.find(e => e.scheme === id);
  }

  function getCategoryByID(scheme, id) {
    return scheme.categories.find(e => e.index === id);
  }

  function getItemByID(scheme, catID, id) {
    return getCategoryByID(scheme, catID).items.find(e => e.index === id);
  }

  function validate(asDraft) {
    $('#newRfaModal .modal-body .item').each(function () {
      var self = $(this);
      if (self.hasClass('placeholder')) {
        return;
      }

      var remarksContainer = self.find('.remarks');
      var remarks = self.find('.remarks-input').val().trim();
      var attachments = self.find('.remarks .attachments ul li');

      if (!asDraft && remarks.length == 0 && attachments.length > 0) {
        hasError = true;
        self.addClass('error');
      } else {
        self.removeClass('error');
      }
    });

    return $('#newRfaModal .modal-body .item.error').length === 0;
  }

  function onFileUploaded(data) {
    appendAttachment(currentCategory, currentItem, currentScheme, data[0]);
  }

  function appendAttachment(categoryID, itemID, schemeID, file) {
    var container = $('#newRfaModal .modal-body .item[data-category="' + categoryID + '"][data-scheme="' + schemeID + '"][data-item="' + itemID + '"] .attachments ul');

    var elem = '<li data-fileid="' + file.fileID + '" data-filename="' + file.fileName + '" data-size="' + file.size + '" data-extension="' + file.extension + '">' +
      '<div class="thumb">' +
      '<i class="mdi ' + app.utils.fileExtensionIcon(file.extension) + '"></i>' +
      '</div>' +
      '<div class="details">' +
      '<p class="file-name">' + file.fileName + '</p>' +
      '<div class="buttons">' +
      '<p class="file-size">' + app.utils.formatFileSize(file.size) + '</p>' +
      '<a href="#" class="remove-attachment" data-fileid="' + file.fileID + '">Remove</a> ' +
      '</div>' +
      '</div>' +
      '</li>';

    container.append(elem);
  }

  function removeAttachment(fileID) {
    showProgress('Removing. Please wait...');

    fetch('/api/file/' + fileID, {
      method: 'DELETE',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(r => {
        $('#newRfaModal .modal-body li[data-fileid="' + fileID + '"]').remove();

        dismissProgress();
      })
      .catch(app.http.catch);
  }

  function showProgress(msg) {
    swal.fire({
      title: msg,
      timerProgressBar: true,
      allowOutsideClick: false,
      onBeforeOpen: () => {
        Swal.showLoading()
      }
    });
  }

  function dismissProgress() {
    setTimeout(function () {
      swal.close();
    }, 700);
  }

  function showError(msg) {
    Swal.fire({
      icon: 'error',
      title: 'Oops...',
      html: msg
    });
  }

  $(function () {
    $(".rfaModal #fileuploader").uploadFile({
      url: "/api/file",
      allowedTypes: "pdf,doc,docx,xls,xlsx,csv,png,bmp,jpg,gif,jpeg,txt",
      maxFileSize: 5000000,
      onSelect: function (files, data) {

        var file = files[0];
        if (file.size > 5000000) {
          showError(file.name + ' is not allowed!<br>Allowed max size: <strong>5MB</strong>.');
          return false;
        }

        var allowedExtensions = /(\.jpg|\.jpeg|\.png|\.gif|\.pdf|\.doc|\.docx|\.xls|\.xlsx|\.csv|\.txt)$/i;
        if (!allowedExtensions.exec(file.name)) {
          showError(file.name + ' is not allowed!<br>Allowed extensions: <strong>pdf,doc,xls,csv,png,jpg,jpeg,txt</strong>');
        }

        return true;
      },
      onSubmit: function (e) {
        showProgress('Uploading! Please wait...');
      },
      onSuccess: function (files, data) {
        onFileUploaded(data);
        dismissProgress();
      },
      onError: function (e) {
        showError('Something went wrong!');
      },
      onCancel: function (e) {
        alert(JSON.stringify(e));
      }
    });

    $('.rfa .modal-body').on('click', '.upload-btn', function (e) {
      var self = $(this);
      currentCategory = self.data('category');
      currentItem = self.data('item');
      currentScheme = self.data('scheme');

      $('.rfa .ajax-file-upload input[type="file"]').click();
    });

    $('#newRfaModal .modal-body').on('click', '.remove-attachment', function (e) {
      removeAttachment($(this).data('fileid'));
    });

    $('#newRfaSubmitButton').click(function () {
      submit();
    });

    $('#newRfaDraftButton').click(function () {
      submit(true);
    });

    $('#newRfaDiscardButton').click(function () {
      discard();
    });

    $('#newRfaModal .modal-body').on('change input paste', '.remarks-input', function () {
      if ($(this).val().trim().length > 0) {
        $(this).parent().parent().removeClass('error');
      }
    });

    $('#newRfaModal .modal-body').on('click', '.others .plus', function () {
      addOthers($(this).data('scheme'));
    });
  });

}).apply(app.page.rfa.new = app.page.rfa.new || {});