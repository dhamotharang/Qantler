(function () {
  'use strict'

  var controller;

  var updateCallback;

  this.render = function (data) {
    if (!data) {
      return;
    }

    updateViewState(data);
    renderInfo(data);
  }

  function updateViewState(data) {
    var approveBtn = $('#approveButton');
    var rejectBtn = $('#rejectButton');

    approveBtn.addClass('d-none');
    rejectBtn.addClass('d-none');

    if (data.status == 200
      && data.mode == 1
      && app.hasPermission(14)) {
      approveBtn.removeClass('d-none');
      rejectBtn.removeClass('d-none');
    }

    if (data.mode == 1) {
      approveBtn.attr('data-toggle', 'modal');
      approveBtn.attr('data-target', '#offlinePaymentModal');
    }
  }

  function renderInfo(data) {
    if (!data) {
      return;
    }

    app.page.info.render(data);

    app.page.bank.render(data.bank);

    app.page.contactinfo.init(data.contactPerson);

    app.page.invoice.render(data);

    app.page.notes.render(data.notes);

    app.page.activities.render(data.logs);
  }

  function invalidate(data) {
    updateViewState(data);
    renderInfo(data);
  }

  function doAction(action, callback) {
    dispose();
    controller = new AbortController();

    app.showProgress('Processing. Please wait...');

    fetch('/api/payments/' + app.page.id + '/action?status=' + action, {
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

        invalidate(res);

        if (updateCallback) {
          updateCallback(res);
        }

        callback();

      }).catch(app.http.catch);
  }

  function dispose() {
    if (controller) {
      controller.abort();
    }
  }  

  this.updateCallback = function (callback) {
    updateCallback = callback;
  }

  $(function () {
    $('#approveButton').click(function () {

      var data = app.page.model;
      if (data.mode == 0) {
        Swal.fire({
          title: 'Input Amount Received',
          input: 'text',
          inputLabel: 'Amount Received',
          inputValue: '',
          inputPlaceholder: 'Enter amount received',
          icon: 'warning',
          showCancelButton: true,
          confirmButtonColor: '#2196f3',
          confirmButtonText: 'Yes, approve it!',
          inputValidator: (value) => {
            if (!value) {
              return 'You need to enter paid amount!'
            }
            if (value != app.page.model.totalAmount) {
              return 'Paid amount isn\'t equal to bill amount!'
            }
          }
        }).then((result) => {
          if (result.isConfirmed) {
            doAction(300, function () {
              Swal.fire(
                'Approved!',
                'Payment has been approved.',
                'success'
              )
            });
          }
        });

        $('.swal2-confirm').prop('disabled', true);

        $('.swal2-popup .swal2-input').on('change input paste', function () {

          if ($('.swal2-popup .swal2-input').val() != app.page.model.totalAmount) {
            $('.swal2-confirm').prop('disabled', true);
            return;
          }
          $('.swal2-confirm').prop('disabled', false);
        });
      }
      else if (data.mode == 1) {
        //$('#offlinePaymentModal').modal('show');
        app.page.offline.init(data);
      }
    });    

    $('#rejectButton').click(function () {

      Swal.fire({
        text: 'Are you sure you want to reject this payment?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#ff6258',
        confirmButtonText: 'Yes, reject it!'
      }).then((result) => {
        if (result.isConfirmed) {
          doAction(400, function () {
            Swal.fire(
              'Rejected!',
              'Payment has been rejected.',
              'success'
            )
          });
        }
      });
    });

    $('#closeButton').click(function () {
      dispose();
    });

    $('#notesButton').click(function () {
      app.page.notes.new.init(app.page.id);
    });
  });

}).apply(app.page = app.page || {});

$(document).ready(function () {
  app.page.render(app.page.model);
});