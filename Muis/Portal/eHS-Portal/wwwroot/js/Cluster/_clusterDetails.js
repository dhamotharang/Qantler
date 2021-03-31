(function () {
  var data = app.page.model;
  var cDetails_elem = ''
  $("#ClusterDetailsID").empty();
  if (data != null) {
    cDetails_elem = '<div class="form-group row">' +
                                 '<label class="col-sm-3 col-form-label" > Name  </label>' +
                                 '<div class="col-sm-9">' +
                                 '<input type="text" maxlength="15" id="district" class="form-control form-control-lg" placeholder = "Enter name"/>' +
                                 '<label class="error mt-2 text-danger">Name is required</label></div>' +
                                 '</div >' +
                                 '<div class="form-group row">' +
                                 '<label class="col-sm-3 col-form-label" > General Location</label>' +
                                 '<div class="col-sm-9">' +
                                 '<input type="text" maxlength="500" id="locations" class="form-control form-control-lg" ' +
                                 'placeholder = "Enter general location here" /> ' +
                                 '<label class="error mt-2 text-danger">General location is required</label></div>' +
                                 '</div>'
  }
  else {
    cDetails_elem = '<div class="form-group row">' +
                                 '<label class="col-sm-3 col-form-label" > Name</label>' +
                                 '<div class="col-sm-9">' +
                                 '<input type="text" maxlength="15" id="district" class="form-control form-control-lg" value="" placeholder = "Enter name"/>' +
                                 '<label class="error mt-2 text-danger">Name is required</label></div>' +
                                 '</div >' +
                                 '<div class="form-group row">' +
                                 '<label class="col-sm-3 col-form-label" > General Location</label>' +
                                 '<div class="col-sm-9">' +
                                 '<input type="text" maxlength="500" id="locations" class="form-control form-control-lg" ' +
                                 'value="" placeholder = "Enter general location here" /> ' +
                                 '<label class="error mt-2 text-danger">General location is required</label></div>' +
                                 '</div>'
  }

  $("#ClusterDetailsID").append(cDetails_elem);

  if (app.page.model != null) {
    $('#locations').val(app.page.model.data.locations);

    $('#district').val(app.page.model.data.district);
  }

}).apply(app.page.clusterDetails = app.page.clusterDetails || {});