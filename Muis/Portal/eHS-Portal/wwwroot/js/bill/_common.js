(function (self) {
  self.statusColor = function (status) {
    switch (status) {
      case 100: // draft
      case 200: // pending
        return 'dark';
      case 300: // paid
        return 'success'
      case 400: // overdue
        return 'danger';
      case 500: // cancelled             
      default:
        return 'dark';
    }
  };
})(app.page);