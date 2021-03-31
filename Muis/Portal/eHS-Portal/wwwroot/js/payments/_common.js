(function (self) {
  self.statusColor = function (status) {
    switch (status) {
      case 100: // draft
      case 200: // pending
        return 'dark';
      case 300: // processed
        return 'success'
      case 400: // rejected
      case 500: // cancelled
        return 'danger';
      case 600: // expired
        return 'warning';
      case 700: // pending payment
        return 'info';
      default:
        return 'dark';
    }
  };
})(app.page);