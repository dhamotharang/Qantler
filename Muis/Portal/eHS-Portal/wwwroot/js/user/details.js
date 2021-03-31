(function (self) {

  var controller;

  function init() {
    var data = self.model.data;

    $('.basic #name').val(data.name);
    $('.basic #desig').val(data.designation);
    $('.basic #email').val(data.email.replace('@muis.gov.sg', ''));

    if (data.permissions) {
      var i;
      for (i = 0; i < data.permissions.length; i++) {
        var hasPermission = app.permission.hasPermission(data.permissions, i);

        if (hasPermission) {
          $('#permissions-' + i).iCheck('check');
        }
      }
    }

    $('#role').val(data.role).trigger('change');
    $('#status').val(data.status).trigger('change');

    if (isClusterAndRequestTypesVisible()) {
      $('.clusters').removeClass('d-none');
      $('.request-types').removeClass('d-none');

      if (data.clusters) {
        var clusters = data.clusters.map(e => e.id.toString());
        $('#clusters').val(clusters).trigger('change');
      }

      if (data.requestTypes) {
        data.requestTypes.forEach(e => {
          $('#request-type-' + e).iCheck('check');
        });
      }
    }
  }

  function isClusterAndRequestTypesVisible() {
    if (self.model.data.role == 800) {
      return false;
    }

    return app.permission.hasPermission(self.model.data.permissions, 1)
      || app.permission.hasPermission(self.model.data.permissions, 22)
      || app.permission.hasPermission(self.model.data.permissions, 9);
  }

  function confirmResetPassword() {
    Swal.fire({
      text: "Are you sure you want to reset user's password?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#2196f3',
      confirmButtonText: 'Yes, reset it!'
    }).then((result) => {
      if (result.isConfirmed) {
        resetPassword();
      }
    });
  }

  function resetPassword() {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Resetting password. Please wait...');

    fetch('/api/user/' + self.id + '/password', {
      method: 'DELETE',
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

        Swal.fire(
          'Reset!',
          'Password has been reset.',
          'success'
        );

      }).catch(app.http.catch);
  }

  function setupPermissions() {
    //if (self.id != app.identity.id
    //  &&
      if (app.hasPermission(18)) {
      $('.reset').removeClass('d-none');
      $('.edit').removeClass('d-none');
    }
  }

  $(function () {
    var clusters = app.page.model.clusters.map(e => {
      var nodes = '';
      if (e.nodes) {
        e.nodes.forEach(n => {
          if (nodes.length > 0) {
            nodes += ', ';
          }
          nodes += n;
        });
      }

      return { id: e.id, text: e.district + (nodes ? ' (' + nodes + ')' : '') };
    });


    $(':input').inputmask();

    $('.icheck-flat input').iCheck({
      checkboxClass: 'icheckbox_flat-blue',
      radioClass: 'iradio_flat'
    });

    $('.iCheck-helper').each((i, e) => {
      $(e).css('pointer-events', 'none');
    });

    $('.select-multiple').select2({
      disabled: true
    });

    $('.select-single').select2({
      disabled: true
    });

    $('#clusters').select2({
      data: clusters
    });

    $('#editButton').click(function () {
      window.location.href = '/user/form/' + app.page.model.data.id;
    });

    $('#resetButton').click(function () {
      confirmResetPassword();
    });

    init();
    setupPermissions();
  });

})(app.page);