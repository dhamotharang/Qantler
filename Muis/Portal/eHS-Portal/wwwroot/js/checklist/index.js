(function (page) {
  'use strict';
  var checklist;
  var latestEffectiveDate;
  var selectedVersionHistoryId = 0;
  var selectedVersion = 0;
  var latestVersion = 0;
  var checklistTemplate = $('.checklist-template .checklist');
  var itemTemplate = $('.checklist-template .checklist-item');
  var infoTemplate = $('.checklist-template .info');
  var categoryTemplate = $('.checklist-template .info.category');

  page.getChecklistVersion = function getChecklistVersion() {
    $('.checklist-versions').innerHTML = "";
    var versionHtml = "";

    $.each(checklist, function (index, value) {
      if (value.createdByName == undefined) {
        value.createdByName = "";
      }

      versionHtml += '<li class="border-top py-2 versions" id="version-index-' + index + '" onclick="app.page.getChecklistItems(' + index + ')">' +
        '<div class="row" style="padding-left: 16px;padding-right: 16px;">' +
        '<div class="col"> ' +
        '<strong class="d-block">Version ' + value.version + '</strong>' +
        '<span>' + value.createdByName + '</span>' +
        '</div>' +
        '<div class="col text-right">' + app.utils.formatShortDate(value.effectiveFrom, true) + '</div>' +
        '</div>' +
        '</li> ';
    });

    $('.checklist-versions').html(versionHtml.toString());
    page.getChecklistItems(0);
  }

  page.load = function load(items) {
    $('#checklist-value').html('');
    var checklistTemp = checklistTemplate.clone();
    var currItem;
    items.forEach(function (category) {

      checklistTemp.append(categoryTemplate
        .prop('outerHTML')
        .replaceAll('{id}', category.index)
        .replaceAll('{text}', category.text));

      category.items.forEach(function (item) {
        currItem = itemTemplate.clone();

        currItem.append(infoTemplate
          .clone()
          .prop('outerHTML')
          .replaceAll('{id}', category.index + '.' + item.index)
          .replaceAll('{text}', item.text)
          .replaceAll('{tooltipText}', item.notes)
          .replaceAll('{tooltipVisibility}', item.notes != undefined && item.notes != '' ? 'block' : 'd-none'));

        checklistTemp.append(currItem);
      });

    });
    $('#checklist-value').append(checklistTemp);
  };

  page.getChecklistItems = function getChecklistItems(index) {
    $('.versions').removeClass('version-selected');

    $('#version-index-' + index).addClass('version-selected');

    selectedVersion = checklist[index].version;

    selectedVersionHistoryId = checklist[index].id;

    page.load(checklist[index].categories);
  }

  $('.add-new-btn').click(function () {
    var url = window.location.origin + "/checklist/details?lastVersion=" + latestVersion + "&scheme=" + $('#schemeSelect').val();
    window.location.href = url;
  });

  $('#new-version').click(function () {
    var url = window.location.origin + "/checklist/details?historyID=" + selectedVersionHistoryId + "&userAction=Clone&lastVersion=" + latestVersion + "&scheme=" + $('#schemeSelect').val();
    window.location.href = url;
  });

  $('#edit-version').click(function () {
    var url = window.location.origin + "/checklist/details?historyID=" + selectedVersionHistoryId + "&userAction=Edit&lastVersion=" + selectedVersion + "&scheme=" + $('#schemeSelect').val();
    window.location.href = url;
  });

  page.getChecklist = function getChecklist(id) {
    app.showProgress('Processing. Please wait...');

    fetch('/api/checklist/scheme/' + id, {
      method: 'GET',
      cache: 'no-cache',
      credentials: 'include',
      headers: {
        'Accept': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(r => {
        app.dismissProgress();
        if (r.length == 0) {
          $('#checklist-value').html("");

          $('.checklist-versions').html("");

          latestEffectiveDate = "";

          checklist = [];

          $('#new-version').hide();

          $('#edit-version').hide();

          $('#new-version-new').show();

          latestVersion = 0;
        }
        else {
          checklist = r;
          latestVersion = checklist[0].version;

          page.getChecklistVersion();

          latestEffectiveDate = checklist[0].effectiveFrom;
          var effectiveDate = new Date(latestEffectiveDate);
          effectiveDate.setHours(0, 0, 0, 0);
          var today = new Date();
          today.setHours(0, 0, 0, 0);

          if (today > effectiveDate) {
            $('#new-version').show();
            $('#edit-version').hide();
            $('#new-version-new').show();
          }
          else {
            $('#new-version').hide();
            $('#edit-version').show();
            $('#new-version-new').hide();
          }
        }

      })
      .catch(app.http.catch);
  }

})(app.page);

$(document).ready(function () {
  $('#schemeSelect').select2();
  $('#schemeSelect').val(100);
  $('#schemeSelect').trigger('change');
});