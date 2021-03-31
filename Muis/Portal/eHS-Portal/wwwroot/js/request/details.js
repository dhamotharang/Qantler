(function (page) {
  'use strict';

  var controller;
  var data;

  page.init = function () {
    data = page.model;
    page.invalidate();
  }

  page.getData = function () {
    return data;
  }

  page.isDirty = function () {
    return app.page.main.menu.isDirty()
      || app.page.main.ingredients.isDirty();
  }

  page.onDirtyStateChanged = function () {
    var menuSave = $('#dropdown-save');

    if (page.isDirty()) {
      app.utils.show(menuSave);
    } else {
      app.utils.hide(menuSave);
    }
  }

  page.save = function () {

    app.showProgress('Saving. Please wait...');

    var success = function () {
      app.utils.hide($('#dropdown-save'));

      app.dismissProgress();
    }

    var error = function (err) {
      app.http.catch(err);
    }

    app.page.main.menu.save(
      app.page.main.ingredients.save(success, error), error);
  }

  page.invalidate = function () {
    updateTitle();

    page.rfa.init(data.rfAs);
    page.activities.init(data.logs);
    page.attachments.setData(data);
    page.review.init(data);
    page.review.approval.init(data);
    page.approval.init(data);
    page.reaudit.init(data);
    page.relatedrequests.init(data.id);

    if (page.main) {
      page.main.init(data);
    }
    invalidateSteps();
    setupPermissions();
  };

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

  function invalidateSteps() {
    $('.md-stepper .md-step').each(function () {
      var step = $(this).data('step');
      var currStep = 0;

      var status = data.status;
      if (status == 1300) {
        status = data.oldStatus;
      }

      switch (status) {
        case 100:
        case 200:
        case 250:
          currStep = 0;
          break;       
        case 300:
          currStep = 1;
          break;
        case 400:
        case 500:
        case 1000:
          currStep = 2;
          break;
        case 550:
          currStep = 3;
          break;
        case 600:
        case 700:
          currStep = 4;
          break;
        default:
          currStep = 5;
          break;

      }

      if (step <= currStep) {
        $(this).addClass('active');
      }

      if (step === currStep) {
        $(this).addClass('current');
      }
    });
  }

  function updateTitle() {
    $('.page-title').html(data.typeText);

    var quickLinks = $('.page-header .quick-link-wrapper ul');
    quickLinks.empty();

    quickLinks.append('<li><span class="text-' + app.utils.requestStatusColor(data.status) + '">' + data.statusText + '</span></li>');

    if (data.expedite) {
      quickLinks.append('<li><span class="badge badge-outline-danger">Express</span></li>');
    }

    if (data.dueOn) {
      quickLinks.append('<li>Due: ' + app.utils.formatDate(data.dueOn, true) + '</li>');
    }
  };

  function setupPermissions() {

    var status = data.status;

    var id = app.identity.id;

    switch (status) {
      case 100:
        if (app.hasPermission(23)) {
          $('#dropdown-menu-proceedforreview').removeClass('d-none');
        }
      case 200: // open

        if (id == data.assignedTo
          || app.hasPermission(7)) {
          $('#dropdown-menu-schedule').removeClass('d-none');
          $('#dropdown-menu-recommend').removeClass('d-none');
          $('#dropdown-menu-kiv').removeClass('d-none');
          $('.dropdown-divider').removeClass('d-none');

          $('#action-escalate').removeClass('d-none');
        }

        if (id == data.assignedTo
          || app.hasPermission(0)
          || app.hasPermission(1)
          || app.hasPermission(2)
          || app.hasPermission(3)
          || app.hasPermission(4)
          || app.hasPermission(5)
          || app.hasPermission(6)
          || app.hasPermission(7)
          || app.hasPermission(21)) {
          $('#dropdown-menu-rfa').removeClass('d-none');
        }

        if (app.hasPermission(5)
          || app.hasPermission(7)) {
          $('#dropdown-menu-reassign').removeClass('d-none');
          $('.dropdown-divider').removeClass('d-none');
        }

        break;
      case 250: 

        if (id == data.assignedTo
          || app.hasPermission(22)) {
          $('#dropdown-menu-review-recommend').removeClass('d-none');
        }

        break;
      case 300: // for-inspection

        if (id == data.assignedTo) {
          $('#action-escalate').removeClass('d-none');
        }

        if (data.statusMinor == 320
          && (id == data.assignedTo
            || app.hasPermission(7))) {
          $('#dropdown-menu-recommend').removeClass('d-none');
          $('.dropdown-divider').removeClass('d-none');
        }

        break;
      case 400: // for approval

        if (app.hasPermission(2)
          || app.hasPermission(7)) {
          $('#dropdown-menu-approve').removeClass('d-none');
          $('#dropdown-menu-reaudit').removeClass('d-none');
          $('#dropdown-menu-kiv').removeClass('d-none');
          $('.dropdown-divider').removeClass('d-none');

          $('#action-escalate').removeClass('d-none');
        }

        break;
      case 600: // billing in progress

        if (data.statusMinor == 610
          && (app.hasPermission(3)
            || app.hasPermission(7))) {
          $('#dropdown-menu-payment').removeClass('d-none');
          $('#dropdown-menu-kiv').removeClass('d-none');
          $('.dropdown-divider').removeClass('d-none');

          $('#action-escalate').removeClass('d-none');
        }

        break;
      case 1300: // kiv

        if (id == data.assignedTo) {
          $('#dropdown-menu-endkiv').removeClass('d-none');
          $('.dropdown-divider').removeClass('d-none');
        }
      case 1200: // Expired
        if (app.hasPermission(7)) {
          $('#dropdown-menu-reinstate').removeClass('d-none');
        }
        break;
    }

    if (data.escalateStatus == 100) {
      if ((app.hasPermission(6)
        || app.hasPermission(7))) {
        $('#action-escalate').removeClass('d-none');
      } else {
        $('#action-escalate').remove();
      }
    }
  }

  function proceedForReview() {
    // SHow progress
    // Call API to proceed for review
    // if sucess, reload
    // if error, show error message

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Initializing. Please wait...');

    fetch('/api/request/proceedreview?id=' + data.id, {
      method: 'POST',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(res => {
        location.reload(true);
      }).catch(app.http.catch);
  }  

  $(function () {
    $('#dropdown-menu-attachments').click(function () {
      app.page.attachments.init();
    });

    $('#dropdown-menu-rfa').click(function () {
      if (typeof data !== 'undefined') {
        app.page.rfa.new.show(data.lineItems.map(e => e.checklistHistoryID));
      }
    });

    $('#dropdown-menu-checklist').click(function () {
      app.page.checklist.show(data.lineItems.map(e => e.checklistHistoryID));
    });

    $('#dropdown-menu-schedule').click(function () {
      app.page.schedule.init(data);
    });

    $('#dropdown-menu-kiv').click(function () {
      app.page.kiv.init(data);
    });

    $('#dropdown-menu-endkiv').click(function () {
      app.page.kiv.revert(data);
    });

    $('#dropdown-save').click(function () {
      app.page.save();
    })

    $('#escalate').click(function () {
      app.page.escalate.init(data.id, data.escalateStatus);
    });

    $('#dropdown-menu-recommend').click(function () {
      app.page.review.show();
    });

    $('#dropdown-menu-review-recommend').click(function () {
      app.page.review.approval.show();
    });

    $('#dropdown-menu-approve').click(function () {
      app.page.approval.show();
    });

    $('#dropdown-menu-reassign').click(function () {
      app.page.reassign.init(data);
    });

    $('#dropdown-menu-payment').click(function () {
      app.page.finance.submit(data.id);
    });

    $('#dropdown-menu-certificate').click(function () {
      var ids = data.lineItems.filter(e => e.certificateID)
        .map(e => e.certificateID);

      app.page.certificate.preview(ids);
    });

    $('#dropdown-menu-notes').click(function () {
      app.page.notes.new.init(page.id);
    });

    $('#dropdown-menu-proceedforreview').click(function () {
      proceedForReview();
    });

    $('#dropdown-menu-reinstate').click(function () {
      app.page.reinstate.init(data);
    });
   
    app.page.notes.new.callback = app.page.notes.add;
  });


})(app.page);

$(document).ready(function () {
  app.page.init();

  $(".nav-tabs a").click(function () {
    var type = $(this).data('type');
    if (type == 'finance') {

      app.page.finance.init(app.page.model);

    } else if (type == 'menu'
      || type == 'ingredients') {

      setTimeout(function () {
        $($.fn.dataTable.tables(true)).css('width', '100%');
        $($.fn.dataTable.tables(true)).DataTable().columns.adjust().draw();
      }, 200);

    } else if (type == 'notes') {
      app.page.notes.init(app.page.id);
    }
  });

  $('#exportPDF').click(function () {
    app.page.export.show();
  });

  app.onHashtagTapped = function (type, id) {
    if (type === 'rfa') {

      var rfas = app.page.getData().rfAs;
      if (rfas.length > 0
        && rfas.filter(e => e.id == id).length == 1) {
        $('.rfa .container a[data-id="' + id + '"]')[0].click();
        return true;
      }

    }
    return false;
  };
});

window.onbeforeunload = e => {
  return app.page.isDirty() ? 'You have unsaved changes that will be lost. Are you sure you want to leave this page?' : null;
};
