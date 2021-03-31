(function (self) {

  var data;
  var request;

  var controller;

  var currentBill;

  var template = '<div class="invoice #class">' +
    '<div class="sno"><p>#sno</p></div>' +
    '<div class="text"><p>#text</p></div>' +
    '<div class="qty"><p>#qty</p></div>' +
    '<div class="unitPrice"><p>#unitPrice</p></div>' +
    '<div class="fee"><p>#fee</p></div>' +
    '<div class="gst"><p>#gst</p></div>' +
    '<div class="total"><p>#total</p></div>' +
    '</div>';

  var others = [];

  self.init = function (d) {
    request = d;
    if (!data) {
      fetchData(d.id);
    }
  }

  self.submit = function (requestID) {

    Swal.fire({
      text: "Are you sure you want to proceed to payment?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#2196f3',
      confirmButtonText: 'Yes, proceed!'
    }).then((result) => {
      if (result.isConfirmed) {

        dispose();

        controller = new AbortController();

        app.showProgress('Processing. Please wait...');

        fetch('/api/request/' + requestID + '/status/payment?billID=' + currentBill.id, {
          method: 'POST',
          cache: 'no-cache',
          credentials: 'include',
          signal: controller.signal,
          body: JSON.stringify(others),
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
          }
        }).then(app.http.errorInterceptor)
          .then(res => res.json())
          .then(res => {

            location.reload(true);

          }).catch(app.http.catch);
      }
    });
  }

  function dispose() {
    if (controller) {
      controller.abort();
    }
  }

  function fetchData(requestID) {
    dispose();

    controller = new AbortController();

    app.showProgress('Loading. Please wait...');

    fetch('/api/bill/list?requestID=' + requestID, {
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
        data = res.sort((a, b) => moment.utc(b.issuedOn).diff(moment.utc(a.issuedOn)));

        app.dismissProgress();

        invalidate();
      }).catch(app.http.catch);
  }

  function invalidate() {
    if (!data) {
      return;
    }

    var hasPayment = data.filter(e => e.payments).length > 0;
    if (hasPayment) {
      $('.payments').removeClass('d-none');
    }

    renderInvoices();
  }

  function renderInvoices() {
    var container = $('.invoices');
    container.empty();

    var totalFee = 0;
    var totalGst = 0;
    var gst = 0;

    data.forEach(b => {

      totalFee += b.amount;
      totalGst += b.gstAmount;

      var html = '<div class="col-md-12 grid-margin">' +
        '<div class="card">' +
        '<div class="card-body">';

      html += '<div class="invoice info">' +
        '<div><p><b>Issued On:</b>&nbsp;' + app.utils.formatDate(b.issuedOn, true) + '</p></div>' +
        '<div class="action">';

      if (b.invoiceNo) {
        html += '<p><b>Invoice #:</b>&nbsp;' + b.invoiceNo + '</p>';
      }
        

      html += '</div>' +
        '</div>';

      html += template.replace('#class', 'header')
        .replace('#sno', '')
        .replace('#text', 'Fee Type')
        .replace('#qty', 'Qty')
        .replace('#unitPrice', 'Unit Price')
        .replace('#fee', 'Fee')
        .replace('#gst', 'GST(' + app.number.precise(app.number.format(b.gst * 100)) + '%)')
        .replace('#total', 'Total');

      var othersData = { a: 0, b: 0 };

      if (b.lineItems) {
        var lineItems = b.lineItems.sort((a, b) => {
          var result = a.sectionIndex - b.sectionIndex;
          if (result == 0) {
            result = a.index - b.index;
          }
          return result;
        });

        var indexOffset = 0;
        var currSection;

        lineItems.forEach((li, i) => {

          if (li.sectionIndex != currSection) {

            // This assumes to insert an editable section before deduction sections.
            if (request.status == 600
              && request.statusMinor == 610
              && b.status == 100
              && li.sectionIndex == 4) {
              currentBill = b;
              othersData = insertEditableSection();
              html += othersData.c;

              indexOffset = others.length;
            }

            currSection = li.sectionIndex;

            html += template.replace('#class', 'section')
              .replace('#sno', '')
              .replace('#text', li.section)
              .replace('#qty', '')
              .replace('#unitPrice', '')
              .replace('#fee', '')
              .replace('#gst', '')
              .replace('#total', '');
          }

          html += template.replace('#class', 'item')
            .replace('#sno', i + indexOffset + 1)
            .replace('#text', li.descr)
            .replace('#qty', app.number.precise(li.qty))
            .replace('#unitPrice', app.number.financial(li.unitPrice))
            .replace('#fee', app.number.financial(li.amount))
            .replace('#gst', app.number.financial(li.gstAmount))
            .replace('#total', app.number.financial(li.totalAmount));

          // Insert editable section if last item is not deductions.
          if (request.status == 600
            && request.statusMinor == 610
            && b.status == 100
            && li.sectionIndex != 4
            && i == lineItems.length - 1) {
            currentBill = b;

            othersData = insertEditableSection();

            indexOffset = others.length;

            html += othersData.c;
          }
        });
      }

      // Invoice footer
      html += '<div class="invoice footer sub">' +
        '<p>' + (b.type == 0 ? 'Total Amount:' : 'Total Outstanding Amount') + '</p>' +
        '<p class="value">' + app.number.financial(b.amount + b.gstAmount + othersData.a + othersData.b) + '</p>' +
        '</div>';

      html += '</div>' +
        '</div>' +
        '</div>';

      container.append(html);

      renderPayment(b.typeText, b.payments);
    });
  }

  function insertEditableSection(index) {

    var html = '<div class="invoice section others">' +
      '<div class="sno"><p></p></div>' +
      '<div class="text"><p>Others</p></div>' +
      '<div class="action">' +
      '<button type="button" class="btn btn-light btn-fw" data-toggle="modal" data-target="#invoiceAddLineItemModal"><i class="mdi mdi-plus" ></i>Add Item</button>' +
      '</div></div>';

    if (others.length == 0) {
      html += '<div class="invoice section others">' +
        '<div class="placeholder"><span>To add custom line item. Tap "+ Add Item" button.</span></div>' +
        '</div>';
    }

    var amount = 0;
    var gstAmount = 0;

    others.forEach((e, i) => {
      html += template.replace('#class', 'item')
        .replace('#sno', '<span class="mdi mdi-minus-circle minus" data-index="' + i + '"></span>')
        .replace('#text', e.descr)
        .replace('#qty', app.number.precise(e.qty))
        .replace('#unitPrice', app.number.financial(e.unitPrice))
        .replace('#fee', app.number.financial(e.amount))
        .replace('#gst', app.number.financial(e.gstAmount))
        .replace('#total', app.number.financial(e.totalAmount));

      amount += e.amount;
      gstAmount += e.gstAmount;
    });

    if (!app.hasPermission(3)
      && !app.hasPermission(7)) {
      html = '';
    }

    return { a: amount, b: gstAmount, c: html };
  }

  function renderPayment(billType, payments) {
    if (!payments) {
      return;
    }

    payments = payments.sort((a, b) => b.id - a.id);

    payments.forEach(payment => {
      var html = '<li class="timeline-item">' +
        '<p><span class="font-weight-bold">' + billType + '&nbsp;Payment</span>&nbsp;&nbsp;<span class="badge badge-inverse-' + app.common.paymentStatusColor(payment.status) + ' status">' + payment.statusText + '</span></p>' +
        '<p>' + payment.transactionNo + ',&nbsp;<span class="font-weight-bold">' + app.number.financial(payment.amount + payment.gstAmount) + ' SGD</span></p>' +
        '<p><span class="font-weight-bold">' + payment.modeText + '</span>,&nbsp;' + payment.methodText + '</p>' +
        '<p class="event-time">' + app.utils.formatDateTime(payment.paidOn, true) + '</p>' +
        '</li>';

      $('.payments .timeline').append(html);
    });
  }

  $(function () {

    $('.invoices').on('click', '.invoice.section.others button', function () {
      self.invoice.init(currentBill.gst);
    });

    $('.invoices').on('click', '.minus', function () {
      var index = $(this).data('index');
      others = others.filter((e, i) => i != index);
      renderInvoices();
    });

    app.page.finance.invoice.callback = function (d) {
      others.push(d);

      renderInvoices();
    };

  });

})(app.page.finance = app.page.finance || {});