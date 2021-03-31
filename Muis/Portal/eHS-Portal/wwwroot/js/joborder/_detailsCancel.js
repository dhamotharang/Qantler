(function (self) {
  var data;
  var controller;
  var masterList;
  var model = {};

  self.init = function (d) {
    data = d;
    if (!masterList) {
      fetchMaster();
    }
    else {
      $('#cancelReasonSelect').val(masterList[0].id).trigger("change")
    }

  }

  function fetchMaster() {
    $(".reschedule textarea").val("");

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Initializing. Please wait...');

    fetch('/api/master/201', {
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

        $('#cancelReasonSelect').select2({
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
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();
    app.showProgress('Processing. Please wait...');

    fetch('/api/jobOrder/' + data.id + '/cancel', {
      method: 'POST',
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
      }).catch(app.http.catch);
  }

  $(function () {
    $('.cancel .primary').click(function () {
      submit();
    });

    $('#cancelReasonSelect').on('select2:select', function (e) {
      model.reason = {
        id: e.params.data.id,
        value: e.params.data.text
      };
    });

    $(".cancel textarea").on('change input paste', function () {
      model.notes = $(this).val().trim();
    });

  });
})(app.page.cancel = app.page.cancel || {});