(function () {

  this.getLetterByID = function (model, letterID = null, activityID = null) {
    var letter = {
      id: 0,
      body: "",
      status: 100
    };

    if (activityID != null) {
      var letters = model.activities.find(x => x.id == activityID).letters;
      letter = letters.find(x => x.id == letterID);
    }

    return Object.assign({}, letter);
  }

  this.getLatestSanction = function (data, type) {
    if (!data
      || data.sanctionInfos == null
      || data.sanctionInfos.length == 0) {
      return;
    }

    return data.sanctionInfos
      .filter(e => !type || e.type == type)
      .sort((a, b) => b.id - a.id)[0];
  }

}).apply(app.page.helper = app.page.helper || {});