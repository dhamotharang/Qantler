(function(self){

  self.errorInterceptor = function (res) {
    if (!res.ok) {
      throw res.json();
    }
    return res;
  }

  self.catch = function (err) {
    err.then(data => {

      if (data.Code == 400) {
        Swal.fire({
          icon: 'error',
          title: 'Oops...',
          text: data.Message
        });
      } else {
        window.location.href = '/error/' + data.Code + '?message=' + encodeURI(data.Message);
      }

    });
  }

})(app.http = app.http || {});