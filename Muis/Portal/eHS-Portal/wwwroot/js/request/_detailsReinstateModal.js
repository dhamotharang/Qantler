(function () {

  var controller;

  var data;

  var masterList;

  var model = {};

  this.init = function (d) {
    data = d;

    $(".reinstate textarea").val("");

    if (!masterList) {
      fetchMaster();
    }
  }

  function fetchMaster() {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Initializing. Please wait...');

    fetch('/api/master/101', {
      method: 'GET',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(res => {
        app.dismissProgress();

        masterList = res;

        $('#reasonSelect').select2({
          placeholder: "Select",
          data: res.map(e => {
            return { id: e.id, text: e.value }
          })
        });

        if (res && res.length > 0) {
          model.reason = res[0];
        }

      }).catch(app.http.catch);
  }

  function submit() {
    if (!validate()) {
      return;
    }

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Re-instating. Please wait...');

    fetch('/api/request/' + data.id + '/reinstate', {
      method: 'PUT',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      body: JSON.stringify(model),
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(r => {
        location.reload(true);
      })
      .catch(app.http.catch);
  }

  function validate() {
    clearError();

    var reason = $('#reasonSelect').select2('data');
    if (reason.length <= 0) {
      $('.modal.reinstate #reasonSelect').closest('.form-group').addClass('has-danger');
    }

    return $('.modal.reinstate .has-danger').length == 0;
  }

  function clearError() {
    $('.modal.reinstate .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  }

  $(function () {
    $('.reinstate .submit').click(function () {
      submit();
    });

    $('#reasonSelect').on('select2:select', function (e) {
      model.reason = {
        id: e.params.data.id,
        value: e.params.data.text
      };
    });

    $(".reinstate textarea").on('change input paste', function () {
      model.notes = $(this).val().trim();
    });
  });

}).apply(app.page.reinstate = app.page.reinstate || {});