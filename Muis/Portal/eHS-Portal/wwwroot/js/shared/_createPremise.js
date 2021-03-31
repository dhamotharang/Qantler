app.select = app.select || {};
app.select.premise = app.select.premise || {};

(function (self) {
  'use strict';

  var model = {};

  var controller;
  var isSave = false;

  $('.create-premise .integerInput').inputmask({
    regex: '[0-9]*'
  });

  self.init = function (customerID) {
    model = {
      CustomerID: customerID,
      Type: 1
    };

    setup();
  }

  self.dismiss = function () {
    $('.create-premise .close').click();
  }

  self.createCallback = function (c) {
  }

  function setup() {
    $('.create-premise #buildingName').val(null);
    $('.create-premise #floorNo').val(null);
    $('.create-premise #unitNo').val(null);
    $('.create-premise #blockNo').val(null);
    $('.create-premise #street').val(null);
    $('.create-premise #postal').val(null);

    clearError();
    isSave = false;
  }

  function validate() {
    clearError();

    if (app.utils.isNullOrEmpty(model.Postal)) {
      $('.create-premise #postal').closest('.form-group').addClass('has-danger');
    }

    if (app.utils.isNullOrEmpty(model.UnitNo)) {
      $('.create-premise #unitNo').closest('.form-group').addClass('has-danger');
    }

    if (app.utils.isNullOrEmpty(model.FloorNo)) {
      $('.create-premise #floorNo').closest('.form-group').addClass('has-danger');
    }

    return $('.create-premise .has-danger').length == 0;
  }

  function clearError() {
    $('.create-premise .has-danger').each(function () {
      $(this).removeClass('has-danger');
    });
  }

  function submit() {
    isSave = true;

    if (!validate()) {
      return;
    }

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Creating. Please wait...');

    fetch('/api/premise', {
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
      .then(res => {
        app.dismissProgress();

        self.dismiss();
        self.createCallback(res);

      }).catch(app.http.catch);
  }

  $(function () {

    $('.create-premise #buildingName').on('change input paste', function () {
      model.BuildingName = $(this).val().trim();
    });

    $('.create-premise #floorNo').on('change input paste', function () {
      model.FloorNo = $(this).val().trim();
      if (isSave) {
        validate()
      }
    });

    $('.create-premise #unitNo').on('change input paste', function () {
      model.UnitNo = $(this).val().trim();
      if (isSave) {
        validate()
      }
    });

    $('.create-premise #blockNo').on('change input paste', function () {
      model.BlockNo = $(this).val().trim();
    });

    $('.create-premise #street').on('change input paste', function () {
      model.Address1 = $(this).val().trim();
    });

    $('.create-premise #postal').on('change input paste', function () {
      model.Postal = $(this).val().trim();
      if (isSave) {
        validate()
      }
    });

    $('.create-premise #createBtn').click(function () {
      submit();
    });
  });

})(app.select.premise.create = app.select.premise.create || {});