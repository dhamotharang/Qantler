(function (self) {

  self.hasPermission = function (permissions, i) {
    if (!permissions || i > permissions.length - 1) {
      return false;
    }
    return permissions.charAt(i) == '1';
  }

  self.setPermission = function (permissions, i, val) {
    permissions = permissions || '';

    if (permissions.length <= i) {
      permissions += "0".repeat(i + 1 - permissions.length);
    }

    return permissions.substring(0, i) + val + permissions.substring(i + 1)
  }

})(app.permission = app.permission || {});