(function () {
  var temp = [];
  var fromDate = new Date();
  var dateText;
  var toAdd = false;

  Inputmask.extendAliases({
    'decimal': {
      integerDigits: 6,
      digits: 2,
      allowPlus: false,
      allowMinus: false,
      digitsOptional: false,
      numericInput: true,
      autoGroup: true,
      groupSeparator: ",",
      radixPoint: ".",
      placeholder: "0",
    }
  });

  var defaultTableElem = '<tr>' +
    '<td class="text-right">' +
    '<input  type="text"  id="newAmountID" onKeyUp="app.page.amountValidate(this)" ' +
    'class="form-control text-center font-weight-bold amount"  value=" "/>' +
    '<span class="err_message cust_AmtPosition text-danger"></span></td >' +
    '<td class="text-right">' +
    '<div id="newEffectiveDate" style="position:relative" class="input-group date datepicker effectiveDate">' +
    '<input type="text" class="form-control">' +
    '<span class="input-group-addon input-group-append border-left">' +
    '<span class="mdi mdi-calendar input-group-text"></span>' +
    '</span>' +
    '</div>' +
    '<span class="err_message cust_DatePosition text-danger"></span></td >' +
    '</tr>';

  app.page.duplicateCheck = function duplicateCheck() {
    var result = true;

    $.each($('[id^="effectiveFrom_"]'), function (parentIndex, parentElement) {
      var id = "effectiveFrom_" + parentIndex;
      var value = new Date(app.utils.formatDate($('#' + id).datepicker('getDate'))).toISOString().slice(0, 10);
      var duplicateValueIndex = [];

      $.each($('[id^="effectiveFrom_"]'), function (index, element) {
        var tempId = $(element).attr('id');
        var tempIndex = $(element).attr('index');
        var tempValue = new Date(app.utils.formatDate($('#' + tempId).datepicker('getDate'))).toISOString().slice(0, 10);

        if (value == tempValue) {
          duplicateValueIndex.push(tempIndex);
        }

      });

      duplicateValueIndex.splice(-1, 1);

      $.each(duplicateValueIndex, function (index, element) {
        result = false;
        $('#effectiveFrom_' + element).next(".err_message").html("Duplicate Effective From");
      });

    });

    return result;
  };

  app.page.validate = function validate() {
    if ($('#newEffectiveDate').datepicker('getDate') == null) {
      $('#newEffectiveDate').next(".err_message").html("Effective From is required");
      $('#newEffectiveDate').addClass('border-danger');
    }
    else {
      $('#newEffectiveDate').next(".err_message").html("");
      $('#newEffectiveDate').addClass('');
    }

    if ($('#newAmountID').val() == "" || $('#newAmountID').val() == "0.00") {
      $('#newAmountID').next(".err_message").html("Amount is required");
      $('#newAmountID').addClass('border-danger');
    }
    else {
      $('#newAmountID').next(".err_message").html("");
      $('#newAmountID').removeClass('border-danger');
    }

    if ($('#newAmountID').val() != "" && $('#newEffectiveDate').datepicker('getDate') != null && $('#newAmountID').val()
      != undefined && $('#newEffectiveDate').datepicker('getDate') != undefined) {
      var objIndex = temp.findIndex((obj => new Date(app.utils.formatDate(obj.effectiveFrom)).toISOString().slice(0, 10) ==
        new Date(app.utils.formatDate($('#newEffectiveDate').datepicker('getDate'))).toISOString().slice(0, 10)));

      if (objIndex != -1) {
        $('#newEffectiveDate').next(".err_message").html("Duplicate Effective From");
        $('#newEffectiveDate').addClass('border-danger');
      }

    }

  }

  app.page.validationCheck = function validationCheck() {
    var isValid = true;

    $.each($('[id^="amount_"]'), function (index, element) {

      if ($(element).val() == "" || $(element).val() == "0.00") {
        $(element).next(".err_message").html("Amount is required");
        $(element).addClass('border-danger');
        isValid = false;
      }

    });

    $.each($('[id^="effectiveFrom_"]'), function (index, element) {

      if ($(element).datepicker('getDate') == "") {
        isValid = false;
      }

    });

    return isValid;
  };

  app.page.reInitAmount = function reInitAmount() {
    var elem = defaultTableElem;

    temp.sort(function (a, b) {

      if (a.effectiveFrom < b.effectiveFrom) {
        return 1;
      }

      if (a.effectiveFrom > b.effectiveFrom) {
        return -1;
      }

      return 0;
    });

    $("#amountHistoryID").empty();
    var index = 0;

    temp.forEach(a => {
      elem += CreateElement(a.amount, a.effectiveFrom, index);
      index = index + 1;
    });

    $("#amountHistoryID").append(elem);

    DatePickerLoad('.date');

    $('#newEffectiveDate').datepicker('setDate', '');

    $(".amount").inputmask('decimal');

    $.each($('[id^="amount_"]'), function (index, element) {
      $(element).val(parseFloat(temp[index].amount.toString().replace(",","")).toFixed(2).toString().replace(".", ""));
    });

    $('.enteredDateValue').each(function () {
      $(this).datepicker('setDate', new Date($(this).find('input').val()));
    });

    app.page.validationCheck();

  };


  app.page.init = function init() {
    var History_elem = defaultTableElem;

    if (app.page.model.data.priceHistory != undefined && app.page.model.data.priceHistory.length > 0) {

      app.page.model.data.priceHistory.sort(function (a, b) {
        if (a.effectiveFrom < b.effectiveFrom) {
          return 1;
        }

        if (a.effectiveFrom > b.effectiveFrom) {
          return -1;
        }

        return 0;
      });

      var index = 0;

      app.page.model.data.priceHistory.forEach(a => {
        temp.push(a);
        History_elem += CreateElement(a.amount, a.effectiveFrom, index);
        index = index + 1;
      });

    }

    $("#amountHistoryID").empty();
    $("#amountHistoryID").append(History_elem);

    DatePickerLoad(".date");

    $('.amount').inputmask('decimal');

    $('#newEffectiveDate').datepicker('setDate', '');

    $.each($('[id^="amount_"]'), function (index, element) {
      $(element).val(parseFloat(temp[index].amount.toString().replace(",", "")).toFixed(2).toString().replace(".", ""));
    });

    $('div.datepicker').each(function () {
      $(this).datepicker('setDate', new Date($(this).find('input').val()));
    });

  };

  function CreateElement(amount, effectiveDate, index) {

    if (new Date(effectiveDate).setHours(0, 0, 0, 0) >= new Date().setHours(0, 0, 0, 0)) {
      return '<tr>' +
        '<td> <input id=amount_' + index + ' index=' + index + ' onChange="app.page.updateAmountHistory(this)" onKeyUp="app.page.amountValidate(this)"' +
        'value="" class="form-control amount text-center font-weight-bold" />' +
        '<span class="err_message cust_AmtPosition  text-danger"></span></td > ' +
        '<td class="text-right">' +
        '<div  id=effectiveFrom_' + index + ' index=' + index + ' style="position:relative" class="input-group date datepicker enteredDateValue">' +
        '<input type="text" value = "' + moment(new Date(effectiveDate)).format('DD MMM YYYY') + '" class="form-control">' +
        '<span class="input-group-addon input-group-append border-left">' +
        '<span class="mdi mdi-calendar input-group-text"></span>' +
        '</span>' +
        '</span>' +
        '</div>' +
        '<span class="err_message cust_DatePosition text-danger"></span></td >' +
        '</tr>';
    }
    else {
      return '<tr>' +
        '<tr>' +
        '<td> ' + amount + ' </td>' +
        '<td> ' + moment(new Date(effectiveDate)).format('DD MMM YYYY') + ' </td>' +
        '</tr>';
    }

  }

  function DatePickerLoad(ele) {
    $(ele).datepicker({
      startDate: fromDate,
      orientation: "top"
    }).on("hide", function (e) {
      var index = $(this).attr('index');

      if (e.type == "hide" && index != undefined) {
        dateText = $('.datepicker[index="' + index + '"]').datepicker('getDate');

        if (dateText != null) {
          temp[index].effectiveFrom = moment(new Date(dateText)).format('YYYY-MM-DD');

          var tempAmount = $('#newAmountID').val();

          var tempEffectiveFrom = $('#newEffectiveDate').find('input').val();

          app.page.reInitAmount();

          if (tempAmount != '') {
            $('#newAmountID').val(tempAmount.toString().replace(".", ""));
          }

          if (tempEffectiveFrom != '') {
            $('#newEffectiveDate').datepicker('setDate', new Date(tempEffectiveFrom));
          }

          if (toAdd == true) {
            app.page.validate();
          }

        }
        else {
          $('.datepicker[index="' + index + '"]').datepicker('setDate', new Date(temp[index].effectiveFrom));
        }
      }
      else if (index == undefined && toAdd == true) {

        dateText = $('#newEffectiveDate').datepicker('getDate');

        if ($('#newEffectiveDate').datepicker('getDate') == null) {
          $('#newEffectiveDate').next(".err_message").html("Effective From is required");
          $('#newEffectiveDate').addClass('border-danger');
        }
        else {
          $('#newEffectiveDate').next(".err_message").html("");
          $('#newEffectiveDate').removeClass('border-danger');

          var newObjIndex = temp.findIndex((obj => new Date(app.utils.formatDate(obj.effectiveFrom)).toISOString().slice(0, 10) ==
            new Date(app.utils.formatDate(dateText)).toISOString().slice(0, 10)));

          if (newObjIndex != -1) {
            $('#newEffectiveDate').next(".err_message").html("Duplicate Effective From");
            $('#newEffectiveDate').addClass('border-danger');
          }

        }

      }

    });
  }

  app.page.amountValidate = function amountValidate(e) {
    var id = $(e).attr('id');

    if ($(e).val() == "" || $(e).val() == "0.00" && (id.startsWith("amount_") == true || toAdd == true)) {
      $(e).next(".err_message").html("Amount is required");
      $(e).addClass('border-danger');
    }
    else if (id.startsWith("amount_") == true || toAdd == true) {
      $(e).next(".err_message").html("");
      $(e).removeClass('border-danger');
    }

  };

  app.page.updateAmountHistory = function updateAmountHistory(e) {
    var index = $(e).attr('index');
    temp[index].amount = $(e).val();
  };


  $(".btn-add").click(function () {
    toAdd = true;

    app.page.validate();

    if ($('#newAmountID').val() != "" && $('#newEffectiveDate').datepicker('getDate') != null && $('#newAmountID').val()
      != undefined && $('#newEffectiveDate').datepicker('getDate') != undefined) {
      var objIndex = temp.findIndex((obj => new Date(app.utils.formatDate(obj.effectiveFrom)).toISOString().slice(0, 10) ==
        new Date(app.utils.formatDate($('#newEffectiveDate').datepicker('getDate'))).toISOString().slice(0, 10)));

      if (objIndex === -1) {
        temp.push({
          "id": 0,
          "amount": $('#newAmountID').val(),
          "effectiveFrom": moment(new Date($('#newEffectiveDate').datepicker('getDate'))).format('YYYY-MM-DD'),
          "transactionCodeID": app.page.model.data.id
        })

        $('#newAmountID').val("");
        $('#newEffectiveDate').datepicker('setDate', '');
        var elem = defaultTableElem;

        temp.sort(function (a, b) {

          if (a.effectiveFrom < b.effectiveFrom) {
            return 1;
          }

          if (a.effectiveFrom > b.effectiveFrom) {
            return -1;
          }

          return 0;
        });

        $("#amountHistoryID").empty();
        var index = 0;

        temp.forEach(a => {
          elem += CreateElement(a.amount, a.effectiveFrom, index);
          index = index + 1;
        });

        $("#amountHistoryID").append(elem);

        DatePickerLoad('.date');

        $('#newEffectiveDate').datepicker('setDate', '');

        $(".amount").inputmask('decimal');

        $.each($('[id^="amount_"]'), function (index, element) {
          $(element).val(parseFloat(temp[index].amount.toString().replace(",", "")).toFixed(2).toString().replace(".", ""));
        });

        $('.enteredDateValue').each(function () {
          $(this).datepicker('setDate', new Date($(this).find('input').val()));
        });
        toAdd = false;

        app.page.validationCheck();
      }
      else {
        $('#newEffectiveDate').next(".err_message").html("Duplicate Effective From");
        $('#newEffectiveDate').addClass('border-danger');
      }

    }

  });

  $('.saveTransactionBtn').click(function () {
    var IsDuplicateExist = app.page.duplicateCheck();
    var IsValid = app.page.validationCheck();
    toAdd = false;
    $('#newEffectiveDate').next(".err_message").html("");
    $('#newEffectiveDate').removeClass('border-danger');
    $('#newAmountID').next(".err_message").html("");
    $('#newAmountID').removeClass('border-danger');

    var index = 0;
    temp.forEach(a => {
      temp[index].amount = parseFloat(a.amount.toString().replace(",", "")).toFixed(2);
      index = index + 1;
    });

    if (IsDuplicateExist == true && IsValid == true && temp.length > 0) {
      var controller;
      if (controller) {
        controller.abort();
      }
      controller = new AbortController();

      app.showProgress('Processing. Please wait...');

      fetch('/api/transactionCode', {
        method: 'PUT',
        cache: 'no-cache',
        credentials: 'include',
        signal: controller.signal,
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(temp)
      }).then(app.http.errorInterceptor)
        .then(res => res.json())
        .then(r => {
          location.reload(true);
        }).catch(app.http.catch);
    }
  });

}).apply(app.page.amountHistory = app.page.amountHistory || {});

$(document).ready(function () {
  app.page.init();
});
