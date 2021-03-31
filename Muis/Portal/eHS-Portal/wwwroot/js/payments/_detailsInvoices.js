(function () {
  'use strict'

  var controller;

  function clear() {
    $('#invoices').empty();
  }

  this.render = function (data) {
    if (!data) {
      return;
    }

    clear();

    renderInvoices(data.bills);
  }

  function renderInvoices(bills) {
    if (!bills) {
      return;
    }

    var lineItem = $("#invoiceItem");
    var lineSection = $("#invoiceSection");

    var container = $('#invoices');
    container.empty();

    bills = bills.sort((a, b) => a.id - b.id);

    var totalFee = 0;
    var totalGst = 0;
    var gst = 0;

    bills.forEach((b, bi) => {
      var template = $("#invoiceMain").prop('innerHTML');

      totalFee += b.amount;
      totalGst += b.gstAmount;

      var html = '';

      template = template.replace("{invoiceIssuedOn}", app.utils.formatDate(b.issuedOn, true))
        .replace("{showissuedon}", "d-none")
        .replace("{invoiceno}", b.invoiceNo)
        .replace("{showinvoiceno}", "d-none")
        .replace("{requestid}", b.requestID)
        .replace("{showviewrequest}", "d-none")
        .replace("{gst}", app.number.precise(app.number.format(b.gst * 100)));

      if (b.lineItems) {
        var lineItems = b.lineItems.sort((a, b1) => {
          var result = a.sectionIndex - b1.sectionIndex;
          if (result == 0) {
            result = a.index - b1.index;
          }
          return result;
        });
      }

      var currSection;

      lineItems.forEach(li => {
        if (li.sectionIndex != currSection) {
          currSection = li.sectionIndex;

          html += lineSection
            .prop('innerHTML')
            .replace('{section}', li.section);
        }

        html += lineItem
          .prop('innerHTML')
          .replace('#class', 'item')
          .replace('{invoiceItemSNo}', li.index)
          .replace('{invoiceItemText}', li.descr)
          .replace('{invoiceItemQty}', app.number.precise(li.qty))
          .replace('{invoiceItemUnitPrice}', app.number.financial(li.unitPrice))
          .replace('{invoiceItemFee}', app.number.financial(li.amount))
          .replace('{invoiceItemGST}', app.number.financial(li.gstAmount))
          .replace('{invoiceItemTotal}', app.number.financial(li.totalAmount));
      });
      template = template.replace("{invoicebody}", html);

      template = template.replace("{invoiceAmount}", app.number.financial(b.amount + b.gstAmount));

      container.append(template + "<br>");
    });
    var amountSummary = $("#invoiceFooterSummary")

    container.append(amountSummary.prop('innerHTML')
      .replace('{invoiceTotalValue}', app.number.financial(totalFee + totalGst)));
  }

  function dispose() {
    if (controller) {
      controller.abort();
    }
  }

}).apply(app.page.invoice = app.page.invoice || {});