(function () {

  var data;
  var certificateIDs;

  var controller;

  function reset() {
    $('.certificate .body').empty();
  }

  function render() {
    if (!data) {
      fetchData();
      return;
    }

    certificate.render(data);
  }

  function fetchData() {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Initiazing. Please wait..');

    var params = '';

    $.each(certificateIDs, function (i, e) {
      params += '&ids[' + i + ']=' + e;
    });

    fetch('/api/certificate/preview?isPreview=true' + params, {
      method: 'GET',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'appliation/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(res => {
        app.dismissProgress();

        data = res;

        render();

      }).catch(app.http.catch);
  }

  this.preview = function (ids) {
    certificateIDs = ids;
    render();
  }

}).apply(app.page.certificate = app.page.certificate || {});