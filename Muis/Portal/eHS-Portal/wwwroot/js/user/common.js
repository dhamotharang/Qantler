(function (self) {

  self.statusColor = function (status) {
    switch (status) {
      case 0: // active
        return 'success';
      case 1: // in-active
        return 'dark';
      case 2: // suspended
        return 'danger';
      default:
        return 'dark';
    }
  }

})(app.page.common = app.page.common || {});