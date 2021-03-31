(function (self) {

  self.init = function (data) {
    if (!data) {
      $('#contactPerson').addClass('d-none');
      return;
    }

    render(data);
  }

  function render(data) {
    var contactTemplate = $('.joborder-template .contact');

    $('#contactinfo').removeClass('d-none');
    $("#contactName").html(data.name);

    $("#contactInfos").empty();
    if (data.contactInfos) {
      data.contactInfos.forEach(e => {

        $("#contactInfos").append(contactTemplate.prop('outerHTML')
          .replaceAll('{label}', e.typeText)
          .replaceAll('{value}', '   ' + e.value)
          .replaceAll('{bg}', e.isPrimary ? 'badge-primary' : 'badge-outline-dark'));
      });
    }
  }  
})(app.page.contactinfo = app.page.contactinfo || {});