(function (self) {
  var data;

  var request;

  var controller;

  var currentBill;

  var template = '';

  var others = [];

  self.init = function (d) {
    app.showProgress('Loading. Please wait...');

    data = d;

    invalidate();

    app.dismissProgress();
  }

  function dispose() {
    if (controller) {
      controller.abort();
    }
  }

  function invalidate() {
    if (!data) {
      return;
    }

    renderInvoices();
  }

  function renderInvoices() {
    var container = $('#invoiceBody');
    container.empty();

    var totalFee = 0;
    var totalGst = 0;
    var gst = 0;

    var b = data;

    totalFee += b.amount;
    totalGst += b.gstAmount;

    if (b.invoiceNo == null) {
      $("#info-invoiceNo").hide();
    }
    else {
      $("#invoiceIssuedID").html(b.invoiceNo);
    }

    if (b.issuedOn == null) {
      $("#info-issuedOn").hide();
    }
    else {
      $("#invoiceIssuedOn").html(app.utils.formatDate(b.issuedOn, true));
    }

    $("#invoiceGST").html(app.number.precise(app.number.format(b.gst * 100)));

    var html = '';
    var lineItem = $("#invoiceItem")
    var lineSection = $("#invoiceSection")

    var othersData = { a: 0, b: 0 };

    if (b.lineItems) {
      var lineItems = b.lineItems.sort((a, c) => {
        var result = a.sectionIndex - c.sectionIndex;
        if (result == 0) {
          result = a.index - c.index;
        }
        return result;
      });

      var indexOffset = 0;
      var currSection = -1;

      lineItems.forEach((li, i) => {

        if (li.sectionIndex != currSection) {
          currSection = li.sectionIndex;

          html += lineSection
            .prop('innerHTML')
            .replace('{section}', li.section);
        }

        html += lineItem
          .prop('innerHTML')
          .replace('#class', 'item')
          .replace('{invoiceItemSNo}', i + indexOffset + 1)
          .replace('{invoiceItemText}', li.descr)
          .replace('{invoiceItemQty}', app.number.precise(li.qty))
          .replace('{invoiceItemUnitPrice}', app.number.financial(li.unitPrice))
          .replace('{invoiceItemFee}', app.number.financial(li.amount))
          .replace('{invoiceItemGST}', app.number.financial(li.gstAmount))
          .replace('{invoiceItemTotal}', app.number.financial(li.totalAmount));
      });
    }

    $("#invoiceAmountType").html((b.type == 0 ? 'Total Amount:' : 'Total Outstanding Amount'));
    $("#invoiceAmount").html(app.number.financial(b.amount + b.gstAmount));

    container.append(html);
  }

})(app.page.finance = app.page.finance || {});

$(document).ready(function () {
  app.page.finance.init(app.page.model);
});