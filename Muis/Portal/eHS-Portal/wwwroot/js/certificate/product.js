var certificate = certificate || {};

(function () {

  this.renderer = function (template, data, isPreview) {
    if (!template) {
      return '';
    }
    var self = this;

    var content = '';

    if (data.itemGroups) {

      $.each(data.itemGroups, function (i, e) {

        if (i > 0) {
          if (isPreview) {
            content += '<div class="page-break"></div>';
          } else {
            content += '<div style="page-break-before: always;"></div>';
          }
        }

        var products = '';

        $.each(e.items, function (ii, ie) {

          products += '<li>' + ie.toUpperCase() + '</li>';
          console.log(i);
        });

        content += template.replace('#number', data.number)
          .replace("#customer", data.customerName.toUpperCase())
          .replace("#premise", certificate.formatPremise(data.premise))
          .replace("#expiry", certificate.formatDate(data.expiresOn))
          .replace("#issuedOnVal", certificate.formatDate(data.issuedOn))
          .replace("#issuedOnVisibility", (data.issuedOn ? '' : 'd-none'))
          .replace("#scheme", data.scheme.toUpperCase())
          .replace("#subScheme", (data.subScheme ? data.subScheme.toUpperCase() : ''))
          .replace("#serialNo", data.serialNo)
          .replace("#start", e.startIndex)
          .replace("#products", products)
          .replace("#backgroundVisibility", (isPreview ? '' : 'd-none'));
      });
    }

    return content;
  };

}).apply(certificate.product = certificate.product || {});