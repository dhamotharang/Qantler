(function (self) {

  $(function () {

    self.main.init(self.model);
    self.activities.init(self.model.logs);

    var schemes = self.model.lineItems.map(e => {
      return { id: e.scheme, text: e.schemeText };
    });

    self.findings.init(schemes, self.model.findings);

    self.invites.init(self.model);

    self.contactinfo.init(self.model.contactPerson);

    $('.quick-links #reschedule').click(function () {
      self.reschedule.init(self.model);
    });

    $('.quick-links #schedule').click(function () {
      self.schedule.init(self.model);
    });

    $('.quick-links #cancel').click(function () {
      self.cancel.init(self.model);
    });

    $('#dropdown-menu-notes').click(function () {
      app.page.notes.new.init(app.page.id);
    });
    app.page.notes.new.callback = app.page.notes.add;

  });

})(app.page);

$(document).ready(function () {
  $(".nav-tabs a").click(function () {
    var type = $(this).data('type');
    if (type == 'notes') {
      app.page.notes.init(app.page.id);
    }
  });
});