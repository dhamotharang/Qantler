(function () {

  var cache = {};

  var controller;

  function reset() {
    $('.certificate .body').empty();
  }

  function render(batchID) {
    var data = cache[batchID];
    if (!data) {
      fetchData(batchID);
      return;
    }

    certificate.render(data);
  }

  function fetchData(batchID) {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Initiazing. Please wait..');

    fetch('/api/certificate/batch/preview/' + batchID + '?isPreview=true', {
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

        cache[batchID] = res;

        render(batchID);

      }).catch(app.http.catch);
  }

  this.preview = function (batchID) {
    reset();
    render(batchID);
  }

}).apply(app.page.preview = app.page.preview || {});