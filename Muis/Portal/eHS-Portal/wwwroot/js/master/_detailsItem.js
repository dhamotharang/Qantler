var value = "";
var dataitem;
var type;
var controller;

(function () {

  this.init = function (temp, tempType) {
    type = tempType;

    if (temp != null) {
      dataitem = temp;
      $('.value').val(dataitem.value);
      $('.modal-title').html('Edit Reason');
    }
    else {
      $('.modal-title').html('Add Reason');
    }

    this.enableDisableButton();
  };

  this.enableDisableButton = function enableDisableButton() {
    if ($('.value').val() != '') {
      $('.item-button-save').prop('disabled', false);
    }
    else {
      $('.item-button-save').prop('disabled', true);
    }
  };

  function submit() {
    if (controller) {
      controller.abort();
    }

    var saveModel = "";
    var method = "";
    if (dataitem == '' || dataitem == undefined) {
      saveModel =
      {
        Value: $('.value').val(),
        Type: type
      };
      method = "Post";
    }
    else {
      saveModel =
      {
        Type: dataitem.type,
        ID: dataitem.id,
        Value: $('.value').val()
      }

      method = "Put";
    }

    app.showProgress('Processing. Please wait...');

    $("#MasterDetailModal").removeClass("in");
    $(".modal-backdrop").remove();
    $("#MasterDetailModal").hide();

    fetch('/api/master/', {
      method: method,
      cache: 'no-cache',
      credentials: 'include',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(saveModel)
    })
      .then(app.http.errorInterceptor)
      .then(response => response.json())
      .then(r => {
        if (r) {        
          app.dismissProgress();
          location.reload(true);
        }
      }).catch(app.http.catch);
  };

  $(function () {
    $('.item-button-save').click(function () {
      submit();
    });

    $('#item-text').keyup(function () {
      app.page.Item.enableDisableButton();
    });

    $('.item-button-close').click(function () {
      $('#item-text').val('');
      $("#MasterDetailModal").removeClass("in");
      $(".modal-backdrop").remove();
      $("#MasterDetailModal").hide();
    });
  });
}).apply(app.page.Item = app.page.Item || {});