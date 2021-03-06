(function () {
  var data = app.page.model.data;
  $("#activitieTable").empty();
  var elem = '';

  if (app.page.model.data.logs != undefined && data.logs.length > 0) {
    elem +=
      '<div class="card-body"><div class="wrapper">' +
      '<h4 class="font-weight-bold mb-4"> Activities</h4 > ' +
      '<ul class="activities-container timeline">';

    data.logs.forEach(a => {
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
      '<div class="widget-placeholder" style = "height:0px">' +
      '<span class="text-muted" > No Activities yet.</span > ' +
      '</div>' +
      '</div>' +
      '</div>' +
      '</div>';
  }

  $("#activitieTable").append(elem);

  function createActivitieElement(name, action, createdDate) {
    return '<li class="timeline-item">' +
      '<p class="timeline-content">' +
      '<code>' + app.utils.titleCase(name) + '</code> ' + action + '</p> ' +
      '<p class="event-time text-muted"><i class="mdi mdi-clock-outline">' +
    '</i > ' + app.utils.timeAgo(createdDate) + '</p ></li > ';
  }

  function formatLog(e) {
    var result = e.action;

    if (e.params) {
      var i;
      for (i = 0; i < e.params.length; i++) {

        var val = e.params[i];

        if (!isNaN(Date.parse(val)) && val.toString().indexOf(".") == -1) {
          val = moment(new Date(val)).format('DD MMM YYYY : h:mmA');
        }
        else {
          val = Number(val).toLocaleString();
        }

        result = result.replace("{" + i + "}", '<b>' + val + '</b>');
      }
    }

    result += app.common.createHashtag(e.type, e.refID);

    return result;
  }

}).apply(app.page.activities = app.page.activities || {});