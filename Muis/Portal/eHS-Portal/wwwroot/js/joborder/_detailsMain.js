(function (self) {

  self.init = function (d) {

    var schemes = '';

    if (d.lineItems) {
      d.lineItems.forEach(e => {
        if (schemes.length > 0) {
          schemes += ', ';
        }

        schemes += app.common.formatScheme(e.schemeText, e.subSchemeText);
      });
    }

    $('.main .schemes span').html(schemes);

    $('.main .scheduledOn span').html(app.utils.formatDateTime(d.scheduledOn));

    $('.main .scheduledOnTo span').html(app.utils.formatDateTime(d.scheduledOnTo));

    $('.main .premise span').html(app.utils.formatPremise(d.premises[0]));

    $('.main .targetDate span').html(app.utils.formatDate(d.targetDate));

    if (d.type != 0) {
      $('.main .refNo').remove();
    }
  }

})(app.page.main = app.page.main || {});