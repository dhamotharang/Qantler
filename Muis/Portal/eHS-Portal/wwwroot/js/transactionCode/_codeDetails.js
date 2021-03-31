(function () {
  var data = app.page.model.data;
  var container = $('.tcDataAppend');
  var cond = '';
  var splitString = app.page.model.cond.split(' ');
  var letters = /^[A-Za-z]+$/;

  container.find('#code').html(data.code);
  container.find('#glEntry').html(data.glEntry);
  container.find('#currency').html(data.currency);

  if (app.page.model.cond == "" || app.page.model.cond == null) {
    $('.condition').css('display', 'none');
  }

  for (var i = 0; i < splitString.length; i++) {
    var resultString = operatorSwitch(splitString[i]);
    cond += '&nbsp;';
    if (resultString.match(letters)) {
      cond += '&nbsp;&nbsp;<a style="font-weight: bold;">' + resultString + '</a>';
    }
    else {
      cond += '<a>' + resultString + '</a>&nbsp;';
    }
  }

  $('#condition').empty();
  $('#condition').append(cond);

  $('.tcNote').empty();
  $('.tcNote').append('<div class="p-3 mb-2 bg-inverse-info">' + data.text + '</div >');

  function operatorSwitch(op) {
    switch (op) {
      case "Equal":
        return "=";
      case "LessThan":
        return "<";
      case "GreaterThan":
        return ">";
      case "LessThanOrEqual":
        return "<=";
      case "GreaterThanOrEqual":
        return ">=";
      case "NotEqual":
        return "!=";
      default:
        return op;
    }
  }
}).apply(app.page.codeDetails = app.page.codeDetails || {});