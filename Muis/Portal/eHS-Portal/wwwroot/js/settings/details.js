(function (page) {

  var isSave = false;

  app.page.model.requestSettings.forEach(a => {
    $('#req_' + a.type).val(a.value);
  });

  app.page.model.jobOrderSettings.forEach(a => {
    $('#job_' + a.type).val(a.value);
  });

  $(".btn-update").click(function () {
    var valid = true;
    isSave = true;
    $(".err_message").html("");
    $('#form1').find('input[type="text"]').each(function () {
      if ($(this).val() == "" || $(this).val() == undefined || $(this).val() == "0") {
        var inputID = $(this).attr("id");
        $('#' + inputID + '').next().html("Required");
        $('#' + inputID + '').addClass('required');
        valid = false;
      }
      else {
        var inputID = $(this).attr("id");
        $('#' + inputID + '').next().html("");
        $('#' + inputID + '').removeClass('required');
      }

    });
    if (valid) {
      updateSystemConfig()
    }

  });

  function updateSystemConfig() {
    var rData = app.page.model.requestSettings;
    var jData = app.page.model.jobOrderSettings;
    var rTempData = [], jTempData = [];
    var a = 0, c = 0;

    for (var m = 0; m < rData.length; m++) {
      var objIndex = rData.findIndex((obj => obj.id == m + 1));
      if (objIndex != null && objIndex > -1) {
        if (rData[objIndex].value != $('#req_' + rData[objIndex].type + '').val()) {
          rTempData[a] = rData[objIndex];
          rTempData[a].value = $('#req_' + rData[objIndex].type + '').val();
          a += 1;
        }
      }
    }

    for (var o = 0; o < jData.length; o++) {
      var objIndex = jData.findIndex((obj => obj.id == o + 1));

      if (objIndex != null && objIndex > -1) {
        if (jData[objIndex].value != $('#job_' + jData[objIndex].type + '').val()) {
          jTempData[c] = jData[objIndex];
          jTempData[c].value = $('#job_' + jData[objIndex].type + '').val();
          c += 1;
        }
      }
    }

    if (rTempData.length > 0 || jTempData.length > 0) {
      var data = {
        requestSettings: rTempData,
        jobOrderSettings: jTempData
      }
      var controller;
      if (controller) {
        controller.abort();
      }

      controller = new AbortController();

      app.showProgress('Processing. Please wait...');

      fetch('/api/settings/systemConfig', {
        method: 'PUT',
        cache: 'no-cache',
        credentials: 'include',
        signal: controller.signal,
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
      }).then(app.http.errorInterceptor)
        .then(res => res.json())
        .then(r => {
          app.dismissProgress();
          location.reload(true);
        })
        .catch(app.http.catch);
    } else {
      Swal.fire({
        title: 'None of the system configuration has modified.'
      })
    }
  }

  $('.integerInput').inputmask({
    regex: '[1-9][0-9]',
    placeholder: ""
  });

  $('input').keyup(function () {
    if (isSave) {
      if ($(this).val() == "" || $(this).val() == undefined || $(this).val() == "0") {
        var inputID = $(this).attr("id");

        $('#' + inputID + '').next().html("Required");

        $('#' + inputID + '').addClass('required');
      }
      else {
        var inputID = $(this).attr("id");

        $('#' + inputID + '').next().html("");

        $('#' + inputID + '').removeClass('required');
      }
    }
  });

})(app.page);