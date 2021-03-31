(function (self) {

  self.defaultOptions = function () {
    return {
      url: '/api/file',
      maxFileSize: 5000000,
      allowedTypes: 'pdf,doc,docx,xls,xlsx,csv,png,bmp,jpg,gif,jpeg,txt',
      allowedExtensions: /(\.jpg|\.jpeg|\.png|\.gif|\.pdf|\.doc|\.docx|\.xls|\.xlsx|\.csv|\.txt)$/i,
      allowedExtensionsText: 'pdf,doc,xls,csv,png,jpg,jpeg,txt',
      onSubmit: function (e) {
        app.showProgress('Uploading! Please wait...');
      },
      onSuccess: function (e) {
        app.dismissProgress();
      },
      onError: function (e) {
        Swal.fire({
          icon: 'error',
          title: 'Oops...',
          html: 'Something went wrong!'
        });
      },
      onCancel: function (e) {
      }
    }
  };

  self.fileUploadConfig = function (options = self.defaultOptions()) {
    return {
      url: options.url,
      allowedTypes: options.allowedTypes,
      maxFileSize: options.maxFileSize,
      onSelect: function (files, data) {

        var file = files[0];
        if (file.size > options.maxFileSize) {
          Swal.fire({
            icon: 'error',
            title: 'Oops...',
            html: file.name + ' is not allowed!<br>Allowed max size: <strong>5MB</strong>.'
          });

          return false;
        }

        var allowedExtensions = options.allowedExtensions;
        if (!allowedExtensions.exec(file.name)) {
          Swal.fire({
            icon: 'error',
            title: 'Oops...',
            html: file.name + ' is not allowed!<br>Allowed extensions: <strong>' + options.allowedExtensionsText + '</strong>'
          });
        }

        return true;
      },
      onSubmit: function (e) {
        if (options.onSubmit) {
          options.onSubmit(e);
        }
      },
      onSuccess: function (files, data) {
        if (options.onSuccess) {
          options.onSuccess(data[0]);
        }
      },
      onError: function (e) {
        if (options.onError) {
          options.onError(e);
        }
      },
      onCancel: function (e) {
        if (options.onCancel) {
          options.onCancel(e);
        }
      }
    }
  };

})(app.file = app.file || {});