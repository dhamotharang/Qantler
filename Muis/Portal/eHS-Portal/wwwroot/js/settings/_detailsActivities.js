(function () {
  var rData = app.page.model.requestSettings;
  var jData = app.page.model.jobOrderSettings;
  var tempData = [];
  var elem = '';

  for (var i = 0; i <= 10; i++) {
    if (rData[i] != undefined) {
      tempData = tempData.concat(rData[i].logs);
    }

    if (jData[i] != undefined) {
      tempData = tempData.concat(jData[i].logs);
    }
  }

  tempData.sort(sortFunction);

  $("#activitieTable").empty();
  
  if (tempData.length > 0) {
    elem +=
      '<div class="card-body"><div class="wrapper">' +
      '<h4 class="font-weight-bold mb-4"> Activities</h4 > ' +
      '<ul class="activities-container timeline">';

    tempData.forEach(a => {
      var action = formatLog(a);

      elem += createActivitieElement(a.userName, action, a.createdOn);
    });

    elem += '</ul> </div> </div>';
  }
  else {
    elem =
      '<div class="card-body">' +
      '<div class="wrapper">' +
      '<h4 class="font-weight-bold mb-4"> Activities</h4>' +
      '<div class="related-container">' +
      '<div class="widget-placeholder">' +
      '<span class="text-muted" > No Activities yet.</span > ' +
      '</div>' +
      '</div>' +
      '</div>' +
      '</div>';
  }

  $("#activitieTable").append(elem);

  function createActivitieElement(name, action, createdDate) {
    return '<li class="timeline-item">' +
               '<p class="timeline-content">'+
               '<code> ' + app.utils.titleCase(name) + '</code> ' + action +
               '</p>' +
               '<p class="event-time text-muted">'+
               '<i class="mdi mdi-clock-outline"> ' + '</i> ' + app.utils.timeAgo(createdDate) +
               '</p>' +
               '</li> ';
  }

  function formatLog(e) {
    var result = e.action;
    if (e.params) {
      for (var i = 0; i < e.params.length; i++) {

        var val = e.params[i];

        result = result.replace("{" + i + "}", '<b>' + val + '</b>');
      }
    }

    result += app.common.createHashtag(e.type, e.refID);

    return result;
  }

  function sortFunction(a, b) {
    var dateA = new Date(a.createdOn);
    var dateB = new Date(b.createdOn);
    return dateA < dateB ? 1 : -1;
  }; 

}).apply(app.page.activitySettings = app.page.activitySettings || {});