(function () {

  var selectedOfficer;

  var controller;

  var data;

  this.init = function (d) {
    data = d;
    reset();
    selectedOfficer = null;
  }

  function setSelectedOfficer(o) {
    selectedOfficer = o;

    if (selectedOfficer) {
      $('.modal.reassign #to').val(app.utils.titleCase(o.name));
    }
  }

  function reset() {
    $('.modal.reassign #to').val('');
    $('.modal.reassign textarea').val('');

    clearError();
  }

  function submit() {
    if (!validate()) {
      return;
    }

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Re-assigning. Please wait...');

    var param = 'officerID=' + encodeURI(selectedOfficer.id) +
      '&officerName=' + encodeURI(selectedOfficer.name) +
      '&notes=' + encodeURI($('.modal.reassign textarea').val());

    fetch('/api/request/' + data.id + '/reassign?' + param, {
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
        app.dismissProgress();
        location.reload(true);
      }).catch(app.http.catch);
  }

  function validate() {
    clearError();

    if (selectedOfficer == null) {
      $('.modal.reassign #to').closest('.form-group').addClass('has-danger');
    }

    var textarea = $('.modal.reassign textarea');

    if (app.utils.isNullOrEmpty(textarea.val().trim())) {
      textarea.closest('.form-group').addClass('has-danger');
    }

    return $('.modal.reassign .has-danger').length == 0;
  }

  function clearError() {
    $('.modal.reassign .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  }

  function initSearchOfficer() {
    app.select.officer.onSelect = function (o) {
      setSelectedOfficer(o);
    };

    var skip = [data.assignedTo];
    if (selectedOfficer) {
      skip.push(selectedOfficer.id);
    }

    app.select.officer.init({
      key: 'auditors',
      title: 'Select Auditor',
      permissions: [1],
      skip: skip
    });
  }

  $(function () {
    $('.modal.reassign .search').click(function () {
      initSearchOfficer();
    });

    $('.modal.reassign .submit').click(function () {
      submit();
    });
  });

}).apply(app.page.reassign = app.page.reassign || {});