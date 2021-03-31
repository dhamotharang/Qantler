(function (self) {
  'use strict'

  var controller;

  var data;

  function init() {
    var data = self.model.data;

    $('.basic #name').val(data.name);
    $('.basic #desig').val(data.designation);

    if (data.email) {
      $('.basic #email').val(data.email.replace('@muis.gov.sg', ''));
    }

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

  function submit() {
    if (!validate()) {
      return;
    }

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress(app.page.model.isEdit
      ? 'Updating user. Please wait...'
      : 'Creating user. Please wait...');

    fetch('/api/user/form', {
      method: app.page.model.isEdit ? 'PUT' : 'POST',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      body: JSON.stringify(data),
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(res => {
        var isEdit = app.page.model.isEdit;
        var title = isEdit ? 'Updated!' : 'Created!';
        var msg = isEdit ? 'User has been updated.' : 'User has been created.';

        Swal.fire(
          title,
          msg,
          'success'
        ).then(result => {
          window.location.href = '/user/details/' + res.id;
        });
      }).catch(app.http.catch);
  }

  function validate() {
    clearError();

    if (app.utils.isNullOrEmpty($('#name').val().trim())) {
      $('#name').closest('.form-group').addClass('has-danger');
    }

    if (app.utils.isNullOrEmpty($('#desig').val().trim())) {
      $('#desig').closest('.form-group').addClass('has-danger');
    }

    var email = $('#email').val().replace('@muis.gov.sg', '');
    if (app.utils.isNullOrEmpty(email.trim())) {
      $('.email .error').html('Email is required');
      $('#email').closest('.form-group').addClass('has-danger');
    }
    else if (!app.utils.isValidEmail(email + '@muis.gov.sg')) {
      $('.email .error').html('Email is invalid');
      $('#email').closest('.form-group').addClass('has-danger');
    }

    var role = $('#role').select2('data');
    if (role.length == 0) {
      $('#role').closest('.form-group').addClass('has-danger');
    }

    return $('.has-danger').length == 0;
  }

  function clearError() {
    $('.has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  }

  function onRoleSelected(e) {
    data.permissions = '';

    switch (parseInt(e.id)) {
      case 100:
        data.permissions = '11';
        break;
      case 101:
        data.permissions = '10000000000000000000010';
        break;
      case 150:
        data.permissions = '10000000000000000000000000000110000000000';
        break;
      case 200:
        data.permissions = '1010001';
        break;
      case 300:
        data.permissions = '10000000000001111';
        break;
      case 400:
        data.permissions = '10011';
        break;
      case 500:
        data.permissions = '1000000011';
        break;
      case 600:
        data.permissions = '11111111111111111';
        break;
      case 700:
        data.permissions = '0000000000111';
        break;
      case 800:
        data.permissions = '111111111111111111111111111111111111111111';
        break;
    }

    data.role = parseInt(e.id);

    resetPermission();
  }

  function resetPermission() {
    var i;
    for (i = 0; i <= 22; i++) {
      var hasPermission = app.permission.hasPermission(data.permissions, i);

      $('#permissions-' + i).iCheck((hasPermission ? 'check' : 'uncheck'));
    }

    invalidate();
  }

  function invalidate() {
    if (isClusterAndRequestTypesVisible()) {
      $('.clusters').removeClass('d-none');
      $('.request-types').removeClass('d-none');
    } else {
      data.requestTypes = [];
      data.clusters = [];

      $('#clusters').val(null).trigger('change');
      $('input[type="checkbox"].request-type').iCheck('uncheck');


      $('.clusters').addClass('d-none');
      $('.request-types').addClass('d-none');
    }
  }

  function isClusterAndRequestTypesVisible() {
    if (data.role == 800) {
      return false;
    }

    return app.permission.hasPermission(data.permissions, 1)
      || app.permission.hasPermission(data.permissions, 9)
      || app.permission.hasPermission(data.permissions, 22);
  }

  function hasSelectedRequestTypes() {
    return data.requestTypes && data.requestTypes.length > 0;
  }

  $(function () {
    data = app.page.model.data; role

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

      return { id: e.id, text: e.district + (nodes ? ' (' + nodes + ')' : ''), district: e.district };
    });

    $(':input').inputmask();

    $('.icheck-flat input').iCheck({
      checkboxClass: 'icheckbox_flat-blue',
      radioClass: 'iradio_flat'
    });

    $('input').on('ifChecked', function (event) {
      var self = $(this);
      var id = self.data('id');
      if (self.hasClass('permission')) {
        data.permissions = app.permission.setPermission(data.permissions, id, '1');
        invalidate();
      } else if (self.hasClass('request-type')) {
        data.requestTypes = data.requestTypes || [];

        if (data.requestTypes.filter(e => e == id).length == 0) {
          data.requestTypes.push(id);
        }
      }
    });

    $('input').on('ifUnchecked', function (event) {
      var self = $(this);
      var id = self.data('id');
      if (self.hasClass('permission')) {
        data.permissions = app.permission.setPermission(data.permissions, id, '0');
        invalidate();
      } else if (self.hasClass('request-type')) {
        data.requestTypes = data.requestTypes || [];
        data.requestTypes = data.requestTypes.filter(e => e != id);
      }
    });

    $('.select-multiple').select2();

    $('.select-single').select2();

    $('#role').select2({
      placeholder: "Select a role"
    });

    $('#clusters').select2({
      placeholder: "Select a cluster",
      data: clusters
    });

    $('#role').on('select2:select', function (e) {
      onRoleSelected(e.params.data);
    });

    $('#status').on('select2:select', function (e) {
      data.status = parseInt(e.params.data.id);
    });

    $('#saveButton').click(function () {
      submit();
    });

    $("#name").on('change input paste', function () {
      data.name = $(this).val().trim();
    });

    $("#desig").on('change input paste', function () {
      data.designation = $(this).val().trim();
    });

    $("#email").on('change input paste', function () {
      data.email = ($(this).val().replace('@muis.gov.sg', '') + '@muis.gov.sg').trim();
    });

    $('#clusters').on('select2:select', function (e) {
      data.clusters = data.clusters || [];

      if (data.clusters.filter(c => c.id == e.params.data.id).length == 0) {
        data.clusters.push({
          id: parseInt(e.params.data.id),
          district: clusters.filter(c => c.id == e.params.data.id)[0].district
        });
      }
    });

    $('#clusters').on('select2:unselect', function (e) {
      data.clusters = data.clusters || [];
      data.clusters = data.clusters.filter(c => c.id != e.params.data.id);
    });

    init();
  });

})(app.page);