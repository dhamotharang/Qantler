var certificate = certificate || {};

(function () {
  this.formatDate = function (d) {
    if (!d) {
      return '';
    }
    return moment.utc(d).format('DD MMM YYYY');
  }

  this.formatPremise = function (prem) {
    var result = '';

    function append(val, separator) {
      if (!val || val.trim().length === 0) {
        return;
      }
      if (result.length > 0 && separator) {
        result += separator;
      }
      result += ' ' + val;
    }

    append(prem.schedule);

    append(prem.blockNo);
    append(prem.address1);
    append(prem.address2);

    var floorUnit = '';
    if (prem.floorNo) {
      floorUnit += '#' + prem.floorNo;
    }

    if (prem.unitNo) {
      if (floorUnit.length > 0) {
        floorUnit += '-';
      }
      floorUnit += prem.unitNo;
    }

    if (floorUnit.length > 0) {
      append(floorUnit);
    }

    append(prem.buildingName);
    append(prem.city, ',');
    append(prem.province, ',');
    append(prem.country, ',');
    append(prem.postal, '');

    return result.toUpperCase();
  }

  this.defaultRenderer = function (template, data, isPreview) {
    var self = this;
    if (!template) {
      return '';
    }

    return template.replace('#number', data.number)
      .replace("#customer", data.customerName.toUpperCase())
      .replace("#premise", certificate.formatPremise(data.premise))
      .replace("#expiry", certificate.formatDate(data.expiresOn))
      .replace("#issuedOnVal", certificate.formatDate(data.issuedOn))
      .replace("#issuedOnVisibility", (data.issuedOn ? '' : 'd-none'))
      .replace("#scheme", data.scheme.toUpperCase())
      .replace("#subScheme", (data.subScheme ? data.subScheme.toUpperCase() : ''))
      .replace("#serialNo", data.serialNo)
      .replace("#backgroundVisibility", (isPreview ? '' : 'd-none'));
  };

  this.getRenderer = function (template) {
    if (template == 0
      || template == 1) {
      return this.product.renderer;
    }
    return this.defaultRenderer;
  }

  this.onRender = function (model) {
    var self = this;
    var main = $('.certificate');
    if (model.isPreview) {
      main.addClass('preview');
    }

    var body = $('.certificate .body');

    var content = '';

    $.each(model.certificates, function (e, i) {
      if (e > 0) {
        if (model.isPreview) {
          content += '<div class="page-break"></div>';
        } else {
          content += '<div style="page-break-before: always;"></div>';
        }
      }

      var renderer = self.getRenderer(i.template);
      var template = self.getTemplate(i.template);

      content += renderer(template, i, model.isPreview);
    });

    body.html(content);
  }

  this.render = function (model) {
    this.onRender(model);
  };

}).apply(certificate = certificate || {});